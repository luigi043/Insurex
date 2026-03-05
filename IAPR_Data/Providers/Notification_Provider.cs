using IAPR_Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAPR_Data.Classes;
using System.Configuration;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using Microsoft.Data.SqlClient;

using System.Net;
using System.Net.Mail;
using U = IAPR_Data.Utils;
using IAPR_Data.Services;
namespace IAPR_Data.Providers
{
    public class Notification_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public void SendMailNotification(List<string> receipientAddress, string eMailSubject, string eMailbody)
        {
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());
            smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            NetworkCredential networkCredential = new NetworkCredential(ConfigurationManager.AppSettings["SMTPServerAccount"].ToString(), ConfigurationManager.AppSettings["SMTPServerPassword"].ToString());
            smtpClient.Credentials = (ICredentialsByHost)networkCredential;
            try
            {
                foreach (string email in receipientAddress)
                {
                    message.To.Add(email);
                }

                message.From = new MailAddress("notifications@IAPR.com", "IAPR.com");
                message.Subject = eMailSubject;
                message.IsBodyHtml = true;
                StringBuilder stringBuilder = new StringBuilder();
                message.Body = eMailbody.ToString();
                message.IsBodyHtml = true;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                message.Priority = MailPriority.High;
                smtpClient.Send(message);
            }
            catch (SmtpException ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                if (ex.StatusCode == SmtpStatusCode.InsufficientStorage)
                    smtpClient.Send(message);
                else
                    //U.ErrorLogger eL = new U.ErrorLogger();
                    eL.LogErrorInDB(ex, "Notification_Provider", "Save_New_Vehicle_Asset");
            }
            finally
            {
                message.Dispose();
            }

        }
        public void SendMailNotificationCSV(MailMessage message, string receipientAddress, string eMailSubject)
        {

            SmtpClient smtpClient = new SmtpClient(ConfigurationManager.AppSettings["SMTPServer"].ToString());
            smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
            NetworkCredential networkCredential = new NetworkCredential(ConfigurationManager.AppSettings["SMTPServerAccount"].ToString(), ConfigurationManager.AppSettings["SMTPServerPassword"].ToString());
            smtpClient.Credentials = (ICredentialsByHost)networkCredential;
            try
            {
                //foreach (string email in receipientAddress)
                //{
                message.To.Add(receipientAddress);
                //}

                message.From = new MailAddress("notifications@IAPR.com", "IAPR.com");
                message.Subject = eMailSubject;
                message.IsBodyHtml = true;
                StringBuilder stringBuilder = new StringBuilder();
                //message.Body = eMailbody.ToString();
                message.IsBodyHtml = true;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                message.Priority = MailPriority.High;
                smtpClient.Send(message);


                //*******************************************







            }
            catch (SmtpException ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                if (ex.StatusCode == SmtpStatusCode.InsufficientStorage)
                    smtpClient.Send(message);
                else
                    //U.ErrorLogger eL = new U.ErrorLogger();
                    eL.LogErrorInDB(ex, "Notification_Provider", "Save_New_Vehicle_Asset");
            }
            finally
            {
                message.Dispose();
            }

        }

        private DataSet Get_Notification_Emails_For_Policy_NonPayment(int ipolicy_Id, int ipolicy_Transaction_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();

            try
            {


                SqlCommand cmd = new SqlCommand("dbo.spGet_Financier_Notification_Details_NonPayment", sqlConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@iPolicy_Id", SqlDbType.Int).Value = ipolicy_Id;
                cmd.Parameters.Add("@ipolicy_Transaction_Id", SqlDbType.Int).Value = ipolicy_Transaction_Id;
                sqlConn.Open();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                sqlConn.Close();

                return ds;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public bool NotifyNewUser(string receipientName, string receipientAddress, string partnerName, string changePasswordLink, string password, string messageType)

        {
            bool sent = false;
            StringBuilder messagebody = GetMessageBody(messageType);
            string finalMessageBody = messagebody.ToString();
            finalMessageBody = finalMessageBody.Replace("{0}", receipientName);// string.Format(finalMessageBody, receipientName, registrationConfirmationURL);
            finalMessageBody = finalMessageBody.Replace("{1}", partnerName);
            finalMessageBody = finalMessageBody.Replace("{2}", changePasswordLink);
            finalMessageBody = finalMessageBody.Replace("{3}", password);

            DeliverMailToUser(receipientAddress, "InsureX user registration", finalMessageBody);
            return sent;
        }

        public bool Customer_Confirm_Policy_Details(string receipientName, string receipientAddress, string partnerName, string Link, string messageType)

        {
            bool sent = false;
            StringBuilder messagebody = GetMessageBody(messageType);
            string finalMessageBody = messagebody.ToString();
            finalMessageBody = finalMessageBody.Replace("{0}", receipientName);// string.Format(finalMessageBody, receipientName, registrationConfirmationURL);
            finalMessageBody = finalMessageBody.Replace("{1}", partnerName);
            finalMessageBody = finalMessageBody.Replace("{2}", Link);


            DeliverMailToUser(receipientAddress, "InsureX asset policy details confirmation", finalMessageBody);
            return sent;
        }
        public bool Customer_Confirm_Policy_Details_After_Rejection(string receipientName, string receipientAddress, string partnerName, string Link, string messageType)

        {
            bool sent = false;
            StringBuilder messagebody = GetMessageBody(messageType);
            string finalMessageBody = messagebody.ToString();
            finalMessageBody = finalMessageBody.Replace("{0}", receipientName);// string.Format(finalMessageBody, receipientName, registrationConfirmationURL);
            finalMessageBody = finalMessageBody.Replace("{1}", partnerName);
            finalMessageBody = finalMessageBody.Replace("{2}", Link);


            DeliverMailToUser(receipientAddress, "InsureX asset policy details confirmation", finalMessageBody);
            return sent;
        }

        public bool Customer_NonPayment_Notification_Personal(string receipientName, string receipientAddress,
            string financer, string policyNumber,
            string affectedPeriod, string affectedYear, string assetType, string assetSubType, string assetIdentifierDetails, string messageType,
            string insurer)

        {

            bool sent = false;
            StringBuilder messagebody = GetMessageBody(messageType);
            string finalMessageBody = messagebody.ToString();
            finalMessageBody = finalMessageBody.Replace("{0}", receipientName);// string.Format(finalMessageBody, receipientName, registrationConfirmationURL);
            finalMessageBody = finalMessageBody.Replace("{1}", financer);
            finalMessageBody = finalMessageBody.Replace("{2}", policyNumber);

            finalMessageBody = finalMessageBody.Replace("{3}", affectedPeriod);
            finalMessageBody = finalMessageBody.Replace("{4}", affectedYear);
            finalMessageBody = finalMessageBody.Replace("{5}", assetType);
            finalMessageBody = finalMessageBody.Replace("{6}", assetSubType);
            finalMessageBody = finalMessageBody.Replace("{7}", assetIdentifierDetails);
            finalMessageBody = finalMessageBody.Replace("{8}", insurer);

            DeliverMailToUser(receipientAddress, "Notice from " + financer, finalMessageBody);
            return sent;
        }

        public bool Customer_NonPayment_Notification_Business(string receipientName, string receipientAddress,
            string partnerName, string policyNumber,
            string affectedPeriod, string affectedYear, string assetType, string assetSubType, string assetIdentifierDetails, string messageType
            )

        {

            bool sent = false;
            StringBuilder messagebody = GetMessageBody(messageType);
            string finalMessageBody = messagebody.ToString();
            //finalMessageBody = finalMessageBody.Replace("{0}", receipientName);// string.Format(finalMessageBody, receipientName, registrationConfirmationURL);
            finalMessageBody = finalMessageBody.Replace("{1}", partnerName);
            finalMessageBody = finalMessageBody.Replace("{2}", policyNumber);

            finalMessageBody = finalMessageBody.Replace("{3}", affectedPeriod);
            finalMessageBody = finalMessageBody.Replace("{4}", affectedYear);
            finalMessageBody = finalMessageBody.Replace("{5}", assetType);
            finalMessageBody = finalMessageBody.Replace("{6}", assetSubType);
            finalMessageBody = finalMessageBody.Replace("{7}", assetIdentifierDetails);

            DeliverMailToUser(receipientAddress, "Notice from " + partnerName, finalMessageBody);
            return sent;
        }

        public bool Send_Policy_Notification_NonPayment(int ipolicy_Id, int ipolicy_Transaction_Id)
        {
            bool sent = false;
            try
            {
                DataSet ds = Get_Notification_Emails_For_Policy_NonPayment(ipolicy_Id, ipolicy_Transaction_Id);


                //Loop for eack asst under the policy
                foreach (DataRow assetRow in ds.Tables[0].Rows)
                {
                    //Add All financier emails 
                    List<string> emails = new List<string>();
                    foreach (DataRow receipientRow in ds.Tables[1].Rows)
                    {
                        if (assetRow[2].ToString() == receipientRow[1].ToString())
                        {
                            emails.Add(receipientRow[4].ToString());
                        }
                    }

                    string body = ds.Tables[2].Rows[0][3].ToString() + "<br/> Affected period: " + ds.Tables[2].Rows[0][5].ToString() + "<br/>Affected year : " + ds.Tables[2].Rows[0][6].ToString();
                    body = body + "<br/><br/><b>Asset details</b>";
                    body = body + "<br/>Asset type : " + ds.Tables[0].Rows[0][6].ToString();
                    body = body + "<br/>Vehicle type : " + ds.Tables[0].Rows[0][8].ToString();
                    body = body + "<br/>Vin number : " + ds.Tables[0].Rows[0][11].ToString();

                    body = body + "<br/><br/><b>Insurance policy holder details </b>";
                    body = body + "<br/>Name : " + ds.Tables[3].Rows[0][1].ToString();
                    body = body + "<br/>Contact number : " + ds.Tables[3].Rows[0][2].ToString();
                    body = body + "<br/>Contact email : " + ds.Tables[3].Rows[0][3].ToString();
                    // SendMailNotification(emails, "Insurance status update: Ref: Finance agreement: " + assetRow[4].ToString(), body);
                    // ddlInsuranceCompanies.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                }


            }
            catch (Exception)
            {

                throw;
            }


            return sent;


        }


        public bool Password_Reminder_Request(string receipientAddress, string changePasswordLink, string messageType)
        {
            bool sent = false;
            StringBuilder messagebody = GetMessageBody(messageType);
            string finalMessageBody = messagebody.ToString();
            finalMessageBody = finalMessageBody.Replace("{0}", changePasswordLink);// string.Format(finalMessageBody, receipientName, registrationConfirmationURL);



            DeliverMailToUser(receipientAddress, "InsureX password reminder request", finalMessageBody);
            return sent;




        }
        public bool Password_Reminder_Confirm(string receipientAddress, string changePasswordLink, string messageType)
        {
            bool sent = false;
            StringBuilder messagebody = GetMessageBody(messageType);
            string finalMessageBody = messagebody.ToString();
            finalMessageBody = finalMessageBody.Replace("{0}", U.CryptorEngine.ValidationDecrypt(changePasswordLink, true));// string.Format(finalMessageBody, receipientName, registrationConfirmationURL);



            DeliverMailToUser(U.CryptorEngine.ValidationDecrypt(receipientAddress, true), "InsureX password reminder request", finalMessageBody);
            return sent;



        }
        public bool ContactUsFormToSupport(string senderName, string senderAddress, string mailSubject, string messageBody)
        {
            bool bMailSent = false;
            try
            {


                DeliverMailToSupport(senderAddress, senderAddress, mailSubject, messageBody);
            }
            catch (Exception ex)
            {

                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "", "");
            }
            return bMailSent;
        }
        public bool ContactUsFormConfirm(string messageType, string receipientAddress, string receipientName, string mailSubject)
        {
            bool bMailSent = false;
            try
            {

                StringBuilder messagebody = GetMessageBody(messageType);
                string finalMessageBody = messagebody.ToString();
                finalMessageBody = finalMessageBody.Replace("{0}", receipientName);// string.Format(finalMessageBody, receipientName, registrationConfirmationURL);

                DeliverMailToUser(receipientAddress, mailSubject, finalMessageBody);
            }
            catch (Exception ex)
            {

                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "", "");
            }
            return bMailSent;
        }


        private StringBuilder GetMessageBody(string messageType)
        {



            StringBuilder messageBody = new StringBuilder();
            try
            {


                switch (messageType)
                {
                    case "NewUser":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\NewUser.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;
                    case "CustomerConfirmPolicyDetails":

                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\CustomerConfirmPolicyDetails.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;
                    case "CustomerConfirmPolicyDetailsAfterRejection":

                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\CustomerConfirmPolicyDetailsAFterRejection.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;

                    case "CustomerNonPaymentPersonal":

                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\CustomerNonPaymentPersonal_Notification.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;

                    case "CustomerNonPaymentBusiness":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\CustomerNonPaymentBusiness_Notification.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;

                    case "PreGameNotification":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\Pre_game_advice.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;
                    case "TeamKnockedOut":
                        using (StreamReader reader = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\team_knocked_out.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;
                    case "TeamPassedRound":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\TeamPassedRound.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;
                    case "PasswordReminderRequest":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\PasswordReminder.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;
                    case "PasswordReminderConfirm":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\PasswordReminderConfirm.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }

                        break;
                    case "AccountReactivationRequest":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\AccountReactivationRequest.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }
                        break;
                    case "AccountReactivationConfirm":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\AccountReactivationConfirm.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }
                        break;
                    case "AccountEmailUpdate":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\AccountEmailUpdate.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }
                        break;
                    case "DeRegisterAccount":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\DeRegisterAccount.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }
                        break;

                    case "ContactUsFormConfirm":
                        using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\MailTemplates\\ContactUsFormConfirm.html"))//System.Web.// ((System.Web.HttpContext)null) /* HttpContext.Current legacy */ /* Legacy incompatibility */.Server.MapPath("~/NewRegistration.html")))
                        {
                            messageBody.Append(reader.ReadToEnd());
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "", "");
            }
            return messageBody;
        }
        private StringBuilder GetMailWrapper()
        {
            StringBuilder messageWrapper = new StringBuilder();
            messageWrapper.Append("<html>");
            messageWrapper.Append("<head>");
            messageWrapper.Append("<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />");

            messageWrapper.Append("</head>");

            messageWrapper.Append("<body style='margin: 0' bgcolor='#4d4d4d'>");
            messageWrapper.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#4d4d4d' style='border-top: 2px solid #febf00'>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td>&nbsp;</td>");
            messageWrapper.Append("<td width='50%'>");
            messageWrapper.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td>");
            messageWrapper.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td>");
            messageWrapper.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td>");
            messageWrapper.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td width='26'>");
            messageWrapper.Append("<img src='http://www.ticcets.com/images/mailer/generic_02_01_01.jpg' width='26' height='129' style='display: block' /></td>");
            messageWrapper.Append("<td bgcolor='#dddddd'>");
            messageWrapper.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td height='10'></td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td style='font-family: Arial, sans-serif; font-size: 30px; font-weight: bold; color: #c8000a; line-height: 36px; letter-spacing: -1px; -webkit-text-size-adjust: none'>Ticcets.com</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td style='font-family: Arial, sans-serif; font-size: 30px; font-weight: bold; color: #373737; line-height: 36px; letter-spacing: -1px; -webkit-text-size-adjust: none'>For the ultimate access to the Championships</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("</table>");
            messageWrapper.Append("</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("</table>");
            messageWrapper.Append("</td>");
            messageWrapper.Append("</tr>");

            messageWrapper.Append("</table>");
            messageWrapper.Append("</td>");
            messageWrapper.Append("<td>");
            messageWrapper.Append("<img src='http://www.ticcets.com/images/mailer/F1.jpg' height='130' style='display: block' /></td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("</table>");
            messageWrapper.Append("</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr style='background-color: whitesmoke;'>");
            messageWrapper.Append("<td></td>");
            messageWrapper.Append("</tr>");

            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td>");
            messageWrapper.Append("<img src='http://www.ticcets.com/images/mailer/generic_02_03_05.jpg' width='100%' height='17' style='display: block' /></td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td>");
            messageWrapper.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td width='28' valign='top'>");
            messageWrapper.Append("<img src='http://www.ticcets.com/images/mailer/yellownote_mail_11.jpg' width='28' height='410' style='display: block' /></td>");
            messageWrapper.Append("<td valign='top' bgcolor='#FFFFFF'>");
            messageWrapper.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td height='28'></td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td style='font-family: Arial, sans-serif; font-size: 15px; color: #7d7d7d; line-height: 17px; -webkit-text-size-adjust: none;'>");
            messageWrapper.Append("{0}");
            messageWrapper.Append("</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td height='25'></td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td style='font-family: Arial, sans-serif; font-size: 15px; color: #7d7d7d; line-height: 17px; -webkit-text-size-adjust: none;'>Regards,<br />");
            messageWrapper.Append("<b>The Team at Ticcets</b><br />");
            messageWrapper.Append("</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("</table>");
            messageWrapper.Append("</td>");
            messageWrapper.Append("<td width='27' valign='top'>");
            messageWrapper.Append("<img src='http://www.ticcets.com/images/mailer/yellownote_mail_12.jpg' width='27' height='410' style='display: block' /></td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("</table>");
            messageWrapper.Append("</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td>");
            messageWrapper.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0' style='padding: 0px 4px 0px 4px;'>");
            messageWrapper.Append("<tr style='background-color: white;'>");
            messageWrapper.Append("<td style='padding-left: 1%; width: 7%;'><a href='#' target='_blank'>");
            messageWrapper.Append("<img src='http://www.ticcets.com/images/mailer/twitter1.jpg' alt='' border='0' style='display: block; height: 35px;' /></a></td>");
            messageWrapper.Append("<td bgcolor='#FFFFFF'><a href='#' target='_blank'>");
            messageWrapper.Append("<img src='http://www.ticcets.com/images/mailer/facebook1.jpg' alt='' border='0' style='display: block; height: 35px;' /></a></td>");
            messageWrapper.Append("<td align='right' bgcolor='#FFFFFF'>");
            messageWrapper.Append("<img src='http://www.ticcets.com/images/mailer/Ticcets_Logo1.png' alt='' width='113' height='69' style='display: block' /></td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("</table>");
            messageWrapper.Append("</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td style='font-family: Arial, sans-serif; font-size: 12px; color: #7d7d7d; line-height: 18px; -webkit-text-size-adjust: none'>Copyright © 2016 Ticcets. All rights reserved</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("<tr>");
            messageWrapper.Append("<td style='font-family: Arial, sans-serif; font-size: 12px; color: #7d7d7d; line-height: 18px; -webkit-text-size-adjust: none'>You are receiving this mail as you previously agreed to receive further correspondence from Ticcets. <a href='https://www.ticcets.com' target='_blank' style='color: #ff8900; font-weight: bold;'>unsubscribe from this list</a>");
            messageWrapper.Append("<br />");
            messageWrapper.Append("<a href='https://www.ticcets.com' target='_blank' style='color: #ff8900; font-weight: bold;'>update subscription preferences</a> </td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("</table>");
            messageWrapper.Append("</td>");
            messageWrapper.Append("<td>;&nbsp;</td>");
            messageWrapper.Append("</tr>");
            messageWrapper.Append("</table>");
            messageWrapper.Append("</body>");
            messageWrapper.Append("</html>");
            return messageWrapper;
        }
        public void DeliverMailToUser(string receipientAddress, string eMailSubject, string eMailbody)
        {
            // Delegate to the secure NotificationService singleton (shared SmtpClient + retry logic)
            NotificationService.Instance.Send(receipientAddress, eMailSubject, eMailbody);
        }
        public void DeliverMailToSupport(string senderName, string senderAddress, string eMailSubject, string eMailbody)
        {
            // Delegate to the secure NotificationService singleton
            NotificationService.Instance.SendToSupport(eMailSubject, eMailbody);
        }

        public bool SendCartJson(string receipientAddress, string json)
        {
            bool bMailSent = false;
            try
            {

                DeliverMailToUser(receipientAddress, "Cart jason", json);
            }
            catch (Exception ex)
            {
                U.ErrorLogger eL = new U.ErrorLogger();
                eL.LogErrorInDB(ex, "", "");
            }
            return bMailSent;
        }

        public bool SendAllAwaitingNotifications()
        {
            bool completed = false;

            Policy_Provider pPro = new Policy_Provider();
            SqlDataReader dr = pPro.Get_Asset_Communications_Awaiting_Processing();
            Policy_Provider pro = new Policy_Provider();
            while (dr.Read())
            {
                DataSet ds = pro.Get_Policy_Holder_All_Assets(Convert.ToInt32(dr["iPolicy_Id"].ToString()));
                DateTime date = new DateTime(Convert.ToInt32(dr["iAffected_Year"].ToString()), Convert.ToInt32(dr["iAffected_Month"].ToString()), 1);

                string insurer = string.Empty;
                string policyTypeId = string.Empty;
                string receipientName = string.Empty;
                string receipientAddress = string.Empty;
                string partnerName = string.Empty;
                string policyNumber = string.Empty;


                string assetType = string.Empty;
                string assetSubType = string.Empty;
                string assetIdentifierDetails = string.Empty;

                insurer = ds.Tables[0].Rows[0]["Insurer"].ToString();
                policyTypeId = ds.Tables[0].Rows[0]["iPolicy_Type_Id"].ToString();
                receipientName = policyTypeId == "1" ? ds.Tables[1].Rows[0]["Firstname"].ToString() : "";
                receipientAddress = ds.Tables[1].Rows[0]["Email address"].ToString();// : ds.Tables[0].Rows[1]["Email address"].ToString();
                policyNumber = ds.Tables[0].Rows[0][1].ToString();

                foreach (DataRow row in ds.Tables[4].Rows)
                {
                    partnerName = row["Financer"].ToString();
                    assetType = row["vcAsset_Type_Description"].ToString();
                    assetSubType = row["vcAsset_Sub_Type_Description"].ToString();
                    if (row["Make/Model/Description"].ToString() != "")
                    {
                        assetIdentifierDetails = "<p>" + assetIdentifierDetails + ds.Tables[4].Columns["Make/Model/Description"].ColumnName + ": " + row["Make/Model/Description"].ToString() + " </p>";
                    }
                    if (row["Asset Identifier"].ToString() != "")
                    {
                        assetIdentifierDetails = "<p>" + assetIdentifierDetails + ds.Tables[4].Columns["Asset Identifier"].ColumnName + ": " + row["Asset Identifier"].ToString() + " </p>";
                    }

                    Customer_NonPayment_Notification_Personal(receipientName, receipientAddress,
                        partnerName, policyNumber, date.ToString("MMMM"), dr["iAffected_Year"].ToString(), assetType,
                        assetSubType, assetIdentifierDetails, "CustomerNonPaymentPersonal", insurer);
                    assetIdentifierDetails = "";

                    pro.Close_Asset_Communications_Awaiting_Processing(Convert.ToInt64(dr["Id"].ToString()));
                }
            }

            return completed;
        }

    }
}
////Loop through the columns and rows and add a comma to the end for the csv.
//foreach (DataColumn column in ds.Tables[0].Columns)
//{
//    sw.Write(column.ColumnName.ToString() + ",");
//}

//sw.WriteLine();

//foreach (DataRow row in ds.Tables[0].Rows)
//{
//    for (int i = 0; i < row.ItemArray.Length; i++)
//    {
//        string rowText = row.ItemArray[i].ToString();
//        if (rowText.Contains(","))
//        {
//            rowText = rowText.Replace(",", "/");
//        }

//        sw.Write(rowText + ",");
//    }
//    sw.WriteLine();
//}
////sw.Flush();
////return mem;




//////StreamWriter sw = new StreamWriter(mem);
//////headers    
////for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
////{
////    sw.Write(ds.Tables[0].Columns[i]);
////    if (i < ds.Tables[0].Columns.Count - 1)
////    {
////        sw.Write(",");
////    }
////}
////sw.Write(sw.NewLine);
////foreach (DataRow dr in ds.Tables[0].Rows)
////{
////    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
////    {
////        if (!Convert.IsDBNull(dr[i]))
////        {
////            string value = dr[i].ToString();
////            if (value.Contains(','))
////            {
////                value = String.Format("\"{0}\"", value);
////                sw.Write(value);
////            }
////            else
////            {
////                sw.Write(dr[i].ToString());
////            }
////        }
////        if (i < ds.Tables[0].Columns.Count - 1)
////        {
////            sw.Write(",");
////        }
////    }
////    sw.Write(sw.NewLine);

////}







