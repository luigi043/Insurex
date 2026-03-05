using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using P = IAPR_Data.Providers;

namespace InsureX.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AssetController : ControllerBase
{
    // GET /api/asset/search?q=search_term
    [HttpGet("search")]
    public IActionResult Search([FromQuery] string q)
    {
        try
        {
            int partnerId = GetClaim("iPartner_Id");
            var prov = new P.Asset_Provider();
            var ds = prov.Find_Assets(partnerId, q ?? "");

            if (ds?.Tables.Count > 0)
                return Ok(new { data = DataTableToList(ds.Tables[0]) });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { data = Array.Empty<object>() });
    }

    // GET /api/asset/unconfirmed
    [HttpGet("unconfirmed")]
    public IActionResult GetUnconfirmed()
    {
        try
        {
            int partnerId = GetClaim("iPartner_Id");
            var prov = new P.Asset_Provider();
            var ds = prov.Get_Unconfirmed_Insurance(partnerId);

            if (ds?.Tables.Count > 0)
                return Ok(new { data = DataTableToList(ds.Tables[0]) });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { data = Array.Empty<object>() });
    }

    // PUT /api/asset/{id}/finance-value
    [HttpPut("{assetId}/finance-value")]
    public IActionResult UpdateFinanceValue(int assetId, [FromBody] UpdateFinanceValueRequest request)
    {
        try
        {
            var prov = new P.Asset_Provider();
            prov.Update_Finance_Value(assetId, request.NewValue);
            return Ok(new { message = "Finance value updated" });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }

    // GET /api/asset/types
    [HttpGet("types")]
    public IActionResult GetAssetTypes()
    {
        try
        {
            int partnerId = GetClaim("iPartner_Id");
            int userTypeId = GetClaim("iUser_Type_Id");
            var frmF = new P.GetFormFields_Provider();

            DataSet ds;
            if (userTypeId >= 3)
                ds = frmF.GetFormFieldsAssetsFinancedByFinancer(partnerId);
            else
            {
                ds = frmF.GetFormFieldsVehicleAsset();
                // Return Tables[14] which holds asset types
                if (ds.Tables.Count > 14)
                {
                    var types = new List<object>();
                    foreach (DataRow row in ds.Tables[14].Rows)
                        types.Add(new { value = row[0]?.ToString(), label = row[1]?.ToString() });
                    return Ok(new { data = types });
                }
            }

            if (ds?.Tables.Count > 0)
            {
                var types = new List<object>();
                foreach (DataRow row in ds.Tables[0].Rows)
                    types.Add(new { value = row[0]?.ToString(), label = row[1]?.ToString() });
                return Ok(new { data = types });
            }
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { data = Array.Empty<object>() });
    }

    // POST /api/asset
    [HttpPost]
    public IActionResult CreateAsset([FromBody] IAPR_Data.Classes.AssetTypes.Vehicle_Asset asset)
    {
        try
        {
            var prov = new P.Vehicle_Asset_Provider();
            prov.Save_New_Vehicle_Asset(asset);
            return Ok(new { message = "Asset registered successfully" });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
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

public class UpdateFinanceValueRequest
{
    public decimal NewValue { get; set; }
}
