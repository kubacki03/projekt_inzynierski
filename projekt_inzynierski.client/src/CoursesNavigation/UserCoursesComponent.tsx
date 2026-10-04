/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import axios from "axios";
import  { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { motion, AnimatePresence } from "framer-motion";
import Shuffle from "../Challenges/Shuffle";
import './style.css'
function UserCoursesComponent() {
    type Course = {
        id: string;
        name: string;
        description: string;
        progress: number;
        image: string;
    };

    const [userCourses, setCourses] = useState<Course[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const API_URL = import.meta.env.VITE_API_URL;

    useEffect(() => {
        const fetchCourses = async () => {
            try {
                const response = await axios.get(`${API_URL}/UserCourses/GetUserCourses`, {
                    withCredentials: true,
                });
                if (!response) throw new Error("Brak odpowiedzi z serwera");
                setCourses(response.data);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };
        fetchCourses();
    }, []);

    const itemsPerPage = 12;
    const [currentPage, setCurrentPage] = useState(1);
    const totalPages = Math.ceil(userCourses.length / itemsPerPage);
    const startIndex = (currentPage - 1) * itemsPerPage;
    const currentCourses = userCourses.slice(startIndex, startIndex + itemsPerPage);

    const handlePrev = () => {
        if (currentPage > 1) setCurrentPage(currentPage - 1);
    };

    const handleNext = () => {
        if (currentPage < totalPages) setCurrentPage(currentPage + 1);
    };

    return (
        <motion.div
            className="mt-3 min-h-screen p-4 text-white"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ duration: 0.4 }}
        >
            <Shuffle text={"Moje kursy"} />

            {/* 🔄 Ładowanie */}
            {loading && (
                <motion.div
                    className="flex h-40 items-center justify-center text-lg font-semibold text-teal-400"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: [0.3, 1, 0.3] }}
                    transition={{ repeat: Infinity, duration: 1.5 }}
                >
                    Ładowanie Twoich kursów...
                </motion.div>
            )}

            {/* ❌ Błąd */}
            {error && (
                <motion.div
                    className="mt-4 text-center text-red-400"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                >
                    Wystąpił błąd: {error}
                </motion.div>
            )}

            {/* 🎓 Karty kursów */}
            <motion.div
                layout
                className="grid grid-cols-1 gap-4 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4"
                initial="hidden"
                animate="visible"
                variants={{
                    visible: { transition: { staggerChildren: 0.1 } },
                }}
            >
                <AnimatePresence>
                    {!loading &&
                        currentCourses.map((c) => (
                            <motion.div
                                key={c.id}
                                initial={{ opacity: 0, y: 40 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, y: -20 }}
                                transition={{ duration: 0.4 }}
                            >
                                <Link
                                    to={`/course/${c.id}`}
                                    className="border-2 flex cursor-pointer flex-col rounded-2xl border-indigo-400 bg-gray-800 p-4 shadow-md transition-all hover:shadow-lg hover:shadow-indigo-500/30"
                                    style={{
                                        height: "360px", // 🧱 Stała wysokość kart
                                    }}
                                >
                                    <motion.img
                                        src={c.image}
                                        alt={c.name}
                                        className="mb-2 h-24 w-full rounded-md object-contain"
                                        whileHover={{ scale: 1.05 }}
                                        transition={{ duration: 0.3 }}
                                    />

                                    <h2 className="mb-1 line-clamp-1 text-lg font-semibold">
                                        {c.name}
                                    </h2>

                                    {/* 🧾 Opis z przewijaniem */}
                                    <div
                                        className="scrollbar-thin flex-grow overflow-y-auto pr-1 text-sm text-gray-300"
                                        style={{ maxHeight: "120px" }}
                                    >
                                        {c.description}
                                    </div>


                                    {/* 🔵 Postęp */}
                                    <motion.span
                                        className="mt-3 inline-block rounded-lg bg-indigo-600 px-4 py-2 text-center font-semibold text-white transition duration-200 hover:bg-indigo-500"
                                        whileTap={{ scale: 0.95 }}
                                    >
                                        Postęp: {(c.progress * 100).toFixed(0)}%
                                    </motion.span>
                                </Link>
                            </motion.div>
                        ))}
                </AnimatePresence>
            </motion.div>

            {/* 📄 Paginacja */}
            {!loading && totalPages > 1 && (
                <motion.div
                    className="mt-6 flex justify-center gap-4"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    transition={{ delay: 0.2 }}
                >
                    <button
                        onClick={handlePrev}
                        disabled={currentPage === 1}
                        className="rounded bg-teal-600 px-4 py-2 transition hover:bg-teal-500 disabled:opacity-50"
                    >
                        ⬅ Poprzednia
                    </button>

                    <span className="self-center text-gray-300">
                        Strona {currentPage} z {totalPages}
                    </span>

                    <button
                        onClick={handleNext}
                        disabled={currentPage === totalPages}
                        className="rounded bg-teal-600 px-4 py-2 transition hover:bg-teal-500 disabled:opacity-50"
                    >
                        Następna ➡
                    </button>
                </motion.div>
            )}
        </motion.div>
    );
}

export default UserCoursesComponent;
