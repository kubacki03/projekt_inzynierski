
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-explicit-any */
import React, { useEffect, useState } from "react";


export interface User {
  id: number;
  publicId: string;
  email: string;
  passwordHash: string;
  firstName: string;
  nickname: string;
  birthDate: string;
  educationLevel: string;
  experience: string;
  gender: string;
  points: number;
  goldenPoints: number;
    selectedAvatar: string;
    date: Date
}

export interface Badge {
  id: number;
  title: string;
  imagePath: string;
}

export interface Avatar {
  id: number;
  name: string;
  imageUrl: string;
  isActive: boolean;
}

const Card: React.FC<React.PropsWithChildren> = ({ children }) => (
  <div className="mx-auto mt-20 max-w-md rounded-2xl bg-white p-6 shadow">
    {children}
  </div>
);

const AvatarImg: React.FC<{ src?: string; alt?: string; onClick?: () => void }> = ({
  src,
  alt,
  onClick,
}) => (
  <div
    onClick={onClick}
    className="mb-4 flex h-24 w-24 cursor-pointer items-center justify-center overflow-hidden rounded-full bg-gray-200 hover:opacity-80"
  >
    {src ? (
      <img src={src} alt={alt} className="h-full w-full object-cover" />
    ) : (
      <span className="text-3xl">??</span>
    )}
  </div>
);

const UserProfile: React.FC = () => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [badges, setBadges] = useState<Badge[]>([]);
  const [badgesError, setBadgesError] = useState<string | null>(null);
    const API_URL = import.meta.env.VITE_API_URL;

  const [avatars, setAvatars] = useState<Avatar[]>([]);
  const [showAvatarModal, setShowAvatarModal] = useState(false);

  const fetchUser = async () => {
    try {
      const response = await fetch(`${API_URL}/User/Get`, {
        method: "GET",
        credentials: "include",
      });
      if (!response.ok) throw new Error(`HTTP error! status: ${ response.status } `);
      const data = await response.json();
      setUser(data);
    } catch (err: unknown) {
      if (err instanceof Error) setError(err.message);
      else setError(String(err));
    } finally {
      setLoading(false);
    }
  };

  const fetchBadges = async () => {
    try {
      const response = await fetch(`${API_URL}/UserChallenge/GetBadges`, {
        method: "GET",
        credentials: "include",
      });
      if (!response.ok) throw new Error(`HTTP error! status: ${ response.status } `);
      const data = await response.json();
      setBadges(data);
    } catch (err: unknown) {
      if (err instanceof Error) setBadgesError(err.message);
      else setBadgesError(String(err));
    }
  };

  const fetchAvatars = async () => {
    try {
      const res = await fetch(`${API_URL}/UserAvatar/GetUserAvatars`, {
        method: "GET",
        credentials: "include",
      });
      if (!res.ok) throw new Error("Błąd ładowania avatarów");
      const data = await res.json();
      setAvatars(data);
      setShowAvatarModal(true);
    } catch (err) {
      console.error(err);
    }
  };

  const changeAvatar = async (id: number) => {
    try {
        await fetch(`${ API_URL }/UserAvatar/ChangeAvatar?id=${id}`, {
method: "PUT",
    credentials: "include",
      });
setShowAvatarModal(false);
await fetchUser();
    } catch (err) {
    console.error(err);
}
  };

useEffect(() => {
    fetchUser();
    fetchBadges();
}, []);

