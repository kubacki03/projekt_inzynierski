/* eslint-disable @typescript-eslint/no-explicit-any */

import  { useEffect, useState } from "react";
import { motion } from "framer-motion";
import RotatingText from "../components/RotatingText";
import { toast, ToastContainer } from "react-toastify";
import { Link } from "react-router-dom";


type Course = {

    image: string;
    title: string;
    level: string;
    description: string;
};

export default function CodeOdysseyHome() {
    const [course, setCourse] = useState<Course>();
    const [courses, setCourses] = useState<Course[]>([]);
   
    const API_URL = import.meta.env.VITE_API_URL;
    useEffect(() => {
        const fetchCourses = async () => {
            try {
                const response = await fetch(`${API_URL}/FeaturedCourses/GetFeaturedCourse`);
                if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
                const data = await response.json();
              
                setCourse(data);
            } catch (err: any) {
                console.log(err);
                toast.error("Wystąpił błąd pobierania danych");
            } 


            try {
                const response = await fetch(`${API_URL}/FeaturedCourses/Get`);
                if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
                const data = await response.json();
                
                setCourses(data);
            } catch (err: any) {
                console.log(err);
                toast.error("Wystąpił błąd pobierania danych");
            } 
        };

        fetchCourses();
    }, []);

    return (
        <div className=" min-h-screen via-sky-50 to-sky-100 text-slate-900">

            <ToastContainer position="top-right" autoClose={3000} />
            <main className="mx-auto max-w-7xl px-6">
                <section className="grid grid-cols-1 items-center gap-8 py-12 lg:grid-cols-2">
                    <div>
                        <motion.h2
                            initial={{ opacity: 0, y: 10 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ duration: 0.5 }}
                            className="text-4xl leading-tight font-extrabold text-indigo-400 sm:text-5xl"
                        >
                            Wyrusz w podróż przez świat kodu z{' '}
                            <span className="inline-flex items-center">
                                <RotatingText
                                    texts={[' Code Odyssey', ' Grywalizacją', 'Asystentem AI']}
                                    mainClassName="py-0.5 justify-center overflow-hidden rounded-lg bg-indigo-800 px-2 sm:px-2 sm:py-1 md:px-3 md:py-2"
                                    staggerFrom="last"
                                    initial={{ y: '100%' }}
                                    animate={{ y: 0 }}
                                    exit={{ y: '-120%' }}
                                    staggerDuration={0.025}
                                    splitLevelClassName="pb-0.5 overflow-hidden sm:pb-1 md:pb-1"
                                    transition={{ type: 'spring', damping: 30, stiffness: 400 }}
                                    rotationInterval={2500}
                                />
                            </span>
                        </motion.h2>


                        <p className="mt-4 max-w-prose text-slate-300">
                            Interaktywne ścieżki nauki, projekty od zera do wdrożenia i mentorskie wsparcie - wszystko
                            przygotowane dla osób zaczynających i rozwijających karierę w programowaniu.
                        </p>

                        <div className="mt-6 flex flex-col gap-3 sm:flex-row">
                            <a href="/login" className="inline-flex items-center justify-center gap-2 rounded-md bg-indigo-600 px-5 py-3 font-semibold text-white shadow-md">
                                Rozpocznij za darmo
                                <svg xmlns="http://www.w3.org/2000/svg" className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M14 5l7 7m0 0l-7 7m7-7H3" />
                                </svg>
                            </a>

                            <a href="#courses" className="inline-flex items-center justify-center gap-2 rounded-md border border-slate-200 bg-white px-5 py-3 text-indigo-600">
                                Przegląd kursów
                            </a>
                        </div>

                        <div className="mt-6 flex flex-wrap gap-4 text-sm text-slate-300">
                            <div className="flex items-center gap-2">
                                <span className="inline-block rounded-md bg-indigo-50 px-2 py-1 font-medium text-indigo-700">W 100% za darmo</span>
                                <span>• 100+ lekcji</span>
                            </div>
                            <div className="flex items-center gap-2">
                                <span className="inline-block rounded-md bg-cyan-50 px-2 py-1 font-medium text-indigo-700">Projekty</span>
                                <span>• Portfolio-ready</span>
                            </div>
                        </div>
                    </div>

                    <motion.div
                        initial={{ opacity: 0, scale: 0.98 }}
                        animate={{ opacity: 1, scale: 1 }}
                        transition={{ duration: 0.6 }}
                        className="relative mx-auto w-full max-w-lg"
                    >
                        <div className="rounded-2xl bg-white p-6 shadow-xl">
                            <div className="flex items-center justify-between">
                                <div>
                                    <div className="text-xs text-slate-400">Kurs</div>
                                    <h3 className="font-semibold">{course?.title}</h3>
                                </div>
                                <div className="text-sm text-slate-500">Poziom: {course?.level}</div>
                            </div>

                            <div className="mt-4 grid grid-cols-2 gap-4">
                                <div className="space-y-1">
                                    <div className="text-xs text-slate-400">Lekcji</div>
                                    <div className="font-medium">15</div>
                                </div>
                                <div className="space-y-1">
                                    <div className="text-xs text-slate-400">Czas</div>
                                    <div className="font-medium">8 godz.</div>
                                </div>
                            </div>

                            <div className="mt-4">
                                <div className="text-xs text-slate-400">Opis</div>
                                <p className="mt-1 text-sm text-slate-600">{course?.description}</p>
                            </div>

                            <div className="mt-4 flex items-center justify-between">
                                
                                <div className="text-sm text-slate-500">Średnia ocena: 4.8/5</div>
                            </div>
                        </div>

                     
                    </motion.div>
                </section>


                <section id="features" className="py-12">
                    <h3 className="text-2xl font-bold text-indigo-400">Dlaczego CodeOdyssey?</h3>
                    <p className="mt-2 max-w-prose text-slate-300">Zaprojektowane kursy, prawdziwe projekty, i wsparcie mentora AI.</p>

                    <div className="mt-6 grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
                        {[
                            { title: "Ścieżki kariery", desc: "Frontend, Backend, Data Science, DevOps" },
                            { title: "Projekty", desc: "Praktyczne zadania" },
                            { title: "Mentoring", desc: "Feedback od eksperta AI" },
                            { title: "Certyfikat", desc: "Potwierdzenie umiejętności" },
                        ].map((f) => (
                            <div key={f.title} className="rounded-xl bg-white p-4 shadow-sm">
                                <div className="font-semibold text-indigo-600">{f.title}</div>
                                <div className="mt-2 text-sm text-slate-600">{f.desc}</div>
                            </div>
                        ))}
                    </div>
                </section>

                <section id="courses" className="py-12">
                    <div className="flex items-center justify-between">
                        <h3 className="text-2xl font-bold text-indigo-400">Popularne kursy</h3>
                 
                    </div>

                    <div className="mt-6 grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
                        {courses.map((c) => (
                            <article key={c.title} className="rounded-2xl bg-white p-5 shadow transition-shadow hover:shadow-md">
                                <div className="flex items-start gap-4">
                                    <div className="flex h-14 w-14 items-center justify-center rounded-lg">< img src={c.image} width="50px" /></div>
                                    <div className="flex-1">
                                        <h4 className="font-semibold">{c.title}</h4>
                                        <p className="mt-1 text-xs text-slate-500">{c.description} - {c.level} </p>
                                    </div>
                                </div>

                                
                            </article>
                        ))}
                    </div>
                </section>

          
                <section className="py-12">
                    <h3 className="text-2xl font-bold text-indigo-400">Opinie kursantów</h3>
                    <div className="mt-6 grid grid-cols-1 gap-4 md:grid-cols-3">
                        {[
                            { name: "Marta", text: "Dzięki CodeOdyssey dostałam pracę jako junior frontend!" },
                            { name: "Kamil", text: "Kursy są praktyczne, a wsparcie AI bardzo pomocne." },
                            { name: "Anna", text: "Świetne projekty do portfolio." },
                        ].map((t) => (
                            <blockquote key={t.name} className="rounded-xl bg-white p-4 shadow">
                                <p className="text-slate-700">"{t.text}"</p>
                                <footer className="mt-3 text-sm text-slate-500">- {t.name}</footer>
                            </blockquote>
                        ))}
                    </div>
                </section>

                <section id="pricing" className="py-12">
                    <div className="flex flex-col items-center justify-between gap-6 rounded-2xl bg-indigo-600 p-8 text-white md:flex-row">
                        <div>
                            <h3 className="text-2xl font-bold">Gotowy rozpocząć naukę?</h3>
                            <p className="mt-2 max-w-prose text-indigo-100">Dołącz teraz i zyskaj dostęp do darmowych lekcji oraz projektów.</p>
                        </div>
                        <div className="flex gap-3">
                            <Link
                                to="/login"
                                className="rounded-md bg-white px-5 py-3 font-semibold text-indigo-600"
                            >
                                Zarejestruj się
                            </Link>
                            <a href="#pricing-details" className="rounded-md border border-white/40 px-5 py-3 text-white">Zobacz plany</a>
                        </div>
                    </div>
                </section>

              
               
            </main>
        </div>
    );
}
