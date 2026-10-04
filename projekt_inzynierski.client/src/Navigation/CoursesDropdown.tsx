/* eslint-disable @typescript-eslint/no-empty-object-type */
import React, { useState } from "react";

interface CoursesDropdownProps { }

const CoursesDropdown: React.FC<CoursesDropdownProps> = React.memo(() => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <li
            className="relative"
            onMouseEnter={() => setIsOpen(true)}
            onMouseLeave={() => setIsOpen(false)}
        >
            <button className="border-x border-teal-700 px-2">Kursy</button>
            {isOpen && (
                <ul className="absolute top-full left-0 w-40 rounded border bg-white shadow-lg">
                    <li><a href="/myCourses" className="block px-4 py-2 hover:bg-gray-100">Moje kursy</a></li>
                    <li><a href="/popularCourses" className="block px-4 py-2 hover:bg-gray-100">Popularne</a></li>
                    <li><a href="/latestCourses" className="block px-4 py-2 hover:bg-gray-100">Wszystkie</a></li>
                    <li><a href="/adaptive" className="block px-4 py-2 hover:bg-gray-100">Adaptacyjne</a></li>
                </ul>
            )}
        </li>
    );
});

export default CoursesDropdown;
