import React, { useEffect, useState } from 'react';
import { intelligenceClient } from '../../api/clients';
import { 
    Zap, ArrowRight,
    TrendingUp, ShieldAlert, Sparkles
} from 'lucide-react';
import { Link } from 'react-router-dom';

const InsightsSummary: React.FC = () => {
    const [insights, setInsights] = useState<any[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchInsights = async () => {
            try {
                const response = await intelligenceClient.getInsights();
                if (response.success) {
                    setInsights(response.data);
                }
            } catch (error) {
                console.error('Failed to fetch insights:', error);
            } finally {
                setLoading(false);
            }
        };
        fetchInsights();
    }, []);

    if (loading) return (
        <div className="bg-white rounded-3xl p-6 border border-gray-100 shadow-sm animate-pulse">
            <div className="h-4 bg-gray-100 rounded w-1/4 mb-4"></div>
            <div className="space-y-3">
                <div className="h-12 bg-gray-50 rounded-2xl"></div>
                <div className="h-12 bg-gray-50 rounded-2xl"></div>
            </div>
        </div>
    );

    if (insights.length === 0) return null;

    return (
        <div className="bg-gradient-to-br from-blue-600 to-indigo-700 rounded-[2rem] p-8 text-white shadow-2xl shadow-blue-200 overflow-hidden relative group">
            <div className="absolute top-0 right-0 p-8 opacity-10 group-hover:scale-110 transition-transform duration-500">
                <Sparkles className="w-32 h-32" />
            </div>

            <div className="relative z-10">
                <div className="flex items-center gap-3 mb-6">
                    <div className="w-10 h-10 bg-white/20 rounded-xl flex items-center justify-center backdrop-blur-md">
                        <Zap className="w-5 h-5 text-yellow-300 fill-yellow-300" />
                    </div>
                    <div>
                        <h3 className="text-xl font-black tracking-tight leading-none">Intelligence Insights</h3>
                        <p className="text-blue-100 text-xs font-bold mt-1 uppercase tracking-widest opacity-80">Proactive Compliance Guard</p>
                    </div>
                </div>

                <div className="space-y-4">
                    {insights.map((insight, idx) => (
                        <div key={idx} className="bg-white/10 hover:bg-white/15 backdrop-blur-lg border border-white/10 rounded-2xl p-4 transition-all flex items-start gap-4 group/item">
                            <div className={`w-10 h-10 rounded-xl flex items-center justify-center shrink-0 ${
                                insight.Severity === 'High' ? 'bg-red-400' : 'bg-amber-400'
                            }`}>
                                <ShieldAlert className="w-5 h-5 text-white" />
                            </div>
                            <div className="flex-1">
                                <p className="text-sm font-black leading-tight mb-1">{insight.Message}</p>
                                <div className="flex items-center gap-4 text-[10px] font-bold uppercase tracking-widest text-blue-100">
                                    <span className="flex items-center gap-1">
                                        <TrendingUp className="w-3 h-3" /> Potential Risk: {insight.Type}
                                    </span>
                                    <Link 
                                        to={`/assets/${insight.AssetId}`}
                                        className="flex items-center gap-1 text-white hover:underline"
                                    >
                                        Take Action <ArrowRight className="w-3 h-3 group-hover/item:translate-x-1 transition-transform" />
                                    </Link>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>

                <div className="mt-8 flex items-center justify-between">
                    <div className="flex -space-x-3">
                        {[1, 2, 3].map(i => (
                            <div key={i} className="w-8 h-8 rounded-full border-2 border-blue-600 bg-blue-400/50 flex items-center justify-center text-[10px] font-black">AI</div>
                        ))}
                    </div>
                    <p className="text-[10px] font-black uppercase tracking-[0.2em] text-blue-200">Processing real-time insurer signals</p>
                </div>
            </div>
        </div>
    );
};

export default InsightsSummary;
