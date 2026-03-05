import axios from 'axios';
import type { AxiosInstance, AxiosError } from 'axios';
import { useAuthStore } from '../../stores/authStore';

// Error response interface (matching backend ApiResponse)
export interface ErrorResponse {
    success: boolean;
    message: string;
    correlationId?: string;
}

export const API: AxiosInstance = axios.create({
    // Targeting the IAPR_API endpoint (placeholder URL until dev server is specified)
    baseURL: import.meta.env.VITE_API_URL || (import.meta.env.PROD ? '/api' : 'http://localhost:5000/api'),
    timeout: 15000,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Request interceptor: Inject Bearer token
API.interceptors.request.use(
    (config) => {
        const token = useAuthStore.getState().token;
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        // Add correlation ID for tracing
        config.headers['X-Correlation-Id'] = `${Date.now()}-${Math.random().toString(36).substring(2, 9)}`;

        return config;
    },
    (error) => Promise.reject(error)
);

// Response interceptor: Handle errors and token expiry
API.interceptors.response.use(
    (response) => response,
    (error: AxiosError<ErrorResponse>) => {
        // Handle 401 Unauthorized (token expired or invalid)
        if (error.response?.status === 401) {
            useAuthStore.getState().logout();
            window.location.href = '/login';
        }

        const errorMessage = error.response?.data?.message || error.message || 'An unexpected error occurred';
        console.error('API Error:', errorMessage);

        return Promise.reject(error);
    }
);



