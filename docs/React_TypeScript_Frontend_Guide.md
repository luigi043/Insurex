# InsureX Frontend – React + TypeScript Development Guide

---

## QUICK START

### Setup
```bash
# Option A: Create-React-App with TypeScript
npx create-react-app InsureX.Frontend --template typescript

# Option B: Vite (recommended for speed)
npm create vite@latest InsureX.Frontend -- --template react-ts
cd InsureX.Frontend
npm install

# Install dependencies
npm install axios zustand react-router-dom
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p

# Start dev server
npm run dev
```

---

## PROJECT STRUCTURE

```
src/
├── api/
│   ├── clients/
│   │   ├── assetClient.ts
│   │   ├── complianceClient.ts
│   │   ├── caseClient.ts
│   │   ├── authClient.ts
│   │   └── index.ts                    (export all clients)
│   ├── types/
│   │   ├── Asset.ts
│   │   ├── Compliance.ts
│   │   ├── Case.ts
│   │   ├── Common.ts
│   │   └── index.ts                    (export all types)
│   └── utils/
│       ├── api.ts                      (Axios instance + interceptors)
│       └── errors.ts
├── components/
│   ├── layouts/
│   │   ├── MainLayout.tsx
│   │   ├── AuthLayout.tsx
│   │   └── NavigationBar.tsx
│   ├── pages/
│   │   ├── LoginPage.tsx
│   │   ├── AssetsPage.tsx
│   │   ├── CompliancePage.tsx
│   │   ├── CasesPage.tsx
│   │   ├── DashboardPage.tsx
│   │   └── NotFoundPage.tsx
│   ├── tables/
│   │   ├── AssetTable.tsx
│   │   ├── ComplianceTable.tsx
│   │   ├── CaseTable.tsx
│   │   └── Table.tsx                   (generic)
│   ├── forms/
│   │   ├── AssetFilterForm.tsx
│   │   ├── AssetForm.tsx
│   │   ├── LoginForm.tsx
│   │   └── CaseActionForm.tsx
│   ├── shared/
│   │   ├── Pagination.tsx
│   │   ├── StatusBadge.tsx
│   │   ├── LoadingSpinner.tsx
│   │   ├── Modal.tsx
│   │   ├── Button.tsx
│   │   ├── Input.tsx
│   │   └── Alert.tsx
│   └── index.ts
├── hooks/
│   ├── useAuth.ts
│   ├── usePagination.ts
│   ├── useApi.ts
│   └── useTenant.ts
├── stores/
│   ├── authStore.ts                   (Zustand store)
│   ├── tenantStore.ts
│   └── uiStore.ts
├── utils/
│   ├── formatters.ts
│   ├── validators.ts
│   ├── dateUtils.ts
│   └── constants.ts
├── styles/
│   ├── globals.css
│   ├── tailwind.css
│   └── variables.css
├── App.tsx
├── main.tsx
└── index.css
```

---

## PART 1: API CONFIGURATION

### 1.1 Common Types

**src/api/types/Common.ts**
```typescript
// Pagination
export interface PaginatedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNext: boolean;
}

export interface PaginationParams {
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortDir?: 'asc' | 'desc';
}

// Error response
export interface ErrorResponse {
  type: string;
  title: string;
  status: number;
  detail?: string;
  errors?: Record<string, string[]>;
}

// Auth
export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiresIn: number;
}

export interface TokenPayload {
  tenant_id: string;
  org_id: string;
  sub: string;
  name: string;
  roles: string[];
  iat: number;
  exp: number;
}

// Common entity
export interface BaseEntity {
  id: string;
  createdUtc: string;
  modifiedUtc: string;
}
```

