import { API } from '../utils/api';
import type { Asset, AssetFilterParams, AssetDetail } from '../types/Asset';
import type { ComplianceCase, CaseFilterParams } from '../types/Case';
import type { AuditEntry, AuditFilterParams } from '../types/Audit';
import type { User, Tenant, AdminFilterParams } from '../types/Admin';
import type { PaginatedResponse, ApiResponse } from '../types/Common';

export const assetClient = {
    getAssets: async (params: AssetFilterParams): Promise<PaginatedResponse<Asset>> => {
        const response = await API.get<PaginatedResponse<Asset>>('/assets', { params });
        return response.data;
    },

    getAsset: async (id: string): Promise<ApiResponse<AssetDetail>> => {
        const response = await API.get<ApiResponse<AssetDetail>>(`/assets/${id}`);
        return response.data;
    },
};

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

export const auditClient = {
    getAuditLogs: async (params: AuditFilterParams): Promise<PaginatedResponse<AuditEntry>> => {
        const response = await API.get<PaginatedResponse<AuditEntry>>('/compliance/audit', { params });
        return response.data;
    },
};

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

export const intelligenceClient = {
    getInsights: async (): Promise<ApiResponse<any[]>> => {
        const response = await API.get<ApiResponse<any[]>>('/dashboard/insights');
        return response.data;
    }
};
