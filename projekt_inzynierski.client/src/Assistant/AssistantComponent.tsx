/* eslint-disable @typescript-eslint/no-unused-vars */
import React, { useState, useRef, useEffect } from 'react';
import DecryptedText from '../components/DecryptedText';

interface Message {
    id: string;
    text: string;
    sender: 'user' | 'assistant';
    timestamp: Date;
    status?: 'sending' | 'delivered' | 'error';
}

interface ApiResponse {
    success: boolean;
    message: string;
    error?: string;
}

const AssistantComponent: React.FC = () => {
    const [messages, setMessages] = useState<Message[]>([]);
    const [inputValue, setInputValue] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const messagesEndRef = useRef<null | HTMLDivElement>(null);
    const API_URL = import.meta.env.VITE_API_URL;

    const sendMessageToApi = async (message: string): Promise<ApiResponse> => {
        const response = await fetch(`${API_URL}/AssistantChat/GetChatResponse`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ message })
        });
        return await response.json();
    };

    const handleSendMessage = async () => {
        if (!inputValue.trim()) return;

        const userMessage: Message = {
            id: Date.now().toString(),
            text: inputValue,
            sender: 'user',
            timestamp: new Date(),
            status: 'sending'
        };

        setMessages(prev => [...prev, userMessage]);
        setInputValue('');
        setIsLoading(true);

        try {
            const response = await sendMessageToApi(inputValue);

            setMessages(prev =>
                prev.map(msg =>
                    msg.id === userMessage.id ? { ...msg, status: 'delivered' } : msg
                )
            );

            if (response.success) {
                const assistantMessage: Message = {
                    id: Date.now().toString(),
                    text: '',
                    sender: 'assistant',
                    timestamp: new Date()
                };

                setMessages(prev => [...prev, assistantMessage]);

                typeWriterEffect(response.message, (partialText) => {
                    setMessages(prev =>
                        prev.map(msg =>
                            msg.id === assistantMessage.id ? { ...msg, text: partialText } : msg
                        )
                    );
                });
            } else {
                throw new Error(response.error || 'Błąd API');
            }
        } catch (error) {
            setMessages(prev =>
                prev.map(msg =>
                    msg.id === userMessage.id ? { ...msg, status: 'error' } : msg
                )
            );

            const errorMessage: Message = {
                id: Date.now().toString(),
                text: 'Przepraszam, wystąpił błąd. Spróbuj ponownie później.',
                sender: 'assistant',
                timestamp: new Date()
            };
            setMessages(prev => [...prev, errorMessage]);
        } finally {
            setIsLoading(false);
        }
    };

    const firstRender = useRef(true);
    useEffect(() => {
        if (firstRender.current) {
            firstRender.current = false;
            return;
        }
        messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
    }, [messages]);

    const handleKeyPress = (e: React.KeyboardEvent) => {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            handleSendMessage();
        }
    };

    const typeWriterEffect = (text: string, onUpdate: (partial: string) => void, speed = 30) => {
        let index = 0;
        const interval = setInterval(() => {
            onUpdate(text.slice(0, index));
            index++;
            if (index > text.length) clearInterval(interval);
        }, speed);
    };

    return (
        <div className="mx-auto mt-[2%] flex h-full max-w-2xl flex-col overflow-hidden rounded-lg bg-gray-700 shadow-lg">
      
            <div className="bg-gradient-to-r flex items-center gap-2 from-blue-600 to-indigo-600 p-4 text-white">
                <span className="text-2xl">🤖</span>
                <div>
                    <h2 className="text-lg font-semibold">Wirtualny Asystent</h2>
                    <p className="text-xs opacity-80">Jestem tutaj, aby Ci pomóc!</p>
                </div>
            </div>

            <div className="max-h-[60vh] flex-1 overflow-y-auto p-4">
                {messages.length === 0 ? (
                    <div className="flex h-full flex-col items-center justify-center text-gray-100">
                        <div className="border-2 flex h-26 w-26 items-center justify-center rounded-xl bg-white">
                            <img
                                src="src/assets/logos43.png"
                                alt="Wydział Informatyki PB"
                                className="h-22 w-22 object-contain"
                            />
                        </div>
                        <DecryptedText
                            text="Zadaj pytanie, a postaram się pomóc!"
                            animateOn="view"
                            revealDirection="center"
                            speed={50}
                            maxIterations={10}
                        />
                    </div>
                ) : (
                    messages.map((message) => (
                        <div
                            key={message.id}
                            className={`flex mb-4 ${message.sender === 'user' ? 'justify-end' : 'justify-start'}`}
                        >
                            <div
                                className={`max-w-xs md:max-w-md px-4 py-2 rounded-lg ${message.sender === 'user'
                                        ? 'bg-blue-500 text-white rounded-br-none'
                                        : 'bg-gray-200 text-gray-800 rounded-bl-none'
                                    }`}
                            >
                                <p className="whitespace-pre-wrap">{message.text}</p>
                                <div className="mt-1 flex justify-between">
                                    <span className="text-xs opacity-70">
                                        {message.timestamp.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                                    </span>
                                    {message.status === 'sending' && (
                                        <span className="text-xs opacity-70">Wysyłanie...</span>
                                    )}
                                    {message.status === 'error' && (
                                        <span className="text-xs text-red-500">Błąd</span>
                                    )}
                                </div>
                            </div>
                        </div>
                    ))
                )}
                <div ref={messagesEndRef} />
            </div>

          
            <div className="rounded-b-lg border-t border-gray-600 bg-gray-800 p-4">
                <div className="flex">
                    <textarea
                        value={inputValue}
                        onChange={(e) => setInputValue(e.target.value)}
                        onKeyDown={handleKeyPress}
                        placeholder="Wpisz swoją wiadomość..."
                        className="flex-1 border rounded-l-lg p-3 resize-none focus:outline-none focus:ring-2 focus:ring-blue-500 bg-white"
                        rows={1}
                        disabled={isLoading}
                    />
                    <button
                        onClick={handleSendMessage}
                        disabled={isLoading || !inputValue.trim()}
                        className={`bg-blue-600 text-white px-6 rounded-r-lg font-medium ${isLoading || !inputValue.trim()
                                ? 'opacity-50 cursor-not-allowed'
                                : 'hover:bg-blue-700'
                            }`}
                    >
                        {isLoading ? (
                            <div className="flex items-center justify-center">
                                <div className="h-5 w-5 animate-spin rounded-full border-b-2 border-white"></div>
                            </div>
                        ) : (
                            'Wyślij'
                        )}
                    </button>
                </div>
                <p className="mt-2 text-xs text-gray-400">
                    Asystent może popełniać błędy. Sprawdź ważne informacje.
                </p>
            </div>
        </div>
    );
};

export default AssistantComponent;
