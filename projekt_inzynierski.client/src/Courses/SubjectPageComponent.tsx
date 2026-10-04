import axios from "axios";
import { useEffect, useState } from "react";
import { Link, useLocation, useParams } from "react-router-dom";
import QuizComponent from "./QuizComponent";
import Loader from "./Loader";
import RotatingText from "../components/RotatingText";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

interface Quiz {
    id: number;
    question: string;
    answers: Answer[];
    subjectId: number;
    isCompleted: boolean;
}
interface Answer {
    id: number;
    text: string;
    isCorrect: boolean;
}
interface Exercise {
    id: number;
    task: string;
    subjectId: number;
    isDone: boolean;
}
interface Theory {
    publicId: number;
    name: string;
    isDone: boolean;
}
interface SubjectData {
    quizzes: Quiz[];
    theories: Theory[];
    tasks: Exercise[];
}

function SubjectPageComponent() {
    const { subjectId, courseId } = useParams<{ subjectId: string; courseId: string }>();
    const [subjectData, setSubjectData] = useState<SubjectData[]>([]);
    const location = useLocation();
    const { subjectName } = location.state || {};
    const [selectedPdf, setSelectedPdf] = useState<File | null>(null);
    const [uploading, setUploading] = useState(false);
    const [loading, setLoading] = useState(false);
    const [showQuizzes, setShowQuizzes] = useState(true);
    const API_URL = import.meta.env.VITE_API_URL;

    useEffect(() => {
        fetchSubjectData();
    }, [subjectId]);

    const fetchSubjectData = async () => {
        try {
            setLoading(true);
            await axios.post(`${API_URL}/CheckIfContnentIsGenerated/${subjectId}`, {}, { withCredentials: true });

            const [theoryRes, exerciseRes, quizRes] = await Promise.all([
                axios.get(`${API_URL}/GetTheory/${subjectId}`, { withCredentials: true }),
                axios.get(`${API_URL}/GetExercise/${subjectId}`, { withCredentials: true }),
                axios.get(`${API_URL}/GetQuiz/${subjectId}`, { withCredentials: true }),
            ]);

            setSubjectData([
                {
                    theories: theoryRes.data || [],
                    tasks: exerciseRes.data || [],
                    quizzes: quizRes.data || [],
                },
            ]);
        } catch {
            alert("Błąd podczas pobierania danych");
        } finally {
            setLoading(false);
        }
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files?.length) setSelectedPdf(e.target.files[0]);
    };

    const handleUploadPdf = async () => {
        if (!selectedPdf) return;
        setUploading(true);
        const formData = new FormData();
        formData.append("pdfFile", selectedPdf);

        try {
            await axios.post(`${API_URL}/GenerateTheoryFromPdf/${subjectId}/${courseId}`, formData, {
                withCredentials: true,
                headers: { "Content-Type": "multipart/form-data" },
            });
            toast.success("PDF został przesłany i teoria wygenerowana!");
            setSelectedPdf(null);
            await fetchSubjectData();
        } catch {
            alert("Wystąpił błąd podczas przesyłania PDF.");
        } finally {
            setUploading(false);
        }
    };

    interface Item {
        title: string;
        link: string;
    }

    function AccordionSection({ title, items }: { title: string; items: Item[] }) {
        const [isOpen, setIsOpen] = useState(true);
        return (
            <div className="mb-6">
                <button
                    onClick={() => setIsOpen(!isOpen)}
                    className="w-full flex justify-between items-center rounded-xl bg-gray-900/80 px-4 py-3 text-left text-lg font-semibold text-blue-300 shadow-md hover:bg-gray-800 transition-all duration-200"
                >
                    <span>{title}</span>
                    <span>{isOpen ? "▲" : "▼"}</span>
                </button>

                {isOpen && (
                    <ul className="mt-3 space-y-3">
                        {items.map((item, idx) => (
                            <Link
                                key={idx}
                                to={item.link}
                                className="block rounded-2xl border border-indigo-500 bg-gray-900/70 p-4 shadow-md transition-all hover:shadow-lg hover:-translate-y-1"
                            >
                                <p className="font-semibold text-gray-200">{item.title}</p>
                            </Link>
                        ))}
                    </ul>
                )}
            </div>
        );
    }

    const theoriesItems = subjectData.flatMap((subject) =>
        subject.theories.slice(0, 1).map((theory) => ({
            title: theory.name,
            link: `/theory/${subjectId}`,
        }))
    );

    const tasksItems = subjectData.flatMap((subject) =>
        subject.tasks.map((task) => ({
            title: `${task.task} ${task.isDone ? "✅ (ukończone)" : "❌ (nieukończone)"}`,
            link: `/exercise/${task.id}`,
        }))
    );

    function getProgress<T extends { isDone?: boolean; isCompleted?: boolean }>(
        items: T[],
        key: "isDone" | "isCompleted"
    ) {
        const completed = items.filter((i) => i[key]).length;
        return `${completed}/${items.length}`;
    }

    return (
        <div className="rounded-2xl bg-gray-950/60 p-6 text-white shadow-lg backdrop-blur-md">
            <ToastContainer position="top-right" autoClose={3000} />
            {!loading ? (
                <>
                    <h1 className="mb-6 text-3xl font-bold text-white">{subjectName}</h1>

                    <div className="mb-8 rounded-xl bg-gray-900/80 p-4 shadow-md">
                        <p className="mb-3 font-semibold text-gray-200">
                            Dodaj własny PDF, aby wygenerować teorię:
                        </p>
                        <div className="flex flex-col items-start gap-3 md:flex-row">
                            <input
                                type="file"
                                accept="application/pdf"
                                onChange={handleFileChange}
                                className="text-sm text-gray-300 file:mr-4 file:rounded-md file:border-0 file:bg-blue-600 file:px-3 file:py-2 file:text-white hover:file:bg-blue-500"
                            />
                            <button
                                onClick={handleUploadPdf}
                                disabled={!selectedPdf || uploading}
                                className="flex items-center gap-2 rounded-md bg-teal-600 px-4 py-2 transition-all hover:bg-teal-500 disabled:opacity-50"
                            >
                                {uploading && (
                                    <div className="border-2 border-t-transparent h-4 w-4 animate-spin rounded-full border-white"></div>
                                )}
                                {uploading ? "Wysyłanie..." : "Wyślij PDF"}
                            </button>
                        </div>
                    </div>

                    <AccordionSection title="Materiał teoretyczny" items={theoriesItems} />
                    <AccordionSection
                        title={`Zadania praktyczne (${getProgress(subjectData.flatMap((s) => s.tasks), "isDone")})`}
                        items={tasksItems}
                    />

                    <div className="mb-4">
                        <button
                            onClick={() => setShowQuizzes((prev) => !prev)}
                            className="w-full flex justify-between items-center rounded-xl bg-gray-900/80 px-4 py-3 text-left text-lg font-semibold text-blue-300 shadow-md hover:bg-gray-800 transition-all"
                        >
                            <span>
                                Test wiedzy
                            </span>
                            <span>{showQuizzes ? "▲" : "▼"}</span>
                        </button>
                        {showQuizzes && (
                            <div className="mt-3">
                                {subjectData.map((subject, idx) => (
                                    <QuizComponent key={idx} quizzes={subject.quizzes} />
                                ))}
                            </div>
                        )}
                    </div>
                </>
            ) : (
                <div className="flex flex-col items-center justify-center gap-16 py-20">
                    <Loader />
                    <RotatingText
                        texts={[
                            "Start sekwencji startowej",
                            "Analiza postępów",
                            "T minus 10 sekund",
                            "Generowanie materiałów",
                            "Dostosowywanie zadań",
                            "Wyznaczanie poziomu trudności",
                        ]}
                        mainClassName="text-center text-3xl text-blue-300"
                        rotationInterval={4000}
                    />
                </div>
            )}
        </div>
    );
}

export default SubjectPageComponent;
