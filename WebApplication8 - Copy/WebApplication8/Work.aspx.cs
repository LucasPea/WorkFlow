using System;
using System.Web;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Cookies;

namespace WebApplication8
{
    public partial class Work : System.Web.UI.Page
    {
        public string acc_token;
        public string subscription;
        public int type;
        protected void Page_Load(object sender, EventArgs e)
        {
            SignIn();
            TokenClass tk = new TokenClass();
            acc_token = tk.GetToken();
            subscription = tk.GetSubscription(acc_token);
            type = 5;
            //if (Session["Type"] != null)
            //{
            //    TokenClass tk = new TokenClass();
            //    acc_token = tk.GetToken();
            //    subscription = tk.GetSubscription(acc_token);
            //    type = (int)Session["Type"];
            //}
            //else
            //{
            //    Response.Redirect("Login.aspx");
            //}
        }

        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                
                HttpContext.Current.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        /// <summary>
        /// Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);
        }
    }
}