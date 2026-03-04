export interface DashboardSummary {
    totalAssets: number;
    totalValue: number;
    uninsuredAssets: number;
    uninsuredValue: number;
    uninsuredPercentage: number;
    adequatelyInsuredAssets: number;
    adequatelyInsuredValue: number;
    underInsuredAssets: number;
    underInsuredValue: number;
    premiumUnpaidAssets: number;
    premiumUnpaidValue: number;
    noInsuranceDetailsAssets: number;
    noInsuranceDetailsValue: number;
    charts: ChartSeries[];
}

export interface ChartSeries {
    title: string;
    xAxisName: string;
    yAxisName: string;
    data: ChartDataPoint[];
}

export interface ChartDataPoint {
    label: string;
    value: string;
    category?: string;
    secondaryValue?: string;
}
