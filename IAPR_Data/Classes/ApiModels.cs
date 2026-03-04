using System;
using System.Collections.Generic;

namespace IAPR_Data.Classes
{
    /// <summary>
    /// Unified DTO for dashboard summary statistics across different user roles.
    /// Provides a consistent structure for Admin, Financer, and Insurer views.
    /// </summary>
    public class DashboardSummaryDto
    {
        public int TotalAssets { get; set; }
        public decimal TotalValue { get; set; }
        
        public int UninsuredAssets { get; set; }
        public decimal UninsuredValue { get; set; }
        public double UninsuredPercentage { get; set; }

        public int AdequatelyInsuredAssets { get; set; }
        public decimal AdequatelyInsuredValue { get; set; }
        
        public int UnderInsuredAssets { get; set; }
        public decimal UnderInsuredValue { get; set; }

        public int PremiumUnpaidAssets { get; set; }
        public decimal PremiumUnpaidValue { get; set; }

        public int NoInsuranceDetailsAssets { get; set; }
        public decimal NoInsuranceDetailsValue { get; set; }

        public List<ChartSeriesDto> Charts { get; set; } = new List<ChartSeriesDto>();
    }

    /// <summary>
    /// Represents a data series for charts, adaptable for various chart types 
    /// (Pie, Bar, Line, Spline).
    /// </summary>
    public class ChartSeriesDto
    {
        public string Title { get; set; }
        public string XAxisName { get; set; }
        public string YAxisName { get; set; }
        public List<ChartDataPointDto> Data { get; set; } = new List<ChartDataPointDto>();
    }

    /// <summary>
    /// Individual data point for a chart series.
    /// </summary>
    public class ChartDataPointDto
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public string Category { get; set; } // For Grouped/Stacked charts
        public string SecondaryValue { get; set; } // For Dual-Y Axis charts
    }

    /// <summary>
    /// DTO for Case Management statistics.
    /// </summary>
    public class CaseStatsDto
    {
        public int OpenCases { get; set; }
        public int OverdueCases { get; set; }
        public int CriticalCases { get; set; }
        public int EscalatedCases { get; set; }
        public decimal AverageResolutionTimeDays { get; set; }
    }

    public class AssetDto
    {
        public int Id { get; set; }
        public int? TenantId { get; set; }
        public string AssetType { get; set; }
        public string AssetIdentifier { get; set; }
        public string RegistrationNumber { get; set; }
        public decimal FinancedAmount { get; set; }
        public string BorrowerReference { get; set; }
        public DateTime LoanStartDate { get; set; }
        public DateTime LoanEndDate { get; set; }
        public string Status { get; set; }
        public string ComplianceStatus { get; set; }
    }

    public class AssetDetailDto : AssetDto
    {
        public BorrowerDto Borrower { get; set; }
        public List<PolicyDto> Policies { get; set; } = new List<PolicyDto>();
        public List<ComplianceStateDto> ComplianceHistory { get; set; } = new List<ComplianceStateDto>();
    }

    public class BorrowerDto
    {
        public string Name { get; set; }
        public string IDNumber { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class PolicyDto
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public string InsurerName { get; set; }
        public string Status { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal InsuredValue { get; set; }
    }

    public class ComplianceStateDto
    {
        public int Id { get; set; }
        public string Outcome { get; set; }
        public string Reason { get; set; }
        public DateTime EvaluatedAt { get; set; }
        public string CorrelationId { get; set; }
    }

    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public int? TenantId { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }

    public class TenantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Type { get; set; } // Financer | Insurer | Broker | Admin
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
