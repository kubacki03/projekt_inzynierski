import { useCallback, useEffect, useRef, useState } from "react";
import { Link, useParams } from "react-router-dom";
import axios from "axios";
import * as signalR from "@microsoft/signalr";

type GameSession = {
    sessionId: string,
    technology: string,
    level: string,
    opponentId: string,
    opponentNickname: string,
    createdAt: string
}

type Question = {
    index: number,
    total: number,
    text: string,
    options: string[],
    activeUserId: string,
    timeLimitMs: number,
    remainingMs: number
}

type Resolved = {
    index: number,
    userId: string,
    selectedIndex: number,
    correctIndex: number,
    isCorrect: boolean,
    scores: Record<string, number>
}

type Finished = {
    winnerId: string | null,
    reason: string,
    scores: Record<string, number>,
    coinsAwarded: number
}

type FocusLost = {
    userId: string,
    count: number,
    max: number
}

type GameState = {
    status: "WaitingForPlayers" | "InProgress" | "Finished",
    yourUserId: string,
    opponentUserId: string,
    totalQuestions: number,
    scores: Record<string, number>,
    focusLosses: Record<string, number>,
    maxFocusLosses: number,
    question: Question | null,
    answer: Resolved | null,
    result: Finished | null
}

const API_URL = import.meta.env.VITE_API_URL;

const REASON_LABELS: Record<string, string> = {
    FocusLost: "przez opuszczenie karty przeglądarki",
    OpponentAbsent: "przeciwnik nie dołączył do gry",
    Error: "wystąpił błąd gry",
    Completed: ""
};

