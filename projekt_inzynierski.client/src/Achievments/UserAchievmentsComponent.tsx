/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import Shuffle from '../Challenges/Shuffle';
import { motion } from 'framer-motion';
import { toast, ToastContainer } from 'react-toastify';

// Typy dla osiągnięć
type BadgeType = 'bronze' | 'silver' | 'gold' | 'platinum';
type SkillCategory = 'frontend' | 'backend' | 'fullstack' | 'database' | 'algorithms'|'common';

interface Achievement {
    id: string;
    title: string;
    description: string;
    dateEarned: Date;
    badgeType: BadgeType;
    progress?: number;
    completed: boolean;
    skillCategory: SkillCategory;
}

const UserAchievementsComponent: React.FC = () => {
    const badgeColors: Record<BadgeType, string> = {
        bronze: 'bg-yellow-900 text-yellow-100',
        silver: 'bg-gray-300 text-gray-800',
        gold: 'bg-yellow-400 text-yellow-900',
        platinum: 'bg-blue-100 text-blue-900'
    };

    const skillIcons: Record<SkillCategory, string> = {
        frontend: '💻',
        backend: '⚙️',
        fullstack: '🌐',
        database: '💾',
        algorithms: '🧠',
        common:'🧑‍💻'
    };

    const categoryColors: Record<SkillCategory, string> = {
        frontend: 'bg-blue-100 border-blue-300',
        backend: 'bg-green-100 border-green-300',
        fullstack: 'bg-purple-100 border-purple-300',
        database: 'bg-amber-100 border-amber-300',
        algorithms: 'bg-red-100 border-red-300',
        common: 'bg-cyan-100 border-cyan-500'
    };
    const API_URL = import.meta.env.VITE_API_URL;
    const [userAchievments, setAchievments] = useState<Achievement[]>([]);

    const [error, setError] = useState<string>();

    const [favTopic, setFavTopic] = useState<string>();
    const [percentage, setPercentage] = useState<number>(0.0)
    useEffect(() => {
        const fetchAchievments = async () => {
            try {
                
                const response = await axios.get(`${API_URL}/UserAchievment/GetUserAchievments`, {
                    withCredentials: true
                });

                const percentage = await axios.get(`${API_URL}/UserAchievment/GetAchievementPercentage`, {
                    withCredentials: true
                });
                const topic = await axios.get(`${API_URL}/UserAchievment/FavouriteTopics`, {
                    withCredentials: true
                });

                setFavTopic(topic.data);
                setPercentage(percentage.data)

                console.log(response);
               
                const mapped: Achievement[] = response.data.map((a: any) => ({
                    id: crypto.randomUUID(),                 
                    title: a.name,                           
                    description: a.description,              
                    dateEarned: a.date && a.date !== "0001-01-01T00:00:00"
                        ? new Date(a.date)
                        : new Date(),
                    badgeType: (a.level?.toLowerCase() ?? 'bronze') as BadgeType, 
                    progress: 0,                             
                    completed: a.date && a.date !== "0001-01-01T00:00:00",
                    skillCategory: (a.category?.toLowerCase() ?? 'ogólne') as SkillCategory 
                }));


                setAchievments(mapped);
            } catch (err: any) {
                setError("Wystąpił błąd");
                toast.error("Wystąpił błąd")
            }
        };
        fetchAchievments();
    }, []);

    return (
        <motion.div

            className="mx-auto max-w-6xl p-6"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ duration: 0.6 }}
        >

            <ToastContainer position="top-right" autoClose={3000} />
            <div className="text-white">
                <div className="flex flex-col items-center">
                    <Shuffle text={'Twoje osiągnięcia'} />

                </div>    

            <p className="mb-6 text-center">Świętuj swoje postępy w nauce programowania!</p>
            </div>
            <div className="mb-8 grid grid-cols-1 gap-4 md:grid-cols-3">
                <div className="rounded-lg border-l-4 border-blue-500 bg-white p-4 shadow">
                    <h3 className="text-lg font-bold">Zdobyte odznaki</h3>
                    <p className="text-2xl">{userAchievments.filter(a => a.completed).length}</p>
                </div>
                <div className="rounded-lg border-l-4 border-yellow-500 bg-white p-4 shadow">
                    <h3 className="text-lg font-bold">Twoja ulubiona dziedzina</h3>
                    <p className="text-2xl">{favTopic}</p>
                </div>
                <div className="rounded-lg border-l-4 border-green-500 bg-white p-4 shadow">
                    <h3 className="text-lg font-bold">Wskaźnik ukończenia</h3>
                    <p className="text-2xl">
                        {Math.round(percentage*100)}%
                    </p>
                </div>
            </div>

            {error && (<p className="text-center text-3xl text-red-700">Wystąpił błąd</p>)}


            <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
                {userAchievments.map(achievement => (
                    <motion.div
                        key={achievement.id}
                        className={`rounded-xl overflow-hidden shadow-lg border ${categoryColors[achievement.skillCategory]} transition-transform hover:scale-[1.02]`}
                        initial={{ opacity: 0, y: 20 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.4 }}
                        whileHover={{ scale: 1.03 }}
                    >

                        <div className="p-5">
                            <div className="flex items-start justify-between">
                                <div>
                                    <span className="text-2xl">{skillIcons[achievement.skillCategory]}</span>
                                    <h3 className="mt-2 text-xl font-bold">{achievement.title}</h3>
                                </div>
                                <span className={`${badgeColors[achievement.badgeType]} px-3 py-1 rounded-full text-xs font-bold`}>
                                    {achievement.badgeType.toUpperCase()}
                                </span>
                            </div>

                            <p className="my-3 text-gray-600">{achievement.description}</p>

                            {achievement.completed ? (
                                <div className="mt-4">
                                    <p className="text-sm font-medium text-green-600">✓ Zdobyto: {achievement.dateEarned.toLocaleDateString()}</p>
                                </div>
                            ) : (
                                <div className="mt-4">
                                    <div className="mb-1 flex justify-between text-sm">
                                        <span>Postęp:</span>
                                        <span className="font-medium">{achievement.progress}%</span>
                                    </div>
                                    <div className="h-2 w-full rounded-full bg-gray-200">
                                        <div
                                            className="h-2 rounded-full bg-blue-600"
                                            style={{ width: `${achievement.progress}%` }}
                                        ></div>
                                    </div>
                                    <p className="mt-2 text-xs text-gray-500">Kontynuuj naukę, aby zdobyć tę odznakę!</p>
                                </div>
                            )}
                        </div>
                    </motion.div>
                ))}
            </div>

            <div className="mt-10 rounded-lg bg-gray-50 p-4">
                <h3 className="mb-3 font-bold">Legenda osiągnięć</h3>
                <div className="flex flex-wrap gap-4">
                    <div className="flex items-center">
                        <span className={`w-4 h-4 rounded-full mr-2 ${badgeColors.bronze}`}></span>
                        <span>Brązowe - początkujący</span>
                    </div>
                    <div className="flex items-center">
                        <span className={`w-4 h-4 rounded-full mr-2 ${badgeColors.silver}`}></span>
                        <span>Srebrne - średniozaawansowany</span>
                    </div>
                    <div className="flex items-center">
                        <span className={`w-4 h-4 rounded-full mr-2 ${badgeColors.gold}`}></span>
                        <span>Złote - zaawansowany</span>
                    </div>
                    <div className="flex items-center">
                        <span className={`w-4 h-4 rounded-full mr-2 ${badgeColors.platinum}`}></span>
                        <span>Platynowe - ekspert</span>
                    </div>
                </div>
            </div>
        </motion.div>
    );
};

export default UserAchievementsComponent;
