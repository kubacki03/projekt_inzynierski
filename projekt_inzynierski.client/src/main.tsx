import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App'

import { AuthProvider } from './Auth/AuthContext'
import Galaxy from './components/Galaxy'
import { useEffect, useState } from 'react'

function RootApp() {
    const [showGalaxy, setShowGalaxy] = useState(false)
    const [theme, setTheme] = useState<'auto' | 'galaxy' | 'simple'>('auto')

    useEffect(() => {
        const memory = (navigator as Navigator & { deviceMemory?: number }).deviceMemory || 4
        const storedTheme = localStorage.getItem('theme') as 'auto' | 'galaxy' | 'simple' | null

        if (storedTheme) {
            setTheme(storedTheme)
        }

        if (memory > 7) {
            setShowGalaxy(true)
        }
    }, [])

    const renderBackground = () => {
        if (theme === 'galaxy' || (theme === 'auto' && showGalaxy)) {
            return (
                <Galaxy
                    mouseRepulsion={true}
                    mouseInteraction={true}
                    density={0.5}
                    glowIntensity={0.2}
                    saturation={0.3}
                    hueShift={330}
                />
            )
        } else {
            return <div className="fixed inset-0 -z-10 bg-gray-950"></div>
        }
    }

    const changeTheme = (newTheme: 'auto' | 'galaxy' | 'simple') => {
        setTheme(newTheme)
        localStorage.setItem('theme', newTheme)
    }

    return (
        <main>
            <div className="relative min-h-screen overflow-hidden">
                <AuthProvider>
                    <div className="fixed inset-0 -z-10">{renderBackground()}</div>

                    <div className="relative z-10">
                        <App />
                    </div>
                </AuthProvider>
            </div>

            <footer id="contact" className="relative z-10 bg-gray-950 py-10 text-sm text-slate-500">
                <div className="border-t border-slate-200 pt-6">
                    <div className="mx-auto flex max-w-7xl flex-col items-center justify-between gap-4 px-6 md:flex-row">
                        <div>© {new Date().getFullYear()} CodeOdyssey — Wszystkie prawa zastrzeżone</div>
                        <div className="flex items-center gap-4">
                            <a href="#" className="hover:underline">Regulamin</a>
                            <a href="#" className="hover:underline">Polityka prywatności</a>
                            <a href="#" className="hover:underline">Kontakt</a>

                            <div className="ml-4 flex gap-2">
                                <button
                                    className="rounded bg-gray-700 px-2 py-1 text-white hover:bg-gray-600"
                                    onClick={() => changeTheme('auto')}
                                >
                                    Auto
                                </button>
                                <button
                                    className="rounded bg-gray-700 px-2 py-1 text-white hover:bg-gray-600"
                                    onClick={() => changeTheme('galaxy')}
                                >
                                    Galaxy
                                </button>
                                <button
                                    className="rounded bg-gray-700 px-2 py-1 text-white hover:bg-gray-600"
                                    onClick={() => changeTheme('simple')}
                                >
                                    Proste
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </footer>
        </main>
    )
}

createRoot(document.getElementById('root')!).render(<RootApp />)
