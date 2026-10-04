import React, { useState, useEffect } from "react";
import axios from "axios";
import UsersList from "./UsersList";
import { toast, ToastContainer } from "react-toastify";

interface CourseRequest {
    title: string;
    description: string;
    isPublic: boolean;
    language: string;
    level: string;
}

const AdminPanel: React.FC = () => {
    const [courseCount, setCourseCount] = useState<number>(0);
    const [userCount, setUserCount] = useState<number>(0);
    const [averageQuizAttempts, setAverageQuizAttempts] = useState<number>(0);
    const [averageExerciseAttempts, setAverageExerciseAttempts] = useState<number>(0);
    const API_URL = import.meta.env.VITE_API_URL;
    const [newCourse, setNewCourse] = useState<CourseRequest>({
        title: "",
        description: "",
        isPublic: true,
        language: "",
        level: "",
    });

    useEffect(() => {
        fetchStats();
    }, []);

    const fetchStats = async () => {
        const [courseRes, userRes, quizRes, exerciseRes] = await Promise.all([
            axios.get(`${API_URL}/api/admin/courses/count`),
            axios.get(`${API_URL}/api/admin/users/count`),
            axios.get(`${API_URL}/api/admin/content/quiz/average`),
            axios.get(`${API_URL}/api/admin/content/exercise/average`),
        ]);

        setCourseCount(courseRes.data);
        setUserCount(userRes.data);
        setAverageQuizAttempts(quizRes.data);
        setAverageExerciseAttempts(exerciseRes.data);
    };

    const handleCreateCourse = async () => {
        await axios.post(`${API_URL}/api/admin/courses`, newCourse);
        toast.success("Kurs został stworzony");
        setNewCourse({
            title: "",
            description: "",
            isPublic: true,
            language: "",
            level: "",
        });
        fetchStats();
    };

    return (
        <div className="min-h-screen bg-gray-900 p-4 md:p-6">
            <ToastContainer position="top-right" autoClose={3000} theme="dark" />

            <header className="mb-8">
                <h1 className="mb-2 text-3xl font-bold text-white md:text-4xl">Panel administratora</h1>
                <p className="text-gray-400">Zarządzaj kursami, użytkownikami i statystykami</p>
            </header>

            <section className="mb-8 rounded-xl bg-gray-800 p-6 shadow-lg">
                <h2 className="mb-6 border-b border-gray-700 pb-2 text-2xl font-semibold text-white">Statystyki systemu</h2>
                <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-4">
                    <div className="rounded-lg bg-gray-700 p-5">
                        <p className="mb-1 text-sm text-gray-400">Liczba kursów</p>
                        <p className="text-3xl font-bold text-white">{courseCount}</p>
                    </div>

                    <div className="rounded-lg bg-gray-700 p-5">
                        <p className="mb-1 text-sm text-gray-400">Liczba użytkowników</p>
                        <p className="text-3xl font-bold text-white">{userCount}</p>
                    </div>

                    <div className="rounded-lg bg-gray-700 p-5">
                        <p className="mb-1 text-sm text-gray-400">Zdawalność quizów</p>
                        <p className="text-3xl font-bold text-white">
                            {averageQuizAttempts === -1 ? "Brak aktywności" : `${Math.round(100 * averageQuizAttempts)}%`}
                        </p>
                    </div>

                    <div className="rounded-lg bg-gray-700 p-5">
                        <p className="mb-1 text-sm text-gray-400">Zdawalność zadań</p>
                        <p className="text-3xl font-bold text-white">
                            {averageExerciseAttempts === -1 ? "Brak aktywności" : `${Math.round(100 * averageExerciseAttempts)}%`}
                        </p>
                    </div>
                </div>
            </section>

            <section className="mb-8 rounded-xl bg-gray-800 p-6 shadow-lg">
                <h2 className="mb-6 border-b border-gray-700 pb-2 text-2xl font-semibold text-white">Tworzenie nowego kursu</h2>

                <div className="mb-6 grid grid-cols-1 gap-6 md:grid-cols-2">
                    <div>
                        <label className="mb-2 block font-medium text-gray-300">Tytuł kursu</label>
                        <input
                            type="text"
                            placeholder="Wprowadź tytuł kursu"
                            value={newCourse.title}
                            onChange={(e) => setNewCourse({ ...newCourse, title: e.target.value })}
                            className="w-full bg-gray-700 border border-gray-600 text-white rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent placeholder-gray-400"
                        />
                    </div>

                    <div>
                        <label className="mb-2 block font-medium text-gray-300">Język/Technologia</label>
                        <input
                            type="text"
                            placeholder="np. JavaScript, Python"
                            value={newCourse.language}
                            onChange={(e) => setNewCourse({ ...newCourse, language: e.target.value })}
                            className="w-full bg-gray-700 border border-gray-600 text-white rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent placeholder-gray-400"
                        />
                    </div>

                    <div>
                        <label className="mb-2 block font-medium text-gray-300">Poziom trudności</label>
                        <input
                            type="text"
                            placeholder="np. Początkujący, Średniozaawansowany"
                            value={newCourse.level}
                            onChange={(e) => setNewCourse({ ...newCourse, level: e.target.value })}
                            className="w-full bg-gray-700 border border-gray-600 text-white rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent placeholder-gray-400"
                        />
                    </div>

                    <div className="flex items-end">
                        <label className="flex cursor-pointer items-center text-gray-300">
                            <input
                                type="checkbox"
                                checked={newCourse.isPublic}
                                onChange={(e) => setNewCourse({ ...newCourse, isPublic: e.target.checked })}
                                className="w-5 h-5 mr-3 rounded bg-gray-700 border-gray-600 text-blue-500 focus:ring-blue-600 focus:ring-offset-gray-900"
                            />
                            <span className="font-medium">Kurs publiczny</span>
                        </label>
                    </div>
                </div>

                <div className="mb-6">
                    <label className="mb-2 block font-medium text-gray-300">Opis kursu</label>
                    <textarea
                        placeholder="Dodaj szczegółowy opis kursu..."
                        value={newCourse.description}
                        onChange={(e) => setNewCourse({ ...newCourse, description: e.target.value })}
                        rows={3}
                        className="w-full bg-gray-700 border border-gray-600 text-white rounded-lg px-4 py-3 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent placeholder-gray-400 resize-none"
                    />
                </div>

                <div className="flex justify-end">
                    <button
                        onClick={handleCreateCourse}
                        className="bg-gradient-to-r transform rounded-lg from-blue-600 to-blue-700 px-6 py-3 font-semibold text-white shadow-lg transition-all duration-200 hover:from-blue-700 hover:to-blue-800 hover:scale-105 hover:shadow-xl active:scale-95"
                    >
                        Stwórz kurs
                    </button>
                </div>
            </section>

            <section className="rounded-xl bg-gray-800 p-6 shadow-lg">
                <h2 className="mb-6 border-b border-gray-700 pb-2 text-2xl font-semibold text-white">Zarządzanie użytkownikami</h2>
                <UsersList />
            </section>
        </div>
    );
};

export default AdminPanel;