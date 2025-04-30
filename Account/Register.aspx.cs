using Laba3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Laba3.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClientPanel.Visible = true;
            }
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var db = new ApplicationDbContext();

                var client = new Client
                {
                    Name = ClientName.Text,
                    Address = Address.Text,
                    Telephone = Telephone.Text,
                    Email = Email.Text,
                    PasswordHash = ComputeSha256Hash(Password.Text),
                    Role = "CLIENT"
                };

                db.Clients.Add(client);
                db.SaveChanges();

                RegisterIdentityUser();

                Response.Redirect("~/Account/Login.aspx");
            }
        }

        private void RegisterIdentityUser()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);

            if (!result.Succeeded)
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
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