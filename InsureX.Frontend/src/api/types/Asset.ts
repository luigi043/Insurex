import type { BaseEntity } from './Common';

export interface Asset extends BaseEntity {
    tenantId: number;
    assetType: 'Motor' | 'NonMotor';
    assetIdentifier: string;
    registrationNumber?: string;
    financedAmount: number;
    borrowerReference?: string;
    loanStartDate: string;
    loanEndDate: string;
    status: 'Active' | 'Settled' | 'Closed';
    complianceStatus?: string;
}

export interface Policy {
    id: number;
    policyNumber: string;
    insurerName: string;
    status: string;
    expiryDate: string;
    insuredValue: number;
}

export interface Borrower {
    name: string;
    idNumber: string;
    email: string;
    phone: string;
}

export interface ComplianceHistory {
    id: number;
    outcome: string;
    reason: string;
    evaluatedAt: string;
    correlationId: string;
}

export interface AssetDetail extends Asset {
    borrower: Borrower;
    policies: Policy[];
    complianceHistory: ComplianceHistory[];
}

export interface AssetFilterParams {
    page?: number;
    pageSize?: number;
    status?: string;
    assetType?: string;
    registrationNumber?: string;
}
