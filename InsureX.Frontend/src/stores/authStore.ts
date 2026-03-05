import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { jwtDecode } from 'jwt-decode';
import type { TokenPayload } from '../api/types/Common';

interface AuthState {
    token: string | null;
    user: TokenPayload | null;
    isAuthenticated: boolean;

    setToken: (token: string) => void;
    logout: () => void;
}

export const useAuthStore = create<AuthState>()(
    persist(
        (set) => ({
            token: null,
            user: null,
            isAuthenticated: false,

            setToken: (token: string) => {
                try {
                    const decoded = jwtDecode<TokenPayload>(token);
                    set({
                        token,
                        user: decoded,
                        isAuthenticated: true,
                    });
                } catch (error) {
                    console.error('Failed to decode token:', error);
                    set({ token: null, user: null, isAuthenticated: false });
                }
            },

            logout: () => {
                set({
                    token: null,
                    user: null,
                    isAuthenticated: false,
                });
            },
        }),
        {
            name: 'insurex-auth-storage',
        }
    )
);
