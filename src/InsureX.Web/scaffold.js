const fs = require('fs');
const path = require('path');

const structure = {
  "public/images": {},
  "public/fonts": {},
  "public/favicon.ico": "file",
  "src/api/auth.js": "file",
  "src/api/assets.js": "file",
  "src/api/policies.js": "file",
  "src/api/billing.js": "file",
  "src/api/reports.js": "file",
  "src/api/axiosConfig.js": "file",
  "src/assets/css/main.css": "file",
  "src/assets/css/theme.css": "file",
  "src/assets/css/dashboard.css": "file",
  "src/assets/scss": {},
  "src/assets/images": {},
  "src/assets/fonts": {},
  "src/components/common/Header.jsx": "jsx",
  "src/components/common/Footer.jsx": "jsx",
  "src/components/common/Sidebar.jsx": "jsx",
  "src/components/common/LoadingSpinner.jsx": "jsx",
  "src/components/common/ErrorBoundary.jsx": "jsx",
  "src/components/common/Modal.jsx": "jsx",
  "src/components/forms/Input.jsx": "jsx",
  "src/components/forms/Select.jsx": "jsx",
  "src/components/forms/DatePicker.jsx": "jsx",
  "src/components/forms/FileUpload.jsx": "jsx",
  "src/components/forms/FormValidator.js": "file",
  "src/components/tables/DataTable.jsx": "jsx",
  "src/components/tables/TableActions.jsx": "jsx",
  "src/components/tables/Pagination.jsx": "jsx",
  "src/components/tables/ExportButtons.jsx": "jsx",
  "src/components/charts/FusionChartsWrapper.jsx": "jsx",
  "src/components/charts/ChartConfig.js": "file",
  "src/features/auth/components/LoginForm.jsx": "jsx",
  "src/features/auth/components/RegisterForm.jsx": "jsx",
  "src/features/auth/components/PasswordReminder.jsx": "jsx",
  "src/features/auth/components/ChangePassword.jsx": "jsx",
  "src/features/auth/pages/LoginPage.jsx": "jsx",
  "src/features/auth/pages/RegisterPage.jsx": "jsx",
  "src/features/auth/pages/ProfilePage.jsx": "jsx",
  "src/features/auth/hooks/useAuth.js": "file",
  "src/features/auth/context/AuthContext.jsx": "jsx",
  "src/features/auth/index.js": "file",
  "src/features/admin/components/PartnerManagement.jsx": "jsx",
  "src/features/admin/components/UserManagement.jsx": "jsx",
  "src/features/admin/components/BulkImport.jsx": "jsx",
  "src/features/admin/pages/AdminDashboard.jsx": "jsx",
  "src/features/admin/pages/ManagePartnersPage.jsx": "jsx",
  "src/features/admin/pages/BulkImportPage.jsx": "jsx",
  "src/features/admin/index.js": "file",
  "src/features/assets/components/AssetForm.jsx": "jsx",
  "src/features/assets/components/AssetList.jsx": "jsx",
  "src/features/assets/components/AssetSearch.jsx": "jsx",
  "src/features/assets/components/assetTypes/VehicleAsset.jsx": "jsx",
  "src/features/assets/components/assetTypes/BuildingAsset.jsx": "jsx",
  "src/features/assets/components/assetTypes/StockAsset.jsx": "jsx",
  "src/features/assets/components/assetTypes/MachineryAsset.jsx": "jsx",
  "src/features/assets/components/assetTypes/ElectronicAsset.jsx": "jsx",
  "src/features/assets/components/assetTypes/index.js": "file",
  "src/features/assets/pages/AssetManagementPage.jsx": "jsx",
  "src/features/assets/pages/AddAssetPage.jsx": "jsx",
  "src/features/assets/pages/AssetDetailsPage.jsx": "jsx",
  "src/features/assets/index.js": "file",
  "src/features/policies/components/PolicyForm.jsx": "jsx",
  "src/features/policies/components/PolicyList.jsx": "jsx",
  "src/features/policies/components/PolicyTransactions.jsx": "jsx",
  "src/features/policies/components/PolicyConfirmation.jsx": "jsx",
  "src/features/policies/pages/PolicyManagementPage.jsx": "jsx",
  "src/features/policies/pages/AddPolicyPage.jsx": "jsx",
  "src/features/policies/pages/PolicyDetailsPage.jsx": "jsx",
  "src/features/policies/index.js": "file",
  "src/features/billing/components/InvoiceList.jsx": "jsx",
  "src/features/billing/components/ChargeForm.jsx": "jsx",
  "src/features/billing/components/BillingSummary.jsx": "jsx",
  "src/features/billing/pages/BillingPage.jsx": "jsx",
  "src/features/billing/pages/InvoiceDetailsPage.jsx": "jsx",
  "src/features/billing/index.js": "file",
  "src/features/reports/components/ReportFilters.jsx": "jsx",
  "src/features/reports/components/ReportViewer.jsx": "jsx",
  "src/features/reports/components/ExportPanel.jsx": "jsx",
  "src/features/reports/components/reportTypes/AllAssetsReport.jsx": "jsx",
  "src/features/reports/components/reportTypes/MonthlyReport.jsx": "jsx",
  "src/features/reports/components/reportTypes/UninsuredAssetsReport.jsx": "jsx",
  "src/features/reports/components/reportTypes/ReinstatedCoverReport.jsx": "jsx",
  "src/features/reports/pages/ReportsPage.jsx": "jsx",
  "src/features/reports/pages/ReportDetailsPage.jsx": "jsx",
  "src/features/reports/index.js": "file",
  "src/features/financer/components/FinancerDashboard.jsx": "jsx",
  "src/features/financer/pages/FinancerHomePage.jsx": "jsx",
  "src/features/insurer/components/InsurerDashboard.jsx": "jsx",
  "src/features/insurer/pages/InsurerHomePage.jsx": "jsx",
  "src/layouts/MainLayout.jsx": "jsx",
  "src/layouts/AdminLayout.jsx": "jsx",
  "src/layouts/FinancerLayout.jsx": "jsx",
  "src/layouts/InsurerLayout.jsx": "jsx",
  "src/layouts/AuthLayout.jsx": "jsx",
  "src/hooks/useApi.js": "file",
  "src/hooks/useForm.js": "file",
  "src/hooks/usePagination.js": "file",
  "src/hooks/useExport.js": "file",
  "src/hooks/usePermissions.js": "file",
  "src/context/ThemeContext.jsx": "jsx",
  "src/context/NotificationContext.jsx": "jsx",
  "src/context/PermissionContext.jsx": "jsx",
  "src/utils/formatters.js": "file",
  "src/utils/validators.js": "file",
  "src/utils/constants.js": "file",
  "src/utils/helpers.js": "file",
  "src/utils/encryption.js": "file",
  "src/utils/exportHelpers.js": "file",
  "src/services/authService.js": "file",
  "src/services/assetService.js": "file",
  "src/services/policyService.js": "file",
  "src/services/billingService.js": "file",
  "src/services/reportService.js": "file",
  "src/services/emailService.js": "file",
  "src/routes/AppRoutes.jsx": "jsx",
  "src/routes/PrivateRoute.jsx": "jsx",
  "src/routes/RoleBasedRoute.jsx": "jsx",
  "src/routes/index.js": "file",
  "src/styles/variables.css": "file",
  "src/styles/globals.css": "file",
  "src/styles/themes/lightTheme.js": "file",
  "src/styles/themes/darkTheme.js": "file",
  "src/App.jsx": "jsx",
  "src/index.js": "file",
  "src/config.js": "file",
  "jsconfig.json": "json"
};

