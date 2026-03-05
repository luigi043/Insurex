import axios from 'axios';
import { API } from '../utils/api';
import type { Asset, AssetFilterParams, AssetDetail } from '../types/Asset';
import type { ComplianceCase, CaseFilterParams } from '../types/Case';
import type { AuditEntry, AuditFilterParams } from '../types/Audit';
import type { User, Tenant, AdminFilterParams } from '../types/Admin';
import type { PaginatedResponse, ApiResponse } from '../types/Common';

// --- Auth ---
export const authClient = {
    login: async (userName: string, password: string) => {
        const response = await API.post('/auth/login', { userName, password });
        return response.data;
    },
    refresh: async () => {
        const response = await API.post('/auth/refresh');
        return response.data;
    },
};

// --- Dashboard ---
export const dashboardClient = {
    getDashboard: async () => {
        const response = await API.get('/dashboard');
        return response.data;
    },
    getChartInsuranceStatus: async () => {
        const response = await API.get('/dashboard/charts/insurance-status');
        return response.data;
    },
    getChartUninsuredByFinancer: async () => {
        const response = await API.get('/dashboard/charts/uninsured-by-financer');
        return response.data;
    },
};

// --- Assets ---
export const assetClient = {
    getAssets: async (params: AssetFilterParams): Promise<PaginatedResponse<Asset>> => {
        const response = await API.get<PaginatedResponse<Asset>>('/assets', { params });
        return response.data;
    },
    getAsset: async (id: string): Promise<ApiResponse<AssetDetail>> => {
        const response = await API.get<ApiResponse<AssetDetail>>(`/assets/${id}`);
        return response.data;
    },
    search: async (query: string) => {
        const response = await API.get('/asset/search', { params: { q: query } });
        return response.data;
    },
    getUnconfirmed: async () => {
        const response = await API.get('/asset/unconfirmed');
        return response.data;
    },
    updateFinanceValue: async (assetId: number, newValue: number) => {
        const response = await API.put(`/asset/${assetId}/finance-value`, { newValue });
        return response.data;
    },
    getAssetTypes: async () => {
        const response = await API.get('/asset/types');
        return response.data;
    },
};

// --- Policy ---
export const policyClient = {
    getTransactions: async () => {
        const response = await API.get('/policy/transactions');
        return response.data;
    },
    getPendingConfirmations: async () => {
        const response = await API.get('/policy/pending-confirmations');
        return response.data;
    },
    confirmPolicy: async (policyId: number, action: string) => {
        const response = await API.post('/policy/confirm', { policyId, action });
        return response.data;
    },
    getFormFields: async () => {
        const response = await API.get('/policy/form-fields');
        return response.data;
    },
    getPolicyAssets: async (policyId: number) => {
        const response = await API.get(`/policy/${policyId}/assets`);
        return response.data;
    },
};

// --- Billing ---
export const billingClient = {
    getInvoices: async () => {
        const response = await API.get('/billing/invoices');
        return response.data;
    },
    addCharge: async (partnerId: number, chargeType: string, amount: number, description: string) => {
        const response = await API.post('/billing/charges', { partnerId, chargeType, amount, description });
        return response.data;
    },
    updateCharge: async (chargeId: number, amount: number) => {
        const response = await API.put(`/billing/charges/${chargeId}`, { amount });
        return response.data;
    },
};

// --- Compliance Cases ---
export const caseClient = {
    getCases: async (params: CaseFilterParams): Promise<PaginatedResponse<ComplianceCase>> => {
        const response = await API.get<PaginatedResponse<ComplianceCase>>('/compliance/cases', { params });
        return response.data;
    },
    getCase: async (id: string): Promise<ApiResponse<ComplianceCase>> => {
        const response = await API.get<ApiResponse<ComplianceCase>>(`/compliance/cases/${id}`);
        return response.data;
    },
    resolveCase: async (id: string, resolution: string): Promise<ApiResponse<null>> => {
        const response = await API.post<ApiResponse<null>>(`/compliance/cases/${id}/resolve`, { resolution });
        return response.data;
    },
};

// --- Audit ---
export const auditClient = {
    getAuditLogs: async (params: AuditFilterParams): Promise<PaginatedResponse<AuditEntry>> => {
        const response = await API.get<PaginatedResponse<AuditEntry>>('/compliance/audit', { params });
        return response.data;
    },
};

// --- Admin ---
export const adminClient = {
    getUsers: async (params: AdminFilterParams): Promise<PaginatedResponse<User>> => {
        const response = await API.get<PaginatedResponse<User>>('/admin/users', { params });
        return response.data;
    },
    getTenants: async (params: AdminFilterParams): Promise<PaginatedResponse<Tenant>> => {
        const response = await API.get<PaginatedResponse<Tenant>>('/admin/tenants', { params });
        return response.data;
    },
};

// --- Intelligence / Insights ---
export const intelligenceClient = {
    getInsights: async (): Promise<ApiResponse<any[]>> => {
        const response = await API.get<ApiResponse<any[]>>('/dashboard/insights');
        return response.data;
    }
};

// --- Reports (CSV export) ---
export const reportClient = {
    exportAuditLog: async () => {
        const baseUrl = (axios.defaults.baseURL || '').replace(/\/+$/, '');
        const url = `${baseUrl}/api/report/audit/export`;
        const a = document.createElement('a');
        a.href = url;
        a.download = `AuditLog_Export.csv`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    },
    exportAssets: async () => {
        const baseUrl = (axios.defaults.baseURL || '').replace(/\/+$/, '');
        const url = `${baseUrl}/api/report/assets/export`;
        const a = document.createElement('a');
        a.href = url;
        a.download = `Assets_Portfolio_Export.csv`;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    },
};