**src/api/types/Asset.ts**
```typescript
import { BaseEntity } from './Common';

export interface Asset extends BaseEntity {
  tenantId: string;
  bankOrganisationId: string;
  assetType: 'Motor' | 'NonMotor';
  assetIdentifier: string;
  registrationNumber?: string;
  financedAmount: number;
  borrowerReference?: string;
  borrowerId?: string;
  loanStartDate: string;
  loanEndDate: string;
  status: 'Active' | 'Settled' | 'Closed';
  policies?: Policy[];
  complianceState?: ComplianceState;
}

export interface Policy extends BaseEntity {
  tenantId: string;
  assetId?: string;
  insurerOrganisationId: string;
  policyNumber: string;
  productType: string;
  status: 'Active' | 'Lapsed' | 'Cancelled';
  effectiveDate: string;
  expiryDate: string;
  insuredValue: number;
  premiumAmount: number;
  paymentStatus: 'Paid' | 'Overdue' | 'Arrears';
  lastPremiumPaymentDate?: string;
}

export interface Borrower extends BaseEntity {
  tenantId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  idNumber: string;
}

export interface CreateAssetRequest {
  assetType: string;
  assetIdentifier: string;
  registrationNumber?: string;
  financedAmount: number;
  borrowerReference: string;
  loanStartDate: string;
  loanEndDate: string;
}

export interface UpdateAssetRequest extends Partial<CreateAssetRequest> {}

export interface AssetFilterParams extends PaginationParams {
  status?: string;
  assetType?: string;
  fromDate?: string;
  toDate?: string;
  borrowerReference?: string;
}
```

**src/api/types/Compliance.ts**
```typescript
import { BaseEntity } from './Common';

export interface ComplianceState extends BaseEntity {
  tenantId: string;
  assetId: string;
  status: 'Compliant' | 'NonCompliant' | 'Pending' | 'Unknown';
  nonComplianceReason?: string;
  lastEvaluatedUtc?: string;
  lastChangedUtc?: string;
  activeCaseId?: string;
}

export interface ComplianceDecision extends BaseEntity {
  tenantId: string;
  assetId: string;
  oldStatus: string;
  newStatus: string;
  decisionReason: string;
  ruleSetVersion?: string;
}

export interface ComplianceDetailDto {
  state: ComplianceState;
  asset: Asset;
  policies: Policy[];
  recentDecisions: ComplianceDecision[];
  activeCases: NonComplianceCase[];
}

export interface ComplianceFilterParams extends PaginationParams {
  status?: string;
  fromDate?: string;
  toDate?: string;
}
```

**src/api/types/Case.ts**
```typescript
import { BaseEntity } from './Common';

export interface NonComplianceCase extends BaseEntity {
  tenantId: string;
  assetId: string;
  complianceStateId: string;
  caseNumber: string;
  nonComplianceReason: string;
  status: 'Open' | 'InProgress' | 'Escalated' | 'Resolved' | 'Closed';
  severity: 'Low' | 'Medium' | 'High';
  assignedToUserId?: string;
  dueDate?: string;
  resolvedDate?: string;
  tasks?: CaseTask[];
  events?: CaseEvent[];
}

export interface CaseTask extends BaseEntity {
  tenantId: string;
  caseId: string;
  taskType: string;
  status: 'Pending' | 'InProgress' | 'Completed' | 'Failed';
  description?: string;
  retryCount: number;
}

export interface CaseEvent extends BaseEntity {
  tenantId: string;
  caseId: string;
  eventType: string;
  description: string;
}

export interface EscalateCaseRequest {
  reason: string;
}

export interface CloseCaseRequest {
  resolutionNotes: string;
}

export interface CaseFilterParams extends PaginationParams {
  status?: string;
  severity?: string;
  assignedToUserId?: string;
}
```

### 1.2 API Client Setup

**src/api/utils/api.ts**
```typescript
import axios, { AxiosInstance, AxiosError, AxiosRequestConfig } from 'axios';
import { authStore } from '../../stores/authStore';
import { ErrorResponse } from '../types/Common';

export const API: AxiosInstance = axios.create({
  baseURL: process.env.REACT_APP_API_URL || 'http://localhost:5000/api/v1',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor
API.interceptors.request.use(
  (config) => {
    // Add authorization header
    const token = authStore.getState().token;
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    // Add correlation ID
    config.headers['X-Correlation-Id'] = generateCorrelationId();

    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor
API.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ErrorResponse>) => {
    // Handle 401 (unauthorized)
    if (error.response?.status === 401) {
      authStore.getState().logout();
      window.location.href = '/login';
      return Promise.reject(error);
    }

    // Handle other errors
    console.error('API Error:', error.response?.data || error.message);
    return Promise.reject(error);
  }
);

export function generateCorrelationId(): string {
  return `${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
}

