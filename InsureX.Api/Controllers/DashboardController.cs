using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using P = IAPR_Data.Providers;

namespace InsureX.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private static readonly CultureInfo ZaCulture = new("en-ZA");

    // GET /api/dashboard
    [HttpGet]
    public IActionResult GetDashboard()
    {
        try
        {
            int userTypeId = GetClaim("iUser_Type_Id");
            int partnerId = GetClaim("iPartner_Id");
            var dP = new P.Daschboard_Provider();
            DataSet? ds = null;

            if (userTypeId <= 2)
                ds = dP.Get_Admin_Landing_DashboardTable();
            else if (userTypeId <= 4)
                ds = dP.Get_Financer_Landing_DashboardTable(partnerId);
            else
                ds = dP.Get_Insurer_Landing_DashboardTable(partnerId);

            if (ds != null && ds.Tables.Count >= 6)
            {
                return Ok(new
                {
                    allAssetCount = SafeInt(ds.Tables[2], "iNumber_Of_Assets"),
                    allAssetTotal = SafeDec(ds.Tables[2], "dcFinance_Value"),
                    insuredAssetCount = SafeInt(ds.Tables[3], "iNumber_Of_Assets"),
                    insuredAssetTotal = SafeDec(ds.Tables[3], "dcFinance_Value"),
                    premiumUnpaidCount = SafeInt(ds.Tables[0], "iNumber_Of_Assets"),
                    premiumUnpaidTotal = SafeDec(ds.Tables[0], "dcUninsured_Finance_Value"),
                    noInsuranceCount = SafeInt(ds.Tables[1], "iNumber_Of_Assets"),
                    noInsuranceTotal = SafeDec(ds.Tables[1], "dcUninsured_Finance_Value"),
                    adequatelyInsuredTotal = SafeDec(ds.Tables[4], "dcFinance_Value"),
                    underInsuredTotal = SafeDec(ds.Tables[5], "dcFinance_Value")
                });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }

        return Ok(new { });
    }

    // GET /api/dashboard/charts/insurance-status
    [HttpGet("charts/insurance-status")]
    public IActionResult ChartInsuranceStatus()
    {
        try
        {
            int userTypeId = GetClaim("iUser_Type_Id");
            int partnerId = GetClaim("iPartner_Id");
            var dP = new P.Daschboard_Provider();
            var ds = userTypeId <= 2
                ? dP.Get_Admin_Landing_Dashboard_ArrearVsUnconfirmed()
                : dP.Get_Financer_Landing_Dashboard_ArrearVsUnconfirmed(partnerId);

            if (ds?.Tables.Count > 0)
            {
                var data = new List<object>();
                foreach (DataRow row in ds.Tables[0].Rows)
                    data.Add(new { label = row[0]?.ToString(), value = row[1]?.ToString() });
                return Ok(new { data });
            }
        }
        catch { }
        return Ok(new { data = Array.Empty<object>() });
    }

    // GET /api/dashboard/charts/uninsured-by-financer
    [HttpGet("charts/uninsured-by-financer")]
    public IActionResult ChartUninsuredByFinancer()
    {
        try
        {
            var dP = new P.Daschboard_Provider();
            var ds = dP.Get_Admin_Landing_Dashboard_UninsuredByFinancer();
            if (ds?.Tables.Count > 0)
            {
                var data = new List<object>();
                foreach (DataRow row in ds.Tables[0].Rows)
                    data.Add(new { label = row["Financer"]?.ToString(), value = row["Finance_Value"]?.ToString() });
                return Ok(new { data });
            }
        }
        catch { }
        return Ok(new { data = Array.Empty<object>() });
    }

    private int GetClaim(string name)
    {
        var val = User.FindFirst(name)?.Value;
        return int.TryParse(val, out int result) ? result : 0;
    }

    private static int SafeInt(DataTable dt, string col)
        => dt.Rows.Count > 0 && dt.Columns.Contains(col) ? Convert.ToInt32(dt.Rows[0][col]) : 0;

    private static decimal SafeDec(DataTable dt, string col)
        => dt.Rows.Count > 0 && dt.Columns.Contains(col) ? Convert.ToDecimal(dt.Rows[0][col]) : 0m;
}