if (loading) return <p>Ładowanie...</p>;
if (error) return <p>Błąd: {error}</p>;
if (!user) return null;
    const getRemainingTime = (endDate: string | Date) => {
        const now = new Date();
        const end = new Date(endDate);
        const diffMs = end.getTime() - now.getTime(); // różnica w ms

        if (diffMs <= 0) return "Premium wygasło";

        const diffMinutes = Math.floor(diffMs / 1000 / 60);
        const diffHours = Math.floor(diffMinutes / 60);
        const diffDays = Math.floor(diffHours / 24);

        if (diffDays >= 1) return `${diffDays} dni`;
        if (diffHours >= 1) return `${diffHours} godzin`;
        return `${diffMinutes} minut`;
    };

    return (
        <main >
          
    <Card>
        <div className="flex flex-col items-center justify-center">
            <AvatarImg
                src={`${API_URL}/${user.selectedAvatar}`}
                alt={user.nickname}
                onClick={fetchAvatars}
            />

            <h2 className="text-xl font-bold">
                {user.firstName} ({user.nickname})
            </h2>
                    <p className="text-gray-500">{user.email}</p>
                    <p className="flex items-center justify-center gap-2 text-lg font-semibold">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="h-5 w-5 text-yellow-400"
                            viewBox="0 0 20 20"
                            fill="currentColor"
                        >
                            <path d="M10 15l-5.878 3.09L5.4 11.545 1 7.91l6.062-.91L10 2l2.938 5l6.062.91l-4.4 3.636l1.278 6.545z" />
                        </svg>
                        Premium: {getRemainingTime(user.date)}
                    </p>


            <div className="mt-4 grid w-full grid-cols-2 gap-4 text-sm">
                <div className="flex flex-col items-center">
                    <span className="font-medium">
                        {new Date(user.birthDate).toLocaleDateString()}
                    </span>
                    <span className="text-gray-500">Data urodzenia</span>
                </div>
                <div className="flex flex-col items-center">
                    <span className="font-medium">{user.educationLevel}</span>
                    <span className="text-gray-500">Wykształcenie</span>
                </div>
            </div>

            <div className="mt-4 w-full text-left">
                <p>
                    <strong>Doświadczenie:</strong> {user.experience}
                </p>
                <p>
                    <strong>Płeć:</strong> {user.gender}
                        </p>
                       
            </div>

            <div className="mt-4 grid w-full grid-cols-2 gap-4">
                <div className="rounded-xl bg-gray-100 p-2 text-center">
                    <span className="block font-bold">{user.points}</span>
                    <span className="text-xs text-gray-500">Punkty</span>
                </div>
                <div className="rounded-xl bg-yellow-100 p-2 text-center">
                    <span className="block font-bold">{user.goldenPoints}</span>
                    <span className="text-xs text-yellow-700">Złote punkty</span>
                </div>
            </div>

            <div className="mt-6 w-full">
                <h3 className="mb-2 text-lg font-bold">Odznaki</h3>
                {badgesError && <p className="text-red-500">Błąd: {badgesError}</p>}
                {badges.length === 0 ? (
                    <p className="text-gray-500">Brak odznak</p>
                ) : (
                    <div className="grid grid-cols-3 gap-4">
                        {badges.map((badge) => (
                            <div key={badge.id} className="flex flex-col items-center">
                                <img
                                    src={`${API_URL}/Images/${badge.imagePath}`}
                                    alt={badge.title}
                                    className="h-16 w-16 rounded-full border object-cover"
                                />
                                <span className="mt-1 text-center text-xs">{badge.title}</span>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>

        {showAvatarModal && (
            <div className=" fixed inset-0 inset-0 flex items-center justify-center bg-black/50">
                <div className="rounded-xl bg-white p-6 shadow-lg">
                    <h3 className="mb-4 text-lg font-bold">Wybierz avatar</h3>
                    <div className="grid grid-cols-3 gap-4">
                        {avatars.map((a) => (
                            <img
                                key={a.id}
                                src={`${API_URL}/${a.imageUrl}`}
                                alt={a.name}
                                className="h-20 w-20 cursor-pointer rounded-full border hover:opacity-80"
                                onClick={() => changeAvatar(a.id)}
                            />
                        ))}
                    </div>
                    <button
                        className="mt-4 rounded bg-gray-300 px-4 py-2 hover:bg-gray-400"
                        onClick={() => setShowAvatarModal(false)}
                    >
                        Zamknij
                    </button>
                </div>
            </div>
        )}
            </Card>
        </main>
);
};

export default UserProfile;