export interface ApiResponse<T> {
  data: T;
  status: number;
}

export async function handleApiError(
  error: unknown
): Promise<ErrorResponse | null> {
  if (axios.isAxiosError(error)) {
    return error.response?.data as ErrorResponse;
  }
  return null;
}
```

### 1.3 Client Implementations

**src/api/clients/authClient.ts**
```typescript
import { API } from '../utils/api';
import { LoginRequest, LoginResponse } from '../types/Common';

export const authClient = {
  login: async (request: LoginRequest): Promise<LoginResponse> => {
    const response = await API.post<LoginResponse>('/auth/login', request);
    return response.data;
  },

  logout: async (): Promise<void> => {
    // Optional: notify backend
    await API.post('/auth/logout');
  },
};
```

**src/api/clients/assetClient.ts**
```typescript
import { API } from '../utils/api';
import {
  Asset,
  CreateAssetRequest,
  UpdateAssetRequest,
  AssetFilterParams,
  PaginatedResponse,
} from '../types';

export const assetClient = {
  getAssets: async (
    params: AssetFilterParams
  ): Promise<PaginatedResponse<Asset>> => {
    const response = await API.get<PaginatedResponse<Asset>>('/assets', {
      params: {
        page: params.page || 1,
        pageSize: params.pageSize || 25,
        status: params.status,
        assetType: params.assetType,
        fromDate: params.fromDate,
        toDate: params.toDate,
      },
    });
    return response.data;
  },

  getAsset: async (id: string): Promise<Asset> => {
    const response = await API.get<Asset>(`/assets/${id}`);
    return response.data;
  },

  createAsset: async (data: CreateAssetRequest): Promise<Asset> => {
    const response = await API.post<Asset>('/assets', data);
    return response.data;
  },

  updateAsset: async (id: string, data: UpdateAssetRequest): Promise<Asset> => {
    const response = await API.put<Asset>(`/assets/${id}`, data);
    return response.data;
  },

  deleteAsset: async (id: string): Promise<void> => {
    await API.delete(`/assets/${id}`);
  },

  importAssets: async (file: File): Promise<{ jobId: string }> => {
    const formData = new FormData();
    formData.append('file', file);
    const response = await API.post<{ jobId: string }>('/assets/import', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
    return response.data;
  },
};
```

**src/api/clients/complianceClient.ts**
```typescript
import { API } from '../utils/api';
import {
  ComplianceState,
  ComplianceDecision,
  ComplianceDetailDto,
  ComplianceFilterParams,
  PaginatedResponse,
} from '../types';

export const complianceClient = {
  getComplianceStates: async (
    params: ComplianceFilterParams
  ): Promise<PaginatedResponse<ComplianceState>> => {
    const response = await API.get<PaginatedResponse<ComplianceState>>(
      '/compliance/assets',
      { params }
    );
    return response.data;
  },

  getAssetCompliance: async (assetId: string): Promise<ComplianceDetailDto> => {
    const response = await API.get<ComplianceDetailDto>(
      `/compliance/assets/${assetId}`
    );
    return response.data;
  },

  getDecisionHistory: async (
    assetId: string,
    params: ComplianceFilterParams
  ): Promise<PaginatedResponse<ComplianceDecision>> => {
    const response = await API.get<PaginatedResponse<ComplianceDecision>>(
      `/compliance/assets/${assetId}/decisions`,
      { params }
    );
    return response.data;
  },
};
```

**src/api/clients/caseClient.ts**
```typescript
import { API } from '../utils/api';
import {
  NonComplianceCase,
  CaseFilterParams,
  EscalateCaseRequest,
  CloseCaseRequest,
  PaginatedResponse,
} from '../types';

export const caseClient = {
  getCases: async (
    params: CaseFilterParams
  ): Promise<PaginatedResponse<NonComplianceCase>> => {
    const response = await API.get<PaginatedResponse<NonComplianceCase>>(
      '/cases',
      { params }
    );
    return response.data;
  },

  getCase: async (id: string): Promise<NonComplianceCase> => {
    const response = await API.get<NonComplianceCase>(`/cases/${id}`);
    return response.data;
  },

  escalateCase: async (
    id: string,
    request: EscalateCaseRequest
  ): Promise<void> => {
    await API.post(`/cases/${id}/actions/escalate`, request);
  },

  closeCase: async (
    id: string,
    request: CloseCaseRequest
  ): Promise<void> => {
    await API.post(`/cases/${id}/actions/close`, request);
  },
};
```

---

## PART 2: STATE MANAGEMENT (ZUSTAND)

**src/stores/authStore.ts**
```typescript
import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { TokenPayload } from '../api/types/Common';
import jwt_decode from 'jwt-decode';

interface AuthState {
  token: string | null;
  user: TokenPayload | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;

  setToken: (token: string) => void;
  logout: () => void;
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      token: null,
      user: null,
      isAuthenticated: false,
      isLoading: false,
      error: null,

      setToken: (token: string) => {
        try {
          const decoded = jwt_decode<TokenPayload>(token);
          set({
            token,
            user: decoded,
            isAuthenticated: true,
            error: null,
          });
        } catch (error) {
          set({ error: 'Invalid token' });
        }
      },

      logout: () => {
        set({
          token: null,
          user: null,
          isAuthenticated: false,
        });
      },

      setLoading: (loading: boolean) => set({ isLoading: loading }),
      setError: (error: string | null) => set({ error }),
    }),
    {
      name: 'auth-storage',
      partialize: (state) => ({ token: state.token, user: state.user }),
    }
  )
);
```

**src/stores/tenantStore.ts**
```typescript
import { create } from 'zustand';

