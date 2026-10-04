import { useState, useEffect, useRef } from "react";
import { Card, CardContent } from "@/components/ui/card";
import MonacoEditor from "@monaco-editor/react";
import Shuffle from "../Challenges/Shuffle";

type Language = "csharp" | "java" | "python" | "javascript" | "typescript";

interface CodeRunnerResponse {
    output: string;
    errors: string;
}

interface CodeAnalysisResponse {
    isDoneGood?: boolean;
    review?: string;
 
}

const sampleCode: Record<Language, string> = {
    csharp: `using System;

class Program {
    static void Main() {
        Console.WriteLine("Hello from C#!");
    }
}`,
    java: `public class Main {
    public static void main(String[] args) {
        System.out.println("Hello from Java!");
    }
}`,
    python: `print("Hello from Python!")`,
    javascript: `console.log("Hello from JavaScript!");`,
    typescript: `console.log("Hello from TypeScript!");`
};

export default function CodeRunner() {
    const API_URL = import.meta.env.VITE_API_URL as string;

    const [language, setLanguage] = useState<Language>("csharp");
    const [code, setCode] = useState<string>(sampleCode[language]);
    const [output, setOutput] = useState<string>("");
    const [errors, setErrors] = useState<string>("");
    const [loading, setLoading] = useState<boolean>(false);
    const [analysisResult, setAnalysisResult] = useState<CodeAnalysisResponse | null>(null); 
    const lastSentCode = useRef<string>(code); 

 
    const runCode = async () => {
        setLoading(true);
        setOutput("");
        setErrors("");

        try {
            const response = await fetch(`${API_URL}/api/CodeRunner`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ code, language }),
            });

            const result: CodeRunnerResponse = await response.json();
            setOutput(result.output || "");
            setErrors(result.errors || "");
        } catch (err) {
            setErrors("Network error: " + (err instanceof Error ? err.message : String(err)));
        } finally {
            setLoading(false);
        }
    };

    const handleLanguageChange = (lang: Language) => {
        setLanguage(lang);
        setCode(sampleCode[lang]);
    };

    const monacoLangMap: Record<Language, string> = {
        csharp: "csharp",
        java: "java",
        python: "python",
        javascript: "javascript",
        typescript: "typescript"
    };

    
    useEffect(() => {
        const interval = setInterval(async () => {
            if (code !== lastSentCode.current) { 
                try {
                    const response = await fetch(`${API_URL}/CodeAssistant/CheckCode`, {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({ code, language }),
                    });
                    if (response.ok) {
                        const result = await response.json();
                        
                        setAnalysisResult(result);
                        lastSentCode.current = code; 
                    }
                } catch (err) {
                    console.error("Auto-analysis failed:", err);
                }
            }
        },2* 5 * 1000); 

        return () => clearInterval(interval);
    }, [code, language, API_URL]);

    return (
        <div className="rounded-2xl bg-gray-950/60 p-8 text-white shadow-lg backdrop-blur-md">
            <div className="mb-6">
                <Shuffle text={'CodeRunner'} />
                <p className="text-lg leading-relaxed text-blue-400">
                    Wybierz język programowania i uruchom swój kod
                </p>
            </div>

            <div className="mb-6">
                <label className="mr-3 text-lg font-semibold text-gray-200">Język:</label>
                <select
                    value={language}
                    onChange={(e) => handleLanguageChange(e.target.value as Language)}
                    className="rounded-lg bg-gray-800 px-4 py-2 text-white border border-gray-700 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500/20 transition-all"
                >
                    <option value="csharp">C#</option>
                    <option value="java">Java</option>
                    <option value="python">Python</option>
                    <option value="javascript">JavaScript</option>
                    <option value="typescript">TypeScript</option>
                </select>
            </div>
            {analysisResult && (
                <Card className="border border-gray-800 bg-gray-900/50 shadow-lg">
                    <CardContent className="space-y-3 p-6">
                        <h2 className="text-lg font-semibold text-indigo-400">Analiza kodu:</h2>
                        {analysisResult.isDoneGood && (
                            <p className="text-yellow-300">Ocena: {analysisResult.isDoneGood}</p>
                        )}
                        {analysisResult.review && (
                            <p className="whitespace-pre-wrap text-indigo-300">{analysisResult.review}</p>
                        )}

                    </CardContent>
                </Card>
            )}
            <Card className="mb-6 overflow-hidden border border-gray-800 bg-gray-900/50 shadow-lg">
                <CardContent className="p-0">
                    <MonacoEditor
                        height="400px"
                        language={monacoLangMap[language]}
                        value={code}
                        onChange={(value) => setCode(value || "")}
                        theme="vs-dark"
                        options={{
                            fontSize: 14,
                            automaticLayout: true,
                            minimap: { enabled: false },
                            wordWrap: "on",
                            scrollBeyondLastLine: false,
                            padding: { top: 16, bottom: 16 },
                        }}
                    />
                </CardContent>
            </Card>

            <div className="mb-6 flex items-center gap-3">
                <button
                    onClick={runCode}
                    disabled={loading}
                    className="flex items-center gap-2 rounded-md bg-indigo-700 px-5 py-2 font-semibold text-white shadow-md transition-all hover:bg-indigo-500 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                    {loading && (
                        <div className="border-2 border-t-transparent h-4 w-4 animate-spin rounded-full border-white"></div>
                    )}
                    {loading ? "Uruchamianie..." : "Uruchom kod"}
                </button>

                <button
                    onClick={() => setCode(sampleCode[language])}
                    className="rounded-md bg-gray-700 px-5 py-2 font-semibold text-white shadow-md transition-all ease-in hover:bg-gray-600 border border-gray-600"
                >
                    🔄 Resetuj
                </button>
            </div>

            {(output || errors) && (
                <Card className="mb-6 border border-gray-800 bg-gray-900/50 shadow-lg">
                    <CardContent className="space-y-4 p-6">
                        {output && (
                            <div>
                                <h2 className="mb-2 text-lg font-semibold text-green-400">Output:</h2>
                                <pre className="rounded-xl border border-green-900/50 bg-black/80 p-4 whitespace-pre-wrap text-green-300">
                                    {output}
                                </pre>
                            </div>
                        )}
                        {errors && (
                            <div>
                                <h2 className="mb-2 text-lg font-semibold text-red-400">Errors:</h2>
                                <pre className="rounded-xl border border-red-900/50 bg-black/80 p-4 whitespace-pre-wrap text-red-400">
                                    {errors}
                                </pre>
                            </div>
                        )}
                    </CardContent>
                </Card>
            )}

            
            
        </div>
    );
}
