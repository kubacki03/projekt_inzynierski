/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useState } from "react";
import axios from "axios";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import Shuffle from "../Challenges/Shuffle";
import { motion, AnimatePresence } from "framer-motion";

interface ApiResponse {
    success: boolean;
    [key: string]: any;
}

export default function AdaptiveCourseCreator() {
    const [Technologies, setTechnologies] = useState<string>("");
    const [Description, setDescription] = useState<string>("");
    const [step, setStep] = useState<number>(1);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const API_URL = import.meta.env.VITE_API_URL;

    const handleFirstSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        try {
            const res = await axios.post<ApiResponse>(
                `${API_URL}/Learning/canBeCreated`,
                { Technologies, Description },
                {
                    headers: {
                        "Content-Type": "application/json",
                    },
                    withCredentials: true,
                }
            );

            if (res.status === 200 && res.data.success) {
                setStep(2);
            } else if (res.status === 418) {
                setError("Kurs nie może zostać utworzony (I'm a teapot ☕).");
            } else {
                setError("Nie udało się utworzyć kursu.");
            }
        } catch (err: any) {
            if (err.response?.status === 418) {
                setError("Kurs nie może zostać utworzony (I'm a teapot ☕).");
            } else {
                setError("Błąd połączenia z API.");
            }
        } finally {
            setLoading(false);
        }
    };

    const handlePdfUpload = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setLoading(true);

        const formData = new FormData();
        const fileInput = e.currentTarget.querySelector('input[type="file"]') as HTMLInputElement;

        formData.append("Technologies", Technologies);
        formData.append("Description", Description);

        if (fileInput?.files) {
            for (let i = 0; i < fileInput.files.length; i++) {
                formData.append("pdf", fileInput.files[i]);
            }
        }

        try {
            const res = await fetch(`${API_URL}/UserCourses/CreateAdaptiveCourse`, {
                method: "POST",
                body: formData,
                credentials: "include",
            });

            if (res.ok) {
                toast.success("Kurs został utworzony pomyślnie!");
                setTechnologies("");
                setDescription("");
                setStep(1);
            } else {
                const errorData = await res.json();
                toast.error("Nie udało się utworzyć kursu.");
                console.error("Błąd backend:", errorData);
            }
        } catch (error) {
            toast.error("Błąd podczas wysyłania żądania.");
            console.error("Błąd sieci:", error);
        } finally {
            setLoading(false);
        }
    };

    return (
        <main>
            <div className="flex min-h-screen items-center justify-center p-4">
                <ToastContainer position="top-right" autoClose={3000} />

                <motion.div
                    className="w-full max-w-lg space-y-6 rounded-2xl bg-white p-6 shadow-lg"
                    initial={{ opacity: 0, y: -20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.5 }}
                >
                    <div className="text-center">
                        <Shuffle text={"Kreator"} />
                    </div>

                    <AnimatePresence mode="wait">

                        {step === 1 && (
                            <motion.div
                                key="step1"
                                initial={{ opacity: 0, x: -50 }}
                                animate={{ opacity: 1, x: 0 }}
                                exit={{ opacity: 0, x: 50 }}
                                transition={{ duration: 0.3 }}
                            >
                                <div className="space-y-2 rounded-lg bg-blue-50 p-4">
                                    <h2 className="text-lg font-bold">Dlaczego warto uczyć się adaptacyjnie?</h2>
                                    <ul className="list-inside list-disc space-y-1 text-sm text-gray-700">
                                        <li>📌 Nauka dopasowana do Twojego poziomu i tempa.</li>
                                        <li>📌 Skupienie na praktycznych umiejętnościach, które naprawdę Cię interesują.</li>
                                        <li>📌 Możliwość tworzenia własnych materiałów i dodawania PDF-ów do kursu.</li>
                                        <li>📌 Efektywne przyswajanie wiedzy dzięki spersonalizowanemu planowi nauki.</li>
                                    </ul>
                                </div>

                                <form onSubmit={handleFirstSubmit} className="mt-4 space-y-4">
                                    <h2 className="text-xl font-bold">Stwórz adaptacyjny kurs</h2>

                                    <div className="flex flex-col space-y-1">
                                        <label htmlFor="technologies" className="font-medium">
                                            Język / technologia
                                        </label>
                                        <input
                                            id="technologies"
                                            className="rounded border p-2"
                                            placeholder="np. Python, React, Java"
                                            value={Technologies}
                                            onChange={(e) => setTechnologies(e.target.value)}
                                            required
                                        />
                                    </div>

                                    <div className="flex flex-col space-y-1">
                                        <label htmlFor="description" className="font-medium">
                                            Czego chcesz się nauczyć?
                                        </label>
                                        <input
                                            id="description"
                                            className="rounded border p-2"
                                            placeholder="np. testy jednostkowe, backend, algorytmy"
                                            value={Description}
                                            onChange={(e) => setDescription(e.target.value)}
                                            required
                                        />
                                    </div>

                                    {error && <p className="text-sm text-red-500">{error}</p>}

                                    <motion.button
                                        type="submit"
                                        disabled={loading}
                                        whileHover={{ scale: 1.05 }}
                                        whileTap={{ scale: 0.95 }}
                                        className="w-full rounded bg-indigo-600 p-2 text-white transition disabled:bg-blue-300"
                                    >
                                        {loading ? "Sprawdzanie..." : "Dalej"}
                                    </motion.button>
                                </form>
                            </motion.div>
                        )}

                        {step === 2 && (
                            <motion.div
                                key="step2"
                                initial={{ opacity: 0, x: 50 }}
                                animate={{ opacity: 1, x: 0 }}
                                exit={{ opacity: 0, x: -50 }}
                                transition={{ duration: 0.3 }}
                            >
                                <form onSubmit={handlePdfUpload} className="space-y-4">
                                    <h2 className="text-xl font-bold">Dodaj własne materiały (PDF) - opcjonalnie</h2>

                                    <motion.div
                                        className="rounded-lg bg-gray-50 p-4"
                                        initial={{ opacity: 0 }}
                                        animate={{ opacity: 1 }}
                                        transition={{ duration: 0.4 }}
                                    >
                                        <h3 className="mb-2 font-medium">Twój kurs:</h3>
                                        <p><strong>Technologie:</strong> {Technologies}</p>
                                        <p><strong>Cel:</strong> {Description}</p>
                                    </motion.div>

                                    <div className="flex flex-col space-y-1">
                                        <label htmlFor="pdf" className="font-medium">
                                            Wybierz pliki PDF (opcjonalnie)
                                        </label>
                                        <input
                                            id="pdf"
                                            type="file"
                                            name="pdf"
                                            accept="application/pdf"
                                            multiple
                                            className="block w-full rounded border p-2 text-sm text-gray-700"
                                        />
                                        <p className="text-xs text-gray-500">Możesz wybrać wiele plików</p>
                                    </div>

                                    <div className="flex space-x-2">
                                        <motion.button
                                            type="button"
                                            onClick={() => setStep(1)}
                                            whileHover={{ scale: 1.05 }}
                                            whileTap={{ scale: 0.95 }}
                                            className="w-1/3 rounded bg-gray-600 p-2 text-white transition hover:bg-gray-700"
                                        >
                                            Wstecz
                                        </motion.button>
                                        <motion.button
                                            type="submit"
                                            disabled={loading}
                                            whileHover={{ scale: 1.05 }}
                                            whileTap={{ scale: 0.95 }}
                                            className="flex-1 rounded bg-green-600 p-2 text-white transition disabled:bg-green-300"
                                        >
                                            {loading ? "Tworzenie kursu..." : "Utwórz kurs"}
                                        </motion.button>
                                    </div>
                                </form>
                            </motion.div>
                        )}
                    </AnimatePresence>
                </motion.div>
            </div>
        </main>
    );
}
