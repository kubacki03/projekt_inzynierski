
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import Editor from "@monaco-editor/react";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import './Shuffle'
import Shuffle from './Shuffle';
import { motion } from 'framer-motion';
type ChallengeStatus = 'completed' | 'inProgress' | 'notStarted';

type Challenge = {
    id: number;
    isWeekly: boolean;
    name: string;
    description: string;
    rewardDescription: string;
    points: number;
    status: ChallengeStatus;
    type: 'user' | 'weekly';
};

const ChallengesComponent: React.FC = () => {
    const [challenges, setChallenges] = useState<Challenge[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const [selectedChallenge, setSelectedChallenge] = useState<number>(0);
    const [solution, setSolution] = useState<string>("");
    const API_URL = import.meta.env.VITE_API_URL;

    useEffect(() => {
        const fetchChallenges = async () => {
            try {
                const response = await axios.get(`${API_URL}/UserChallenge/Get`, {
                    withCredentials: true
                });

                const mappedChallenges: Challenge[] = response.data.map((c: any) => ({
                    id: c.id ?? crypto.randomUUID(),
                    name: c.name,
                    description: c.description,
                    rewardDescription: c.rewardDescription,
                    points: c.points,
                    status: c.isDone ? 'completed' : 'notStarted',
                    type: c.isWeekly ? 'weekly' : 'user',
                }));

                setChallenges(mappedChallenges);
            } catch (err: unknown) {
                if (err instanceof Error) setError("Wystąpił błąd ładowania wyzwań");
                else {
                    setError("Nieznany błąd")

                };
            } finally {
                setLoading(false);
            }
        };

        fetchChallenges();
    }, []);

    const statusStyles: Record<ChallengeStatus, string> = {
        completed: 'bg-green-300 text-green-800',
        inProgress: 'bg-yellow-100 text-yellow-800',
        notStarted: 'bg-gray-100 text-gray-800'
    };

    const handleSubmitSolution = async () => {
        if (!selectedChallenge) {
            toast.warning("Wybierz wyzwanie!");
            return;
        }
        try {
            const response = await axios.post(
                `${API_URL}/UserChallenge/Submit`,
{
    Task: solution,
        AchievementId: selectedChallenge
},
{ withCredentials: true }
            );

if (response.data === true) {
    setChallenges(prevChallenges =>
        prevChallenges.map(c =>
            c.id === selectedChallenge ? { ...c, status: 'completed' } : c
        )
    );
    toast.success("Rozwiązanie zostało przesłane i wyzwanie ukończone!");
    setSolution("");
} else {
    toast.info("Rozwiązanie nie zostało zatwierdzone.");
}
        } catch (err) {
    toast.error("Błąd podczas przesyłania rozwiązania");
}
    };

    const renderChallenge = (challenge: Challenge) => (
        <motion.div
            key={challenge.id}
            className="mb-6 rounded-lg border border-indigo-400 bg-gray-800 p-4 shadow-sm transition-shadow hover:shadow-lg"
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.4 }}
            whileHover={{ scale: 1.02 }}
        >

            <div className="flex items-start justify-between">
                <h3 className="text-lg font-bold">{challenge.name}</h3>
                <span className={`px-2 py-1 rounded-full text-xs ${statusStyles[challenge.status]}`}>
                    {challenge.status === 'completed' && 'Ukończono'}
                    {challenge.status === 'inProgress' && 'W trakcie'}
                    {challenge.status === 'notStarted' && 'Nie rozpoczęto'}
                </span>
            </div>
            <p className="my-2 text-gray-200">{challenge.description}</p>
            <div className="mt-3 flex items-center justify-between">
                <span className="text-sm text-gray-300">
                    {challenge.type === 'user' ? 'Wyzwanie osobiste' : 'Wyzwanie tygodnia'}
                </span>
                <span className="font-bold text-blue-600">{challenge.rewardDescription} i +{challenge.points} pkt</span>
            </div>
        </motion.div>
    );


const weeklyChallenges = challenges.filter(c => c.type === 'weekly');
const userChallenges = challenges.filter(c => c.type !== 'weekly');

return (
    <div className="mx-auto max-w-2xl p-5 text-white">

        <div className="flex flex-col items-center">
            <Shuffle text={'Twoje wyzwania'} />
        </div>
        {loading && <p className="text-gray-400 italic">Ładowanie...</p>}
        {error && <p className="text-red-400 italic">{error}</p>}

        {!loading && !error && (
            <>
                <section className="mb-8">
                    <h2 className="mb-4 border-b pb-2 text-xl font-semibold">Wyzwania Tygodniowe</h2>
                    {weeklyChallenges.length > 0 ? (
                        weeklyChallenges.map(renderChallenge)
                    ) : (
                        <p className="text-gray-500 italic">Brak aktywnych wyzwań tygodniowych</p>
                    )}
                </section>

                <section>
                    <h2 className="mb-4 border-b pb-2 text-xl font-semibold">Twoje Osobiste Wyzwania</h2>
                    {userChallenges.length > 0 ? (
                        userChallenges.map(renderChallenge)
                    ) : (
                        <p className="text-gray-500 italic">Brak aktywnych wyzwań osobistych</p>
                    )}
                </section>

                <section className="mt-8 border-t pt-6">
                    <h2 className="mb-3 text-xl font-semibold">Prześlij swoje rozwiązanie</h2>

                    <select
                        value={selectedChallenge}
                        onChange={(e) => setSelectedChallenge(Number(e.target.value))}
                        className="mb-3 w-full rounded-md border border-gray-600 bg-gray-900 p-2 text-white"
                    >
                        <option value={0}>-- wybierz wyzwanie --</option>
                        {challenges
                            .filter(c => c.status !== "completed")
                            .map(c => (
                                <option key={c.id} value={c.id}>
                                    {c.name} ({c.type === "user" ? "osobiste" : "tygodniowe"})
                                </option>
                            ))}
                    </select>

                    <Editor
                        height="40vh"
                        defaultLanguage="plaintext"
                        theme="vs-dark"
                        value={solution}
                        onChange={(value) => setSolution(value || "")}
                    />

                    <motion.button
                        onClick={handleSubmitSolution}
                        className="mt-3 w-full rounded-lg bg-teal-500 px-4 py-2 font-semibold text-white hover:bg-teal-600"
                        whileTap={{ scale: 0.95 }}
                    >
                        Prześlij rozwiązanie
                    </motion.button>

                </section>
            </>
        )}

    
        <ToastContainer position="top-right" autoClose={3000} hideProgressBar={false} newestOnTop closeOnClick pauseOnHover />
    </div>
);
};

export default ChallengesComponent;

