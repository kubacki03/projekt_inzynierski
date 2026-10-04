import  { useState } from 'react';
import Shuffle from '../Challenges/Shuffle';

const FaqComponent = () => {
    const [activeIndex, setActiveIndex] = useState<number | null>(null);

    const toggleAccordion = (index: number) => {
        setActiveIndex(activeIndex === index ? null : index);
    };

    const faqItems = [
        {
            question: "Czym jest ta platforma?",
            answer: "To interaktywna platforma do nauki programowania, która łączy w sobie elementy grywalizacji, personalizację ścieżek nauki oraz wsparcie sztucznej inteligencji. Platforma umożliwia naukę różnych języków programowania poprzez praktyczne zadania, wyzwania i interakcję z wirtualnym asystentem."
        },
        {
            question: "Jakie języki programowania są dostępne?",
            answer: "Platforma oferuje naukę popularnych języków programowania takich jak Python, Java, C++ oraz JavaScript. Każdy język ma podzielone materiały na poziomy trudności: podstawowy, średniozaawansowany i zaawansowany."
        },
        {
            question: "Jak działa personalizacja nauki?",
            answer: "System analizuje Twoje postępy i umiejętności, a następnie dostosowuje ścieżkę nauki do Twoich potrzeb. Im więcej rozwiązujesz zadań, tym lepiej platforma rozpoznaje Twoje mocne i słabe strony, proponując optymalne materiały do dalszej nauki."
        },
        {
            question: "Jak działa automatyczne generowanie testów?",
            answer: "Dzięki integracji z modelami sztucznej inteligencji, platforma automatycznie generuje testy i zadania praktyczne na podstawie dostępnych materiałów. AI analizuje Twoje rozwiązania, dostosowuje poziom trudności i tworzy spersonalizowane zadania, które najlepiej pomogą Ci rozwijać umiejętności."
        },
        {
            question: "Jak działa wirtualny asystent?",
            answer: "Asystent oparty na dużych modelach językowych (LLM) pomaga w czasie rzeczywistym podczas rozwiązywania zadań. Potrafi:"
                + "\n• Wskazywać błędy w kodzie"
                + "\n• Wyjaśniać koncepcje programistyczne"
                + "\n• Sugerować optymalizacje kodu"
                + "\n• Odpowiadać na pytania dotyczące programowania"
                + "\n• Proponować alternatywne rozwiązania problemów"
        },
        {
            question: "Jakie elementy grywalizacji oferuje platforma?",
            answer: "System motywacyjny obejmuje:"
                + "\n• Zdobywanie odznak i osiągnięć za ukończone zadania"
                + "\n• System poziomów zaawansowania"
                + "\n• Rankingi użytkowników"
                + "\n• Wirtualne nagrody i punkty doświadczenia"
                + "\n• Codzienne wyzwania z dodatkowymi nagrodami"
                + "\n• Wizualizacja postępów w formie wykresów i statystyk"
        },
        {
            question: "Czym są tygodniowe wyzwania?",
            answer: "To specjalne zadania o różnym poziomie trudności, które zmieniają się każdego tygodnia. Ukończenie wyzwania daje bonusowe punkty, odznaki i pozycję w rankingu. Wyzwania mają na celu zachęcić do regularnej nauki i eksperymentowania z nowymi koncepcjami programistycznymi."
        },
        {
            question: "Jak platforma generuje materiały teoretyczne?",
            answer: "Oprócz tradycyjnych materiałów przygotowanych przez ekspertów, platforma wykorzystuje AI do:"
                + "\n• Automatycznego generowania wyjaśnień koncepcji programistycznych"
                + "\n• Tworzenia przykładów kodu dostosowanych do poziomu użytkownika"
                + "\n• Uzupełniania luk w materiałach na podstawie pytań użytkowników"
                + "\n• Tłumaczenia skomplikowanych tematów na prostszy język"
        },
        {
            question: "Czy platforma jest bezpłatna?",
            answer: "Podstawowe funkcje platformy są dostępne bezpłatnie. Istnieje również opcja premium, która oferuje dodatkowe funkcje takie jak:"
                + "\n• Rozszerzone materiały szkoleniowe"
                + "\n• Zaawansowane statystyki postępów"
                + "\n• Dostęp do ekskluzywnych wyzwań i konkursów"
        },
        {
            question: "Informacje o pracy dyplomowej",
            answer: "Platforma powstała jako praca inżynierska na Wydziale Informatyki Politechniki Białostockiej."
                + "\n\nTemat: Interaktywna platforma do nauki programowania z elementami grywalizacji i wsparciem sztucznej inteligencji"
                + "\nTemat (ENG): An Interactive Programming Learning Platform with Gamification Elements and AI Support"
                + "\nAutor: Jakub Dąbrowski"
                + "\n\nGłówne cele pracy:"
                + "\n• Stworzenie kompleksowej platformy edukacyjnej do nauki programowania"
                + "\n• Integracja mechanizmów grywalizacji zwiększających zaangażowanie użytkowników"
                + "\n• Implementacja wirtualnego asystenta opartego na modelach LLM"
                + "\n• Opracowanie systemu automatycznego generowania testów i materiałów edukacyjnych"
                + "\n• Badanie skuteczności połączenia gamifikacji i AI w edukacji programistycznej"
                + "\n\nTechnologie: React, TypeScript, Asp.NET Core, OpenAI API, Sql Server"
        }
    ];

    return (
        <div className="mx-auto max-w-4xl p-6">
            <div className="text-w mb-12 text-center text-white">
                <Shuffle text={'Często Zadawane Pytania'} />
                
                <p className="text-gray-200">
                    Poznaj szczegóły dotyczące platformy do nauki programowania i pracy dyplomowej
                </p>
            </div>

            <div className="overflow-hidden rounded-xl bg-white shadow-lg">
                {faqItems.map((item, index) => (
                    <div
                        key={index}
                        className={`border-b ${index === faqItems.length - 1 ? 'border-b-0' : 'border-gray-200'}`}
                    >
                        <button
                            className="flex w-full items-center justify-between p-6 text-left transition-colors hover:bg-gray-50"
                            onClick={() => toggleAccordion(index)}
                        >
                            <span className="text-lg font-medium text-gray-800">{item.question}</span>
                            <svg
                                className={`w-5 h-5 transition-transform ${activeIndex === index ? 'transform rotate-180' : ''}`}
                                fill="none"
                                stroke="currentColor"
                                viewBox="0 0 24 24"
                            >
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 9l-7 7-7-7"></path>
                            </svg>
                        </button>

                        {activeIndex === index && (
                            <div className="bg-gray-50 px-6 pt-2 pb-6">
                                <p className="whitespace-pre-line text-gray-700">{item.answer}</p>
                            </div>
                        )}
                    </div>
                ))}
            </div>
            <div className="mt-12">
                <iframe
                    width="100%" 
                    height="350"
                    src="https://www.youtube.com/embed/oWyKugnDe1Q?si=moosM45q8TebNPIg"
                    title="YouTube video player"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share; fullscreen;"
                    allowFullScreen 
                >
                </iframe>
            </div>
            <div className="mt-10 rounded-xl border border-blue-100 bg-blue-50 p-6">
                <h2 className="mb-3 text-xl font-bold text-gray-800">O pracy dyplomowej</h2>
                <p className="mb-4 text-gray-700">
                    Platforma powstała jako praca dyplomowa na <span className="font-semibold">Wydziale Informatyki Politechniki Białostockiej</span>.
                    Celem pracy było stworzenie innowacyjnego środowiska do nauki programowania, łączącego najnowsze osiągnięcia
                    w dziedzinie sztucznej inteligencji z mechaniką gier, aby zwiększyć zaangażowanie i skuteczność nauki.
                </p>
                <div className="mt-4 flex items-center">
                    <div className=" flex h-19 w-19 items-center justify-center rounded-xl border-gray-300 bg-gray-100">
                        <img
                            src="src/assets/wipb.jpg"
                            alt="Wydział Informatyki PB"
                            className="h-19 w-19 object-contain"
                        />
                    </div>
                    <div className="ml-4">
                        <p className="font-medium">Wydział Informatyki</p>
                        <p className="text-gray-600">Politechnika Białostocka</p>
                    </div>
            </div>
            </div>
        </div>
    );
};

export default FaqComponent;