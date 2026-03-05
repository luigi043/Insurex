using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using P = IAPR_Data.Providers;

namespace InsureX.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IntelligenceController : ControllerBase
{
    // GET /api/intelligence/risk-score
    [HttpGet("risk-score")]
    public IActionResult GetRiskScore()
    {
        try
        {
            int partnerId = GetClaim("iPartner_Id");
            int userTypeId = GetClaim("iUser_Type_Id");
            var prov = new P.Daschboard_Provider();

            DataSet ds;
            if (userTypeId >= 3) // Financer
                ds = prov.Get_Financer_Landing_Dashboard_ArrearVsUnconfirmed(partnerId);
            else if (userTypeId == 1) // Admin
                ds = prov.Get_Admin_Landing_Dashboard_ArrearVsUnconfirmed();
            else
                return Ok(new { data = new { score = 0, status = "N/A" } });

            if (ds?.Tables.Count > 0)
                return Ok(new { data = DataTableToList(ds.Tables[0]) });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { data = Array.Empty<object>() });
    }

    // GET /api/intelligence/trends
    [HttpGet("trends")]
    public IActionResult GetTrends()
    {
        try
        {
            int partnerId = GetClaim("iPartner_Id");
            int userTypeId = GetClaim("iUser_Type_Id");
            var prov = new P.Daschboard_Provider();

            DataSet ds;
            if (userTypeId >= 3)
                ds = prov.Get_Financer_Landing_Dashboard_NonPayment_History_Chart(partnerId);
            else if (userTypeId == 1)
                ds = prov.Get_Admin_Landing_Dashboard_NonPayment_Annual_Chart();
            else
                return Ok(new { data = Array.Empty<object>() });

            if (ds?.Tables.Count > 0)
                return Ok(new { data = DataTableToList(ds.Tables[0]) });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { data = Array.Empty<object>() });
    }

    // GET /api/intelligence/tenant-health
    [HttpGet("tenant-health")]
    public IActionResult GetTenantHealth()
    {
        try
        {
            int userTypeId = GetClaim("iUser_Type_Id");
            if (userTypeId != 1) return Forbid();

            var prov = new P.Daschboard_Provider();
            var ds = prov.Get_Admin_Landing_Dashboard_UninsuredByFinancer();

            if (ds?.Tables.Count > 0)
                return Ok(new { data = DataTableToList(ds.Tables[0]) });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { data = Array.Empty<object>() });
    }

    #region Helpers

    private int GetClaim(string name)
        => int.TryParse(User.FindFirst(name)?.Value, out int v) ? v : 0;

    private static List<Dictionary<string, object?>> DataTableToList(DataTable dt)
    {
        var list = new List<Dictionary<string, object?>>();
        foreach (DataRow row in dt.Rows)
        {
            var dict = new Dictionary<string, object?>();
            foreach (DataColumn col in dt.Columns)
                dict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
            list.Add(dict);
        }
        return list;
    }

    #endregion
}
