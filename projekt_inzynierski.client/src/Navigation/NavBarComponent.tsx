/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable @typescript-eslint/no-unused-vars */
import  { useState, useEffect } from "react";
import { useAuth } from "../Auth/AuthContext";

import axios from "axios";
import { Link } from "react-router-dom";

function NavBarComponent() {
    const { isLogged, logout, userRole } = useAuth();
    const [isCoursesOpen, setIsCoursesOpen] = useState(false);
    const [notifications] = useState<NotificationDto[]>([]);
    const [isNotifOpen, setIsNotifOpen] = useState(false);
    const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
    const [goldenPoints, setGoldenPoints] = useState<number>(0);
    const API_URL = import.meta.env.VITE_API_URL;

    interface NotificationDto {
        title: string;
        message: string;
        createdAt: string;
        type: string;
        relatedCourseId?: string;
    }
     
    useEffect(() => {
        if (!isLogged || userRole === "admin") return;

        const fetchPoints = async () => {
            try {
                const response = await axios.get(`${API_URL}/User/GetGoldenPoints`, {
                    withCredentials: true,
                });
                setGoldenPoints(response.data);
            } catch (err: any) {
                console.error(err);
            }
        };
         
        fetchPoints();
         
        const interval = setInterval(fetchPoints, 5000);

        return () => clearInterval(interval);
    }, [isLogged, userRole, API_URL]);

    const handleLogout = async () => {
        await logout();
        window.location.href = "/";
    };

    return (
        <nav className="sticky top-0 z-50 bg-white shadow-md">
            <div className="flex items-center justify-between px-4 py-2 md:px-8"> 
                <div className="flex items-center gap-6">
                    <Link to={isLogged ? "/dashboard" : "/"} className="flex items-center gap-2">
                        <img src="/logos43.png" alt="Logo" className="h-6" />
                        <span className="font-semibold text-gray-800">Strona główna</span>
                    </Link>

                    {userRole !== "admin" && (
                        <div className="hidden items-center gap-4 md:flex">
                            {isLogged && (
                                <div
                                    className="relative"
                                    onMouseEnter={() => setIsCoursesOpen(true)}
                                    onMouseLeave={() => setIsCoursesOpen(false)}
                                >
                                    <button className="px-2 py-1 transition-colors hover:text-indigo-700">Kursy</button>
                                    {isCoursesOpen && (
                                        <ul className="absolute top-full left-0 z-50 w-40 rounded border bg-white shadow-lg">
                                            <li><Link to="/myCourses" className="block px-4 py-2 hover:bg-indigo-100 hover:text-indigo-700">Moje kursy</Link></li>
                                            <li><Link to="/popularCourses" className="block px-4 py-2 hover:bg-indigo-100 hover:text-indigo-600">Popularne</Link></li>
                                            <li><Link to="/latestCourses" className="block px-4 py-2 hover:bg-indigo-100 hover:text-indigo-700">Wszystkie</Link></li>
                                            <li><Link to="/adaptive" className="block px-4 py-2 hover:bg-indigo-100 hover:text-indigo-700">Adaptacyjne</Link></li>
                                        </ul>
                                    )}
                                </div>
                            )}

                            {isLogged && (
                                <>
                                    <Link to="/challenges" className="px-2 py-1 transition-colors hover:text-indigo-700">Wyzwania</Link>
                                    <Link to="/assistant" className="px-2 py-1 transition-colors hover:text-indigo-700">Mentor AI</Link>
                                    <Link to="/userAchievments" className="px-2 py-1 transition-colors hover:text-indigo-700">Osiągnięcia</Link>
                                    <Link to="/playground" className="px-2 py-1 transition-colors hover:text-indigo-700">CodeRunner</Link>
                                    <Link to="/pvp" className="px-2 py-1 transition-colors hover:text-indigo-700">Pojedynki</Link>
                                </>
                            )}

                            <Link to="/faq" className="px-2 py-1 transition-colors hover:text-indigo-700">FAQ</Link>

                            {isLogged && (
                                <>
                                    <Link to="/store" className="px-2 py-1 transition-colors hover:text-indigo-700">Sklep</Link>
                                    <span className="px-2 py-1">🌟 {goldenPoints} GP</span>
                                </>
                            )}
                        </div>
                    )}
                </div>
                 
                <div className="flex items-center gap-4">
                    {userRole === "user" && (
                        <button
                            className="block text-2xl md:hidden"
                            onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
                        >
                            ☰
                        </button>
                    )}

                    {userRole === "admin" && (
                        <button onClick={handleLogout} className="text-red-600 transition-colors hover:text-red-800">
                            Wyloguj
                        </button>
                    )}

                    {userRole !== "admin" && (
                        <div className="hidden items-center gap-3 md:flex">
                            {isLogged && (
                                <>
                                    <div className="relative">
                                        <button onClick={() => setIsNotifOpen(!isNotifOpen)} className="relative p-1">
                                            🔔
                                            {notifications.length > 0 && (
                                                <span className="absolute -top-1 -right-1 rounded-full bg-red-500 px-1 text-xs text-white">
                                                    {notifications.length}
                                                </span>
                                            )}
                                        </button>
                                        {isNotifOpen && (
                                            <ul className="absolute right-0 z-50 mt-2 max-h-80 w-64 overflow-y-auto rounded border bg-white shadow-lg">
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
                                                    <Link to="/notifications" className="text-sm text-blue-500 hover:underline">
                                                        Wszystkie powiadomienia
                                                    </Link>
                                                </li>
                                            </ul>
                                        )}
                                    </div>

                                    <Link to="/profile" className="transition-colors hover:text-indigo-700">Profil</Link>
                                    <button onClick={handleLogout} className="text-red-600 transition-colors hover:text-red-800">
                                        Wyloguj
                                    </button>
                                </>
                            )}

                            {!isLogged && (
                                <Link to="/login" className="transition-colors hover:text-indigo-700">Logowanie</Link>
                            )}
                        </div>
                    )}
                </div>
            </div>

            {/* Menu mobilne */}
            {(isMobileMenuOpen && userRole !== "admin") && (
                <div className="space-y-2 border-t border-gray-200 bg-gray-50 px-4 py-3 md:hidden">
                    <Link to="/" className="block py-2">Strona główna</Link>
                    {isLogged && <Link to="/myCourses" className="block py-2">Moje kursy</Link>}
                    <Link to="/popularCourses" className="block py-2">Popularne</Link>
                    <Link to="/latestCourses" className="block py-2">Wszystkie kursy</Link>
                    {isLogged && (
                        <>
                            <Link to="/assistant" className="block py-2">Mentor AI</Link>
                            <Link to="/store" className="block py-2">Sklep</Link>
                            <Link to="/userAchievments" className="block py-2">Osiągnięcia</Link>
                            <Link to="/challenges" className="block py-2">Wyzwania</Link>
                            <Link to="/playground" className="block py-2">CodeRunner</Link>
                            <Link to="/faq" className="block py-2">FAQ</Link>
                            <Link to="/profile" className="block py-2">Profil</Link>
                            <button onClick={handleLogout} className="block w-full py-2 text-left text-red-600">Wyloguj</button>
                        </>
                    )}
                    {!isLogged && <Link to="/login" className="block py-2">Logowanie</Link>}
                </div>
            )}
        </nav>
    );
}

export default NavBarComponent;
