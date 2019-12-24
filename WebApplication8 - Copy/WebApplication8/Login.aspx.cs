using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication8
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string jsMethodName = "Message('Invalid','error')";
            if (!string.IsNullOrEmpty(txtPass.Text) && !string.IsNullOrEmpty(txtUser.Text))
            {
                jsMethodName = "Message('Success','success')";
            }
            else
            {
                jsMethodName = "Message('Empty','error')";
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "uniqueKey", jsMethodName, true);
        }
    }
}