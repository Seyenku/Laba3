using Laba3.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Laba3
{
    public partial class MyTickets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Session["UserRole"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                if (Session["UserRole"].ToString() == "CLIENT")
                {
                    NewTicketButton.Visible = true;
                }

                LoadTickets();
            }
        }

        private void LoadTickets()
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Tasks
                    .Include(t => t.Category)
                    .Include(t => t.Status)
                    .Include(t => t.Personal)
                    .Include(t => t.Client)
                    .AsQueryable();

                if (Session["UserRole"].ToString() == "CLIENT")
                {
                    int clientId = Convert.ToInt32(Session["UserID"]);
                    query = query.Where(t => t.ClientId == clientId);
                }
                else if (Session["UserRole"].ToString() == "IT_STAFF")
                {
                    int personalId = Convert.ToInt32(Session["UserID"]);
                    query = query.Where(t => t.PersonalId == personalId || t.PersonalId == null);
                }

                var ticketsList = query.Select(t => new
                {
                    t.Id,
                    t.Name,
                    CategoryName = t.Category.CategoryName,
                    StatusName = t.Status.StatusName,
                    t.DateCreated,
                    t.DateClosed,
                    t.PersonalId,
                    PersonalName = t.Personal != null ? t.Personal.Fio : string.Empty
                }).ToList();

                TicketsGridView.DataSource = ticketsList;
                TicketsGridView.DataBind();
            }
        }

        protected void NewTicketButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/NewTicket.aspx");
        }

        protected void TicketsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandArgument == null)
                return;

            int taskId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ViewTicket")
            {
                Response.Redirect($"~/TicketDetails.aspx?id={taskId}");
            }
            else if (e.CommandName == "CloseTicket" && Session["UserRole"].ToString() == "CLIENT")
            {
                CloseTicket(taskId);
            }
            else if (e.CommandName == "AssignTicket" && Session["UserRole"].ToString() == "IT_STAFF")
            {
                AssignTicket(taskId);
            }
            else if (e.CommandName == "EditTicket" && Session["UserRole"].ToString() == "CLIENT")
            {
                Response.Redirect($"~/EditTicket.aspx?id={taskId}");
            }
            else if (e.CommandName == "DeleteTicket" && Session["UserRole"].ToString() == "CLIENT")
            {
                DeleteTicket(taskId);
            }
        }

        private void CloseTicket(int taskId)
        {
            int clientId = Convert.ToInt32(Session["UserID"]);

            using (var db = new ApplicationDbContext())
            {
                var task = db.Tasks.FirstOrDefault(t => t.Id == taskId && t.ClientId == clientId);
                if (task != null)
                {
                    var closedStatus = db.Statuses.FirstOrDefault(s => s.StatusName == "Завершена");
                    if (closedStatus != null)
                    {
                        task.StatusId = closedStatus.Id;
                        task.DateClosed = DateTime.Now;
                        db.SaveChanges();

                        if (task.PersonalId.HasValue)
                        {
                            var staff = db.Personnel.Find(task.PersonalId.Value);
                            if (staff != null)
                            {
                                SendEmailNotification(staff.Email, taskId, task.Name, "closed");
                            }
                        }

                        LoadTickets();
                    }
                }
            }
        }

        private void AssignTicket(int taskId)
        {
            int personalId = Convert.ToInt32(Session["UserID"]);

            using (var db = new ApplicationDbContext())
            {
                var task = db.Tasks.FirstOrDefault(t => t.Id == taskId);
                if (task != null && task.PersonalId == null)
                {
                    task.PersonalId = personalId;

                    var inProgressStatus = db.Statuses.FirstOrDefault(s => s.StatusName == "В работе");
                    if (inProgressStatus != null)
                    {
                        task.StatusId = inProgressStatus.Id;
                    }

                    db.SaveChanges();

                    var client = db.Clients.Find(task.ClientId);
                    if (client != null)
                    {
                        var staff = db.Personnel.Find(personalId);
                        SendEmailNotification(client.Email, taskId, task.Name, "assigned", staff?.Fio);
                    }

                    LoadTickets();
                }
            }
        }

        private void DeleteTicket(int taskId)
        {
            int clientId = Convert.ToInt32(Session["UserID"]);

            using (var db = new ApplicationDbContext())
            {
                var task = db.Tasks.FirstOrDefault(t => t.Id == taskId && t.ClientId == clientId);
                if (task != null)
                {
                    db.Tasks.Remove(task);
                    db.SaveChanges();
                    LoadTickets();
                }
            }
        }

        private void SendEmailNotification(string email, int taskId, string taskName, string action, string staffName = null)
        {
            try
            {
                string subject = "";
                string body = "";

                if (action == "assigned")
                {
                    subject = "Статус заявки изменен: #" + taskId;
                    body = string.Format("Уважаемый клиент,<br><br>Ваша заявка #{0} '{1}' взята в работу специалистом {2}.<br><br>" +
                                         "С уважением,<br>Служба поддержки", taskId, taskName, staffName);
                }
                else if (action == "closed")
                {
                    subject = "Заявка закрыта: #" + taskId;
                    body = string.Format("Уважаемый специалист,<br><br>Заявка #{0} '{1}' была закрыта клиентом.<br><br>" +
                                         "С уважением,<br>Служба поддержки", taskId, taskName);
                }

                var emailService = new EmailService();
                emailService.SendEmail(email, subject, body);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка отправки почты: " + ex.Message);
            }
        }
    }
}