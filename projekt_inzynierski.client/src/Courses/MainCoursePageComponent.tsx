/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */

import axios from "axios";
import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import Shuffle from "../Challenges/Shuffle";
import Loader from "./Loader";
import RotatingText from "../components/RotatingText";
import { toast, ToastContainer } from "react-toastify";

function MainCoursePageComponent() {
    const { courseId } = useParams<{ courseId: string }>();

    interface Subject {
        id: number;
        name: string;
        progress: number;
    }

    const API_URL = import.meta.env.VITE_API_URL;
    const [subjects, setSubjects] = useState<Subject[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchSubjects = async () => {
            try {
                const response = await axios.get(`${API_URL}/GetSubjects/${courseId}`, {
                    withCredentials: true,
                });

                setSubjects(response.data);
           } catch (error: any) {
                setError("Wystąpił błąd podczas pobierania danych.");
                toast.error("Wystąpił błąd podczas pobierania danych.")
            } finally {
               setLoading(false);
            }
        };

        if (courseId) fetchSubjects();
    }, [courseId]);

    return (
        <div className="mt-2 text-white">
            <ToastContainer position="top-right" autoClose={3000} />
            <Shuffle text={"Materiały w kursie"} />
            <hr className="mb-4" />

            {loading && (
                <div className="flex flex-col items-center justify-center gap-20 py-15">
                    <Loader />

                    <RotatingText
                        texts={['Analiza danych', 'Rozpocząć końcowe tankowanie.', 'Przełączyć rakietę na zasilanie wewnętrzne.', 'T minus 10 sekund', 'Start sekwencji zapłonu.', 'Silniki główne odpalone.', 'Start! Mamy start rakiety!']}
                        mainClassName="bg-transparent text-center text-3xl"
                        staggerFrom="last"
                        initial={{ y: '100%' }}
                        animate={{ y: 0 }}
                        exit={{ y: '-120%' }}
                        staggerDuration={0.025}
                        splitLevelClassName="pb-0.5 overflow-hidden sm:pb-1 md:pb-1"
                        transition={{ type: 'spring', damping: 30, stiffness: 400 }}
                        rotationInterval={4000}
                    />
                </div>
            )}


            {error && !loading && (
                <p className="mt-4 text-center text-red-400">{error}</p>
            )}

            {!loading && !error && (
                <div className="mt-2">
                    <ul className="flex w-full flex-col gap-5">
                        {subjects.map((subject, index) => (
                            <Link
                                key={index}
                                to={`/course/${courseId}/subject/${subject.id}`}
                                state={{ courseName: "Java", subjectName: subject.name }}
                                className="border-2 block w-[90%] rounded-2xl border-indigo-400 bg-gray-800 p-4 shadow shadow-teal-300 transition-all duration-200 hover:bg-gray-700 hover:-translate-y-1"
                            >
                                <div className="flex flex-col gap-2">
                                    <p className="text-lg font-semibold">{subject.name}</p>
                                    <p className="text-sm text-gray-300">
                                        Postęp: {(subject.progress * 100).toFixed(0)}%
                                    </p>
                                </div>
                            </Link>
                        ))}
                    </ul>
                </div>
            )}
        </div>
    );
}

export default MainCoursePageComponent;
