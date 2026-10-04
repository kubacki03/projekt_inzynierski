import  { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { motion, AnimatePresence } from "framer-motion";
import Shuffle from "../Challenges/Shuffle";

function PagedCourses() {
    type Course = {
        id: number;
        title: string;
        description: string;
        language: string;
        image?: string;
        level: string;
    };

    type PagedResult<T> = {
        items: T[];
        totalCount: number;
        pageNumber: number;
        pageSize: number;
    };

    const navigate = useNavigate();
    const [courses, setCourses] = useState<Course[]>([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [totalCount, setTotalCount] = useState(0);
    const pageSize = 6;

    const [selectedCourseId, setSelectedCourseId] = useState<number | null>(null);
    const [showModal, setShowModal] = useState(false);
    const [searchTitle, setSearchTitle] = useState("");
    const [searchLanguage, setSearchLanguage] = useState("");
    const API_URL = import.meta.env.VITE_API_URL;

    const fetchCourses = async (page: number) => {
        let url = `${API_URL}/FeaturedCourses/paged?pageNumber=${page}&pageSize=${pageSize}`;

        if (searchTitle) {
            url = `${API_URL}/FeaturedCourses/pagedByTitle?title=${encodeURIComponent(
                searchTitle
            )}&pageNumber=${page}&pageSize=${pageSize}`;
        }

        if (searchLanguage) {
            url = `${API_URL}/FeaturedCourses/pagedByLanguage?language=${encodeURIComponent(
                searchLanguage
            )}&pageNumber=${page}&pageSize=${pageSize}`;
        }

        const res = await fetch(url, { credentials: "include" });
        const data: PagedResult<Course> = await res.json();

        setCourses(data.items);
        setTotalCount(data.totalCount);
        setPageNumber(data.pageNumber);
    };

    useEffect(() => {
        fetchCourses(1);
    }, []);

    const totalPages = Math.ceil(totalCount / pageSize);

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

    return (
        <div className="p-4 text-white">
            <Shuffle text={"Wszystkie kursy"} />

            <div className="mb-4 flex gap-2">
                <input
                    type="text"
                    placeholder="Szukaj po tytule..."
                    value={searchTitle}
                    onChange={(e) => setSearchTitle(e.target.value)}
                    className="px-2 py-1 rounded text-white bg-gray-700"
                />
                <select
                    value={searchLanguage}
                    onChange={(e) => setSearchLanguage(e.target.value)}
                    className="px-2 py-1 rounded text-white bg-gray-700"
                >
                    <option value="">Wybierz język</option>
                    <option value="C#">C#</option>
                    <option value="JavaScript">JavaScript</option>
                    <option value="Java">Java</option>
                    <option value="Python">Python</option>
                    <option value="SQL">SQL</option>
                </select>
                <button
                    onClick={() => fetchCourses(1)}
                    className="bg-teal-700 px-3 py-1 rounded hover:bg-teal-600"
                >
                    Szukaj
                </button>
            </div>

            {/* Animowane karty kursów */}
            <motion.div
                layout
                className="grid gap-4 md:grid-cols-2"
                initial="hidden"
                animate="visible"
                variants={{
                    visible: {
                        transition: { staggerChildren: 0.1 },
                    },
                }}
            >
                <AnimatePresence>
                    {courses.map((c) => (
                        <motion.div
                            key={c.id}
                            role="button"
                            onClick={() => handleCourseClick(c.id)}
                            className="rounded-xl border border-indigo-400 bg-gray-800 p-3 shadow-md hover:shadow-lg hover:shadow-indigo-500/30 cursor-pointer"
                            whileHover={{ scale: 1.03 }}
                            initial={{ opacity: 0, y: 30 }}
                            animate={{ opacity: 1, y: 0 }}
                            exit={{ opacity: 0, y: -30 }}
                            transition={{ duration: 0.4 }}
                        >
                            <img
                                src={c.image}
                                alt={c.title}
                                className="mb-2 h-32 w-full rounded-md object-cover"
                            />
                            <h2 className="text-lg font-semibold">{c.title}</h2>
                            <p className="text-sm text-gray-300">{c.description}</p>
                            <p className="mt-1 text-xs text-gray-400">Poziom: {c.level}</p>
                        </motion.div>
                    ))}
                </AnimatePresence>
            </motion.div>

            {/* Paginacja */}
            <div className="mt-6 flex justify-center gap-2">
                <button
                    disabled={pageNumber === 1}
                    onClick={() => fetchCourses(pageNumber - 1)}
                    className="px-3 py-1 bg-gray-700 rounded disabled:opacity-50"
                >
                    ⬅ Poprzednia
                </button>
                <span>
                    Strona {pageNumber} / {totalPages}
                </span>
                <button
                    disabled={pageNumber === totalPages}
                    onClick={() => fetchCourses(pageNumber + 1)}
                    className="px-3 py-1 bg-gray-700 rounded disabled:opacity-50"
                >
                    Następna ➡
                </button>
            </div>

            {/* Animowany modal */}
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
        </div>
    );
}

export default PagedCourses;
