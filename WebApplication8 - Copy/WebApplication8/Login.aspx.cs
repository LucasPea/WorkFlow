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
            Session["Type"] = null;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string jsMethodName = "Message('Invalid','error')";
            if (!string.IsNullOrEmpty(txtPass.Text) && !string.IsNullOrEmpty(txtUser.Text))
            {
                object obj = new object();
                obj = DBConnection.SqlReturn("SELECT Type FROM Users where Name='" + txtUser.Text + "' and Password='" + txtPass.Text + "'");
                if (obj != null)
                {
                    obj=obj.ToString().Replace(" ", string.Empty);
                    int type = Int32.Parse(obj.ToString());
                    Session["Type"] = type;
                    Response.Redirect("Work.aspx");
                }                
            }
            else
            {
                jsMethodName = "Message('Empty','error')";
            }
            ScriptManager.RegisterClientScriptBlock(this, typeof(string), "uniqueKey", jsMethodName, true);
        }
    }
}