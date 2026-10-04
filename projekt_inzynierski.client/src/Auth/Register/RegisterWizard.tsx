/* eslint-disable @typescript-eslint/no-explicit-any */

import  { useState } from "react";
import Step1Form from "./Step1Form";
import Step2Form from "./Step2Form";
import axios from "axios";
import { useNavigate } from "react-router-dom";

function RegisterWizard() {
    const [step, setStep] = useState(1);
    const API_URL = import.meta.env.VITE_API_URL;
    const [formData, setFormData] = useState(() => {
        
        const saved = sessionStorage.getItem("registerData");
        return saved ? JSON.parse(saved) : {};
    });
    const navigate = useNavigate(); 
    const saveStep1Data = (data: any) => {

        const newData = { ...formData, ...data };
        setFormData(newData);
        sessionStorage.setItem("registerData", JSON.stringify(newData));
        setStep(2);
    };
    const [serverError, setServerError] = useState<string | undefined>(undefined);
    const saveStep2Data = async (data: any) => {
        const newData = { ...formData, ...data };
        setFormData(newData);
        sessionStorage.setItem("registerData", JSON.stringify(newData));
        try {
            await axios.post(`${API_URL}/User/Register`, newData, { withCredentials: true });
            sessionStorage.removeItem("registerData");
            alert("Rejestracja zakończona!");
            navigate("/dashboard");
        } catch (error) {
            if (axios.isAxiosError(error)) {
                const body = error.response?.data;
                setServerError((typeof body === "string" && body) || body?.message || "Nie udało się zarejestrować");
            } else {
                setServerError("Nie udało się połączyć z serwerem");
            }
        }
    };

    return (
        <div className="flex w-full max-w-md flex-col items-center rounded-lg  bg-gray-800 p-4 shadow-xl sm:w-[80%] md:w-[60%] lg:w-[33%]">
            {step === 1 && <Step1Form defaultValues={formData} onNext={saveStep1Data} />}
            {step === 2 && <Step2Form defaultValues={formData} onSubmit={saveStep2Data} />}
            {serverError && (<p>{serverError}</p>)}
        </div>
    );
}

export default RegisterWizard;
