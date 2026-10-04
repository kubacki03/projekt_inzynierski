/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import axios from "axios";
import { useEffect, useState } from "react";
import CountUp from "../components/CountUp";

function UserProgressComponent() {
    type LevelDto = {
        totalPoints: number;
        minimumPoints: number;
        maximumPoints: number;
        level: string;
    };

    const [userStats, setStats] = useState<LevelDto | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const API_URL = import.meta.env.VITE_API_URL;
    useEffect(() => {
        const fetchCourses = async () => {
            try {
          

                const response = await axios.get(`${API_URL}/User/Progress`
                    , {
                    withCredentials: true
                }
                );
                
                setStats(response.data);
            } catch (error: any) {

                setError("Wystąpił błąd pobierania poziomu");
            } finally {
                setLoading(false);
            }
        };

        fetchCourses();
    }, []);

    if (loading) return <div>Ładowanie...</div>;
    if (error) return <div>Błąd: {error}</div>;
    if (!userStats) return <div>Brak danych</div>;

    const progress =
        ((userStats.totalPoints - userStats.minimumPoints) /
            (userStats.maximumPoints - userStats.minimumPoints)) *
        100;

    return (
        <div className="space-y-4 text-white">
            <h1 className="text-4xl">Twój progres</h1>
            <div className="border-4 flex h-32 w-32 items-center justify-center rounded-full border-sky-700 bg-sky-900 text-lg font-bold">
                Level {userStats.level}
            </div>

            <h2 className="text-xl font-semibold">Level {userStats.level}</h2>
            <h3>
                <CountUp
                    from={0}
                    to={userStats.totalPoints}
                    separator=","
                    direction="up"
                    duration={0.5}
                    className="count-up-text"
                />  / {userStats.maximumPoints}
            </h3>

            <div className="h-4 w-full max-w-md rounded-full bg-gray-700">
                <div
                    className="h-full rounded-full bg-sky-500 transition-all duration-500" 
                    style={{ width: `${progress}%` }}
                ></div>
            </div>

            <p className="text-sm">{Math.round(progress)}%</p>
        </div>
    );
}

export default UserProgressComponent;
