using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using P = IAPR_Data.Providers;

namespace InsureX.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BillingController : ControllerBase
{
    // GET /api/billing/invoices
    [HttpGet("invoices")]
    public IActionResult GetInvoices()
    {
        try
        {
            int partnerId = GetClaim("iPartner_Id");
            var prov = new P.Billing_Provider();
            var ds = prov.Get_Partner_Invoices(partnerId);

            if (ds?.Tables.Count > 0)
                return Ok(new { data = DataTableToList(ds.Tables[0]) });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }

        return Ok(new { data = Array.Empty<object>() });
    }

    // POST /api/billing/charges
    [Authorize(Roles = "Administrator")]
    [HttpPost("charges")]
    public IActionResult AddCharge([FromBody] ChargeRequest request)
    {
        try
        {
            var prov = new P.Billing_Provider();
            prov.Add_New_Charge(request.PartnerId, request.ChargeType, request.Amount, request.Description);
            return Ok(new { message = "Charge added successfully" });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }

    // PUT /api/billing/charges/{id}
    [Authorize(Roles = "Administrator")]
    [HttpPut("charges/{chargeId}")]
    public IActionResult UpdateCharge(int chargeId, [FromBody] UpdateChargeRequest request)
    {
        try
        {
            var prov = new P.Billing_Provider();
            prov.Update_Charge(chargeId, request.Amount);
            return Ok(new { message = "Charge updated successfully" });
        }
        catch (Exception ex) { return StatusCode(500, new { message = ex.Message }); }
    }

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
}

public class ChargeRequest
{
    public int PartnerId { get; set; }
    public string ChargeType { get; set; } = "";
    public decimal Amount { get; set; }
    public string Description { get; set; } = "";
}

public class UpdateChargeRequest
{
    public decimal Amount { get; set; }
}
