/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useState } from "react";
import axios from "axios";
import { motion } from "framer-motion";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { Select, SelectTrigger, SelectValue, SelectContent, SelectItem } from "@/components/ui/select";

type MaterialType = "theory" | "task" | "question";

interface TheoryMaterial {
    type: "theory";
    title: string;
    content: string;
    code?: string;
}

interface TaskMaterial {
    type: "task";
    description: string;
}

interface QuestionMaterial {
    type: "question";
    question: string;
    answers: string[];
    correctAnswer: number;
}

type CourseMaterial = TheoryMaterial | TaskMaterial | QuestionMaterial;
interface CourseMaterialCreatorProps {
    courseId: string;
}
const CourseMaterialCreator: React.FC<CourseMaterialCreatorProps> = ({ courseId }) => {
    const [materialType, setMaterialType] = useState<MaterialType>("theory");
    const [formData, setFormData] = useState<CourseMaterial>({
        type: "theory",
        title: "",
        content: "",
        code: "",
    } as TheoryMaterial);

    const handleChange = (field: string, value: any) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleSubmit = async () => {
        try {
            await axios.post(`${import.meta.env.VITE_API_URL}/api/admin/courses/${courseId}/materials`, formData);
            alert("Materiał został zapisany!");
        } catch (error) {
            console.error(error);
            alert("Błąd podczas zapisu materiału.");
        }
    };
    const questionData = formData as QuestionMaterial;
    const renderForm = () => {
        switch (materialType) {
            case "theory":
                return (
                    <>
                        <Input
                            placeholder="Tytuł"
                            value={(formData as TheoryMaterial).title}
                            onChange={(e) => handleChange("title", e.target.value)}
                        />
                        <Textarea
                            placeholder="Zawartość teoretyczna"
                            value={(formData as TheoryMaterial).content}
                            onChange={(e) => handleChange("content", e.target.value)}
                        />
                        <Textarea
                            placeholder="Przykładowy kod (opcjonalnie)"
                            value={(formData as TheoryMaterial).code}
                            onChange={(e) => handleChange("code", e.target.value)}
                        />
                    </>
                );

            case "task":
                return (
                    <Textarea
                        placeholder="Treść zadania praktycznego"
                        value={(formData as TaskMaterial).description}
                        onChange={(e) => handleChange("description", e.target.value)}
                    />
                );

            case "question":
               
                return (
                    <>
                        <Input
                            placeholder="Treść pytania"
                            value={questionData.question}
                            onChange={(e) => handleChange("question", e.target.value)}
                        />
                        {questionData.answers.map((ans, i) => (
                            <Input
                                key={i}
                                placeholder={`Odpowiedź ${i + 1}`}
                                value={ans}
                                onChange={(e) => {
                                    const newAnswers = [...questionData.answers];
                                    newAnswers[i] = e.target.value;
                                    handleChange("answers", newAnswers);
                                }}
                            />
                        ))}
                        <Select
                            onValueChange={(val) => handleChange("correctAnswer", Number(val))}
                            value={String(questionData.correctAnswer)}
                        >
                            <SelectTrigger>
                                <SelectValue placeholder="Poprawna odpowiedź" />
                            </SelectTrigger>
                            <SelectContent>
                                <SelectItem value="0">Odpowiedź 1</SelectItem>
                                <SelectItem value="1">Odpowiedź 2</SelectItem>
                                <SelectItem value="2">Odpowiedź 3</SelectItem>
                            </SelectContent>
                        </Select>
                    </>
                );
        }
    };

    const handleTypeChange = (value: MaterialType) => {
        setMaterialType(value);
        if (value === "theory")
            setFormData({ type: "theory", title: "", content: "", code: "" });
        else if (value === "task")
            setFormData({ type: "task", description: "" });
        else
            setFormData({ type: "question", question: "", answers: ["", "", ""], correctAnswer: 0 });
    };

    return (
        <motion.div
            className="mx-auto max-w-2xl bg-gray-950 p-6"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
        >
            <Card className="space-y-4 rounded-2xl p-6 shadow-lg">
                <h2 className="mb-4 text-center text-2xl font-semibold">
                    Tworzenie materiału kursowego
                </h2>

                <Select onValueChange={(v) => handleTypeChange(v as MaterialType)} value={materialType}>
                    <SelectTrigger>
                        <SelectValue placeholder="Wybierz typ materiału" />
                    </SelectTrigger>
                    <SelectContent className="bg-gray-800">
                        <SelectItem value="theory" className="hover:bg-gray-700">Materiał teoretyczny</SelectItem>
                        <SelectItem value="task" className="hover:bg-gray-700">Zadanie praktyczne</SelectItem>
                        <SelectItem value="question" className="hover:bg-gray-700">Pytanie zamknięte</SelectItem>
                    </SelectContent>
                </Select>

                <CardContent className="space-y-3">{renderForm()}</CardContent>

                <Button onClick={handleSubmit} className="w-full">
                    Zapisz materiał
                </Button>
            </Card>
        </motion.div>
    );
};

export default CourseMaterialCreator;
