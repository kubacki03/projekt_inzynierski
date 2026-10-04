/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { motion } from "framer-motion";

type Course = {

    id: number
    title: string;
    level: string;
    description: string;
};

function FeaturedCoursesComponent() {
    const navigate = useNavigate();
    const [courses, setCourses] = useState<Course[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [selectedCourseId, setSelectedCourseId] = useState<number | null>(null);
    const [showModal, setShowModal] = useState(false);
    const API_URL = import.meta.env.VITE_API_URL;
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

    useEffect(() => {
        const fetchCourses = async () => {
            try {
                const response = await fetch(`${API_URL}/FeaturedCourses/Get`);
                if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
                const data = await response.json();
                
                setCourses(data);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchCourses();
    }, []);
    const joinCourse = async () => {
        if (!selectedCourseId) return;
        await fetch(
            `${API_URL}/UserCourses/JoinCourse?courseId=${selectedCourseId}`,
            { method: "POST", credentials: "include" }
        );
        setShowModal(false);
        navigate(`/course/${selectedCourseId}`);
    };

    return (
        <main className="">
        <div className="flex flex-col items-center">
            <h1 className="mb-3 text-center text-4xl">Polecane kursy</h1>

            {loading && <p className="text-center">Ładowanie...</p>}
            {error && <p className="text-center text-red-500">Błąd: {error}</p>}

            <div className="flex flex-col items-stretch gap-10 md:flex-row md:justify-center md:flex-wrap">

                {courses.map((c, index) => (
                    <motion.div
                        role="button"
                        key={index}
                        onClick={() => handleCourseClick(c.id)}
                        className="flex flex-col justify-between w-full md:w-1/3 rounded-2xl bg-gray-800 p-4 text-white outline-none
hover:border hover:border-indigo-400 hover:shadow-[0_0_40px_rgba(14,116,144,0.5),0_0_40px_rgba(14,116,144,0.3)] cursor-pointer"
                        whileHover={{ scale: 1.05, y: -5 }}
                        whileTap={{ scale: 0.97 }}
                        initial={{ opacity: 0, y: 30 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.3, delay: index * 0.1 }}
                    >

                        <h1 className="text-3xl">{c.title}</h1>
                        <p className="text-2xl">{c.level}</p>
                        <p className="mb-2 text-2xl">{c.description}</p>
                        

                      
                    </motion.div>
                ))}
            </div>

            {showModal && (
                <motion.div
                    className="bg-opacity-60 fixed inset-0 z-50 flex items-center justify-center bg-black"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: 1 }}
                    exit={{ opacity: 0 }}
                >
                    <motion.div
                        className="w-96 rounded-2xl bg-gray-900 p-6 text-white shadow-lg"
                        initial={{ scale: 0.8, opacity: 0 }}
                        animate={{ scale: 1, opacity: 1 }}
                        transition={{ type: "spring", stiffness: 120 }}
                    >
                        <h2 className="mb-4 text-xl">Dołącz do kursu</h2>
                        <p className="mb-6">Nie jesteś zapisany do tego kursu. Czy chcesz się zapisać?</p>
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


            </div>
        </main>
    );
}

export default FeaturedCoursesComponent;
