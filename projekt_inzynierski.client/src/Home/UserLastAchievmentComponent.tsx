/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import axios from "axios";

import { useEffect, useState } from "react";

function UserLastAchievmentComponent() {
    type UserAchievments = {
        name: string,
        description: string,
        date: Date
    }
    const [achievments, setAchievements] = useState<UserAchievments[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);
    const API_URL = import.meta.env.VITE_API_URL;
    useEffect(() => {
        const fetchAchievements = async () => {
            try {
                const response = await axios.get(`${API_URL}/UserAchievment/GetUserAchievments`,
                    {
                        withCredentials: true
                    }
                );
                setAchievements(response.data);
            } catch (error: any) {
                setError("Wystąpił błąd pobierania newsów");
            } finally {
                setLoading(false);
            }
        };
        fetchAchievements();
    }, []);
  return (
      <div className="flex flex-col items-center gap-3">
          {error && (<p>{error}</p>)}
          <h1 className="text-4xl">Osiągnięcia</h1>
          {achievments?.length == 0 && (<h1 className="text-2xl">Brak osiągnięć</h1>)}
          {loading && (
              <div className="flex items-center justify-center py-10">
                  <div className="border-4 border-t-transparent h-12 w-12 animate-spin rounded-full border-teal-400"></div>
              </div>
          )}
          {achievments.map((c, index) => (
              <div className="w-auto rounded-2xl bg-gray-800 p-4 text-wrap text-white transition-all duration-200 ease-in-out outline-none hover:border hover:border-indigo-400 hover:shadow-[0_0_20px_rgba(14,116,144,0.5),0_0_40px_rgba(14,116,144,0.3)]">
                  <h1 className="text-3xl" key={index}>{c.name}</h1>
                
              </div>
          ))}
      </div>
  );
}

export default UserLastAchievmentComponent;

