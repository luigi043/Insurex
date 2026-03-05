using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using P = IAPR_Data.Providers;

namespace InsureX.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PolicyController : ControllerBase
{
    // GET /api/policy/transactions
    [HttpGet("transactions")]
    public IActionResult GetTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            int partnerId = GetClaim("iPartner_Id");
            var prov = new P.Policy_Provider();
            var ds = prov.Get_Policy_Transactions(partnerId);

            if (ds?.Tables.Count > 0)
                return Ok(new { data = DataTableToList(ds.Tables[0]) });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { data = Array.Empty<object>() });
    }

    // GET /api/policy/pending-confirmations
    [HttpGet("pending-confirmations")]
    public IActionResult GetPendingConfirmations()
    {
        try
        {
            int partnerId = GetClaim("iPartner_Id");
            var prov = new P.Policy_Provider();
            var ds = prov.Get_Pending_Policy_Confirmations(partnerId);

            if (ds?.Tables.Count > 0)
                return Ok(new { data = DataTableToList(ds.Tables[0]) });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { data = Array.Empty<object>() });
    }

    // POST /api/policy/confirm
    [HttpPost("confirm")]
    public IActionResult ConfirmPolicy([FromBody] PolicyActionRequest request)
    {
        try
        {
            var prov = new P.Policy_Provider();
            if (request.Action == "confirm")
                prov.Confirm_Policy_Cover(request.PolicyId);
            else
                prov.Reject_Policy_Cover(request.PolicyId);

            return Ok(new { message = $"Policy {request.Action}ed successfully" });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }

    // GET /api/policy/form-fields
    [HttpGet("form-fields")]
    public IActionResult GetFormFields()
    {
        try
        {
            var frmF = new P.GetFormFields_Provider();
            var ds = frmF.GetFormFieldsVehicleAsset();

            return Ok(new
            {
                insuranceCompanies = DataTableToSelectList(ds.Tables[0]),
                identificationTypes = DataTableToSelectList(ds.Tables[2]),
                policyTypes = DataTableToSelectList(ds.Tables[3]),
                personTitles = DataTableToSelectList(ds.Tables[4]),
                provinces = DataTableToSelectList(ds.Tables[5]),
                paymentFrequencies = DataTableToSelectList(ds.Tables[10]),
                assetTypes = DataTableToSelectList(ds.Tables[14])
            });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }

    // GET /api/policy/{id}/assets
    [HttpGet("{policyId}/assets")]
    public IActionResult GetPolicyAssets(int policyId)
    {
        try
        {
            var prov = new P.Policy_Provider();
            var ds = prov.GetPolicy_All_Assets(policyId);
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

    private static List<object> DataTableToSelectList(DataTable dt)
    {
        var list = new List<object>();
        foreach (DataRow row in dt.Rows)
            list.Add(new { value = row[0]?.ToString(), label = row[1]?.ToString() });
        return list;
    }

    #endregion
}

public class PolicyActionRequest
{
    public int PolicyId { get; set; }
    public string Action { get; set; } = "confirm";
}
