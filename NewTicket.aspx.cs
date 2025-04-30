using Laba3.Models;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Laba3
{
    public partial class NewTicket : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
                        var newStatus = db.Statuses.FirstOrDefault(s => s.StatusName == "Новая");
                        if (newStatus == null)
                        {
                            ErrorMessage.Visible = true;
                            ErrorMessageText.Text = "Ошибка: не найден статус 'Новая'";
                            return;
                        }

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

                        var client = db.Clients.Find(clientId);
                        SendEmailNotification(client.Email, task.Id, task.Name);

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
                var emailService = new EmailService();
                emailService.SendNewTicketNotification(clientEmail, taskId.ToString(), taskName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка отправки почты: " + ex.Message);
            }
        }
    }
}