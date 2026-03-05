namespace InsureX.Web.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordViewModel
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class PasswordReminderViewModel
    {
        public string Email { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class DashboardViewModel
    {
        public int AllAssetCount { get; set; }
        public decimal AllAssetTotal { get; set; }
        public int InsuredAssetCount { get; set; }
        public decimal InsuredAssetTotal { get; set; }
        public int UninsuredAssetCount { get; set; }
        public decimal UninsuredAssetTotal { get; set; }
        public decimal PremiumUnpaidAssetTotal { get; set; }
        public decimal PremiumUnpaidAssetTotalPercent { get; set; }
        public decimal NoInsuranceAssetTotal { get; set; }
        public decimal NoInsuranceAssetTotalPercent { get; set; }
        public decimal AdequatelyInsuredTotal { get; set; }
        public decimal AdequatelyInsuredTotalPercent { get; set; }
        public decimal UnderInsuredTotal { get; set; }
        public decimal UnderInsuredTotalPercent { get; set; }
        public decimal InsuredShortFall { get; set; }
    }

    public class SearchResultViewModel
    {
        public string Query { get; set; } = string.Empty;
        public List<SearchResultItem> Results { get; set; } = new();
    }

    public class SearchResultItem
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class MonthlyReportViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
