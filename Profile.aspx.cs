using System;
using System.Web.Security;
using System.Linq;
using System.Web;
using Laba3.Models;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;

namespace Laba3
{
    public partial class Profile : System.Web.UI.Page
    {
        private int userId;
        private string userRole;
        private string userEmail;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            userId = Convert.ToInt32(Session["UserID"]);
            userRole = Session["UserRole"].ToString();
            userEmail = Session["UserName"].ToString();

            if (!IsPostBack)
            {
                txtCurrentPassword.Text = "";
                
                LoadUserData();
                LoadTicketStatistics();
                LoadRecentTickets();
            }
        }

        private void LoadUserData()
        {
            using (var db = new ApplicationDbContext())
            {
                if (userRole == "CLIENT")
                {
                    var client = db.Clients.FirstOrDefault(c => c.Id == userId);
                    if (client != null)
                    {
                        string[] nameParts = client.Name.Split(' ');
                        
                        if (nameParts.Length > 0)
                            txtFirstName.Text = nameParts[0];
                        if (nameParts.Length > 1)
                            txtLastName.Text = nameParts[1];
                        else
                            txtLastName.Text = "";
                            
                        txtPhone.Text = client.Telephone;
                        txtEmail.Text = client.Email;

                        lblFullName.Text = client.Name;
                        lblEmail.Text = client.Email;
                        lblRole.Text = "Клиент";
                    }
                }
                else
                {
                    var staff = db.Personnel.FirstOrDefault(s => s.Id == userId);
                    if (staff != null)
                    {
                        string[] nameParts = staff.Fio.Split(' ');
                        
                        if (nameParts.Length > 0)
                            txtFirstName.Text = nameParts[0];
                        if (nameParts.Length > 1)
                            txtLastName.Text = nameParts[1];
                        else
                            txtLastName.Text = "";
                            
                        txtPhone.Text = ""; // В модели Personal нет поля для телефона
                        txtEmail.Text = staff.Email;

                        lblFullName.Text = staff.Fio;
                        lblEmail.Text = staff.Email;
                        lblRole.Text = "Специалист IT";
                    }
                }
            }
        }

        private void LoadTicketStatistics()
        {
            using (var db = new ApplicationDbContext())
            {
                var closedStatusId = db.Statuses
                    .Where(s => s.StatusName == "Завершена")
                    .Select(s => s.Id)
                    .FirstOrDefault();

                var query = db.Tasks.AsQueryable();

                if (userRole == "CLIENT")
                {
                    query = query.Where(t => t.ClientId == userId);
                }
                else
                {
                    query = query.Where(t => t.PersonalId == userId || t.Status.StatusName == "Новая");
                }

                int totalTickets = query.Count();
                int closedTickets = query.Count(t => t.StatusId == closedStatusId);
                int openTickets = totalTickets - closedTickets;

                lblTotalTickets.Text = totalTickets.ToString();
                lblOpenTickets.Text = openTickets.ToString();
                lblClosedTickets.Text = closedTickets.ToString();
            }
        }

        private void LoadRecentTickets()
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Tasks
                    .Include(t => t.Status)
                    .AsQueryable();

                if (userRole == "CLIENT")
                {
                    query = query.Where(t => t.ClientId == userId);
                }
                else
                {
                    query = query.Where(t => t.PersonalId == userId || t.Status.StatusName == "Новая");
                }

                var recentTickets = query
                    .OrderByDescending(t => t.DateCreated)
                    .Take(5)
                    .ToList();

                gvRecentTickets.DataSource = recentTickets;
                gvRecentTickets.DataBind();
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            using (var db = new ApplicationDbContext())
            {
                if (userRole == "CLIENT")
                {
                    var client = db.Clients.FirstOrDefault(c => c.Id == userId);
                    if (client != null)
                    {
                        client.Name = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}";
                        client.Telephone = txtPhone.Text.Trim();
                        
                        db.SaveChanges();
                        
                        LoadUserData();
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                            "alert('Профиль успешно обновлен');", true);
                    }
                }
                else
                {
                    var staff = db.Personnel.FirstOrDefault(s => s.Id == userId);
                    if (staff != null)
                    {
                        staff.Fio = $"{txtFirstName.Text.Trim()} {txtLastName.Text.Trim()}";
                        
                        db.SaveChanges();
                        
                        LoadUserData();
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                            "alert('Профиль успешно обновлен');", true);
                    }
                }
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (newPassword != confirmPassword)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                    "alert('Новый пароль и подтверждение не совпадают');", true);
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                string storedHash = null;
                
                if (userRole == "CLIENT")
                {
                    var client = db.Clients.FirstOrDefault(c => c.Id == userId);
                    if (client != null)
                    {
                        storedHash = client.PasswordHash;
                        
                        if (storedHash != ComputeSha256Hash(currentPassword))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                                "alert('Текущий пароль введен неверно');", true);
                            return;
                        }
                        
                        client.PasswordHash = ComputeSha256Hash(newPassword);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var staff = db.Personnel.FirstOrDefault(s => s.Id == userId);
                    if (staff != null)
                    {
                        storedHash = staff.PasswordHash;
                        
                        if (storedHash != ComputeSha256Hash(currentPassword))
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                                "alert('Текущий пароль введен неверно');", true);
                            return;
                        }
                        
                        staff.PasswordHash = ComputeSha256Hash(newPassword);
                        db.SaveChanges();
                    }
                }
                
                txtCurrentPassword.Text = "";
                txtNewPassword.Text = "";
                txtConfirmPassword.Text = "";
                
                ClientScript.RegisterStartupScript(this.GetType(), "alert", 
                    "alert('Пароль успешно изменен');", true);
            }
        }
        
        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
} 