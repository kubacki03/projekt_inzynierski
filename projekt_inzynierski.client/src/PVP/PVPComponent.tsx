import { useEffect, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import * as signalR from "@microsoft/signalr";

type Game = {
    id: number,
    technology: string,
    level: string,
    usersInQueue: number
}

const API_URL = import.meta.env.VITE_API_URL;

function PVPComponent() {
    const [games, setGames] = useState<Game[]>([]);
    const [queuedGameId, setQueuedGameId] = useState<number | null>(null);
    const [error, setError] = useState<string | null>(null);
    const connectionRef = useRef<signalR.HubConnection | null>(null);
    const navigate = useNavigate();

    useEffect(() => {
        axios.get<Game[]>(`${API_URL}/Matchmaking/games`, { withCredentials: true })
            .then(res => setGames(res.data))
            .catch(() => setError("Nie udało się pobrać listy gier"));

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${API_URL}/hubs/game`, { withCredentials: true })
            .withAutomaticReconnect()
            .build();

        connection.on("QueueUpdated", (updated: Game[]) => setGames(updated));
        connection.on("WaitingForOpponent", (gameId: number) => setQueuedGameId(gameId));
        connection.on("MatchFound", (sessionId: string) => {
            setQueuedGameId(null);
            navigate(`/pvp/game/${sessionId}`);
        });

        connection.start()
            .catch(() => setError("Nie udało się połączyć z serwerem gier"));
        connectionRef.current = connection;

        return () => {
            connection.stop();
            connectionRef.current = null;
        };
    }, [navigate]);

    const joinQueue = async (gameId: number) => {
        try {
            await connectionRef.current?.invoke("JoinQueue", gameId);
        } catch {
            setError("Nie udało się dołączyć do kolejki");
        }
    };

    const leaveQueue = async () => {
        await connectionRef.current?.invoke("LeaveQueue");
        setQueuedGameId(null);
    };

    return (
        <>
            {error && <p className="text-center text-red-400">{error}</p>}
            {games.length === 0 && (
                <h3 className="text-center text-2xl text-white">Brak gier</h3>
            )}
            <ul className="grid grid-cols-3 gap-4 text-white">
                {games.map((n) => (
                    <li key={n.id} className="rounded-xl border border-white/20 p-4">
                        <p className="text-lg font-semibold">{n.technology} - {n.level}</p>
                        <p>W kolejce: {n.usersInQueue}</p>
                        {queuedGameId === n.id ? (
                            <div className="mt-2 flex items-center gap-2">
                                <span className="animate-pulse">Szukanie przeciwnika...</span>
                                <button onClick={leaveQueue} className="rounded bg-red-600 px-3 py-1">Anuluj</button>
                            </div>
                        ) : (
                            <button
                                onClick={() => joinQueue(n.id)}
                                disabled={queuedGameId !== null}
                                className="mt-2 rounded bg-indigo-600 px-3 py-1 disabled:opacity-50"
                            >
                                Dołącz
                            </button>
                        )}
                    </li>
                ))}
            </ul>
        </>
    );
}

export default PVPComponent;
