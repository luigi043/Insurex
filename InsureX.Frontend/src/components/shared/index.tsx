import React from 'react';

// --- Status Badge ---
interface StatusBadgeProps {
    status: string;
    type?: 'compliance' | 'case' | 'priority';
}

export const StatusBadge: React.FC<StatusBadgeProps> = ({ status, type = 'compliance' }) => {
    const getStyles = () => {
        const s = status.toLowerCase();

        if (type === 'priority') {
            if (s === 'critical' || s === 'high') return 'bg-red-100 text-red-700 border-red-200';
            if (s === 'medium') return 'bg-amber-100 text-amber-700 border-amber-200';
            return 'bg-blue-100 text-blue-700 border-blue-200';
        }

        if (s === 'compliant' || s === 'resolved' || s === 'active' || s === 'closed')
            return 'bg-emerald-100 text-emerald-700 border-emerald-200';
        if (s === 'noncompliant' || s === 'open' || s === 'escalated' || s === 'overdue')
            return 'bg-red-100 text-red-700 border-red-200';

        return 'bg-gray-100 text-gray-700 border-gray-200';
    };

    return (
        <span className= {`px-2.5 py-1 rounded-full text-[10px] font-bold uppercase tracking-wider border ${getStyles()}`
}>
    { status }
    </span>
  );
};

// --- Generic Table ---
interface Column<T> {
    header: string;
    key: keyof T | string;
    render?: (item: T) => React.ReactNode;
}

interface TableProps<T> {
    columns: Column<T>[];
    data: T[];
    loading?: boolean;
    onRowClick?: (item: T) => void;
}

export function DataTable<T>({ columns, data, loading, onRowClick }: TableProps<T>) {
    if (loading) {
        return (
            <div className= "w-full space-y-4" >
            {
                [1, 2, 3].map((i) => (
                    <div key= { i } className = "h-16 w-full bg-gray-50 animate-pulse rounded-2xl" > </div>
                ))
            }
            </div>
    );
    }

    return (
        <div className= "overflow-hidden border border-gray-100 rounded-[2rem] bg-white shadow-sm" >
        <table className="w-full text-left border-collapse" >
            <thead className="bg-gray-50/50 border-b border-gray-100" >
                <tr>
                {
                    columns.map((col, i) => (
                        <th key= { i } className = "px-6 py-4 text-[11px] font-black text-gray-400 uppercase tracking-[0.2em]" >
                        { col.header }
                        </th>
                    ))
                }
                </tr>
                </thead>
                < tbody className = "divide-y divide-gray-50" >
                {
                    data.length > 0 ? (
                        data.map((item, i) => (
                            <tr 
                                key= { i } 
                                className = {`transition-colors group ${onRowClick ? 'cursor-pointer hover:bg-blue-50/50' : 'hover:bg-gray-50/30'}`}
                                onClick = {() => onRowClick && onRowClick(item)}
                            >
                            {
                                columns.map((col, j) => (
                                    <td key= { j } className = "px-6 py-4 text-sm font-medium text-gray-600" >
                                    { col.render ? col.render(item) : (item[col.key as keyof T] as unknown as string) }
                                    </td>
                                ))}
                    </tr>
            ))
          ) : (
        <tr>
        <td colSpan= { columns.length } className = "px-6 py-12 text-center text-gray-400 italic" >
            No records found.
              </td>
                </tr>
          )
}
</tbody>
    </table>
    </div>
  );
}

// --- Pagination ---
interface PaginationProps {
    page: number;
    totalPages: number;
    onPageChange: (page: number) => void;
}

export const Pagination: React.FC<PaginationProps> = ({ page, totalPages, onPageChange }) => {
    if (totalPages <= 1) return null;

    return (
        <div className= "flex items-center justify-center gap-2 mt-8" >
        <button
        disabled={ page === 1 }
    onClick = {() => onPageChange(page - 1)}
className = "px-4 py-2 rounded-xl border border-gray-200 text-sm font-bold text-gray-600 hover:bg-gray-50 disabled:opacity-30 disabled:cursor-not-allowed transition-all"
    >
    Previous
    </button>
    < span className = "text-sm font-bold text-gray-400 mx-2" >
        Page < span className = "text-blue-600" > { page } </span> of {totalPages}
            </span>
            < button
disabled = { page === totalPages}
onClick = {() => onPageChange(page + 1)}
className = "px-4 py-2 rounded-xl border border-gray-200 text-sm font-bold text-gray-600 hover:bg-gray-50 disabled:opacity-30 disabled:cursor-not-allowed transition-all"
    >
    Next
    </button>
    </div>
  );
};
