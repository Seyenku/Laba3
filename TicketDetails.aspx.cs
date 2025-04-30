using Laba3.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Laba3
{
    public partial class TicketDetails : System.Web.UI.Page
    {
        private int ticketId;
        private Task currentTicket;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Session["UserRole"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!int.TryParse(Request.QueryString["id"], out ticketId))
            {
                ShowError("Неверный идентификатор заявки");
                return;
            }

            if (!IsPostBack)
            {
                LoadTicketDetails();
            }
        }

        private void LoadTicketDetails()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var ticket = db.Tasks
                        .Include(t => t.Category)
                        .Include(t => t.Status)
                        .Include(t => t.Personal)
                        .Include(t => t.Client)
                        .FirstOrDefault(t => t.Id == ticketId);

                    if (ticket == null)
                    {
                        ShowError("Заявка не найдена");
                        return;
                    }

                    currentTicket = ticket;

                    bool isClient = Session["UserRole"].ToString() == "CLIENT";
                    bool isStaff = Session["UserRole"].ToString() == "IT_STAFF";
                    int userId = Convert.ToInt32(Session["UserID"]);

                    if (isClient && ticket.ClientId != userId)
                    {
                        ShowError("У вас нет доступа к этой заявке");
                        return;
                    }

                    TicketIdLiteral.Text = ticket.Id.ToString();
                    TicketNameLiteral.Text = ticket.Name;
                    CategoryLiteral.Text = ticket.Category.CategoryName;
                    StatusLiteral.Text = ticket.Status.StatusName;
                    ClientLiteral.Text = ticket.Client.Name;
                    ClientContactsLiteral.Text = $"Email: {ticket.Client.Email}<br/>Телефон: {ticket.Client.Telephone}<br/>Адрес: {ticket.Client.Address}";

                    if (ticket.Personal != null)
                    {
                        PersonalLiteral.Text = $"{ticket.Personal.Fio} ({ticket.Personal.Staff})";
                    }
                    else
                    {
                        PersonalLiteral.Text = "Не назначен";
                    }

                    DateCreatedLiteral.Text = ticket.DateCreated.ToString("dd.MM.yyyy HH:mm");

                    if (ticket.DateClosed.HasValue)
                    {
                        DateClosedDiv.Visible = true;
                        DateClosedLiteral.Text = ticket.DateClosed.Value.ToString("dd.MM.yyyy HH:mm");
                    }

                    DescriptionLiteral.Text = HttpUtility.HtmlEncode(ticket.Description ?? "").Replace(Environment.NewLine, "<br/>");

                    if (isClient && !ticket.DateClosed.HasValue)
                    {
                        CloseTicketButton.Visible = true;
                    }

                    if (isStaff)
                    {
                        if (ticket.PersonalId == userId && !ticket.DateClosed.HasValue)
                        {
                            StatusLiteral.Visible = false;
                            StatusDropDown.Visible = true;
                            UpdateStatusButton.Visible = true;

                            var statuses = db.Statuses.Where(s => s.StatusName != "Завершена").ToList();
                            StatusDropDown.DataSource = statuses;
                            StatusDropDown.DataTextField = "StatusName";
                            StatusDropDown.DataValueField = "Id";
                            StatusDropDown.DataBind();
                            StatusDropDown.SelectedValue = ticket.StatusId.ToString();
                        }

                        if (ticket.PersonalId == null && !ticket.DateClosed.HasValue)
                        {
                            AssignTicketButton.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        protected void BackButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyTickets.aspx");
        }

        protected void CloseTicketButton_Click(object sender, EventArgs e)
        {
            if (Session["UserRole"].ToString() != "CLIENT")
            {
                ShowError("У вас нет прав для выполнения этого действия");
                return;
            }

            int clientId = Convert.ToInt32(Session["UserID"]);

            using (var db = new ApplicationDbContext())
            {
                var ticket = db.Tasks
                    .Include(t => t.Client)
                    .Include(t => t.Personal)
                    .FirstOrDefault(t => t.Id == ticketId && t.ClientId == clientId);

                if (ticket == null)
                {
                    ShowError("Заявка не найдена или у вас нет прав доступа к ней");
                    return;
                }

                var closedStatus = db.Statuses.FirstOrDefault(s => s.StatusName == "Завершена");
                if (closedStatus != null)
                {
                    ticket.StatusId = closedStatus.Id;
                    ticket.DateClosed = DateTime.Now;
                    db.SaveChanges();

                    if (ticket.PersonalId.HasValue)
                    {
                        var staff = db.Personnel.Find(ticket.PersonalId.Value);
                        if (staff != null)
                        {
                            SendEmailNotification(staff.Email, ticket.Id, ticket.Name, "closed");
                        }
                    }

                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    ShowError("Не удалось найти статус 'Завершена'");
                }
            }
        }

        protected void AssignTicketButton_Click(object sender, EventArgs e)
        {
            if (Session["UserRole"].ToString() != "IT_STAFF")
            {
                ShowError("У вас нет прав для выполнения этого действия");
                return;
            }

            int personalId = Convert.ToInt32(Session["UserID"]);

            using (var db = new ApplicationDbContext())
            {
                var ticket = db.Tasks
                    .Include(t => t.Client)
                    .FirstOrDefault(t => t.Id == ticketId);

                if (ticket == null || ticket.DateClosed.HasValue)
                {
                    ShowError("Заявка не найдена или уже закрыта");
                    return;
                }

                if (ticket.PersonalId.HasValue)
                {
                    ShowError("Заявка уже назначена другому специалисту");
                    return;
                }

                ticket.PersonalId = personalId;

                var inProgressStatus = db.Statuses.FirstOrDefault(s => s.StatusName == "В работе");
                if (inProgressStatus != null)
                {
                    ticket.StatusId = inProgressStatus.Id;
                }

                db.SaveChanges();

                var staff = db.Personnel.Find(personalId);
                if (staff != null && ticket.Client != null)
                {
                    SendEmailNotification(ticket.Client.Email, ticket.Id, ticket.Name, "assigned", staff.Fio);
                }

                Response.Redirect(Request.RawUrl);
            }
        }

        protected void UpdateStatusButton_Click(object sender, EventArgs e)
        {
            if (Session["UserRole"].ToString() != "IT_STAFF")
            {
                ShowError("У вас нет прав для выполнения этого действия");
                return;
            }

            int personalId = Convert.ToInt32(Session["UserID"]);
            int statusId = Convert.ToInt32(StatusDropDown.SelectedValue);

            using (var db = new ApplicationDbContext())
            {
                var ticket = db.Tasks
                    .Include(t => t.Client)
                    .Include(t => t.Status)
                    .FirstOrDefault(t => t.Id == ticketId && t.PersonalId == personalId);

                if (ticket == null || ticket.DateClosed.HasValue)
                {
                    ShowError("Заявка не найдена, уже закрыта или не назначена вам");
                    return;
                }

                var oldStatus = ticket.Status.StatusName;
                ticket.StatusId = statusId;
                db.SaveChanges();

                var newStatus = db.Statuses.Find(statusId).StatusName;

                if (ticket.Client != null)
                {
                    SendStatusUpdateEmail(ticket.Client.Email, ticket.Id, ticket.Name, oldStatus, newStatus);
                }

                Response.Redirect(Request.RawUrl);
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Visible = true;
            ErrorMessageText.Text = message;
        }

        private void SendEmailNotification(string email, int taskId, string taskName, string action, string staffName = null)
        {
            try
            {
                var emailService = new EmailService();
                
                if (action == "assigned")
                {
                    emailService.SendAssignmentNotification(email, taskId.ToString(), taskName, staffName);
                }
                else if (action == "closed")
                {
                    string subject = "Заявка закрыта: #" + taskId;
                    string body = string.Format("Уважаемый специалист,<br><br>Заявка #{0} '{1}' была закрыта клиентом.<br><br>" +
                                         "С уважением,<br>Служба поддержки", taskId, taskName);
                    
                    emailService.SendEmail(email, subject, body);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка отправки почты: " + ex.Message);
            }
        }

        private void SendStatusUpdateEmail(string email, int taskId, string taskName, string oldStatus, string newStatus)
        {
            try
            {
                var emailService = new EmailService();
                emailService.SendStatusChangeNotification(email, taskId.ToString(), taskName, newStatus);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка отправки почты: " + ex.Message);
            }
        }
    }
}