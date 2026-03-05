using IAPR_Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAPR_Data.Classes;
using System.Configuration;
using System.Net;
using System.IO;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using C = IAPR_Data.Classes;
using U = IAPR_Data.Utils;
namespace IAPR_Data.Providers
{
    public class User_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());


        public C.Common.CurrentUser ValidateUser(string pUsername, string pUserpassword)
        {

            C.Common.CurrentUser f_User = new C.Common.CurrentUser();
            AuthenticateUser(pUsername, pUserpassword, out f_User);



            if (f_User != null)
            {

                if (f_User.iUser_Status_Id == 1)
                {
                    switch (f_User.iUser_Type_Id)
                    {
                        case 1:
                            f_User = AppendUserDetails(f_User);
                            //AddUserToSession(f_User);
                            break;
                        case 2:
                        case 3:

                        case 4:
                        case 5:
                        case 6:
                            f_User = AppendUserDetails(f_User);

                            break;


                    }

                }

            }

            // No longer using session state for CurrentUser. The OWIN claims cookie will hold the state.
            return f_User;

        }
        public C.Common.CurrentUser GetUserFromSession()
        {
            try
            {
                var claimsIdentity = (System.Security.Claims.ClaimsIdentity)null; /* HttpContext.Current legacy incompatibility */
                if (claimsIdentity != null && claimsIdentity.IsAuthenticated)
                {
                    var user = new C.Common.CurrentUser();
                    
                    var idClaim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (int.TryParse(idClaim, out int userId)) { user.iUser_Id = userId; }
                    
                    user.vcUsername = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                    
                    var userTypeClaim = claimsIdentity.FindFirst("iUser_Type_Id")?.Value;
                    if (int.TryParse(userTypeClaim, out int userTypeId)) { user.iUser_Type_Id = userTypeId; }
                    
                    user.vcName = claimsIdentity.FindFirst("vcName")?.Value;
                    user.vcSurname = claimsIdentity.FindFirst("vcSurname")?.Value;
                    
                    var partnerClaim = claimsIdentity.FindFirst("iPartner_Id")?.Value;
                    if (int.TryParse(partnerClaim, out int partnerId)) { user.iPartner_Id = partnerId; }
                    
                    var partnerTypeClaim = claimsIdentity.FindFirst("iPartner_Type_Id")?.Value;
                    if (int.TryParse(partnerTypeClaim, out int partnerTypeId)) { user.iPartner_Type_Id = partnerTypeId; }
                    
                    var userStatusClaim = claimsIdentity.FindFirst("iUser_Status_Id")?.Value;
                    if (int.TryParse(userStatusClaim, out int userStatusId)) { user.iUser_Status_Id = userStatusId; }

                    if (user.iUser_Id > 0)
                    {
                        return user;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private void AuthenticateUser(string userName, string password, out C.Common.CurrentUser p_User)
        {
            p_User = null;
            using (var dbContext = C.ApplicationDbContext.Create(System.Configuration.ConfigurationManager.ConnectionStrings["connIAPRData"].ToString()))
            {
                var user = dbContext.Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    var hasher = new PasswordHasher<C.ApplicationUser>();
                    var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
                    
                    if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        p_User = new C.Common.CurrentUser
                        {
                            iUser_Id = user.LegacyUserId,
                            iUser_Type_Id = user.iUser_Type_Id,
                            vcUsername = user.UserName,
                            iUser_Status_Id = user.iUser_Status_Id,
                            vcUser_Status_Description = user.vcUser_Status_Description,
                            vcName = user.vcName,
                            vcSurname = user.vcSurname,
                            iPartner_Type_Id = user.iPartner_Type_Id ?? 0,
                            iPartner_Id = user.iPartner_Id ?? 0,
                            vcPosition_Title = user.vcPosition_Title,
                            bUserReceiveNotifications = user.bUserReceiveNotifications
                        };
                    }
                }
            }
        }

        public C.Common.CurrentUser AppendUserDetails(C.Common.CurrentUser objDBUser)
        {


            SqlParameter[] arParams = new SqlParameter[2];
            arParams[0] = new SqlParameter("@iPartner_Id", objDBUser.iPartner_Id);
            arParams[1] = new SqlParameter("@iPartner_Type_Id", objDBUser.iPartner_Type_Id);



            DataSet ds = SqlHelper.ExecuteDataset(sqlConn, CommandType.StoredProcedure, "dbo.spGet_AppendUserDeatils", arParams);

            foreach (DataTable oTable in ds.Tables)
            {
                foreach (DataRow oRow in oTable.Rows)
                {
                    if (oRow["vcPartner_Name"] != DBNull.Value) objDBUser.vcPartner_Name = U.CryptorEngine.GenericDecrypt(oRow["vcPartner_Name"].ToString(), true);
                    if (oRow["iPartner_Package_Id"] != DBNull.Value) objDBUser.iPartner_Package_Id = Convert.ToInt32(oRow["iPartner_Package_Id"].ToString());
                    if (oRow["vcPartner_Logo"] != DBNull.Value) objDBUser.vcPartnerLogo = oRow["vcPartner_Logo"].ToString();
                    

                }
            }




            return objDBUser;

        }

        public bool ChangePassword(int iUser_Id, string vcUsername, string vcPassword)
        {
            using (var dbContext = C.ApplicationDbContext.Create(System.Configuration.ConfigurationManager.ConnectionStrings["connIAPRData"].ToString()))
            {
                var user = dbContext.Users.FirstOrDefault(u => u.UserName == vcUsername);
                if (user != null)
                {
                    var hasher = new PasswordHasher<C.ApplicationUser>();
                    user.PasswordHash = hasher.HashPassword(user, vcPassword);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        // GetCurrentUser was removed because Session state is not accessible here in .NET 8.
        // User identity is managed via JWT claims in the API Controllers.
        /*
        public C.Common.CurrentUser GetCurrentUser()
        {
            if (Session["CurrentUser"] == null)
            {
                P.User_Provider uP = new P.User_Provider();
                var objUser = uP.ValidateUser("admin@gmail.com", "password12");
                if (objUser != null)
                {
                    Session["CurrentUser"] = objUser;
                }
            }
            return (C.Common.CurrentUser)Session["CurrentUser"];
        }
        */


        public List<C.Common.CurrentUser> Get_Partner_Users(int iPartner_Id, int iPartner_Type_Id)
        {

            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();


            SqlCommand cmd = new SqlCommand("dbo.spGet_Partner_Users", sqlConn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@iPartner_Id", SqlDbType.Int).Value = iPartner_Id;
            cmd.Parameters.Add("@iPartner_Type_Id", SqlDbType.Int).Value = iPartner_Type_Id;

            sqlConn.Open();
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            sqlConn.Close();


            List<C.Common.CurrentUser> usersL = new List<C.Common.CurrentUser>();
            usersL = (from DataRow dr in ds.Tables[0].Rows
                      select new C.Common.CurrentUser()
                      {
                          iUser_Id = dr["iUser_Id"] != DBNull.Value ? Convert.ToInt32(dr["iUser_Id"].ToString()) : 0,
                          iUser_Type_Id = dr["iUser_Type_Id"] != DBNull.Value ? Convert.ToInt32(dr["iUser_Type_Id"].ToString()) : 0,
                          vcUser_Type_Description = dr["iUser_Type_Description"] != DBNull.Value ? dr["iUser_Type_Description"].ToString() : "",
                          vcName = dr["vcName"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["vcName"].ToString(), true) : "",
                          vcSurname = dr["vcSurname"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["vcSurname"].ToString(), true) : "",
                          vcContactNumber = dr["vcContactNumber"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["vcContactNumber"].ToString(), true) : "",
                          iPartner_Type_Id = dr["iPartner_Type_Id"] != DBNull.Value ? Convert.ToInt32(dr["iPartner_Type_Id"].ToString()) : 0,
                          vcPartner_Type_Description = dr["vcPartner_Type_Description"] != DBNull.Value ? dr["vcPartner_Type_Description"].ToString() : "",
                          iPartner_Id = dr["iPartner_Id"] != DBNull.Value ? Convert.ToInt32(dr["iPartner_Id"].ToString()) : 0,
                          vcPosition_Title = dr["vcPosition_Title"] != DBNull.Value ? U.CryptorEngine.GenericDecrypt(dr["vcPosition_Title"].ToString(), true) : "",
                          vcUsername = dr["vcUsername"] != DBNull.Value ? U.CryptorEngine.ValidationDecrypt(dr["vcUsername"].ToString(), true) : "",
                          iUser_Status_Id = dr["iUser_Status_Id"] != DBNull.Value ? Convert.ToInt32(dr["iUser_Status_Id"].ToString()) : 0,
                          vcUser_Status_Description = dr["vcUser_Status_Description"] != DBNull.Value ? dr["vcUser_Status_Description"].ToString() : "",
                          bUserReceiveNotifications = dr["bUserReceiveNotifications"] != DBNull.Value ? Convert.ToBoolean(dr["bUserReceiveNotifications"].ToString()) : false,
                      }).ToList();


            return usersL;

        }
        public string Update_User(Classes.Common.CurrentUser u)
        {
            string pw = Security_Provider.GeneratePassword(10);
            SqlDataAdapter da = new SqlDataAdapter();

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@iUser_Id", u.iUser_Id),
                new SqlParameter("@vcName",U.CryptorEngine.GenericEncrypt(u.vcName,true)),
                new SqlParameter("@vcSurname",U.CryptorEngine.GenericEncrypt(u.vcSurname,true)),
                new SqlParameter("@vcPosition_Title",U.CryptorEngine.GenericEncrypt(u.vcPosition_Title,true)),

                new SqlParameter("@vcContactNumber",U.CryptorEngine.GenericEncrypt(u.vcContactNumber,true)),
                new SqlParameter("@iUser_Status_Id", u.iUser_Status_Id),
                new SqlParameter("@bUserReceiveNotifications",u.bUserReceiveNotifications)
        };

            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
            "spUpd_User", parameters);



            return pw;
        }

        public SqlDataReader Get_User_Password_Reminder_Details(string vcUserName)
        {
            SqlDataAdapter da = new SqlDataAdapter();

            SqlParameter[] parameters = new SqlParameter[]
            {

                new SqlParameter("@vcUsername", U.CryptorEngine.ValidationEncrypt(vcUserName.ToLower(),true))

        };

            return (SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
             "spGet_User_Password_Reminder_Details", parameters));
        }

        public SqlDataReader Get_User_Password_Reminder(int @iPassword_reminder_Id, string vcGUID)
        {
            string pw = Security_Provider.GeneratePassword(10);

            SqlDataAdapter da = new SqlDataAdapter();

            SqlParameter[] parameters = new SqlParameter[]
            {

                   new SqlParameter("@iPassword_reminder_Id", @iPassword_reminder_Id),
               new SqlParameter("@vcGUID",  vcGUID),


        };

            return (SqlHelper.ExecuteReader(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
             "spGet_User_Password_Reminder", parameters));

        }
    }
}








