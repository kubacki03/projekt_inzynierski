import React, { useState } from "react";

interface NotificationDto {
    title: string;
    message: string;
    createdAt: string;
}

interface NotificationsDropdownProps {
    notifications: NotificationDto[];
}

const NotificationsDropdown: React.FC<NotificationsDropdownProps> = React.memo(({ notifications }) => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <div className="relative">
            <button onClick={() => setIsOpen(!isOpen)} className="relative">
                🔔
                {notifications.length > 0 && (
                    <span className="absolute -top-1 -right-1 rounded-full bg-red-500 px-1 text-xs text-white">
                        {notifications.length}
                    </span>
                )}
            </button>
            {isOpen && (
                <ul className="absolute right-0 mt-2 max-h-80 w-64 overflow-y-auto rounded border bg-white shadow-lg">
                    {notifications.length > 0 ? (
                        notifications.map((note, i) => (
                            <li key={i} className="px-4 py-2 text-sm hover:bg-gray-100">
                                <div className="font-semibold">{note.title}</div>
                                <div>{note.message}</div>
                                <div className="text-xs text-gray-500">
                                    {new Date(note.createdAt).toLocaleString()}
                                </div>
                            </li>
                        ))
                    ) : (
                        <li className="px-4 py-2 text-gray-500">Brak powiadomień</li>
                    )}
                    <li className="border-t py-2 text-center">
                        <a href="/notifications" className="text-sm text-blue-500 hover:underline">
                            Wszystkie powiadomienia
                        </a>
                    </li>
                </ul>
            )}
        </div>
    );
});

export default NotificationsDropdown;
