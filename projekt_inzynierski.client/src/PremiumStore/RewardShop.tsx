/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import axios from "axios";
import React, { useEffect, useState } from "react";
import { motion, AnimatePresence } from "framer-motion";

export interface Reward {
    id: number;
    name: string;
    description: string;
    cost: number;
    imageUrl: string;
    isActive: boolean;
}

const API_URL = import.meta.env.VITE_API_URL;

const RewardShop: React.FC = () => {
    const [rewards, setRewards] = useState<Reward[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const [modalOpen, setModalOpen] = useState(false);
    const [modalMessage, setModalMessage] = useState<string>("");
    const [selectedImage, setSelectedImage] = useState<string | null>(null);

    useEffect(() => {
        const fetchRewards = async () => {
            try {
                setLoading(true);
                const response = await axios.get(`${API_URL}/Store/Get`, {
                    withCredentials: true,
                });
                if (!response.status) throw new Error("Błąd podczas pobierania danych");
                const data: Reward[] = await response.data;
                setRewards(data.filter((r) => r.isActive));
            } catch (err: any) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchRewards();
    }, []);

    const openImage = (url: string) => setSelectedImage(url);
    const closeImage = () => setSelectedImage(null);

    const handleBuy = async (reward: Reward) => {
        try {
            const res = await axios.post(
                `${API_URL}/Store/BuyReward`,
                {},
                { params: { rewardId: reward.id }, withCredentials: true }
            );

            if (res.status === 200) {
                setModalMessage(`Kupiono nagrodę: ${reward.name} za ${reward.cost} punktów 🎉`);
            } else {
                setModalMessage("Nie udało się kupić nagrody ❌");
            }
        } catch (err) {
            console.error("Błąd przy zakupie", err);
            setModalMessage("Za niskie saldo ⚠️");
        } finally {
            setModalOpen(true);
        }
    };

    return (
        <div className="grid grid-cols-1 gap-4 p-4 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
            {/* Podgląd obrazka */}
            {loading && (<p></p>)}
            {error && (<p>Wystapil blad</p>)}
            <AnimatePresence>
                {selectedImage && (
                    <motion.div
                        className="fixed inset-0 z-50 flex items-center justify-center bg-black/80"
                        onClick={closeImage}
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                    >
                        <motion.img
                            src={selectedImage}
                            alt="Podgląd nagrody"
                            className="max-h-[90vh] max-w-[90vw] rounded-xl shadow-lg"
                            initial={{ scale: 0.8, opacity: 0 }}
                            animate={{ scale: 1, opacity: 1 }}
                            exit={{ scale: 0.8, opacity: 0 }}
                            transition={{ duration: 0.3 }}
                        />
                    </motion.div>
                )}
            </AnimatePresence>

            {/* Lista nagród */}
            <AnimatePresence>
                {rewards.map((reward, i) => (
                    <motion.div
                        key={reward.id}
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ delay: i * 0.1 }}
                        whileHover={{ scale: 1.03 }}
                        whileTap={{ scale: 0.98 }}
                        className="flex flex-col overflow-hidden rounded-xl border border-indigo-400 bg-gray-800 shadow-md"
                    >
                        <img
                            src={`${API_URL}/${reward.imageUrl}`}
                            alt={reward.name}
                            onClick={() => openImage(`${API_URL}/${reward.imageUrl}`)}
                            className="w-full aspect-[4/3] object-contain cursor-pointer hover:opacity-90 transition bg-gray-700"

                        />

                        <div className="flex flex-grow flex-col justify-between p-4">
                            <div>
                                <h3 className="mb-2 text-lg font-bold text-white">{reward.name}</h3>
                                <p className="mb-2 text-sm text-gray-300">{reward.description}</p>
                                <p className="font-semibold text-indigo-300">{reward.cost} pkt</p>
                            </div>
                            <motion.button
                                whileHover={{ scale: 1.05 }}
                                whileTap={{ scale: 0.95 }}
                                onClick={() => handleBuy(reward)}
                                className="mt-3 bg-indigo-600 hover:bg-indigo-500 text-white font-bold py-2 px-3 rounded transition"
                            >
                                Kup
                            </motion.button>
                        </div>
                    </motion.div>
                ))}
            </AnimatePresence>

            {/* Modal */}
            <AnimatePresence>
                {modalOpen && (
                    <motion.div
                        className="fixed inset-0 z-50 flex items-center justify-center bg-black/50"
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                    >
                        <motion.div
                            className="w-full max-w-sm rounded-xl bg-gray-900 p-6 text-white shadow-lg"
                            initial={{ scale: 0.8, opacity: 0 }}
                            animate={{ scale: 1, opacity: 1 }}
                            exit={{ scale: 0.8, opacity: 0 }}
                            transition={{ duration: 0.3 }}
                        >
                            <h2 className="mb-4 text-xl font-bold">Informacja</h2>
                            <p>{modalMessage}</p>
                            <div className="mt-6 flex justify-end">
                                <motion.button
                                    whileHover={{ scale: 1.05 }}
                                    whileTap={{ scale: 0.95 }}
                                    onClick={() => setModalOpen(false)}
                                    className="bg-indigo-600 hover:bg-indigo-500 px-4 py-2 rounded"
                                >
                                    OK
                                </motion.button>
                            </div>
                        </motion.div>
                    </motion.div>
                )}
            </AnimatePresence>
        </div>
    );
};

export default RewardShop;