const getJsxTemplate = (name) => `import React from 'react';

const ${name} = () => {
  return (
    <div className="${name.toLowerCase()}-container">
      <h2>${name} (South Africa Region)</h2>
      <p>Component pending final migration.</p>
    </div>
  );
};

export default ${name};
`;

const getJsTemplate = () => `// Core logic holding file (SA Region standard)\nexport {};\n`;

for (const [itemPath, type] of Object.entries(structure)) {
  const fullPath = path.join(__dirname, itemPath);
  const dir = type === 'file' || type === 'jsx' || type === 'json' ? path.dirname(fullPath) : fullPath;
  
  if (!fs.existsSync(dir)) {
    fs.mkdirSync(dir, { recursive: true });
  }

  if (type === 'jsx') {
    if (!fs.existsSync(fullPath)) {
      const name = path.basename(itemPath, '.jsx');
      fs.writeFileSync(fullPath, getJsxTemplate(name));
    }
  } else if (type === 'file') {
    if (!fs.existsSync(fullPath) && !itemPath.endsWith('ico')) {
      fs.writeFileSync(fullPath, getJsTemplate());
    } else if (itemPath.endsWith('ico') && !fs.existsSync(fullPath)) {
      fs.writeFileSync(fullPath, '');
    }
  } else if (type === 'json') {
     if (!fs.existsSync(fullPath)) {
        fs.writeFileSync(fullPath, "{\n  \"compilerOptions\": {\n    \"baseUrl\": \"src\"\n  },\n  \"include\": [\"src\"]\n}\n");
     }
  }
}
console.log('Scaffolding complete.');
