/* eslint-disable @typescript-eslint/no-explicit-any */

import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";

const schema = yup.object().shape({
    email: yup.string().email().required(),
    password: yup.string().min(6).required(),
    firstName: yup.string().required(),
    nickname: yup.string().required(),
});

interface Step1FormProps {
    defaultValues: any;
    onNext: (data: any) => void;
}

function Step1Form({ defaultValues, onNext }: Step1FormProps) {
    const { register, handleSubmit, formState: { errors } } = useForm({
        resolver: yupResolver(schema),
        defaultValues
    });

    const submitHandler = (data: any) => {
        onNext(data);
    };

    return (
        <form onSubmit={handleSubmit(submitHandler)} className=" text-white" >
            <h2 className="mb-4 text-center text-xl font-semibold text-white">Formularz rejestracji</h2>

            <label className="mb-2 block">
                Email:
                <input
                    type="email"
                    {...register("email")}
                    className={`text-black bg-white w-full rounded border border-amber-300 p-2 pr-10 focus:outline-offset-2 focus:outline-amber-600 ${errors.email ? "border-red-600" : "border-gray-300"}`}
                    placeholder="Wpisz swój email"
                />
                

            </label>

            <label className="mb-2 block">
                Hasło:
                <input
                    type="password"
                    {...register("password")}
                    className={`text-black bg-white w-full rounded border border-amber-300 p-2 pr-10 focus:outline-offset-2 focus:outline-amber-600 ${errors.password ? "border-red-600" : "border-gray-300"}`}
                    placeholder="Wpisz hasło"
                />
               
            </label>

            <label className="mb-2 block">
                Imię:
                <input
                    type="text"
                    {...register("firstName")}
                    className={`text-black bg-white w-full rounded border border-amber-300 p-2 pr-10 focus:outline-offset-2 focus:outline-amber-600 ${errors.firstName ? "border-red-600" : "border-gray-300"}`}
                    placeholder="Twoje imię"
                />
             
            </label>

            <label className="mb-4 block">
                Nick:
                <input
                    type="text"
                    {...register("nickname")}
                    className={`text-black bg-white w-full rounded border border-amber-300 p-2 pr-10 focus:outline-offset-2 focus:outline-amber-600 ${errors.nickname ? "border-red-600" : "border-gray-300"}`}
                    placeholder="Twój nick"
                />
               
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

export default Step1Form;
