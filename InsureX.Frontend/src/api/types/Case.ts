import type { BaseEntity } from './Common';

export interface ComplianceCase extends BaseEntity {
    caseNumber: string;
    title: string;
    description: string;
    status: 'Open' | 'InProgress' | 'Escalated' | 'Resolved' | 'Closed';
    priority: 'Low' | 'Medium' | 'High' | 'Critical';
    assignedToUserId?: string;
    openedAt: string;
    dueAt: string;
    resolvedAt?: string;
    escalatedAt?: string;
    tenantId: number;
    correlationId: string;
}

export interface CaseFilterParams {
    page?: number;
    pageSize?: number;
    status?: string;
    priority?: string;
}