interface TenantState {
  tenantId: string | null;
  orgId: string | null;
  tenantType: 'Bank' | 'Insurer' | 'Broker' | 'Admin' | null;

  setTenant: (tenantId: string, orgId: string, type: string) => void;
  clearTenant: () => void;
}

export const useTenantStore = create<TenantState>((set) => ({
  tenantId: null,
  orgId: null,
  tenantType: null,

  setTenant: (tenantId: string, orgId: string, type: string) => {
    set({
      tenantId,
      orgId,
      tenantType: type as any,
    });
  },

  clearTenant: () => {
    set({ tenantId: null, orgId: null, tenantType: null });
  },
}));
```

---

## PART 3: HOOKS

**src/hooks/useAuth.ts**
```typescript
import { useAuthStore } from '../stores/authStore';
import { authClient } from '../api/clients/authClient';
import { LoginRequest } from '../api/types/Common';

export const useAuth = () => {
  const { token, user, isAuthenticated, setToken, logout, setLoading, setError } =
    useAuthStore();

  const login = async (request: LoginRequest) => {
    setLoading(true);
    try {
      const response = await authClient.login(request);
      setToken(response.token);
      return true;
    } catch (error: any) {
      setError(error.message);
      return false;
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    logout();
  };

  return {
    token,
    user,
    isAuthenticated,
    login,
    logout: handleLogout,
  };
};
```

**src/hooks/usePagination.ts**
```typescript
import { useState, useCallback } from 'react';

interface UsePaginationProps {
  initialPage?: number;
  initialPageSize?: number;
}

export const usePagination = ({
  initialPage = 1,
  initialPageSize = 25,
}: UsePaginationProps = {}) => {
  const [page, setPage] = useState(initialPage);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const [totalPages, setTotalPages] = useState(0);
  const [totalItems, setTotalItems] = useState(0);

  const handlePageChange = useCallback((newPage: number) => {
    setPage(Math.max(1, newPage));
  }, []);

  const handlePageSizeChange = useCallback((newSize: number) => {
    setPageSize(Math.min(newSize, 100)); // Max 100
    setPage(1); // Reset to first page
  }, []);

  return {
    page,
    pageSize,
    totalPages,
    totalItems,
    setTotalPages,
    setTotalItems,
    handlePageChange,
    handlePageSizeChange,
  };
};
```

---

## PART 4: COMPONENTS

### 4.1 Layout

**src/components/layouts/MainLayout.tsx**
```typescript
import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import NavigationBar from './NavigationBar';

interface MainLayoutProps {
  children: React.ReactNode;
}

export const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  const { isAuthenticated } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return (
    <div className="flex h-screen bg-gray-100">
      <NavigationBar />
      <main className="flex-1 overflow-auto">
        <div className="p-8">{children}</div>
      </main>
    </div>
  );
};

