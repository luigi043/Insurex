export interface PaginatedResponse<T> {
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
    hasNextPage: boolean;
    hasPreviousPage: boolean;
}

export interface PaginationParams {
    page?: number;
    pageSize?: number;
    sortBy?: string;
    sortDir?: 'asc' | 'desc';
}

export interface ApiResponse<T> {
    success: boolean;
    message: string;
    data: T;
    correlationId?: string;
}

export interface TokenPayload {
    clientId: string;
    tenantId?: number;
    scopes: string[];
    exp: number;
    iat: number;
}

export interface BaseEntity {
    id: string | number;
    createdUtc?: string;
    modifiedUtc?: string;
}
