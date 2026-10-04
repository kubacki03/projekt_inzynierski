import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import axios from "axios";

type GameSession = {
    sessionId: string,
    technology: string,
    level: string,
    opponentId: string,
    opponentNickname: string,
    createdAt: string
}

const API_URL = import.meta.env.VITE_API_URL;

function PVPGameComponent() {
    const { sessionId } = useParams();
    const [session, setSession] = useState<GameSession | null>(null);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        axios.get<GameSession>(`${API_URL}/Matchmaking/session/${sessionId}`, { withCredentials: true })
            .then(res => setSession(res.data))
            .catch(() => setError("Nie znaleziono gry lub nie jesteś jej uczestnikiem"));
    }, [sessionId]);

    if (error) {
        return (
            <div className="text-center text-white">
                <p className="text-red-400">{error}</p>
                <Link to="/pvp" className="underline">Wróć do listy gier</Link>
            </div>
        );
    }

    if (!session) {
        return <p className="text-center text-white">Ładowanie gry...</p>;
    }

    return (
        <div className="text-center text-white">
            <h2 className="text-3xl font-bold">Pojedynek 1 vs 1</h2>
            <p className="mt-2">{session.technology} - {session.level}</p>
            <p className="mt-6 text-2xl">Ty <span className="text-indigo-400">vs</span> {session.opponentNickname}</p>
        </div>
    );
}

export default PVPGameComponent;
