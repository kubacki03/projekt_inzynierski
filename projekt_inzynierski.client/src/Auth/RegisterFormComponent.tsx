/* eslint-disable @typescript-eslint/no-explicit-any */

import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";

const schema = yup.object().shape({
    email: yup.string().email("Nieprawidłowy email").required("Email jest wymagany"),
    password: yup.string().min(6, "Hasło musi mieć co najmniej 6 znaków").required("Hasło jest wymagane"),
    firstName: yup.string().required("Imię jest wymagane"),
    nickname: yup.string().required("Nick jest wymagany"),
});

function RegisterFormComponent() {
    const { register, handleSubmit, formState: { errors } } = useForm({
        resolver: yupResolver(schema)
    });

    const onSubmit = (data: any) => {
       
        alert(`Zarejestrowano: ${JSON.stringify(data)}`);
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="flex w-full max-w-md flex-col items-center rounded-lg border border-amber-200 p-4 shadow-xl sm:w-[80%] md:w-[60%] lg:w-[33%]">
            <h2 className="mb-4 text-xl font-semibold">Formularz rejestracji</h2>

            <label className="mb-2 block">
                Email:
                <input
                    type="email"
                    {...register("email")}
                    className={`text-black !important w-full rounded border border-amber-300 p-2 pr-10 focus:outline-offset-2 focus:outline-amber-600 ${errors.email ? "border-red-600" : "border-gray-300"}`}
                    placeholder="Wpisz swoj email"
                />
                {errors.email && <p className="mt-1 text-sm text-red-600">{errors.email.message}</p>}
            </label>

            <label className="mb-2 block">
                Hasło:
                <input
                    type="password"
                    {...register("password")}
                    className={` w-full rounded border border-amber-300 p-2 pr-10 focus:outline-offset-2 focus:outline-amber-600 ${errors.password ? "border-red-600" : "border-gray-300"}`}
                    placeholder="Wpisz hasło"
                />
                {errors.password && <p className="mt-1 text-sm text-red-600">{errors.password.message}</p>}
            </label>

            <label className="mb-2 block">
                Imię:
                <input
                    type="text"
                    {...register("firstName")}
                    className={` w-full rounded border border-amber-300 p-2 pr-10 focus:outline-offset-2 focus:outline-amber-600 ${errors.firstName ? "border-red-600" : "border-gray-300"}`}
                    placeholder="Twoje imię"
                />
                {errors.firstName && <p className="mt-1 text-sm text-red-600">{errors.firstName.message}</p>}
            </label>

            <label className="mb-4 block">
                Nick:
                <input
                    type="text"
                    {...register("nickname")}
                    className={`text-black w-full rounded border border-amber-300 p-2 pr-10 focus:outline-offset-2 focus:outline-amber-600 ${errors.nickname ? "border-red-600" : "border-gray-300"}`}
                    placeholder="Twój nick"
                />
                {errors.nickname && <p className="mt-1 text-sm text-red-600">{errors.nickname.message}</p>}
            </label>

            <button
                type="submit"
                className="w-full max-w-xs cursor-pointer rounded bg-blue-600 py-2 text-white transition hover:bg-blue-700"
            >
                Dalej
            </button>
        </form>
    );
}

export default RegisterFormComponent;
