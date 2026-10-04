
/* eslint-disable @typescript-eslint/no-explicit-any */

import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";

const schema = yup.object().shape({
  birthDate: yup.date().required("Data urodzenia jest wymagana"),
  educationLevel: yup.string().required("Poziom wykształcenia jest wymagany"),
  experience: yup.string().required("Doświadczenie jest wymagane"),
  gender: yup.string().required("Płeć jest wymagana")
});

interface Step2FormProps {
  defaultValues: any;
  onSubmit: (data: any) => void;
}

function Step2Form({ defaultValues, onSubmit }: Step2FormProps) {
  const { register, handleSubmit } = useForm({
    resolver: yupResolver(schema),
    defaultValues
  });

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="flex flex-col items-center text-white">
          <div className="mb-4 w-full px-2">
                <label className="flex items-center space-x-2 text-white">
                    <span className="inline-block w-40">Data urodzenia:</span>
                    <input type="date" {...register("birthDate")} className="flex-grow" />
                </label>

            </div>
            
          <div className="mb-4 w-full px-2">
                <label className="flex items-center space-x-2">
                    <span className="inline-block w-40">Poziom wykształcenia:</span>
        <select {...register("educationLevel")} >
                        <option className="text-black" value="">Wybierz...</option>
                        <option className="text-black" value="podstawowe">Podstawowe</option>
                        <option className="text-black" value="średnie">Średnie</option>
                        <option className="text-black" value="wyższe">Wyższe</option>
        </select>
      
      </label></div>
          <div className="mb-4 w-full px-2">
                <label className="flex items-center space-x-2">
                    <span className="inline-block w-40">Doświadczenie programistyczne:</span>
        <select {...register("experience")}>
                        <option className="text-black" value="">Wybierz...</option>
                        <option className="text-black" value="brak">Brak</option>
                        <option className="text-black" value="podstawowe">Podstawowe</option>
                        <option className="text-black" value="średnie">Średnie</option>
                        <option className="text-black" value="zaawansowane">Zaawansowane</option>
        </select>
      
              </label>
          </div>
          <div className="mb-4 w-full px-2">
                <label className="flex items-center space-x-2">
                    <span className="inline-block w-40">Płeć:</span>
        <select {...register("gender")}>
                        <option className="text-black" value="">Wybierz...</option>
                        <option className="text-black" value="kobieta">Kobieta</option>
                        <option className="text-black" value="mężczyzna">Mężczyzna</option>
                        <option className="text-black" value="inna">Inna</option>
                        <option className="text-black" value="nie chcę mówić">Helikopter bojowy</option>
        </select>
     
              </label>
          </div>
          <button className="w-full max-w-xs cursor-pointer rounded bg-blue-600 py-2 text-white transition hover:bg-blue-700" type="submit">Zakończ rejestrację</button>
    </form>
  );
}

export default Step2Form;