export default MainLayout;
```

**src/components/layouts/NavigationBar.tsx**
```typescript
import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { useTenantStore } from '../../stores/tenantStore';

const NavigationBar: React.FC = () => {
  const { user, logout } = useAuth();
  const { tenantType } = useTenantStore();

  return (
    <nav className="w-64 bg-white shadow-sm">
      <div className="p-6">
        <h1 className="text-2xl font-bold text-blue-600">InsureX</h1>
        <p className="text-sm text-gray-600">{tenantType} Portal</p>
      </div>

      <div className="border-t">
        <ul className="p-4 space-y-2">
          <li>
            <Link to="/dashboard" className="block p-2 hover:bg-gray-100 rounded">
              Dashboard
            </Link>
          </li>
          <li>
            <Link to="/assets" className="block p-2 hover:bg-gray-100 rounded">
              Assets
            </Link>
          </li>
          <li>
            <Link to="/compliance" className="block p-2 hover:bg-gray-100 rounded">
              Compliance
            </Link>
          </li>
          <li>
            <Link to="/cases" className="block p-2 hover:bg-gray-100 rounded">
              Cases
            </Link>
          </li>
        </ul>
      </div>

      <div className="absolute bottom-0 left-0 right-0 p-4 border-t bg-white">
        <p className="text-sm text-gray-700">{user?.name}</p>
        <button
          onClick={logout}
          className="mt-2 w-full bg-red-600 text-white py-2 rounded hover:bg-red-700"
        >
          Logout
        </button>
      </div>
    </nav>
  );
};

export default NavigationBar;
```

### 4.2 Pages

**src/components/pages/LoginPage.tsx**
```typescript
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import LoginForm from '../forms/LoginForm';

const LoginPage: React.FC = () => {
  const navigate = useNavigate();
  const { login } = useAuth();
  const [error, setError] = useState<string | null>(null);

  const handleLogin = async (username: string, password: string) => {
    setError(null);
    const success = await login({ username, password });
    if (success) {
      navigate('/dashboard');
    } else {
      setError('Login failed. Please check your credentials.');
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <div className="bg-white p-12 rounded-lg shadow-lg max-w-md w-full">
        <h1 className="text-3xl font-bold text-blue-600 mb-6">InsureX</h1>
        <h2 className="text-xl font-semibold mb-6">Login</h2>

        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-6">
            {error}
          </div>
        )}

        <LoginForm onLogin={handleLogin} />
      </div>
    </div>
  );
};

export default LoginPage;
```

**src/components/pages/AssetsPage.tsx**
```typescript
import React, { useState, useEffect } from 'react';
import { assetClient } from '../../api/clients/assetClient';
import { Asset, AssetFilterParams } from '../../api/types';
import { usePagination } from '../../hooks/usePagination';
import AssetTable from '../tables/AssetTable';
import Pagination from '../shared/Pagination';
import AssetFilterForm from '../forms/AssetFilterForm';
import LoadingSpinner from '../shared/LoadingSpinner';

