import type { PaginationParams } from './Common';

export interface User {
    id: string;
    userName: string;
    email: string;
    fullName: string;
    tenantId: number | null;
    role: string;
    isActive: boolean;
}

export interface Tenant {
    id: number;
    name: string;
    identifier: string;
    type: string;
    createdAt: string;
    isActive: boolean;
}

export interface AdminFilterParams extends PaginationParams {
    query?: string;
}
