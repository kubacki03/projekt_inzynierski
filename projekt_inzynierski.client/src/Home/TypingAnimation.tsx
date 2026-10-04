import { useEffect, useState } from "react";

function TypingAnimation() {
    const texts = [
        "Programuj. Eksploruj. Odkrywaj.",
        "Powtórz.",
    ];

    const [text, setText] = useState("");
    const [index, setIndex] = useState(0);
    const [isDeleting, setIsDeleting] = useState(false);

    useEffect(() => {
        const current = texts[index];
        // eslint-disable-next-line prefer-const
        let typingSpeed = isDeleting ? 50 : 100;

        const type = () => {
            if (!isDeleting && text === current) {
               
                setTimeout(() => setIsDeleting(true), 1000);
            } else if (isDeleting && text === "") {
            
                setIsDeleting(false);
                setIndex((prev) => (prev + 1) % texts.length);
            } else {
                const updatedText = isDeleting
                    ? current.substring(0, text.length - 1)
                    : current.substring(0, text.length + 1);
                setText(updatedText);
            }
        };

        const timer = setTimeout(type, typingSpeed);
        return () => clearTimeout(timer);
    }, [text, isDeleting, index]);

    return (
        <h2 className="font-mono text-xl italic">
            {text}
            <span className="animate-pulse border-r-2 border-white"></span>
        </h2>
    );
}

export default TypingAnimation;
