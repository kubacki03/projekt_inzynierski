import React, { useState } from "react";

type Fact = {
    image: string;
    text: string;
};

const FactCard: React.FC = () => {
    // Słownik ciekawostek bezpośrednio w komponencie
    const facts: Fact[] = [
        {
            image: "https://upload.wikimedia.org/wikipedia/commons/7/73/Pale_Blue_Dot.png",
            text: "Spójrz ponownie na tą kropkę. To Nasz dom. To my. Na niej wszyscy, których kochasz, których znasz. O których kiedykolwiek słyszałeś. Każdy człowiek, który kiedykolwiek istniał, przeżył tam swoje zycie. To suma naszych radości i smutków. To tysiące pewnych swego religii, ideoligii i doktryn ekonomicznych. To każdy myśliwy i zbieracz. Każdy bohater i tchórz. Każdy twórca i niszczyciel cywilizacji. Każdy król i chłop. Każda zakochana para. Każda matka, ojciec i każde pełne nadziei dziecko. Każdy wynalazca i odkrywca. Każdy moralista. Każdy skorumpowany polityk. Każdy wielki przywódca i wielka gwiazda. Każdy święty i każdy grzesznik w historii naszego gatunku, żył tam. ",
        },
        {
            image: "https://i.pinimg.com/736x/2f/e2/3e/2fe23ebd5b3cb467bb1a41821a538747.jpg",
            text: "Sowy mogą obracać głowę o 270 stopni.",
        },
        {
            image: "https://images.unsplash.com/photo-1551963831-b3b1ca40c98e",
            text: "Kawa to drugi najczęściej spożywany napój na świecie — zaraz po wodzie.",
        },
    ];

    const [currentIndex, setCurrentIndex] = useState(0);

    const handleNext = () => {
        const nextIndex = Math.floor(Math.random() * facts.length);
        setCurrentIndex(nextIndex);
    };

    const currentFact = facts[currentIndex];

    return (
        <div className=" flex max-w-2xl flex-col overflow-hidden rounded-2xl bg-white shadow-lg md:flex-row">
            {/* Obrazek */}
            <div className="w-full md:w-1/2">
                <img
                    src={currentFact.image}
                    alt="Ciekawostka"
                    className="h-full w-full object-cover"
                />
            </div>

            {/* Tekst */}
            <div className="flex w-full flex-col justify-between p-6 md:w-1/2">
                <p className="text-lg font-medium text-gray-800">{currentFact.text}</p>
                <button
                    onClick={handleNext}
                    className="mt-4 self-start rounded bg-blue-500 px-4 py-2 text-white transition hover:bg-blue-600"
                >
                    Następna ciekawostka
                </button>
            </div>
        </div>
    );
};

export default FactCard;
