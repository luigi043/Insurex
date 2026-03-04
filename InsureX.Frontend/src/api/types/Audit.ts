import type { PaginationParams } from './Common';

export interface AuditEntry {
    id: number;
    correlationId: string;
    entityName: string;
    entityId: string;
    action: string;
    actorName: string;
    tenantId: number | null;
    occurredAt: string;
    notes: string;
}

export interface AuditFilterParams extends PaginationParams {
    entity?: string;
    correlationId?: string;
}
