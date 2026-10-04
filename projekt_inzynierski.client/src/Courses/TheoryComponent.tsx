/* eslint-disable @typescript-eslint/no-unused-vars */
import React, { useEffect, useState } from "react";
import axios from "axios";
import { useParams } from "react-router-dom";
import { Prism as SyntaxHighlighter } from "react-syntax-highlighter";
import { duotoneDark } from "react-syntax-highlighter/dist/esm/styles/prism";

function renderInlineFormatting(text: string) {
    const boldItalicRegex = /\*\*(.*?)\*\*/g;
    const parts: React.ReactNode[] = [];
    let lastIndex = 0;
    let match;

    while ((match = boldItalicRegex.exec(text)) !== null) {
        if (match.index > lastIndex) parts.push(text.slice(lastIndex, match.index));

        parts.push(
            <strong key={match.index} className="font-semibold text-blue-300 italic">
                {match[1]}
            </strong>
        );

        lastIndex = boldItalicRegex.lastIndex;
    }

    if (lastIndex < text.length) parts.push(text.slice(lastIndex));

    return parts;
}

function renderContent(content: string) {
    const codeBlockRegex = /```([\s\S]*?)```/g;
    const parts: React.ReactNode[] = [];
    let lastIndex = 0;
    let match;

    while ((match = codeBlockRegex.exec(content)) !== null) {
        if (match.index > lastIndex) {
            const plainText = content.slice(lastIndex, match.index);
            parts.push(
                <p key={lastIndex} className="mb-3 leading-relaxed text-gray-200">
                    {renderInlineFormatting(plainText)}
                </p>
            );
        }

        const codeLines = match[1].split("\n");
        const codeWithoutFirstLine = codeLines.slice(1).join("\n").trim();

        parts.push(
            <div key={match.index} className="my-4 overflow-x-auto rounded-xl shadow-md">
                <SyntaxHighlighter
                    key={match.index}
                    language="csharp"
                    style={duotoneDark}
                    showLineNumbers
                >
                    {codeWithoutFirstLine}
                </SyntaxHighlighter>

            </div>
        );

        lastIndex = codeBlockRegex.lastIndex;
    }

    if (lastIndex < content.length) {
        const plainText = content.slice(lastIndex);
        parts.push(
            <p key={lastIndex} className="mb-3 leading-relaxed text-gray-200">
                {renderInlineFormatting(plainText)}
            </p>
        );
    }

    return parts;
}

interface Theory {
    id: number;
    name: string;
    content: string;
    subjectId: number;
}

const API_URL = import.meta.env.VITE_API_URL;

const TheoryComponent: React.FC = () => {
    const { subjectId } = useParams<{ subjectId: string }>();
    const [theories, setTheories] = useState<Theory[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchTheories = async () => {
            try {
                setLoading(true);
                setError(null);

                const response = await axios.get<Theory[]>(`${API_URL}/GetTheory/${subjectId}`, {
                    withCredentials: true,
                });

                setTheories(response.data);
            } catch (err) {
                setError("Nie udało się pobrać danych.");
            } finally {
                setLoading(false);
            }
        };

        if (subjectId) fetchTheories();
    }, [subjectId]);

    if (loading)
        return (
            <div className="flex h-48 items-center justify-center text-gray-300">
                <div className="border-t-2 h-8 w-8 animate-spin rounded-full border-blue-400"></div>
                <span className="ml-3">Ładowanie...</span>
            </div>
        );

    if (error)
        return (
            <div className="rounded-lg border border-red-600 bg-red-500/20 p-4 text-red-300">
                {error}
            </div>
        );

    if (theories.length === 0)
        return <p className="text-center text-gray-400">Brak teorii do wyświetlenia.</p>;

    return (
        <div className="rounded-2xl bg-gray-950/70 p-8 text-white shadow-lg backdrop-blur-md">
            <h2 className="pb-6 text-4xl font-bold">
                Temat – {theories[0].name}
            </h2>
            <ul className="space-y-8">
                {theories.map((theory) => (
                    <li
                        key={theory.id}
                        className="rounded-xl bg-gray-900/80 p-6 shadow-md transition-all duration-200 hover:shadow-lg"
                    >
                        <h3 className="mb-4 text-2xl font-semibold text-blue-300">{theory.name}</h3>
                        <div className="prose prose-invert max-w-none">{renderContent(theory.content)}</div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default TheoryComponent;
