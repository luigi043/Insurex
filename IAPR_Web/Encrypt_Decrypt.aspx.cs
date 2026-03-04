using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using U = IAPR_Data.Utils;
namespace IAPR_Web
{
    public partial class Encrypt_Decrypt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnEncryptGen_Click(object sender, EventArgs e)
        {
            lblEncryptedAnswerGen.Text = U.CryptorEngine.GenericEncrypt(txtPLainTextGen.Text, true);
        }

        protected void btnDecryptGen_Click(object sender, EventArgs e)
        {
            lblDecryptedAnswerGen.Text = U.CryptorEngine.GenericDecrypt(txtEncryptedGen.Text, true);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            lblEncryptedAnswerVal.Text = U.CryptorEngine.ValidationEncrypt(txtPLainTextVal.Text, true);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            lblDecryptedAnswerVal.Text = U.CryptorEngine.ValidationDecrypt(txtEncryptedVal.Text, true);
        }
    }
}