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
            Session["UserID"] = null;
            Session["UserRole"] = null;
            Session["UserName"] = null;

            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            Response.Redirect("~/");
        }
    }
} 