const AssetsPage: React.FC = () => {
  const [assets, setAssets] = useState<Asset[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const {
    page,
    pageSize,
    totalPages,
    setTotalPages,
    handlePageChange,
  } = usePagination();

  const [filters, setFilters] = useState<AssetFilterParams>({
    page: 1,
    pageSize: 25,
  });

  const loadAssets = async (params: AssetFilterParams) => {
    setLoading(true);
    setError(null);
    try {
      const response = await assetClient.getAssets({
        ...params,
        page,
        pageSize,
      });
      setAssets(response.items);
      setTotalPages(response.totalPages);
    } catch (err: any) {
      setError(err.message || 'Failed to load assets');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAssets(filters);
  }, [page, pageSize]);

  const handleFilter = (newFilters: AssetFilterParams) => {
    setFilters({ ...newFilters, page: 1, pageSize });
    handlePageChange(1);
  };

  return (
    <div className="space-y-6">
      <h1 className="text-3xl font-bold">Assets Register</h1>

      <AssetFilterForm onFilter={handleFilter} />

      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
          {error}
        </div>
      )}

      {loading ? (
        <LoadingSpinner />
      ) : (
        <>
          <AssetTable assets={assets} />
          <Pagination
            current={page}
            total={totalPages}
            onPageChange={handlePageChange}
          />
        </>
      )}
    </div>
  );
};

export default AssetsPage;
```

### 4.3 Shared Components

**src/components/shared/Pagination.tsx**
```typescript
import React from 'react';

