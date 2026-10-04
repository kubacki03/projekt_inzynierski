/* eslint-disable @typescript-eslint/no-unused-vars */
import { useState } from "react";

interface Answer {
    id: number;
    text: string;
    isCorrect: boolean;
}
interface Quiz {
    id: number;
    question: string;
    answers: Answer[];
    subjectId: number;
    isCompleted: boolean;
}

function QuizComponent({ quizzes }: { quizzes: Quiz[] }) {
    const [selectedAnswers, setSelectedAnswers] = useState<Record<number, number | null>>({});
    const [disabledQuizzes, setDisabledQuizzes] = useState<Record<number, boolean>>({});
    const API_URL = import.meta.env.VITE_API_URL;

    const handleSelect = async (quizId: number, answerId: number, isCompleted: boolean) => {
        if (isCompleted || disabledQuizzes[quizId]) return;
        setSelectedAnswers((prev) => ({ ...prev, [quizId]: answerId }));

        const selectedAnswer = quizzes.find((q) => q.id === quizId)?.answers.find((a) => a.id === answerId);

        if (selectedAnswer?.isCorrect) {
            setDisabledQuizzes((prev) => ({ ...prev, [quizId]: true }));
            setTimeout(() => {
                setDisabledQuizzes((prev) => ({ ...prev, [quizId]: false }));
            }, 1000);
        }

        try {
            await fetch(`${API_URL}/Quiz/SaveAnswer`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                credentials: "include",
                body: JSON.stringify({ quizId, answerId }),
            });
        } catch (error) {
            alert("Błąd przy zapisie odpowiedzi");
        }
    };

    return (
        <div className="space-y-6">
            {quizzes.map((quiz) => {
                const correctAnswer = quiz.answers.find((a) => a.isCorrect);
                return (
                    <div
                        key={quiz.id}
                        className="rounded-2xl border border-indigo-500 bg-gray-900/80 p-6 shadow-md transition-all hover:shadow-lg"
                    >
                        {quiz.isCompleted && (
                            <h1 className="mb-2 font-bold text-green-400">✅ Quiz ukończony</h1>
                        )}
                        <h2 className="mb-4 text-xl font-semibold text-blue-300">{quiz.question}</h2>

                        <ul className="space-y-3">
                            {quiz.answers.map((answer) => {
                                const isSelected = selectedAnswers[quiz.id] === answer.id;
                                const isCorrectAnswer = quiz.isCompleted && correctAnswer?.id === answer.id;
                                return (
                                    <li key={answer.id}>
                                        <button
                                            onClick={() => handleSelect(quiz.id, answer.id, quiz.isCompleted)}
                                            disabled={quiz.isCompleted || disabledQuizzes[quiz.id]}
                                            className={`w-full rounded-lg px-4 py-2 text-left transition-all duration-150
                        ${isCorrectAnswer
                                                    ? "bg-green-600 text-white"
                                                    : isSelected
                                                        ? "bg-blue-700 text-white"
                                                        : "bg-gray-800 hover:bg-gray-700 text-gray-200"
                                                }
                        ${quiz.isCompleted || disabledQuizzes[quiz.id] ? "opacity-70 cursor-not-allowed" : ""}
                      `}
                                        >
                                            {answer.text}
                                        </button>
                                    </li>
                                );
                            })}
                        </ul>

                        {!quiz.isCompleted && selectedAnswers[quiz.id] !== undefined && (
                            <p className="mt-3 text-sm">
                                {quiz.answers.find((a) => a.id === selectedAnswers[quiz.id])?.isCorrect
                                    ? "✅ Poprawna odpowiedź!"
                                    : "❌ Błędna odpowiedź"}
                            </p>
                        )}
                    </div>
                );
            })}
        </div>
    );
}

export default QuizComponent;
