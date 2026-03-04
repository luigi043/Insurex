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
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

using System.Net.Mail;

namespace IAPR_Data.Providers
{
    public class Bulk_Import_Provider
    {
        public SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString());
        public void Save_Bulk_Import_From_Financer(List<Classes.Policy.BulkImportFromFinancer> biList)
        {


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            var dt = ConvertBulkImport_Financer_ToDatatable(biList);


            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spIns_Import_BulkImportFromFinancer",
               new SqlParameter("@dtBulkImportFromFinancer", dt));

        }


        private DataTable ConvertBulkImport_Financer_ToDatatable(IEnumerable<Classes.Policy.BulkImportFromFinancer> listItems)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(Classes.Policy.BulkImportFromFinancer).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in listItems)
            {
                var datos = new Classes.Policy.BulkImportFromFinancer();
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



        public bool Save_Bulk_Import_From_Insurer_Individual(List<Classes.Policy.BulkImportFromInsurance.GenericItems> biList,
            List<Classes.Policy.BulkImportFromInsurance.IndividualHolder> indH, List<Classes.Policy.BulkImportFromInsurance.Phycisal_address> phA, List<Classes.Policy.BulkImportFromInsurance.Postal_Address> poA)
        {
            bool imported = false;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            var dtIG = ConvertBulkImport_InsurerGeneric_ToDatatable(biList);
            var dtIH = ConvertBulkImport_IndividualHolder_ToDatatable(indH);
            var dtphA = ConvertBulkImport_PhysicalAddress_ToDatatable(phA);
            var dtpoA = ConvertBulkImport_PostalAddress_ToDatatable(poA);


            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spIns_Import_BulkImportFromInsurance_Individual",
               new SqlParameter("@dtBulkImportFromInsurance_Individual_Generic", dtIG),
               new SqlParameter("@dtBulkImportFromInsurance_Individual_Holder", dtIH),
               new SqlParameter("@dtPhycisal_address", dtphA),
               new SqlParameter("@dtPostal_address", dtpoA)
        );
            imported = true;

            return imported;
        }

        public bool Save_Bulk_Import_From_Insurer_Business(List<Classes.Policy.BulkImportFromInsurance.GenericItems> biList,
            List<Classes.Policy.BulkImportFromInsurance.BusinessHolder> busH, List<Classes.Policy.BulkImportFromInsurance.Phycisal_address> phA, List<Classes.Policy.BulkImportFromInsurance.Postal_Address> poA)
        {
            bool imported = false;


            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            var dtIG = ConvertBulkImport_InsurerGeneric_ToDatatable(biList);
            var dtBH = ConvertBulkImport_BussinessHolder_ToDatatable(busH);
            var dtphA = ConvertBulkImport_PhysicalAddress_ToDatatable(phA);
            var dtpoA = ConvertBulkImport_PostalAddress_ToDatatable(poA);


            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["connIAPRData"].ToString(), CommandType.StoredProcedure,
               "spIns_Import_BulkImportFromInsurance_Business",
               new SqlParameter("@dtBulkImportFromInsurance_Individual_Generic", dtIG),
               new SqlParameter("@dtBulkImportFromInsurance_Business_Holder", dtBH),
               new SqlParameter("@dtPhycisal_address", dtphA),
               new SqlParameter("@dtPostal_address", dtpoA)
        );
            imported = true;

            return imported;
        }

        private DataTable ConvertBulkImport_InsurerGeneric_ToDatatable(IEnumerable<Classes.Policy.BulkImportFromInsurance.GenericItems> listItems)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(Classes.Policy.BulkImportFromInsurance.GenericItems).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in listItems)
            {
                var datos = new Classes.Policy.BulkImportFromInsurance.GenericItems();
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


        private DataTable ConvertBulkImport_IndividualHolder_ToDatatable(IEnumerable<Classes.Policy.BulkImportFromInsurance.IndividualHolder> listItems)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(Classes.Policy.BulkImportFromInsurance.IndividualHolder).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in listItems)
            {
                var datos = new Classes.Policy.BulkImportFromInsurance.IndividualHolder();
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
        private DataTable ConvertBulkImport_BusinessHolder_ToDatatable(IEnumerable<Classes.Policy.BulkImportFromInsurance.BusinessHolder> listItems)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(Classes.Policy.BulkImportFromInsurance.BusinessHolder).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in listItems)
            {
                var datos = new Classes.Policy.BulkImportFromInsurance.BusinessHolder();
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

        private DataTable ConvertBulkImport_BussinessHolder_ToDatatable(IEnumerable<Classes.Policy.BulkImportFromInsurance.BusinessHolder> listItems)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(Classes.Policy.BulkImportFromInsurance.BusinessHolder).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in listItems)
            {
                var datos = new Classes.Policy.BulkImportFromInsurance.BusinessHolder();
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
        private DataTable ConvertBulkImport_PhysicalAddress_ToDatatable(IEnumerable<Classes.Policy.BulkImportFromInsurance.Phycisal_address> listItems)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(Classes.Policy.BulkImportFromInsurance.Phycisal_address).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in listItems)
            {
                var datos = new Classes.Policy.BulkImportFromInsurance.Phycisal_address();
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
        private DataTable ConvertBulkImport_PostalAddress_ToDatatable(IEnumerable<Classes.Policy.BulkImportFromInsurance.Postal_Address> listItems)
        {
            var dataTable = new DataTable();

            foreach (var info in typeof(Classes.Policy.BulkImportFromInsurance.Postal_Address).GetProperties())
            {
                dataTable.Columns.Add(info.Name, info.PropertyType);
            }
            dataTable.AcceptChanges();


            foreach (var item in listItems)
            {
                var datos = new Classes.Policy.BulkImportFromInsurance.Postal_Address();
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
    }
}
