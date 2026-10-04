/* eslint-disable @typescript-eslint/no-unused-vars */


import axios from "axios";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { useNavigate } from "react-router-dom";
import React from "react";
import { useAuth } from "./AuthContext";



const schema = yup.object({
    email: yup
        .string()
        .email('Niepoprawny email')
        .required('Email jest wymagany'),
    password: yup
        .string()
        .min(6, 'Hasło musi mieć co najmniej 6 znaków')
        .required('Hasło jest wymagane'),
}).required();


type FormData = {
    email: string;
    password: string;
};

const LoginFormComponent: React.FC = () => {
    const API_URL = import.meta.env.VITE_API_URL;
    const { login } = useAuth();

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<FormData>({
        resolver: yupResolver(schema),
    });
    const [serverError, setServerError] = useState<string | undefined>(undefined);
   
    const navigate = useNavigate(); 

    const onSubmit = async (data: FormData) => {
        try {
            const response = await axios.post(`${API_URL}/User/Login`, data, {
                withCredentials: true
            });

            login(response.data.role);
            if (response.data.role == 'admin') {
                navigate("/admin")
            } else {
                navigate("/dashboard");
            }
           
        } catch (error) {
            if (axios.isAxiosError(error)) {
                setServerError(error.response?.data?.message || "Błędne dane logowania");
            } else {
                setServerError("Nie udało się połączyć z serwerem");
            }
        }
    };
    const [showPassword, setShowPassword] = useState(false);


    return (
        <form onSubmit={handleSubmit(onSubmit)} className="flex w-full max-w-md flex-col items-center rounded-lg border border-gray-700 bg-gray-800 p-4 shadow-xl sm:w-[80%] md:w-[60%] lg:w-[33%]">
            <div className="mb-4 w-full px-2">
                <label className="mb-1 block text-xl font-medium text-white">Email</label>
                <input
                    type="email"
                    {...register('email')}
                    className="w-full rounded border border-indigo-400 bg-gray-100 p-2 focus:outline-offset-2 focus:outline-indigo-600"
                />
                {errors.email && (
                    <p className="text-sm text-red-600">{errors.email.message}</p>
                )}
            </div>

            <div className="mb-4 w-full px-2">
                <label className="mb-1 block text-xl font-medium text-white">Hasło</label>
                <div className="relative">
                    <input
                        type={showPassword ? 'text' : 'password'}
                        {...register('password')}
                        className="w-full rounded border border-indigo-400 bg-gray-100 p-2 pr-10 focus:outline-offset-2 focus:outline-indigo-600"
                    />
                    <button
                        type="button"
                        onClick={() => setShowPassword(!showPassword)}
                        className="absolute right-2 top-1/2 -translate-y-1/2 text-sm text-gray-400"
                    >
                        {showPassword ? 'Ukryj' : 'Pokaż'}
                    </button>
                </div>
                {errors.password && (
                    <p className="text-sm text-red-200">{errors.password.message}</p>
                )}
            </div>

            <button
                type="submit"
                className="w-full max-w-xs cursor-pointer rounded bg-indigo-600 py-2 text-white transition hover:bg-indigo-700"
            >
                Zaloguj się
            </button>
            {serverError && <p className="mt-2 text-sm text-red-200">{serverError}</p>}
        </form>

    );
};

export default LoginFormComponent;