interface PaginationProps {
  current: number;
  total: number;
  onPageChange: (page: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({ current, total, onPageChange }) => {
  const pages = Array.from({ length: Math.min(total, 5) }, (_, i) => {
    if (total <= 5) return i + 1;
    if (current <= 3) return i + 1;
    if (current >= total - 2) return total - 4 + i;
    return current - 2 + i;
  });

  return (
    <div className="flex justify-center items-center gap-2 py-4">
      <button
        onClick={() => onPageChange(current - 1)}
        disabled={current === 1}
        className="px-3 py-2 border rounded disabled:opacity-50"
      >
        Previous
      </button>

      {pages.map((page) => (
        <button
          key={page}
          onClick={() => onPageChange(page)}
          className={`px-3 py-2 border rounded ${
            page === current ? 'bg-blue-600 text-white' : 'hover:bg-gray-100'
          }`}
        >
          {page}
        </button>
      ))}

      <button
        onClick={() => onPageChange(current + 1)}
        disabled={current === total}
        className="px-3 py-2 border rounded disabled:opacity-50"
      >
        Next
      </button>

      <span className="text-gray-600">
        Page {current} of {total}
      </span>
    </div>
  );
};

export default Pagination;
```

**src/components/shared/StatusBadge.tsx**
```typescript
import React from 'react';

interface StatusBadgeProps {
  status: string;
  type?: 'compliance' | 'case' | 'asset' | 'policy';
}

const StatusBadge: React.FC<StatusBadgeProps> = ({ status, type = 'compliance' }) => {
  const statusMap: Record<string, { bg: string; text: string }> = {
    // Compliance
    Compliant: { bg: 'bg-green-100', text: 'text-green-800' },
    NonCompliant: { bg: 'bg-red-100', text: 'text-red-800' },
    Pending: { bg: 'bg-yellow-100', text: 'text-yellow-800' },
    Unknown: { bg: 'bg-gray-100', text: 'text-gray-800' },

    // Cases
    Open: { bg: 'bg-blue-100', text: 'text-blue-800' },
    InProgress: { bg: 'bg-purple-100', text: 'text-purple-800' },
    Escalated: { bg: 'bg-orange-100', text: 'text-orange-800' },
    Resolved: { bg: 'bg-green-100', text: 'text-green-800' },
    Closed: { bg: 'bg-gray-100', text: 'text-gray-800' },

    // Assets
    Active: { bg: 'bg-green-100', text: 'text-green-800' },
    Settled: { bg: 'bg-blue-100', text: 'text-blue-800' },

    // Policies
    Paid: { bg: 'bg-green-100', text: 'text-green-800' },
    Overdue: { bg: 'bg-orange-100', text: 'text-orange-800' },
    Arrears: { bg: 'bg-red-100', text: 'text-red-800' },
  };

  const config = statusMap[status] || { bg: 'bg-gray-100', text: 'text-gray-800' };

  return (
    <span className={`px-3 py-1 rounded-full text-sm font-medium ${config.bg} ${config.text}`}>
      {status}
    </span>
  );
};

export default StatusBadge;
```

**src/components/shared/LoadingSpinner.tsx**
```typescript
import React from 'react';

const LoadingSpinner: React.FC = () => (
  <div className="flex justify-center items-center py-12">
    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
  </div>
);

export default LoadingSpinner;
```

---

## PART 5: ROUTING

**src/App.tsx**
```typescript
import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './components/layouts/MainLayout';
import LoginPage from './components/pages/LoginPage';
import DashboardPage from './components/pages/DashboardPage';
import AssetsPage from './components/pages/AssetsPage';
import CompliancePage from './components/pages/CompliancePage';
import CasesPage from './components/pages/CasesPage';
import NotFoundPage from './components/pages/NotFoundPage';
import { useAuth } from './hooks/useAuth';

export const App: React.FC = () => {
  const { isAuthenticated } = useAuth();

  return (
    <Router>
      <Routes>
        <Route
          path="/login"
          element={isAuthenticated ? <Navigate to="/dashboard" /> : <LoginPage />}
        />
        <Route
          path="/dashboard"
          element={
            <MainLayout>
              <DashboardPage />
            </MainLayout>
          }
        />
        <Route
          path="/assets"
          element={
            <MainLayout>
              <AssetsPage />
            </MainLayout>
          }
        />
        <Route
          path="/compliance"
          element={
            <MainLayout>
              <CompliancePage />
            </MainLayout>
          }
        />
        <Route
          path="/cases"
          element={
            <MainLayout>
              <CasesPage />
            </MainLayout>
          }
        />
        <Route path="/404" element={<NotFoundPage />} />
        <Route path="*" element={<Navigate to="/404" />} />
      </Routes>
    </Router>
  );
};

export default App;
```

---

## PART 6: ENVIRONMENT CONFIGURATION

**.env.local** (create this file, add to .gitignore)
```
REACT_APP_API_URL=http://localhost:5000/api/v1
REACT_APP_ENV=development
```

**.env.production**
```
REACT_APP_API_URL=https://api.insurex.example.com/api/v1
REACT_APP_ENV=production
```

---

## PART 7: PACKAGE.JSON REFERENCE

```json
{
  "name": "insurex-frontend",
  "version": "1.0.0",
  "type": "module",
  "scripts": {
    "dev": "vite",
    "build": "tsc && vite build",
    "lint": "eslint . --ext ts,tsx --report-unused-disable-directives --max-warnings 0",
    "preview": "vite preview",
    "test": "vitest"
  },
  "dependencies": {
    "react": "^18.2.0",
    "react-dom": "^18.2.0",
    "react-router-dom": "^6.14.0",
    "axios": "^1.4.0",
    "zustand": "^4.3.7",
    "jwt-decode": "^3.1.2"
  },
  "devDependencies": {
    "@types/react": "^18.0.28",
    "@types/react-dom": "^18.0.11",
    "@vitejs/plugin-react": "^4.0.0",
    "vite": "^4.3.9",
    "typescript": "^5.0.2",
    "tailwindcss": "^3.3.0",
    "postcss": "^8.4.24",
    "autoprefixer": "^10.4.14",
    "@tailwindcss/typography": "^0.5.9"
  }
}
```

---

## DEVELOPMENT CHECKLIST

- [ ] Set up React + TypeScript project
- [ ] Create API types and client functions
- [ ] Implement authentication store + login
- [ ] Build main layout with navigation
- [ ] Create assets page + table
- [ ] Create compliance page + table
- [ ] Create cases page + table
- [ ] Implement pagination + filtering
- [ ] Add status badges + formatting
- [ ] Set up routing
- [ ] Style with Tailwind CSS
- [ ] Test API integration
- [ ] Build production bundle
- [ ] Deploy to App Service / Vercel / etc.

---

**Document Version:** 1.0  
**Last Updated:** January 2026
