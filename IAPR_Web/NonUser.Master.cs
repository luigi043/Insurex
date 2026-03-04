using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IAPR_Web
{
    public partial class NonUser : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "formatCurrencyTextBox()", true);


            string _dataTableScript = @"$('#dataTable').DataTable();";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "mydataTable", _dataTableScript, true);


            string _selectPickerScript = @"$('.selectpicker').selectpicker();";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "myselectPicker", _selectPickerScript, true);

        }
    }
}