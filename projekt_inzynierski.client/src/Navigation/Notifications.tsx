import React, { useEffect, useState } from "react";

interface Notification {
    id: string;
    message: string;
    createdAt: string;
}

interface PagedResult<T> {
    items: T[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
}

const Notifications: React.FC = () => {
    const [notifications, setNotifications] = useState<Notification[]>([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalCount, setTotalCount] = useState(0);
    const pageSize = 6;
    const API_URL = import.meta.env.VITE_API_URL;
    const fetchNotifications = async (page: number) => {
        const res = await fetch(
            `${API_URL}/api/Notification/paged?pageNumber=${page}&pageSize=${pageSize}`,
            { method: "GET", credentials: "include" }
        );

        if (res.ok) {
            const data: PagedResult<Notification> = await res.json();
            setNotifications(data.items);
            setTotalCount(data.totalCount);
        } else {
            alert("Błąd pobierania notyfikacji");
        }
    };

    const joinCourse = async (notificationId: string) => {
        const confirmed = window.confirm("Czy na pewno chcesz dołączyć do kursu?");
        if (!confirmed) return;

        const res = await fetch(
            `${API_URL}/UserCourses/JoinCourseFromNotification?notificationId=${notificationId}`,
            { method: "POST", credentials: "include" }
        );

        if (res.ok) {
            alert("Pomyślnie zapisano na kurs!");
            
            setNotifications((prev) => prev.filter((n) => n.id !== notificationId));
            setTotalCount((prev) => prev - 1);
        } else {
            alert("Wystąpił błąd podczas zapisu na kurs.");
        }
    };

    useEffect(() => {
        fetchNotifications(pageNumber);
    }, [pageNumber]);

    const totalPages = Math.ceil(totalCount / pageSize);

    return (
        <div className="mx-auto max-w-4xl p-4">
            <h2 className="mb-4 text-xl font-bold text-white">Powiadomienia</h2>

            <ul className="grid grid-cols-1 gap-4 md:grid-cols-3">
                {notifications.map((n) => (
                    <li key={n.id} className="rounded-lg border bg-white p-3 shadow">
                        <p>{n.message}</p>
                        <span className="text-sm text-gray-500">
                            {new Date(n.createdAt).toLocaleString()}
                        </span>
                        <button
                            onClick={() => joinCourse(n.id)}
                            className="mt-2 w-full rounded bg-cyan-500 px-3 py-1 text-white hover:bg-cyan-600"
                        >
                            Dołącz do kursu
                        </button>
                    </li>
                ))}
            </ul>

            <div className="mt-4 flex items-center justify-between">
                <button
                    disabled={pageNumber <= 1}
                    onClick={() => setPageNumber((p) => p - 1)}
                    className="px-3 py-1 rounded bg-gray-200 disabled:opacity-50"
                >
                    Poprzednia
                </button>
                <span className="text-cyan-300">
                    Strona {pageNumber} z {totalPages}
                </span>
                <button
                    disabled={pageNumber >= totalPages}
                    onClick={() => setPageNumber((p) => p + 1)}
                    className="px-3 py-1 rounded bg-gray-200 disabled:opacity-50"
                >
                    Następna
                </button>
            </div>
        </div>
    );
};

export default Notifications;
