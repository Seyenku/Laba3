using Microsoft.AspNet.Identity;
using System;
using System.Web;
using System.Web.UI;

namespace Laba3.Account
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear session variables
            Session["UserID"] = null;
            Session["UserRole"] = null;
            Session["UserName"] = null;

            // Sign out from ASP.NET Identity
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            // Redirect to home page
            Response.Redirect("~/");
        }
    }
} 