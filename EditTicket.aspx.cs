using Laba3.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web.UI;

namespace Laba3
{
    public partial class EditTicket : System.Web.UI.Page
    {
        private int ticketId;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Session["UserRole"] == null || Session["UserRole"].ToString() != "CLIENT")
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
                LoadCategories();
                LoadTicketDetails();
            }
        }

        private void LoadCategories()
        {
            using (var db = new ApplicationDbContext())
            {
                TicketCategory.DataSource = db.Categories.ToList();
                TicketCategory.DataTextField = "CategoryName";
                TicketCategory.DataValueField = "Id";
                TicketCategory.DataBind();
            }
        }

        private void LoadTicketDetails()
        {
            int clientId = Convert.ToInt32(Session["UserID"]);

            using (var db = new ApplicationDbContext())
            {
                var ticket = db.Tasks.FirstOrDefault(t => t.Id == ticketId && t.ClientId == clientId);
                
                if (ticket == null)
                {
                    ShowError("Заявка не найдена или у вас нет прав для её редактирования");
                    DisableForm();
                    return;
                }

                if (ticket.DateClosed.HasValue)
                {
                    ShowError("Закрытая заявка не может быть отредактирована");
                    DisableForm();
                    return;
                }

                TicketName.Text = ticket.Name;
                TicketDescription.Text = ticket.Description;
                TicketCategory.SelectedValue = ticket.CategoryId.ToString();
            }
        }

        protected void UpdateTicket_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int clientId = Convert.ToInt32(Session["UserID"]);

            using (var db = new ApplicationDbContext())
            {
                var ticket = db.Tasks.FirstOrDefault(t => t.Id == ticketId && t.ClientId == clientId);
                
                if (ticket == null)
                {
                    ShowError("Заявка не найдена или у вас нет прав для её редактирования");
                    return;
                }

                if (ticket.DateClosed.HasValue)
                {
                    ShowError("Закрытая заявка не может быть отредактирована");
                    return;
                }

                ticket.Name = TicketName.Text.Trim();
                ticket.Description = TicketDescription.Text.Trim();
                ticket.CategoryId = Convert.ToInt32(TicketCategory.SelectedValue);

                try
                {
                    db.SaveChanges();
                    
                    if (ticket.PersonalId.HasValue)
                    {
                        var staff = db.Personnel.Find(ticket.PersonalId.Value);
                        if (staff != null)
                        {
                            SendEmailNotification(staff.Email, ticket.Id, ticket.Name);
                        }
                    }

                    Response.Redirect("~/MyTickets.aspx");
                }
                catch (Exception ex)
                {
                    ShowError("Произошла ошибка при обновлении заявки: " + ex.Message);
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MyTickets.aspx");
        }

        private void ShowError(string message)
        {
            ErrorMessage.Visible = true;
            ErrorMessageText.Text = message;
        }

        private void DisableForm()
        {
            TicketName.Enabled = false;
            TicketDescription.Enabled = false;
            TicketCategory.Enabled = false;
            UpdateTicket.Enabled = false;
        }

        private void SendEmailNotification(string email, int taskId, string taskName)
        {
            try
            {
                string subject = "Заявка обновлена: #" + taskId;
                string body = string.Format("Уважаемый специалист,<br><br>Заявка #{0} '{1}' была обновлена клиентом.<br><br>" +
                                         "С уважением,<br>Служба поддержки", taskId, taskName);

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