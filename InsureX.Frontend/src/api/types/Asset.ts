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

export interface AssetFilterParams {
    page?: number;
    pageSize?: number;
    status?: string;
    assetType?: string;
    registrationNumber?: string;
}
