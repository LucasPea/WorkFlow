using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication8
{
    public partial class Work : System.Web.UI.Page
    {
        public string acc_token;
        public string subscription;
        protected void Page_Load(object sender, EventArgs e)
        {
            TokenClass tk = new TokenClass();
            acc_token=tk.GetToken();
            subscription = tk.GetSubscription(acc_token);
        }
    }
}