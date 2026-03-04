import { API } from '../utils/api';
import type { Asset, AssetFilterParams } from '../types/Asset';
import type { ComplianceCase, CaseFilterParams } from '../types/Case';
import type { PaginatedResponse, ApiResponse } from '../types/Common';

export const assetClient = {
    getAssets: async (params: AssetFilterParams): Promise<PaginatedResponse<Asset>> => {
        const response = await API.get<PaginatedResponse<Asset>>('/assets', { params });
        return response.data;
    },

    getAsset: async (id: string): Promise<ApiResponse<Asset>> => {
        const response = await API.get<ApiResponse<Asset>>(`/assets/${id}`);
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
