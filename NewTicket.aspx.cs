using Laba3.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Laba3
{
    public partial class NewTicket : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is authenticated
            if (Session["UserID"] == null || Session["UserRole"] == null || Session["UserRole"].ToString() != "CLIENT")
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCategories();
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

        protected void SubmitTicket_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int clientId = Convert.ToInt32(Session["UserID"]);
                    int categoryId = Convert.ToInt32(TicketCategory.SelectedValue);

                    using (var db = new ApplicationDbContext())
                    {
                        // Get the "New" status
                        var newStatus = db.Statuses.FirstOrDefault(s => s.StatusName == "Новая");
                        if (newStatus == null)
                        {
                            ErrorMessage.Visible = true;
                            ErrorMessageText.Text = "Ошибка: не найден статус 'Новая'";
                            return;
                        }

                        // Create the task
                        var task = new Task
                        {
                            Name = TicketName.Text,
                            Description = TicketDescription.Text,
                            ClientId = clientId,
                            DateCreated = DateTime.Now,
                            CategoryId = categoryId,
                            StatusId = newStatus.Id
                        };

                        db.Tasks.Add(task);
                        db.SaveChanges();

                        // Send email notification
                        var client = db.Clients.Find(clientId);
                        SendEmailNotification(client.Email, task.Id, task.Name);

                        // Redirect to a success page or list of tickets
                        Response.Redirect("~/MyTickets.aspx");
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage.Visible = true;
                    ErrorMessageText.Text = "Произошла ошибка при создании заявки: " + ex.Message;
                }
            }
        }

        private void SendEmailNotification(string clientEmail, int taskId, string taskName)
        {
            try
            {
                var subject = "Новая заявка создана: #" + taskId;
                var body = string.Format("Уважаемый клиент,<br><br>Ваша заявка #{0} '{1}' успешно создана и зарегистрирована в системе. " +
                                         "Наши специалисты приступят к ее обработке в ближайшее время.<br><br>" +
                                         "С уважением,<br>Служба поддержки", taskId, taskName);

                // This is a placeholder - in a real application, you would use a proper email service
                // SmtpClient client = new SmtpClient("smtp.example.com");
                // client.UseDefaultCredentials = false;
                // client.Credentials = new NetworkCredential("username", "password");

                // MailMessage message = new MailMessage();
                // message.From = new MailAddress("support@example.com");
                // message.To.Add(new MailAddress(clientEmail));
                // message.Subject = subject;
                // message.Body = body;
                // message.IsBodyHtml = true;

                // client.Send(message);
            }
            catch (Exception)
            {
                // Log the error but don't stop execution
            }
        }
    }
}