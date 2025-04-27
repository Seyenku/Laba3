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
                StaffPanel.Visible = false;
            }
        }

        protected void UserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UserType.SelectedValue == "CLIENT")
            {
                ClientPanel.Visible = true;
                StaffPanel.Visible = false;
            }
            else
            {
                ClientPanel.Visible = false;
                StaffPanel.Visible = true;
            }
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var db = new ApplicationDbContext();

                if (UserType.SelectedValue == "CLIENT")
                {
                    // Register as client
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

                    // Also register in ASP.NET Identity for authentication
                    RegisterIdentityUser();

                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    // Register as IT staff
                    var staff = new Personal
                    {
                        Fio = StaffName.Text,
                        Staff = Position.Text,
                        Email = Email.Text,
                        PasswordHash = ComputeSha256Hash(Password.Text),
                        Role = "IT_STAFF"
                    };

                    db.Personnel.Add(staff);
                    db.SaveChanges();

                    // Also register in ASP.NET Identity for authentication
                    RegisterIdentityUser();

                    Response.Redirect("~/Default.aspx");
                }
            }
        }

        private void RegisterIdentityUser()
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            IdentityResult result = manager.Create(user, Password.Text);

            if (result.Succeeded)
            {
                signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
            }
            else
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