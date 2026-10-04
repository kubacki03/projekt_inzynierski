'use client';

import  { useRef, useEffect, useState } from 'react';
import { motion } from 'motion/react';

export const TextHoverEffect = ({
    text,
    duration,
}: {
    text: string;
    duration?: number;
}) => {
    const svgRef = useRef<SVGSVGElement>(null);
    const textRef = useRef<SVGTextElement>(null);
    const [cursor, setCursor] = useState({ x: 0, y: 0 });
    const [hovered, setHovered] = useState(false);
    const [maskPosition, setMaskPosition] = useState({ cx: '50%', cy: '50%' });
    const [fontSize, setFontSize] = useState(50); 


    useEffect(() => {
        if (svgRef.current) {
            const svgRect = svgRef.current.getBoundingClientRect();
            const cxPercentage = ((cursor.x - svgRect.left) / svgRect.width) * 100;
            const cyPercentage = ((cursor.y - svgRect.top) / svgRect.height) * 100;
            setMaskPosition({
                cx: `${cxPercentage}%`,
                cy: `${cyPercentage}%`,
            });
        }
    }, [cursor]);

 
    useEffect(() => {
        if (!svgRef.current || !textRef.current) return;

        const svgWidth = svgRef.current.viewBox.baseVal.width;
        const padding = 20;

        let newFontSize = fontSize;
        const bbox = textRef.current.getBBox();

        if (bbox.width > svgWidth - padding) {
            newFontSize = ((svgWidth - padding) / bbox.width) * fontSize;
            setFontSize(newFontSize);
        }
    }, [text, fontSize]);

    return (
        <svg
            ref={svgRef}
            width="100%"
            height="30%"
            viewBox="0 0 900 100"
            xmlns="http://www.w3.org/2000/svg"
            onMouseEnter={() => setHovered(true)}
            onMouseLeave={() => setHovered(false)}
            onMouseMove={(e) => setCursor({ x: e.clientX, y: e.clientY })}
            className="select-none"
        >
            <defs>
                <linearGradient id="textGradient">
                    {hovered && (
                        <>
                            <stop offset="0%" stopColor="#eab308" />
                            <stop offset="25%" stopColor="#ef4444" />
                            <stop offset="50%" stopColor="#3b82f6" />
                            <stop offset="75%" stopColor="#06b6d4" />
                            <stop offset="100%" stopColor="#8b5cf6" />
                        </>
                    )}
                </linearGradient>

                <motion.radialGradient
                    id="revealMask"
                    gradientUnits="userSpaceOnUse"
                    r="20%"
                    initial={{ cx: '50%', cy: '50%' }}
                    animate={maskPosition}
                    transition={{ duration: duration ?? 0, ease: 'easeOut' }}
                >
                    <stop offset="0%" stopColor="white" />
                    <stop offset="100%" stopColor="black" />
                </motion.radialGradient>

                <mask id="textMask">
                    <rect
                        x="0"
                        y="0"
                        width="100%"
                        height="100%"
                        fill="url(#revealMask)"
                    />
                </mask>
            </defs>

            <motion.text
                ref={textRef}
                x="50%"
                y="50%"
                textAnchor="middle"
                dominantBaseline="middle"
                fontFamily="helvetica"
                fontWeight="bold"
                fill="transparent"
                stroke="#9CA3AF"
                strokeWidth="1"
                style={{ fontSize }}
                initial={{ strokeDasharray: 1000, strokeDashoffset: 1000 }}
                animate={{ strokeDasharray: 1000, strokeDashoffset: 0 }}
                transition={{ duration: 2, ease: 'easeInOut' }}
            >
                {text}
            </motion.text>

            <text
                x="50%"
                y="50%"
                textAnchor="middle"
                dominantBaseline="middle"
                fontFamily="helvetica"
                fontWeight="bold"
                fill="transparent"
                stroke="url(#textGradient)"
                strokeWidth="0.3"
                mask="url(#textMask)"
                style={{ fontSize }}
            >
                {text}
            </text>
        </svg>
    );
};
