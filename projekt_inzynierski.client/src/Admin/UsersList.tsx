import axios from "axios";
import React, { useEffect, useState } from "react";

interface User {
    id: string | number;
    nickname: string;
    points: number;
    banned: boolean;
}

interface PagedResult<T> {
    items: T[];
    totalCount: number;
    page: number;
    pageSize: number;
}

const UsersList: React.FC = () => {
    const [users, setUsers] = useState<User[]>([]);
    const [page, setPage] = useState<number>(1);
    const [pageSize] = useState<number>(5);
    const [totalCount, setTotalCount] = useState<number>(0);
    const [loading, setLoading] = useState<boolean>(false);
    const API_URL = import.meta.env.VITE_API_URL;

    const totalPages = Math.ceil(totalCount / pageSize);

    const fetchUsers = async (page: number) => {
        setLoading(true);
        try {
            const res = await fetch(`${API_URL}/api/admin/users?page=${page}&pageSize=${pageSize}`);
            if (!res.ok) {
                throw new Error("Błąd podczas pobierania użytkowników");
            }
            const data: PagedResult<User> = await res.json();
            setUsers(data.items);
            setTotalCount(data.totalCount);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    const toggleBan = async (user: User) => {
        try {
            if (user.banned) {
                await axios.post(`${API_URL}/api/admin/users/${user.id}/unban`);
            } else {
                await axios.post(`${API_URL}/api/admin/users/${user.id}/ban`);
            }
            fetchUsers(page);
        } catch (error) {
            console.error("Błąd podczas zmiany statusu użytkownika:", error);
        }
    };

    useEffect(() => {
        fetchUsers(page);
    }, [page]);

    const formatUserId = (id: string | number): string => {
        if (typeof id === 'string') {
            return id.substring(0, 8) + '...';
        } else {
            return `ID:${id}`.substring(0, 8) + '...';
        }
    };

    const getUserInitial = (nickname: string): string => {
        return nickname.charAt(0).toUpperCase();
    };

    return (
        <div className="p-1">
            <div className="mb-6 flex flex-col justify-between gap-4 sm:flex-row sm:items-center">
                <div>
                    <p className="text-gray-400">Łącznie {totalCount} użytkowników</p>
                </div>
                <div className="flex items-center space-x-2 rounded-lg bg-gray-700 px-4 py-2">
                    <div className="flex h-6 w-6 items-center justify-center rounded-full bg-gray-600 text-white">
                        👤
                    </div>
                    <span className="text-gray-300">Strona {page} z {totalPages}</span>
                </div>
            </div>

            {loading ? (
                <div className="flex items-center justify-center py-12">
                    <div className="border-t-2 h-12 w-12 animate-spin rounded-full border-b-2 border-blue-500"></div>
                </div>
            ) : (
                <>
                    <div className="overflow-hidden rounded-xl bg-gray-800 shadow-lg">
                        <div className="overflow-x-auto">
                            <table className="w-full">
                                <thead className="bg-gray-700">
                                    <tr>
                                        <th className="px-6 py-4 text-left font-semibold tracking-wider text-gray-300 uppercase">
                                            <div className="flex items-center">
                                                <span className="mr-2">👤</span>
                                                Użytkownik
                                            </div>
                                        </th>
                                        <th className="px-6 py-4 text-left font-semibold tracking-wider text-gray-300 uppercase">
                                            <div className="flex items-center">
                                                <span className="mr-2">⭐</span>
                                                Punkty
                                            </div>
                                        </th>
                                        <th className="px-6 py-4 text-left font-semibold tracking-wider text-gray-300 uppercase">
                                            Status
                                        </th>
                                        <th className="px-6 py-4 text-left font-semibold tracking-wider text-gray-300 uppercase">
                                            Akcje
                                        </th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-gray-700">
                                    {users.length === 0 ? (
                                        <tr>
                                            <td colSpan={4} className="py-8 text-center text-gray-400">
                                                Brak użytkowników do wyświetlenia
                                            </td>
                                        </tr>
                                    ) : (
                                        users.map((u) => (
                                            <tr key={u.id} className="transition-colors duration-150 hover:bg-gray-750">
                                                <td className="px-6 py-4">
                                                    <div className="flex items-center">
                                                        <div className="bg-gradient-to-br flex h-10 w-10 flex-shrink-0 items-center justify-center rounded-full from-blue-500 to-purple-200 font-bold text-white">
                                                            {getUserInitial(u.nickname)}
                                                        </div>
                                                        <div className="ml-4">
                                                            <div className="font-medium text-white">{u.nickname}</div>
                                                            <div className="text-sm text-gray-400">{formatUserId(u.id)}</div>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className="px-6 py-4">
                                                    <div className="flex items-center">
                                                        <div className="bg-gradient-to-r rounded-full from-yellow-600 to-yellow-500 px-4 py-1 font-bold text-white">
                                                            {u.points} pkt
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className="px-6 py-4">
                                                    <div className={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium ${u.banned
                                                        ? "bg-red-900/30 text-red-400"
                                                        : "bg-green-900/30 text-green-400"
                                                        }`}>
                                                        <span className={`w-2 h-2 rounded-full mr-2 ${u.banned ? "bg-red-400" : "bg-green-400"}`}></span>
                                                        {u.banned ? "Zbanowany" : "Aktywny"}
                                                    </div>
                                                </td>
                                                <td className="px-6 py-4">
                                                    <button
                                                        onClick={() => toggleBan(u)}
                                                        className={`px-4 py-2 rounded-lg font-medium transition-all duration-200 transform hover:scale-105 active:scale-95 ${u.banned
                                                            ? "bg-gradient-to-r from-green-600 to-green-700 hover:from-green-700 hover:to-green-800"
                                                            : "bg-gradient-to-r from-red-600 to-red-700 hover:from-red-700 hover:to-red-800"
                                                            }`}
                                                    >
                                                        {u.banned ? (
                                                            <div className="flex items-center">
                                                                <span className="mr-2">✅</span>
                                                                Odbanuj
                                                            </div>
                                                        ) : (
                                                            <div className="flex items-center">
                                                                <span className="mr-2">🚫</span>
                                                                Banuj
                                                            </div>
                                                        )}
                                                    </button>
                                                </td>
                                            </tr>
                                        ))
                                    )}
                                </tbody>
                            </table>
                        </div>
                    </div>

                    {totalPages > 1 && (
                        <div className="mt-6 flex flex-col items-center justify-between gap-4 rounded-xl bg-gray-800 p-4 md:flex-row">
                            <div className="text-gray-400">
                                Pokazano {(page - 1) * pageSize + 1} - {Math.min(page * pageSize, totalCount)} z {totalCount}
                            </div>
                            <div className="flex flex-col items-center space-y-2 sm:flex-row sm:space-y-0 sm:space-x-2">
                                <button
                                    disabled={page === 1}
                                    onClick={() => setPage((prev) => prev - 1)}
                                    className={`flex items-center px-4 py-2 rounded-lg transition-all duration-200 ${page === 1
                                        ? "bg-gray-700 text-gray-500 cursor-not-allowed"
                                        : "bg-gray-700 text-white hover:bg-gray-600 hover:scale-105"
                                        }`}
                                >
                                    <span className="mr-2">←</span>
                                    Poprzednia
                                </button>

                                <div className="flex items-center space-x-1">
                                    {Array.from({ length: Math.min(5, totalPages) }, (_, i) => {
                                        let pageNum;
                                        if (totalPages <= 5) {
                                            pageNum = i + 1;
                                        } else if (page <= 3) {
                                            pageNum = i + 1;
                                        } else if (page >= totalPages - 2) {
                                            pageNum = totalPages - 4 + i;
                                        } else {
                                            pageNum = page - 2 + i;
                                        }

                                        return (
                                            <button
                                                key={pageNum}
                                                onClick={() => setPage(pageNum)}
                                                className={`w-10 h-10 rounded-lg font-medium transition-all duration-200 ${page === pageNum
                                                    ? "bg-gradient-to-r from-blue-600 to-blue-700 text-white scale-105"
                                                    : "bg-gray-700 text-gray-300 hover:bg-gray-600"
                                                    }`}
                                            >
                                                {pageNum}
                                            </button>
                                        );
                                    })}
                                </div>

                                <button
                                    disabled={page === totalPages}
                                    onClick={() => setPage((prev) => prev + 1)}
                                    className={`flex items-center px-4 py-2 rounded-lg transition-all duration-200 ${page === totalPages
                                        ? "bg-gray-700 text-gray-500 cursor-not-allowed"
                                        : "bg-gray-700 text-white hover:bg-gray-600 hover:scale-105"
                                        }`}
                                >
                                    Następna
                                    <span className="ml-2">→</span>
                                </button>
                            </div>
                        </div>
                    )}
                </>
            )}
        </div>
    );
};

export default UsersList;