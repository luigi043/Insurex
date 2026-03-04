using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FusionCharts.Charts;
using System.Windows.Forms;
using FusionCharts.FusionExport.Client;
using System.IO;
using System.Text;
using U = IAPR_Data.Utils;
namespace IAPR_Web.Reporting
{
    public static class BulkExport
    {
        public static Stream GetGraphStream(string chartDeatils, string templateHTML)
        {
            string host = Constants.DEFAULT_HOST;
            int port = Constants.DEFAULT_PORT;

            Stream outS;
            ExportConfig exportConfig = new ExportConfig();
            List<string> results = new List<string>();




            //Export single graph
            //using (ExportManager exportManager = new ExportManager())
            //{
            //    exportConfig.Set("chartConfig", templateHTML);// File.ReadAllText(templatePath + "\\chart-config-file.json"));
            //    exportConfig.Set("type", "pdf");

            //    // Call the Export() method with the export config
            //    results.AddRange(exportManager.Export(exportConfig, outputDir, true));// unzip = true));
            //}

            //Bulk Export
            using (ExportManager exportManager = new ExportManager())
            {
                exportConfig.Set("chartConfig", chartDeatils);
                

                // ATTENTION - Pass the path of the dashboard template
                exportConfig.Set("template", templateHTML); //+ "\\dashboard-template-financer.html");
                //exportConfig.Set("templateFormat", "A3");
                //exportConfig.Set("templateFormat", "letter");
                exportConfig.Set("type", "pdf");
                exportConfig.Set("quality", "best");
                //exportConfig.Set("footerEnabled", true);
                //exportConfig.Set("footer", "InsureX");
                exportConfig.Set("templateWidth", "1300");
                exportConfig.Set("templateHeight", "5000");
                //exportConfig.Set("headerComponents", "{ \"title\": { \"style\": \"color:blue;\" } }");
                exportConfig.Set("footerComponents", "{ \"title\": { \"style\": \"color:blue;\", \"position\": \"left;\" } }");
                // Call the Export() method with the export config
                //results.AddRange(exportManager.Export(exportConfig, outputDir, true));

                Dictionary<string, Stream> files = exportManager.ExportAsStream(exportConfig);
                outS = files["export.pdf"];


                //string path = Path.Combine(@"C:\Mapoza\Insurex\Solution\Insured_Assest_Protection_Register\IAPR_Web\Reporting", "test2.pdf");
                //using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
                //{
                //    files["export.pdf"].CopyTo(outputFileStream);


                //    //StreamReader reader = new StreamReader(files["export.pdf"]);
                //    StreamReader reader = new StreamReader(files["export.pdf"], System.Text.Encoding.UTF8, true);

                //}



                





                return files["export.pdf"];
            }


            //return files["export.pdf"];// outS;


        }

        public static string MapPath(string path)
        {
            return Path.Combine(
                (string)AppDomain.CurrentDomain.GetData("ContentRootPath"),
                path);
        }
    }
}