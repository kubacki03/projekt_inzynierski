/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import Editor from "@monaco-editor/react";
import { toast, ToastContainer } from "react-toastify";

interface Exercise {
    id: number;
    task: string;
    subjectId: number;
    language: string;
}

function ExerciseSolutionComponent() {
    const [feedback, setFeedback] = useState<string | null>(null);
    const { exerciseId } = useParams<{ exerciseId: string }>();
    const [exercise, setExercise] = useState<Exercise | null>(null);
    const [solution, setSolution] = useState("");
    const [status, setStatus] = useState<"idle" | "saving" | "success" | "error">("idle");
    const [loading, setLoading] = useState(true);
    const [lang, setLang] = useState<string>("javascript");
    const API_URL = import.meta.env.VITE_API_URL;

    const mapLanguage = (lang: string): string => {
        switch (lang.toLowerCase()) {
            case "javascript":
                return "javascript";
            case "c#":
                return "csharp";
            case "java":
                return "java";
            case "python":
                return "python";
            case "sql":
                return "sql";
            default:
                return "plaintext";
        }
    };

    useEffect(() => {
        const fetchExercise = async () => {
            try {
                setLoading(true);
                const res = await axios.get(`${API_URL}/GetExerciseById/${exerciseId}`, {
                    withCredentials: true,
                });

                setExercise(res.data);
                setLang(mapLanguage(res.data.language));
            } catch (err) {
               
                toast.error("Błąd połączenia z serwerem");
            } finally {
                setLoading(false);
            }
        };

        fetchExercise();
    }, [exerciseId]);

    const handleSave = async () => {
        try {
            setStatus("saving");
            const res = await axios.post(
                `${API_URL}/Exercise/SaveExerciseSolution`,
                { solution, exerciseId },
                { withCredentials: true }
            );

            setFeedback(res.data.review);
            setStatus("success");
           
        } catch (err) {
            alert("Błąd podczas zapisywania rozwiązania");
            toast.error("Błąd podczas zapisywania rozwiązania");
            setStatus("error");
        }
    };

    if (loading) {
        return (
            <div className="flex items-center justify-center py-20">
                <div className="border-4 border-t-transparent h-12 w-12 animate-spin rounded-full border-blue-400"></div>
            </div>
        );
    }

    if (!exercise) {
        return <p className="mt-10 text-center text-gray-300">Nie udało się wczytać zadania.</p>;
    }

    return (
        <div className="rounded-2xl bg-gray-950/60 p-8 text-white shadow-lg backdrop-blur-md">
            <ToastContainer position="top-right" autoClose={3000} />
            <div className="mb-6">
                <h1 className="mb-3 text-3xl font-bold text-gray-200">Zadanie</h1>
                <p className="text-lg leading-relaxed text-blue-400">{exercise.task}</p>
            </div>

            {status === "success" && feedback && (
                <div className="mb-4 rounded-xl border border-blue-500 bg-blue-900/50 p-3 text-blue-300 shadow-inner">
                    💡 <span className="font-semibold">Feedback:</span> {feedback}
                </div>
            )}

            <div className="mb-6 overflow-hidden rounded-xl border border-gray-800 shadow-lg">
                <Editor
                    height="60vh"
                    defaultLanguage={lang}
                    theme="vs-dark"
                    value={solution}
                    onChange={(value) => setSolution(value || "")}
                    options={{
                        fontSize: 14,
                        minimap: { enabled: false },
                        scrollBeyondLastLine: false,
                        automaticLayout: true,
                    }}
                />
            </div>

            <div className="flex items-center gap-3">
                <button
                    onClick={handleSave}
                    disabled={status === "saving"}
                    className="flex items-center gap-2 rounded-md bg-teal-600 px-5 py-2 font-semibold text-white shadow-md transition-all hover:bg-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                    {status === "saving" && (
                        <div className="border-2 border-t-transparent h-4 w-4 animate-spin rounded-full border-white"></div>
                    )}
                    {status === "saving" ? "Sprawdzanie..." : "💾 Sprawdź rozwiązanie"}
                </button>

                {status === "success" && (
                    <p className="font-medium text-indigo-400">Analizowanie skończone!</p>
                )}
            </div>
        </div>
    );
}

export default ExerciseSolutionComponent;
