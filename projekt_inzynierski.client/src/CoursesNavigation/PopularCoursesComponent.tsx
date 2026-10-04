/* eslint-disable @typescript-eslint/no-explicit-any */
import axios from "axios";
import { useEffect, useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import Shuffle from "../Challenges/Shuffle";
import { useNavigate } from "react-router-dom";

/* eslint-disable @typescript-eslint/no-unused-vars */
function PopularCoursesComponent() {
    type Course = {
        language: string;
        title: string;
        description: string;
        image: string;
        id: number;
    };

    const API_URL = import.meta.env.VITE_API_URL;
    const [selectedCourseId, setSelectedCourseId] = useState<number | null>(null);
    const [showModal, setShowModal] = useState(false);
    const [allCourses, setCourses] = useState<Course[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchCourses = async () => {
            try {
                const response = await axios.get(`${API_URL}/PopularCourses/Get`, {
                    withCredentials: true,
                });
                const data = response.data;
                setCourses(data);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };
        fetchCourses();
    }, []);
    const navigate = useNavigate();
    const [searchTerm, setSearchTerm] = useState("");
    const [selectedLanguage, setSelectedLanguage] = useState("Wszystkie");
    const [sortOption, setSortOption] = useState("name");

    const languages = ["Wszystkie", ...Array.from(new Set(allCourses.map((c) => c.language)))];
    const handleCourseClick = async (courseId: number) => {
        try {
            const res = await fetch(
                `${API_URL}/UserCourses/IsUserInCourse?courseId=${courseId}`,
                { credentials: "include" }
            );
            const isInCourse = await res.json();

            if (isInCourse) {
                navigate(`/course/${courseId}`);
            } else {
                setSelectedCourseId(courseId);
                setShowModal(true);
            }
        } catch (err) {
            console.error("Błąd podczas sprawdzania kursu:", err);
        }
    };

    const joinCourse = async () => {
        if (!selectedCourseId) return;
        await fetch(`${API_URL}/UserCourses/JoinCourse?courseId=${selectedCourseId}`, {
            method: "POST",
            credentials: "include",
        });
        setShowModal(false);
        navigate(`/course/${selectedCourseId}`);
    };
    const filteredCourses = allCourses
        .filter((c) => selectedLanguage === "Wszystkie" || c.language === selectedLanguage)
        .filter((c) => c.title.toLowerCase().includes(searchTerm.toLowerCase()))
        .sort((a, b) => {
            if (sortOption === "name") return a.title.localeCompare(b.title);
            return 0;
        });

    return (
        <motion.div
            className="p-4 text-white"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ duration: 0.4 }}
        >
            <Shuffle text={"Popularne kursy"} />

            {/* 🔍 Wyszukiwarka i filtry */}
            <div className="mb-4 flex flex-wrap gap-2">
                <input
                    type="text"
                    placeholder="Szukaj kursu..."
                    className="px-2 py-1 rounded text-white bg-gray-700"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                />

                <select
                    className="rounded bg-gray-800 p-2 text-gray-200"
                    value={selectedLanguage}
                    onChange={(e) => setSelectedLanguage(e.target.value)}
                >
                    {languages.map((lang) => (
                        <option key={lang} value={lang}>
                            {lang}
                        </option>
                    ))}
                </select>

                <select
                    className="rounded bg-gray-800 p-2 text-gray-200"
                    value={sortOption}
                    onChange={(e) => setSortOption(e.target.value)}
                >
                    <option value="name">Sortuj A-Z</option>
                    <option value="progress">Sortuj wg postępu</option>
                </select>
            </div>

            {/* 🌀 Stan ładowania */}
            {loading && (
                <motion.div
                    className="flex h-40 items-center justify-center text-lg font-semibold text-teal-400"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: [0.3, 1, 0.3] }}
                    transition={{ repeat: Infinity, duration: 1.5 }}
                >
                    Ładowanie kursów...
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
                className="grid gap-4 md:grid-cols-2 lg:grid-cols-3"
                initial="hidden"
                animate="visible"
                variants={{
                    visible: { transition: { staggerChildren: 0.1 } },
                }}
            >
                <AnimatePresence>
                    {!loading &&
                        filteredCourses.map((c, index) => (
                            <motion.div
                                key={index}
                                role="button"
                                onClick={() => handleCourseClick(c.id)}
                                className="border-2 cursor-pointer rounded-2xl border-indigo-400 bg-gray-800 p-4 shadow-md transition-all duration-200 hover:shadow-lg hover:shadow-indigo-500/30"
                                whileHover={{ scale: 1.05 }}
                                initial={{ opacity: 0, y: 40 }}
                                animate={{ opacity: 1, y: 0 }}
                                exit={{ opacity: 0, y: -20 }}
                                transition={{ duration: 0.4 }}
                            >
                                <img
                                    src={c.image}
                                    alt={c.title}
                                    className="mb-3 h-28 w-full rounded-md object-contain"
                                />
                                <h2 className="mb-1 text-xl font-semibold">{c.title}</h2>
                                <p className="line-clamp-3 text-sm text-gray-300">{c.description}</p>

                               
                            </motion.div>
                        ))}
                </AnimatePresence>
            </motion.div>
            <AnimatePresence>
                {showModal && (
                    <motion.div
                        className="fixed inset-0 z-50 flex items-center justify-center bg-black/50"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                    >
                        <motion.div
                            className="w-96 rounded-2xl bg-gray-900 p-6 text-white shadow-lg"
                            initial={{ y: 80, opacity: 0 }}
                            animate={{ y: 0, opacity: 1 }}
                            exit={{ y: 80, opacity: 0 }}
                            transition={{ type: "spring", stiffness: 200, damping: 20 }}
                        >
                            <h2 className="mb-4 text-xl">Dołącz do kursu</h2>
                            <p className="mb-6">
                                Nie jesteś zapisany do tego kursu. Czy chcesz się zapisać?
                            </p>
                            <div className="flex justify-end gap-3">
                                <button
                                    onClick={() => setShowModal(false)}
                                    className="px-4 py-2 rounded bg-gray-700 hover:bg-gray-600"
                                >
                                    Anuluj
                                </button>
                                <button
                                    onClick={joinCourse}
                                    className="rounded bg-teal-700 px-4 py-2 hover:bg-teal-600"
                                >
                                    Dołącz
                                </button>
                            </div>
                        </motion.div>
                    </motion.div>
                )}
            </AnimatePresence>
        </motion.div>
    );
}

export default PopularCoursesComponent;
