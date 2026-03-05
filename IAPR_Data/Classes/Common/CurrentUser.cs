using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAPR_Data.Classes.Common
{
    public class CurrentUser
    {
        public int iUser_Id { get; set; }
        public int iUser_Type_Id { get; set; }
        public string vcUser_Type_Description { get; set; }
        public string vcUsername { get; set; }
        public string vcContactNumber { get; set; }
        public string vcName { get; set; }
        public string vcSurname { get; set; }
        public int iUser_Status_Id { get; set; }
        public string vcUser_Status_Description { get; set; }
        public int iPartner_Type_Id { get; set; }
        public string vcPartner_Type_Description { get; set; }

        public int iPartner_Id { get; set; }
        public int iPartner_Package_Id { get; set; }
        public string vcPartner_Name { get; set; }
        public string vcPassword { get; set; }
        public string vcPosition_Title { get; set; }
        public bool bUserReceiveNotifications { get; set; }
        public string vcPartnerLogo { get; set; }

    }
}