function PVPGameComponent() {
    const { sessionId } = useParams();
    const [session, setSession] = useState<GameSession | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [state, setState] = useState<GameState | null>(null);
    const [question, setQuestion] = useState<Question | null>(null);
    const [resolved, setResolved] = useState<Resolved | null>(null);
    const [selected, setSelected] = useState<number | null>(null);
    const [remainingMs, setRemainingMs] = useState(0);
    const [scores, setScores] = useState<Record<string, number>>({});
    const [focusLosses, setFocusLosses] = useState<Record<string, number>>({});
    const [result, setResult] = useState<Finished | null>(null);
    const connectionRef = useRef<signalR.HubConnection | null>(null);
    const deadlineRef = useRef(0);
    const focusLostRef = useRef(false);
    const inProgressRef = useRef(false);

    const applyState = useCallback((s: GameState) => {
        setState(s);
        setScores(s.scores);
        setFocusLosses(s.focusLosses);
        setQuestion(s.question);
        setResolved(s.answer);
        setSelected(s.answer ? s.answer.selectedIndex : null);
        setResult(s.result);
        if (s.question) {
            deadlineRef.current = Date.now() + s.question.remainingMs;
        }
        inProgressRef.current = s.status === "InProgress";
    }, []);

    useEffect(() => {
        axios.get<GameSession>(`${API_URL}/Matchmaking/session/${sessionId}`, { withCredentials: true })
            .then(res => setSession(res.data))
            .catch(() => setError("Nie znaleziono gry lub nie jesteś jej uczestnikiem"));
    }, [sessionId]);

    useEffect(() => {
        if (!sessionId) {
            return;
        }

        const connection = new signalR.HubConnectionBuilder()
            .withUrl(`${API_URL}/hubs/game`, { withCredentials: true })
            .withAutomaticReconnect()
            .build();

        const join = async () => {
            try {
                const s = await connection.invoke<GameState>("JoinSession", sessionId);
                applyState(s);
            } catch {
                setError("Nie udało się dołączyć do gry");
            }
        };

        connection.on("QuestionStarted", (q: Question) => {
            deadlineRef.current = Date.now() + q.remainingMs;
            setQuestion(q);
            setResolved(null);
            setSelected(null);
            setState(prev => prev ? { ...prev, status: "InProgress" } : prev);
            inProgressRef.current = true;
        });

        connection.on("AnswerResolved", (r: Resolved) => {
            setResolved(r);
            setScores(r.scores);
            setSelected(r.selectedIndex);
        });

        connection.on("FocusLost", (f: FocusLost) => {
            setFocusLosses(prev => ({ ...prev, [f.userId]: f.count }));
        });

        connection.on("GameFinished", (f: Finished) => {
            inProgressRef.current = false;
            setScores(f.scores);
            setResult(f);
            setState(prev => prev ? { ...prev, status: "Finished" } : prev);
        });

        connection.onreconnected(join);

        connection.start()
            .then(join)
            .catch(() => setError("Nie udało się połączyć z serwerem gier"));
        connectionRef.current = connection;

        return () => {
            connection.stop();
            connectionRef.current = null;
        };
    }, [sessionId, applyState]);

    useEffect(() => {
        const interval = setInterval(() => {
            setRemainingMs(Math.max(0, deadlineRef.current - Date.now()));
        }, 100);
        return () => clearInterval(interval);
    }, []);

    useEffect(() => {
        const reportFocusLost = () => {
            if (!inProgressRef.current || focusLostRef.current) {
                return;
            }
            focusLostRef.current = true;
            connectionRef.current?.invoke("ReportFocusLost", sessionId).catch(() => undefined);
        };

        const restoreFocus = () => {
            if (document.visibilityState === "visible" && document.hasFocus()) {
                focusLostRef.current = false;
            }
        };

        const onVisibilityChange = () => {
            if (document.visibilityState === "hidden") {
                reportFocusLost();
            } else {
                restoreFocus();
            }
        };

        window.addEventListener("blur", reportFocusLost);
        window.addEventListener("focus", restoreFocus);
        document.addEventListener("visibilitychange", onVisibilityChange);
        return () => {
            window.removeEventListener("blur", reportFocusLost);
            window.removeEventListener("focus", restoreFocus);
            document.removeEventListener("visibilitychange", onVisibilityChange);
        };
    }, [sessionId]);

    const submitAnswer = async (answerIndex: number) => {
        if (!question || resolved || selected !== null) {
            return;
        }
        setSelected(answerIndex);
        try {
            await connectionRef.current?.invoke("SubmitAnswer", sessionId, question.index, answerIndex);
        } catch {
            setSelected(null);
        }
    };

    if (error) {
        return (
            <div className="text-center text-white">
                <p className="text-red-400">{error}</p>
                <Link to="/pvp" className="underline">Wróć do listy gier</Link>
            </div>
        );
    }

    if (!session || !state) {
        return <p className="text-center text-white">Ładowanie gry...</p>;
    }

    const myId = state.yourUserId;
    const opponentId = state.opponentUserId;
    const isMyTurn = question?.activeUserId === myId;
    const myFocusLosses = focusLosses[myId] ?? 0;
    const opponentFocusLosses = focusLosses[opponentId] ?? 0;
    const timeFraction = question ? Math.min(1, remainingMs / question.timeLimitMs) : 0;

    const optionClass = (index: number) => {
        const base = "w-full rounded-lg border px-4 py-3 text-left transition disabled:cursor-not-allowed";
        if (resolved) {
            if (index === resolved.correctIndex) {
                return `${base} border-green-400 bg-green-600/40`;
            }
            if (index === resolved.selectedIndex) {
                return `${base} border-red-400 bg-red-600/40`;
            }
            return `${base} border-white/20 opacity-60`;
        }
        if (index === selected) {
            return `${base} border-indigo-400 bg-indigo-600/40`;
        }
        return `${base} border-white/20 hover:bg-white/10 disabled:opacity-60`;
    };

    if (result) {
        const won = result.winnerId === myId;
        const draw = result.winnerId === null;
        const reason = REASON_LABELS[result.reason] ?? "";
        return (
            <div className="mx-auto max-w-xl text-center text-white">
                <h2 className="text-3xl font-bold">
                    {draw ? "Remis" : won ? "Wygrana!" : "Przegrana"}
                </h2>
                {reason && <p className="mt-2 text-white/70">{reason}</p>}
                <p className="mt-4 text-2xl">
                    {scores[myId] ?? 0} : {scores[opponentId] ?? 0}
                </p>
                {won && result.coinsAwarded > 0 && (
                    <p className="mt-4 text-xl text-yellow-300">+{result.coinsAwarded} złotych monet</p>
                )}
                <Link to="/pvp" className="mt-6 inline-block rounded bg-indigo-600 px-4 py-2">
                    Wróć do listy gier
                </Link>
            </div>
        );
    }

    return (
        <div className="mx-auto max-w-2xl text-white">
            <div className="flex items-center justify-between">
                <div className="text-center">
                    <p className="font-semibold">Ty</p>
                    <p className="text-3xl font-bold text-indigo-400">{scores[myId] ?? 0}</p>
                    <p className="text-xs text-white/60">Opuszczenia karty: {myFocusLosses}/{state.maxFocusLosses}</p>
                </div>
                <div className="text-center">
                    <p className="text-sm text-white/70">{session.technology} - {session.level}</p>
                    {question && <p className="text-lg">Pytanie {question.index + 1}/{question.total}</p>}
                </div>
                <div className="text-center">
                    <p className="font-semibold">{session.opponentNickname}</p>
                    <p className="text-3xl font-bold text-indigo-400">{scores[opponentId] ?? 0}</p>
                    <p className="text-xs text-white/60">Opuszczenia karty: {opponentFocusLosses}/{state.maxFocusLosses}</p>
                </div>
            </div>

            {myFocusLosses > 0 && (
                <p className="mt-4 rounded bg-red-600/30 p-2 text-center text-red-200">
                    Opuściłeś kartę {myFocusLosses}/{state.maxFocusLosses} razy. Przy {state.maxFocusLosses}. razie automatycznie przegrywasz.
                </p>
            )}

            {!question && (
                <p className="mt-10 animate-pulse text-center text-xl">
                    {state.status === "WaitingForPlayers" ? "Czekanie na przeciwnika..." : "Gra zaraz się rozpocznie..."}
                </p>
            )}

            {question && (
                <div className="mt-6 rounded-xl border border-white/20 p-6">
                    <div className="mb-4 h-2 w-full overflow-hidden rounded bg-white/10">
                        <div
                            className={`h-full ${timeFraction < 0.25 ? "bg-red-500" : "bg-indigo-500"}`}
                            style={{ width: `${timeFraction * 100}%` }}
                        />
                    </div>
                    <p className="mb-1 text-sm text-white/70">
                        {isMyTurn ? `Twoja kolej - ${Math.ceil(remainingMs / 1000)} s` : `Odpowiada ${session.opponentNickname} - ${Math.ceil(remainingMs / 1000)} s`}
                    </p>
                    <h3 className="mb-4 text-xl font-semibold">{question.text}</h3>
                    <div className="grid gap-3">
                        {question.options.map((option, index) => (
                            <button
                                key={index}
                                onClick={() => submitAnswer(index)}
                                disabled={!isMyTurn || resolved !== null || selected !== null}
                                className={optionClass(index)}
                            >
                                {option}
                            </button>
                        ))}
                    </div>
                    {resolved && (
                        <p className="mt-4 text-center">
                            {resolved.selectedIndex < 0
                                ? "Czas minął"
                                : resolved.isCorrect
                                    ? "Poprawna odpowiedź"
                                    : "Błędna odpowiedź"}
                        </p>
                    )}
                </div>
            )}
        </div>
    );
}

export default PVPGameComponent;
