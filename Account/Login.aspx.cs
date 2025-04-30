using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Linq;
using Laba3.Models;

namespace Laba3
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errorMessage.Visible = false;
            }
        }

        protected void LogIn_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string email = Email.Text.Trim();
                string password = Password.Text;

                if (ValidateUser(email, password))
                {
                    FormsAuthentication.SetAuthCookie(email, RememberMe.Checked);

                    StoreUserInfoInSession(email);

                    string returnUrl = Request.QueryString["ReturnUrl"];
                    if (!string.IsNullOrEmpty(returnUrl) && returnUrl.StartsWith("/"))
                    {
                        Response.Redirect(returnUrl);
                    }
                    else
                    {
                        Response.Redirect("~/Default.aspx");
                    }
                }
                else
                {
                    ErrorText.Text = "Неверный email или пароль.";
                    errorMessage.Visible = true;
                }
            }
        }

        private bool ValidateUser(string email, string password)
        {
            try
            {
                var db = new ApplicationDbContext();
                string computedHash = ComputeHash(password);

                var personal = db.Personnel.FirstOrDefault(p => p.Email == email);
                if (personal != null && personal.PasswordHash.Equals(computedHash, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                var client = db.Clients.FirstOrDefault(c => c.Email == email);
                if (client != null && client.PasswordHash.Equals(computedHash, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                ErrorText.Text = "Во время входа произошла ошибка. Пожалуйста, попробуйте позже.";
                errorMessage.Visible = true;
                return false;
            }
        }

        private void StoreUserInfoInSession(string email)
        {
            var db = new ApplicationDbContext();

            var personal = db.Personnel.FirstOrDefault(p => p.Email == email);
            if (personal != null)
            {
                Session["UserRole"] = personal.Role;
                Session["UserID"] = personal.Id;
                Session["UserName"] = personal.Fio;
                return;
            }

            var client = db.Clients.FirstOrDefault(c => c.Email == email);
            if (client != null)
            {
                Session["UserRole"] = client.Role;
                Session["UserID"] = client.Id;
                Session["UserName"] = client.Name;
                return;
            }
        }

        private string ComputeHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}