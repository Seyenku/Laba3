using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Web.UI;

namespace Laba3
{
    public partial class DirectLogin : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear error message
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
                    // Set authentication cookie
                    FormsAuthentication.SetAuthCookie(email, RememberMe.Checked);

                    // Redirect to the requested URL or default page
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
                    // Display error message
                    ErrorText.Text = "Invalid email or password.";
                    errorMessage.Visible = true;
                }
            }
        }

        private bool ValidateUser(string email, string password)
        {
            bool isValid = false;
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT password_hash FROM personal WHERE email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        string storedHash = reader["password_hash"].ToString();

                        string computedHash = ComputeHash(password);

                        isValid = storedHash.Equals(computedHash, StringComparison.OrdinalIgnoreCase);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ErrorText.Text = "An error occurred during login. Please try again later.";
                    errorMessage.Visible = true;
                }
            }

            return isValid;
        }

        private string ComputeHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {

                // Convert to byte array and compute hash
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Convert byte array to hex string
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