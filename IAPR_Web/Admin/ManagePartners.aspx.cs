using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using AT = IAPR_Data.Classes.AssetTypes;
using CP = IAPR_Data.Classes.Policy;
using CCom = IAPR_Data.Classes.Common;
using P = IAPR_Data.Providers;

namespace IAPR_Web.Admin
{
    public partial class ManagePartners : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            CloseAll();
            switch (ddlFunction.SelectedValue)
            {
                case "1":
                    pnlAddPartner.Visible = true;
                    AddPartner1.Visible = true;
                    break;
                case "2":
                    pnlAddPartnerUser.Visible = true;
                    AddPartnerUser1.Visible = true;
                    break;
                case "3":
                    EditPartnerUser1.GetFormFields();
                    pnlEditPartnerUser.Visible = true;
                    break;
                case "4":
                    EditPartner.GetFormFields();
                    pnlEditPartner.Visible = true;
                    break;
            }



        }

        private void CloseAll()
        {
            pnlAddPartner.Visible = false;
            pnlAddPartnerUser.Visible = false;
            pnlEditPartner.Visible = false;
            pnlEditPartnerUser.Visible = false;
        }
    }
}