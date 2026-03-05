using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using System.Data;
using Microsoft.Data.SqlClient;

using C = IAPR_Data.Classes;
using U = IAPR_Data.Utils;
using System.Reflection;

namespace IAPR_Data.Classes.Common
{
    public static class ConvertToDataTable
    {
        public static DataTable ConvertListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static DataSet DecrypDBtField(DataSet ds, int tNumber, int cNumber)
        {
            DataSet dsOut = new DataSet();

            foreach (DataRow r in ds.Tables[tNumber].Rows)
            {
                r[cNumber] = U.CryptorEngine.GenericDecrypt(r[cNumber].ToString(), true);
            }
            dsOut = ds;
            return dsOut;
        }
        public static DataSet DecrypDBtField(DataSet ds, int tNumber, int[] cNumber)
        {
            DataSet dsOut = new DataSet();

            foreach (DataRow r in ds.Tables[tNumber].Rows)
            {
                foreach (int c in cNumber)
                {
                    r[c] = U.CryptorEngine.GenericDecrypt(r[c].ToString(), true);
                }
            }
            dsOut = ds;
            return dsOut;
        }
        public static DataSet DecrypDBtField(DataSet ds, int tNumber, string[] cName)
        {
            DataSet dsOut = new DataSet();

            foreach (DataRow r in ds.Tables[tNumber].Rows)
            {
                foreach (DataColumn dc in r.Table.Columns)
                    foreach (string c in cName)
                    {
                        if (dc.ColumnName == c)
                        {
                            r[dc] = r[dc] != DBNull.Value && r[dc].ToString() != "null" ? U.CryptorEngine.GenericDecrypt(r[dc].ToString(), true) : "Unconfirmed";

                            //if (r[dc].ToString != DBNull.Value)
                            //{
                            //    r[dc] = U.CryptorEngine.GenericDecrypt(r[dc].ToString(), true);
                            //}

                        }
                    }

            }
            dsOut = ds;
            return dsOut;
        }
        public static DataTable ConvertTODataTable_Policy_Nonpayment(IEnumerable<C.Policy.PolicyNonPaymentRequest.PolicyDetails> policy_details, int trasactionId)
        {

            var dtPolicyDetails = new DataTable();
            dtPolicyDetails.Columns.Add("iCounterID", typeof(Int32));
            dtPolicyDetails.Columns.Add("iTransadtionID", typeof(Int32));
            dtPolicyDetails.Columns.Add("vcPolicy_Number", typeof(String));
            dtPolicyDetails.Columns.Add("iAffected_Year", typeof(Int32));
            dtPolicyDetails.Columns.Add("vcAffected_Period", typeof(String));
            dtPolicyDetails.Columns.Add("dtDateOfNonPayment", typeof(DateTime));
            int c = 1;
            foreach (C.Policy.PolicyNonPaymentRequest.PolicyDetails i in policy_details)
            {
                DataRow workRow = dtPolicyDetails.NewRow();
                workRow["iCounterID"] = c;
                workRow["iTransadtionID"] = trasactionId;
                workRow["vcPolicy_Number"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                workRow["iAffected_Year"] = i.affectedYear;
                workRow["vcAffected_Period"] = i.affectedPeriod;
                workRow["dtDateOfNonPayment"] = i.dateOfNonPayment;
                dtPolicyDetails.Rows.Add(workRow);

                c++;
            }

            return dtPolicyDetails;
        }
        public static DataTable ConvertTODataTable_Policy_Status(IEnumerable<C.Policy.PolicyStatusRequest.PolicyDetails> policy_details, int trasactionId)
        {

            var dtPolicyDetails = new DataTable();
            dtPolicyDetails.Columns.Add("iCounterID", typeof(Int32));
            dtPolicyDetails.Columns.Add("iTransadtionID", typeof(Int32));
            dtPolicyDetails.Columns.Add("vcPolicy_Number", typeof(String));
            dtPolicyDetails.Columns.Add("newStatus", typeof(String));
            dtPolicyDetails.Columns.Add("dateStatusChanged", typeof(String));
            int c = 1;
            foreach (C.Policy.PolicyStatusRequest.PolicyDetails i in policy_details)
            {
                DataRow workRow = dtPolicyDetails.NewRow();
                workRow["iCounterID"] = c;
                workRow["iTransadtionID"] = trasactionId;
                workRow["vcPolicy_Number"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                workRow["newStatus"] = i.newStatus;
                workRow["dateStatusChanged"] = i.dateStatusChanged;
                dtPolicyDetails.Rows.Add(workRow);

                c++;
            }

            return dtPolicyDetails;
        }

        public static DataTable ConvertTODataTable_Asset_Finance_Value(IEnumerable<C.AssetTypes.UpdateAssetFinanceValueRequest.AssetDetails> policy_details, int trasactionId)
        {

            var dtPolicyDetails = new DataTable();
            dtPolicyDetails.Columns.Add("iCounterID", typeof(Int32));
            dtPolicyDetails.Columns.Add("iTransadtionID", typeof(String));
            dtPolicyDetails.Columns.Add("vcAssetType", typeof(String));
            dtPolicyDetails.Columns.Add("vcFinanceAgreementNumber", typeof(String));
            dtPolicyDetails.Columns.Add("dcFinanceValue", typeof(decimal));
            int c = 1;
            foreach (C.AssetTypes.UpdateAssetFinanceValueRequest.AssetDetails i in policy_details)
            {
                DataRow workRow = dtPolicyDetails.NewRow();
                workRow["iCounterID"] = c;
                workRow["iTransadtionID"] = trasactionId;
                workRow["vcAssetType"] = i.assetType;
                workRow["vcFinanceAgreementNumber"] = U.CryptorEngine.GenericEncrypt(i.financeAgreementNumber, true);
                workRow["dcFinanceValue"] = i.financeValue;
                dtPolicyDetails.Rows.Add(workRow);

                c++;
            }

            return dtPolicyDetails;
        }
        public static DataTable ConvertTODataTable_Generic(IEnumerable<C.AssetTypes.UpdateAssetInsuredValueRequest.VehicleAssets> lottrLotteryWinners)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(C.AssetTypes.UpdateAssetInsuredValueRequest.VehicleAssets).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in lottrLotteryWinners)
            {
                var datos = new C.AssetTypes.UpdateAssetInsuredValueRequest.VehicleAssets();
                datos = item;
                DataRow row = dataTable.NewRow();

                foreach (var property in datos.GetType().GetProperties())
                {
                    row[property.Name] = property.GetValue(datos, null);
                }
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        #region ConvertInsuredValue
        public static DataTable ConvertTODataTable_InsuredRequest_Vehicle(IEnumerable<C.AssetTypes.UpdateAssetInsuredValueRequest.VehicleAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("vinNumber", typeof(String));
            dt.Columns.Add("newInsuredValue", typeof(decimal));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetInsuredValueRequest.VehicleAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["vinNumber"] = U.CryptorEngine.GenericEncrypt(i.vinNumber, true);
                    workRow["newInsuredValue"] = i.newInsuredValue;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }

            return dt;
        }
        public static DataTable ConvertTODataTable_InsuredRequest_Property(IEnumerable<C.AssetTypes.UpdateAssetInsuredValueRequest.PropertyAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("standNumber_ERFPortion", typeof(String));
            dt.Columns.Add("sectionalTitleNumber", typeof(String));
            dt.Columns.Add("sectionalTitleName", typeof(String));
            dt.Columns.Add("newInsuredValue", typeof(decimal));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetInsuredValueRequest.PropertyAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["standNumber_ERFPortion"] = U.CryptorEngine.GenericEncrypt(i.standNumber_ERFPortion, true);
                    workRow["sectionalTitleNumber"] = U.CryptorEngine.GenericEncrypt(i.sectionalTitleNumber, true);

                    workRow["sectionalTitleName"] = U.CryptorEngine.GenericEncrypt(i.sectionalTitleName, true);
                    workRow["newInsuredValue"] = i.newInsuredValue;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_InsuredRequest_Watercraft(IEnumerable<C.AssetTypes.UpdateAssetInsuredValueRequest.WatercraftAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("identificationNumber", typeof(String));
            dt.Columns.Add("newInsuredValue", typeof(decimal));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetInsuredValueRequest.WatercraftAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["identificationNumber"] = U.CryptorEngine.GenericEncrypt(i.identificationNumber, true);
                    workRow["newInsuredValue"] = i.newInsuredValue;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }
        public static DataTable ConvertTODataTable_InsuredRequest_Aviation(IEnumerable<C.AssetTypes.UpdateAssetInsuredValueRequest.AviationtAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("tailNumber", typeof(String));
            dt.Columns.Add("newInsuredValue", typeof(decimal));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetInsuredValueRequest.AviationtAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["tailNumber"] = U.CryptorEngine.GenericEncrypt(i.tailNumber, true);
                    workRow["newInsuredValue"] = i.newInsuredValue;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }
        public static DataTable ConvertTODataTable_InsuredRequest_Machinery(IEnumerable<C.AssetTypes.UpdateAssetInsuredValueRequest.MachineryAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("serialNumber", typeof(String));
            dt.Columns.Add("newInsuredValue", typeof(decimal));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetInsuredValueRequest.MachineryAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["serialNumber"] = U.CryptorEngine.GenericEncrypt(i.serialNumber, true);
                    workRow["newInsuredValue"] = i.newInsuredValue;
                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_InsuredRequest_PlantEquipment(IEnumerable<C.AssetTypes.UpdateAssetInsuredValueRequest.PlantEquipmentAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("identificationNumber", typeof(String));
            dt.Columns.Add("serialNumber", typeof(String));
            dt.Columns.Add("newInsuredValue", typeof(decimal));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetInsuredValueRequest.PlantEquipmentAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["identificationNumber"] = U.CryptorEngine.GenericEncrypt(i.identificationNumber, true);
                    workRow["serialNumber"] = U.CryptorEngine.GenericEncrypt(i.serialNumber, true);
                    workRow["newInsuredValue"] = i.newInsuredValue;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_InsuredRequest_ElectronicEquipment(IEnumerable<C.AssetTypes.UpdateAssetInsuredValueRequest.ElectronicEquipmentAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("serialNumber", typeof(String));
            dt.Columns.Add("newInsuredValue", typeof(decimal));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetInsuredValueRequest.ElectronicEquipmentAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["serialNumber"] = U.CryptorEngine.GenericEncrypt(i.serialNumber, true);
                    workRow["newInsuredValue"] = i.newInsuredValue;
                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }
        #endregion

        #region ConvertAssetCover
        public static DataTable ConvertTODataTable_CoverRequest_Vehicle(IEnumerable<C.AssetTypes.UpdateAssetCoverRequest.VehicleAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("vinNumber", typeof(String));
            dt.Columns.Add("newCover", typeof(String));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetCoverRequest.VehicleAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["vinNumber"] = U.CryptorEngine.GenericEncrypt(i.vinNumber, true);
                    workRow["newCover"] = i.newAssetCover;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }

            return dt;
        }
        public static DataTable ConvertTODataTable_CoverRequest_Property(IEnumerable<C.AssetTypes.UpdateAssetCoverRequest.PropertyAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("standNumber_ERFPortion", typeof(String));
            dt.Columns.Add("sectionalTitleNumber", typeof(String));
            dt.Columns.Add("sectionalTitleName", typeof(String));
            dt.Columns.Add("newCover", typeof(String));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetCoverRequest.PropertyAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["standNumber_ERFPortion"] = U.CryptorEngine.GenericEncrypt(i.standNumber_ERFPortion, true);
                    workRow["sectionalTitleNumber"] = U.CryptorEngine.GenericEncrypt(i.sectionalTitleNumber, true);

                    workRow["sectionalTitleName"] = U.CryptorEngine.GenericEncrypt(i.sectionalTitleName, true);
                    workRow["newCover"] = i.newAssetCover;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_CoverRequest_Watercraft(IEnumerable<C.AssetTypes.UpdateAssetCoverRequest.WatercraftAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("identificationNumber", typeof(String));
            dt.Columns.Add("newCover", typeof(String));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetCoverRequest.WatercraftAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["identificationNumber"] = U.CryptorEngine.GenericEncrypt(i.identificationNumber, true);
                    workRow["newCover"] = i.newAssetCover;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }
        public static DataTable ConvertTODataTable_CoverRequest_Aviation(IEnumerable<C.AssetTypes.UpdateAssetCoverRequest.AviationtAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("tailNumber", typeof(String));
            dt.Columns.Add("newCover", typeof(String));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetCoverRequest.AviationtAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["tailNumber"] = U.CryptorEngine.GenericEncrypt(i.tailNumber, true);
                    workRow["newCover"] = i.newAssetCover;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }
        public static DataTable ConvertTODataTable_CoverRequest_Machinery(IEnumerable<C.AssetTypes.UpdateAssetCoverRequest.MachineryAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("serialNumber", typeof(String));
            dt.Columns.Add("newCover", typeof(String));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetCoverRequest.MachineryAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["serialNumber"] = U.CryptorEngine.GenericEncrypt(i.serialNumber, true);
                    workRow["newCover"] = i.newAssetCover;
                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_CoverRequest_PlantEquipment(IEnumerable<C.AssetTypes.UpdateAssetCoverRequest.PlantEquipmentAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("identificationNumber", typeof(String));
            dt.Columns.Add("serialNumber", typeof(String));
            dt.Columns.Add("newCover", typeof(String));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetCoverRequest.PlantEquipmentAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["identificationNumber"] = U.CryptorEngine.GenericEncrypt(i.identificationNumber, true);
                    workRow["serialNumber"] = U.CryptorEngine.GenericEncrypt(i.serialNumber, true);
                    workRow["newCover"] = i.newAssetCover;
                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_CoverRequest_ElectronicEquipment(IEnumerable<C.AssetTypes.UpdateAssetCoverRequest.ElectronicEquipmentAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("serialNumber", typeof(String));
            dt.Columns.Add("newCover", typeof(String));
            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.UpdateAssetCoverRequest.ElectronicEquipmentAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["serialNumber"] = U.CryptorEngine.GenericEncrypt(i.serialNumber, true);
                    workRow["newCover"] = i.newAssetCover;
                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }
        #endregion

        #region RemoveAsset
        public static DataTable ConvertTODataTable_RemoveAsset_Vehicle(IEnumerable<C.Policy.RemoveAssetRequest.VehicleAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("vinNumber", typeof(String));


            if (asset_details != null)
            {
                int c = 1;
                foreach (C.Policy.RemoveAssetRequest.VehicleAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["vinNumber"] = U.CryptorEngine.GenericEncrypt(i.vinNumber, true);

                    dt.Rows.Add(workRow);

                    c++;
                }
            }

            return dt;
        }
        public static DataTable ConvertTODataTable_RemoveAsset_Property(IEnumerable<C.Policy.RemoveAssetRequest.PropertyAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("standNumber_ERFPortion", typeof(String));
            dt.Columns.Add("sectionalTitleNumber", typeof(String));
            dt.Columns.Add("sectionalTitleName", typeof(String));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.Policy.RemoveAssetRequest.PropertyAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["standNumber_ERFPortion"] = U.CryptorEngine.GenericEncrypt(i.standNumber_ERFPortion, true);
                    workRow["sectionalTitleNumber"] = U.CryptorEngine.GenericEncrypt(i.sectionalTitleNumber, true);

                    workRow["sectionalTitleName"] = U.CryptorEngine.GenericEncrypt(i.sectionalTitleName, true);

                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_RemoveAsset_Watercraft(IEnumerable<C.Policy.RemoveAssetRequest.WatercraftAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("identificationNumber", typeof(String));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.Policy.RemoveAssetRequest.WatercraftAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["identificationNumber"] = U.CryptorEngine.GenericEncrypt(i.identificationNumber, true);

                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }
        public static DataTable ConvertTODataTable_RemoveAsset_Aviation(IEnumerable<C.Policy.RemoveAssetRequest.AviationtAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("tailNumber", typeof(String));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.Policy.RemoveAssetRequest.AviationtAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["tailNumber"] = U.CryptorEngine.GenericEncrypt(i.tailNumber, true);

                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }
        public static DataTable ConvertTODataTable_RemoveAsset_Machinery(IEnumerable<C.Policy.RemoveAssetRequest.MachineryAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("serialNumber", typeof(String));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.Policy.RemoveAssetRequest.MachineryAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["serialNumber"] = U.CryptorEngine.GenericEncrypt(i.serialNumber, true);

                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_RemoveAsset_PlantEquipment(IEnumerable<C.Policy.RemoveAssetRequest.PlantEquipmentAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("identificationNumber", typeof(String));
            dt.Columns.Add("serialNumber", typeof(String));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.Policy.RemoveAssetRequest.PlantEquipmentAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["identificationNumber"] = U.CryptorEngine.GenericEncrypt(i.identificationNumber, true);
                    workRow["serialNumber"] = U.CryptorEngine.GenericEncrypt(i.serialNumber, true);

                    dt.Rows.Add(workRow);

                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_RemoveAsset_ElectronicEquipment(IEnumerable<C.Policy.RemoveAssetRequest.ElectronicEquipmentAssets> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("iTransadtionID", typeof(String));
            dt.Columns.Add("policyNumber", typeof(String));
            dt.Columns.Add("serialNumber", typeof(String));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.Policy.RemoveAssetRequest.ElectronicEquipmentAssets i in asset_details)
                {
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["iTransadtionID"] = trasactionId;
                    workRow["policyNumber"] = U.CryptorEngine.GenericEncrypt(i.policyNumber, true);
                    workRow["serialNumber"] = U.CryptorEngine.GenericEncrypt(i.serialNumber, true);
                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }
        #endregion


        #region NewAssetByAPI
        //Consumer Customer
        public static DataTable ConvertTODataTable_Consumer_Customers(int iFinancer_Id,
           IEnumerable<C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.ConsumerVehicles> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("vcTrasactionId", typeof(String));
            dt.Columns.Add("iIdentification_Type_Id", typeof(Int32));
            dt.Columns.Add("iPerson_Title_Id", typeof(Int32));
            dt.Columns.Add("vcFirst_Names", typeof(String));
            dt.Columns.Add("vcSurname", typeof(String));
            dt.Columns.Add("vcIdentification_Number", typeof(String));
            dt.Columns.Add("bPostalAddresSameAsPhysical", typeof(Boolean));
            dt.Columns.Add("vcContact_Number", typeof(String));
            dt.Columns.Add("vcAlternative_Contact_Number", typeof(String));
            dt.Columns.Add("vcEmail_Address", typeof(String));
            dt.Columns.Add("vcBuilding_Unit", typeof(String));
            dt.Columns.Add("vcAddress_Line_1", typeof(String));
            dt.Columns.Add("vcAddress_Line_2", typeof(String));
            dt.Columns.Add("vcSuburb", typeof(String));
            dt.Columns.Add("vcCity", typeof(String));
            dt.Columns.Add("vcPostal_Code", typeof(String));
            dt.Columns.Add("iProvince_Id", typeof(Int32));
            dt.Columns.Add("vcPOBox_Bag", typeof(String));
            dt.Columns.Add("vcPost_Office_Name", typeof(String));
            dt.Columns.Add("vcPostalPostal_Code", typeof(String));



            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.ConsumerVehicles i in asset_details)
                {
                    string linkGuid = Guid.NewGuid().ToString();
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;
                    workRow["vcTrasactionId"] = trasactionId;
                    workRow["iIdentification_Type_Id"] = (int)Enum.Parse(typeof(Common.Identification_Types), i.consumerDetails.identificationType);
                    workRow["iPerson_Title_Id"] = (int)Enum.Parse(typeof(Common.Consumer_Titles), i.consumerDetails.consumerTitle);// i.consumerDetails.personTitleId;
                    workRow["vcFirst_Names"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.firstNames, true);
                    workRow["vcSurname"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.surname, true);
                    workRow["vcIdentification_Number"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.identificationNumber, true);
                    workRow["bPostalAddresSameAsPhysical"] = i.consumerDetails.postalAddresSameAsPhysical;
                    workRow["vcContact_Number"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.CellPhoneNumber, true);
                    workRow["vcAlternative_Contact_Number"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.alternativeContactNumber, true);
                    workRow["vcEmail_Address"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.emailAddress, true);
                    workRow["vcBuilding_Unit"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.physicalAddress.buildingUnit, true);
                    workRow["vcAddress_Line_1"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.physicalAddress.addressLine1, true);
                    workRow["vcAddress_Line_2"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.physicalAddress.addressLine2, true);
                    workRow["vcSuburb"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.physicalAddress.suburb, true);
                    workRow["vcCity"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.physicalAddress.city, true);
                    workRow["vcPostal_Code"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.physicalAddress.postalCode, true);
                    workRow["iProvince_Id"] = i.consumerDetails.physicalAddress.provinceId;
                    workRow["vcPOBox_Bag"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.postalAddress.pOBoxBag, true);
                    workRow["vcPost_Office_Name"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.postalAddress.postOfficeName, true);
                    workRow["vcPostalPostal_Code"] = U.CryptorEngine.GenericEncrypt(i.consumerDetails.postalAddress.postPostalCode, true);


                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }
        //Business Customer
        public static DataTable ConvertTODataTable_Business_Customers(int iFinancer_Id,
           IEnumerable<C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.BusinessVehicles> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("vcTrasactionId", typeof(String));
            dt.Columns.Add("vcBusiness_Name", typeof(String));
            dt.Columns.Add("vcBusiness_Registration_Number", typeof(String));
            dt.Columns.Add("vcBusiness_Contact_Fullname", typeof(String));
            dt.Columns.Add("vcBusiness_Contact_Number", typeof(String));
            dt.Columns.Add("vcBusiness_Contact_Alternative_Number", typeof(String));
            dt.Columns.Add("vcBusiness_Email_Address", typeof(String));
            dt.Columns.Add("bPostalAddresSameAsPhysical", typeof(Boolean));
            dt.Columns.Add("vcBuilding_Unit", typeof(String));
            dt.Columns.Add("vcAddress_Line_1", typeof(String));
            dt.Columns.Add("vcAddress_Line_2", typeof(String));
            dt.Columns.Add("vcSuburb", typeof(String));
            dt.Columns.Add("vcCity", typeof(String));
            dt.Columns.Add("vcPostal_Code", typeof(String));
            dt.Columns.Add("iProvince_Id", typeof(Int32));
            dt.Columns.Add("vcPOBox_Bag", typeof(String));
            dt.Columns.Add("vcPost_Office_Name", typeof(String));
            dt.Columns.Add("vcPostalPostal_Code", typeof(String));


            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.BusinessVehicles i in asset_details)
                {
                    string linkGuid = Guid.NewGuid().ToString();
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;

                    workRow["vcTrasactionId"] = trasactionId;
                    workRow["vcBusiness_Name"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.businessName, true);
                    workRow["vcBusiness_Registration_Number"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.registrationNumber, true);
                    workRow["vcBusiness_Contact_Fullname"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.contactFullname, true);
                    workRow["vcBusiness_Contact_Number"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.contactCellPhoneNumber, true);
                    workRow["vcBusiness_Contact_Alternative_Number"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.contactAlternativeNumber, true);
                    workRow["vcBusiness_Email_Address"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.contactEmailAddress, true);
                    workRow["bPostalAddresSameAsPhysical"] = i.businessDetails.postalAddresSameAsPhysical;
                    workRow["vcBuilding_Unit"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.physicalAddress.buildingUnit, true);
                    workRow["vcAddress_Line_1"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.physicalAddress.addressLine1, true);
                    workRow["vcAddress_Line_2"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.physicalAddress.addressLine2, true);
                    workRow["vcSuburb"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.physicalAddress.suburb, true);
                    workRow["vcCity"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.physicalAddress.city, true);
                    workRow["vcPostal_Code"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.physicalAddress.postalCode, true);
                    workRow["iProvince_Id"] = i.businessDetails.physicalAddress.provinceId;
                    workRow["vcPOBox_Bag"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.postalAddress.pOBoxBag, true);
                    workRow["vcPost_Office_Name"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.postalAddress.postOfficeName, true);
                    workRow["vcPostalPostal_Code"] = U.CryptorEngine.GenericEncrypt(i.businessDetails.postalAddress.postPostalCode, true);
                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }


        //Vehicle
        public static DataTable ConvertTODataTable_NewAssetByAPI_Vehicle_Consumer(int iFinancer_Id,
            IEnumerable<C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.ConsumerVehicles> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("vcTrasactionId", typeof(String));
            dt.Columns.Add("iFinancer_Id", typeof(Int32));
            dt.Columns.Add("vcFinance_Agrreement_Number", typeof(String));
            dt.Columns.Add("mAsset_Finance_Value", typeof(Decimal));
            dt.Columns.Add("iAsset_Type", typeof(Int32));
            dt.Columns.Add("vcVehicle_Asset_Type", typeof(String));
            dt.Columns.Add("iVehicle_Asset_Licence_Type_Id", typeof(Int32));
            dt.Columns.Add("vcAsset_Usage_Type", typeof(string));
            dt.Columns.Add("iAsset_Condition_Id", typeof(Int32));
            dt.Columns.Add("vcVehicle_Make_Description", typeof(String));
            dt.Columns.Add("vcVehicle_Model_Description", typeof(String));
            dt.Columns.Add("vcVehicle_Model_Variant_Description", typeof(String));
            dt.Columns.Add("vcVin_Number", typeof(String));
            dt.Columns.Add("vcRegistration_Number", typeof(String));
            dt.Columns.Add("iModel_Year", typeof(Int32));
            dt.Columns.Add("dtFinance_Start_Date", typeof(String));
            dt.Columns.Add("dtFinance_End_Date", typeof(String));
            dt.Columns.Add("vcVehicle_Color", typeof(String));
            dt.Columns.Add("vcLinkKey", typeof(String));
            dt.Columns.Add("bInsuranceDetailsAvalable", typeof(Boolean));
            dt.Columns.Add("vcInsurerName", typeof(String));
            dt.Columns.Add("vcPolicyType", typeof(String));
            dt.Columns.Add("vcPolicyNumber", typeof(String));
            dt.Columns.Add("vcAssetCoverType", typeof(String));
            dt.Columns.Add("vcPremiumFrequency", typeof(String));
            dt.Columns.Add("dInsuranceValue", typeof(Decimal));

            //var a = (Common.Cover_Types)Common.GetEnumFromDescription("3rd Party Only", typeof(Common.Cover_Types));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.ConsumerVehicles i in asset_details)
                {
                    string linkGuid = Guid.NewGuid().ToString();
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;

                    workRow["vcTrasactionId"] = trasactionId;
                    workRow["iFinancer_Id"] = iFinancer_Id;
                    workRow["vcFinance_Agrreement_Number"] = U.CryptorEngine.GenericEncrypt(i.vehicleDetails.financeAgreementNumber, true);
                    workRow["mAsset_Finance_Value"] = i.vehicleDetails.assetFinanceValue;
                    workRow["iAsset_Type_Id"] = 1;
                    workRow["vcVehicle_Asset_Type"] = i.vehicleDetails.vehicleType;
                    workRow["iVehicle_Asset_Licence_Type_Id"] = i.vehicleDetails.vehicleAssetLicenceTypeId;
                    workRow["vcAsset_Usage_Type"] = i.vehicleDetails.assetUsage;
                    workRow["vcAsset_Condition"] = i.vehicleDetails.assetCondition;
                    workRow["vcVehicle_Make_Description"] = i.vehicleDetails.vehicleMakeDescription;
                    workRow["vcVehicle_Model_Description"] = i.vehicleDetails.vehicleModelDescription;
                    workRow["vcVehicle_Model_Variant_Description"] = i.vehicleDetails.vehicleModelVariantDescription;
                    workRow["vcVin_Number"] = U.CryptorEngine.GenericEncrypt(i.vehicleDetails.vinNumber, true);
                    workRow["vcRegistration_Number"] = U.CryptorEngine.GenericEncrypt(i.vehicleDetails.registrationNumber, true);
                    workRow["iModel_Year"] = i.vehicleDetails.modelYear;
                    workRow["dtFinance_Start_Date"] = i.vehicleDetails.financeStartDate;
                    workRow["dtFinance_End_Date"] = i.vehicleDetails.financeEndDate;
                    workRow["vcVehicle_Color"] = "";// i.vehicleDetails.vehicleColor;
                    workRow["vcLinkKey"] = linkGuid;

                    workRow["bInsuranceDetailsAvalable"] = i.insuranceDetailsAvalable;
                    workRow["vcInsurerName"] = i.insuranceDetails.insurerName;
                    workRow["vcPolicyType"] = i.insuranceDetails.policyType;
                    workRow["vcPolicyNumber"] = i.insuranceDetails.policyNumber;
                    workRow["vcAssetCoverType"] = i.insuranceDetails.coverType;
                    workRow["vcPremiumFrequency"] = i.insuranceDetails.premiumFrequency;
                    workRow["dInsuranceValue"] = (decimal)i.insuranceDetails.insuranceValue;
                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }

        public static DataTable ConvertTODataTable_NewAssetByAPI_Vehicle_Business(int iFinancer_Id,
           IEnumerable<C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.BusinessVehicles> asset_details, int trasactionId)
        {

            var dt = new DataTable();
            dt.Columns.Add("iCounterID", typeof(Int32));
            dt.Columns.Add("vcTrasactionId", typeof(String));
            dt.Columns.Add("iFinancer_Id", typeof(Int32));
            dt.Columns.Add("vcFinance_Agrreement_Number", typeof(String));
            dt.Columns.Add("mAsset_Finance_Value", typeof(Decimal));
            dt.Columns.Add("vcAsset_Type", typeof(String));
            dt.Columns.Add("iVehicle_Asset_Type_Id", typeof(Int32));
            dt.Columns.Add("iVehicle_Asset_Licence_Type_Id", typeof(Int32));
            dt.Columns.Add("vcAsset_Usage_Type", typeof(string));
            dt.Columns.Add("vcAsset_Condition", typeof(String));
            dt.Columns.Add("vcVehicle_Make_Description", typeof(String));
            dt.Columns.Add("vcVehicle_Model_Description", typeof(String));
            dt.Columns.Add("vcVehicle_Model_Variant_Description", typeof(String));
            dt.Columns.Add("vcVin_Number", typeof(String));
            dt.Columns.Add("vcRegistration_Number", typeof(String));
            dt.Columns.Add("iModel_Year", typeof(Int32));
            dt.Columns.Add("dtFinance_Start_Date", typeof(String));
            dt.Columns.Add("dtFinance_End_Date", typeof(String));
            dt.Columns.Add("vcVehicle_Color", typeof(String));
            dt.Columns.Add("vcLinkKey", typeof(String));

            dt.Columns.Add("bInsuranceDetailsAvalable", typeof(Boolean));
            dt.Columns.Add("vcInsurerName", typeof(String));
            dt.Columns.Add("vcPolicyType", typeof(String));
            dt.Columns.Add("vcPolicyNumber", typeof(String));
            dt.Columns.Add("vcAssetCoverType", typeof(String));
            dt.Columns.Add("vcPremiumFrequency", typeof(String));
            dt.Columns.Add("dInsuranceValue", typeof(Decimal));

            if (asset_details != null)
            {
                int c = 1;
                foreach (C.AssetTypes.API_NewAssets.addVehicleAssetsRequest.BusinessVehicles i in asset_details)
                {
                    string linkGuid = Guid.NewGuid().ToString();
                    DataRow workRow = dt.NewRow();
                    workRow["iCounterID"] = c;

                    workRow["vcTrasactionId"] = trasactionId;
                    workRow["iFinancer_Id"] = iFinancer_Id;
                    workRow["vcFinance_Agrreement_Number"] = U.CryptorEngine.GenericEncrypt(i.vehicleDetails.financeAgreementNumber, true);
                    workRow["mAsset_Finance_Value"] = i.vehicleDetails.assetFinanceValue;
                    workRow["iAsset_Type_Id"] = 1;
                    workRow["vcVehicle_Asset_Type"] = i.vehicleDetails.vehicleType;
                    workRow["iVehicle_Asset_Licence_Type_Id"] = i.vehicleDetails.vehicleAssetLicenceTypeId;
                    workRow["vcAsset_Usage_Type"] = i.vehicleDetails.assetUsage;
                    workRow["vcAsset_Condition"] = i.vehicleDetails.assetCondition;
                    workRow["vcVehicle_Make_Description"] = i.vehicleDetails.vehicleMakeDescription;
                    workRow["vcVehicle_Model_Description"] = i.vehicleDetails.vehicleModelDescription;
                    workRow["vcVehicle_Model_Variant_Description"] = i.vehicleDetails.vehicleModelVariantDescription;
                    workRow["vcVin_Number"] = U.CryptorEngine.GenericEncrypt(i.vehicleDetails.vinNumber, true);
                    workRow["vcRegistration_Number"] = U.CryptorEngine.GenericEncrypt(i.vehicleDetails.registrationNumber, true);
                    workRow["iModel_Year"] = i.vehicleDetails.modelYear;
                    workRow["dtFinance_Start_Date"] = i.vehicleDetails.financeStartDate;
                    workRow["dtFinance_End_Date"] = i.vehicleDetails.financeEndDate;
                    workRow["vcVehicle_Color"] = "";// i.vehicleDetails.vehicleColor;
                    workRow["vcLinkKey"] = linkGuid;

                    workRow["bInsuranceDetailsAvalable"] = i.insuranceDetailsAvalable;
                    workRow["vcInsurerName"] = i.insuranceDetails.insurerName;
                    workRow["vcPolicyType"] = i.insuranceDetails.policyType;
                    workRow["vcPolicyNumber"] = i.insuranceDetails.policyNumber;
                    workRow["vcAssetCoverType"] = i.insuranceDetails.coverType;
                    workRow["vcPremiumFrequency"] = i.insuranceDetails.premiumFrequency;
                    workRow["dInsuranceValue"] = (decimal)i.insuranceDetails.insuranceValue;
                    dt.Rows.Add(workRow);
                    c++;
                }
            }
            return dt;
        }

        #endregion
    }
}







