/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import  { useEffect, useState } from "react";
import { motion } from "framer-motion";

function LeaderBoardComponent() {
    type User = {
        nickname: string;
        rank: number;
        path: string;
    };

    const API_URL = import.meta.env.VITE_API_URL;
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchCourses = async () => {
            try {
                const response = await fetch(`${API_URL}/User/GetMostActive`);
                if (!response.ok)
                    throw new Error(`HTTP error! status: ${response.status}`);
                const data = await response.json();
                setUsers(data);
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchCourses();
    }, []);

    const fadeInUp = {
        hidden: { opacity: 0, y: 30 },
        visible: { opacity: 1, y: 0 },
    };

    return (
        <div className="flex flex-col items-start gap-3">
            <h1 className="mb-3 text-4xl font-bold">Najaktywniejsi</h1>

            {loading && <p className="text-gray-400">Ładowanie...</p>}
            {error && <p className="text-red-400">{error}</p>}

            {users.map((c, index) => (
                <motion.div
                    key={index}
                    variants={fadeInUp}
                    initial="hidden"
                    animate="visible"
                    transition={{ duration: 0.5, delay: index * 0.15 }}
                    whileHover={{ scale: 1.05 }}
                    className="flex flex-col items-center"
                >
                    {c.rank === 1 && (
                        <motion.div
                            animate={{ y: [0, -5, 0] }}
                            transition={{ repeat: Infinity, duration: 2 }}
                            className="flex items-center gap-2 text-center text-xl"
                        >
                            <img
                                src={`${API_URL}/${c.path}`}
                                className="border-4 h-32 w-32 rounded-full border-yellow-400 object-cover shadow-[0_0_20px_rgba(250,204,21,0.6)]"
                            />
                            <span className="font-semibold text-yellow-400">{c.nickname}</span>
                        </motion.div>
                    )}

                    {c.rank === 2 && (
                        <motion.div
                            animate={{ y: [0, -3, 0] }}
                            transition={{ repeat: Infinity, duration: 2 }}
                            className="flex items-center gap-2 text-center text-lg"
                        >
                            <img
                                src={`${API_URL}/${c.path}`}
                                className="border-4 h-28 w-28 rounded-full border-gray-300 object-cover shadow-[0_0_15px_rgba(209,213,219,0.6)]"
                            />
                            <span className="font-medium text-gray-300">{c.nickname}</span>
                        </motion.div>
                    )}

                    {c.rank === 3 && (
                        <motion.div
                            animate={{ y: [0, -2, 0] }}
                            transition={{ repeat: Infinity, duration: 2 }}
                            className="flex items-center gap-2 text-center"
                        >
                            <img
                                src={`${API_URL}/${c.path}`}
                                className="border-4 h-24 w-24 rounded-full border-amber-700 object-cover shadow-[0_0_15px_rgba(180,83,9,0.6)]"
                            />
                            <span className="text-amber-700">{c.nickname}</span>
                        </motion.div>
                    )}
                </motion.div>
            ))}
        </div>
    );
}

export default LeaderBoardComponent;
