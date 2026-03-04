using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using IAPR_Web.Models;
using C = IAPR_Data.Classes;
using P = IAPR_Data.Providers;

namespace IAPR_Web.Account
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpdatePassword_Click(object sender, EventArgs e)
        {
            C.Common.CurrentUser objUser = new C.Common.CurrentUser();
            P.User_Provider uP = new P.User_Provider();

            objUser = uP.GetUserFromSession();
            if (objUser == null)
            {
                Response.Redirect("/account/login.aspx");
            }
            else
            {
                if (uP.ChangePassword(objUser.iUser_Id, objUser.vcUsername, txtPassword.Text))
                {


                    switch (objUser.iUser_Type_Id)
                    {
                        case 1:
                        case 2:
                            Response.Redirect("/AdminHome.aspx");
                            break;
                        case 3:
                        case 4:
                            Response.Redirect("/FinancerHome.aspx");
                            break;
                        case 5:
                        case 6:
                            Response.Redirect("/InsurerHome.aspx");
                            break;
                    }
                }
            }
        }
    }
}