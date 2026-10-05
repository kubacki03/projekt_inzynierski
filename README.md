# CodeOdyssey – projekt inżynierski

Platforma e-learningowa do nauki programowania (m.in. C#) z elementami grywalizacji i wsparciem AI. Składa się z backendu ASP.NET Core (.NET 9) oraz frontendu React + TypeScript (Vite).

## Funkcje

- **Kursy i lekcje** – teoria, quizy i ćwiczenia praktyczne, śledzenie postępów
- **Edytor kodu i uruchamianie kodu** – Monaco Editor + `CodeRunner`
- **Asystent AI** (OpenAI) – czat, analiza i code review rozwiązań, generowanie treści, adaptacyjna nauka
- **Osiągnięcia i odznaki** – system reguł (serie nauki, liczba lekcji/zadań, wyniki quizów…), powiadomienia w czasie rzeczywistym (SignalR)
- **Wyzwania** – w tym wyzwania tygodniowe
- **PVP** – pojedynki graczy z kolejką matchmakingu (SignalR, `/hubs/game`)
- **Sklep premium**
- **Panel administratora** – zarządzanie użytkownikami i materiałami kursów
- **Uwierzytelnianie** – JWT, rejestracja wieloetapowa

## Technologie

| Warstwa | Stack |
|---|---|
| Backend | ASP.NET Core 9, Entity Framework Core 9 (SQL Server), MediatR, SignalR, JWT Bearer, OpenAI SDK, iText7 |
| Frontend | React 19, TypeScript, Vite, Tailwind CSS 4, React Router, Axios, react-hook-form + yup, Monaco Editor, three.js / GSAP / Framer Motion |
| Testy | xUnit (testy integracyjne i jednostkowe) |

## Struktura repozytorium

```
projekt_inzynierski.sln
├── projekt_inzynierski.Server/   # Web API (moduły: Users, Courses, Content, Achievements,
│                                 #   Challenges, PVP, PremiumStore, AIHelper, CodeRunner, Admin)
├── projekt_inzynierski.client/   # SPA (React + Vite)
└── Tests/                        # Projekt testowy
```

Każdy moduł backendu ma układ warstwowy: `Api` / `Application` / `Domain` / `Infrastructures`.

## Wymagania

- [.NET SDK 9](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (LTS) i npm
- SQL Server (domyślnie LocalDB: `(localdb)\mssqllocaldb`)
- Klucz API OpenAI

## Konfiguracja

Backend (`projekt_inzynierski.Server/appsettings.json`):

| Ustawienie | Opis |
|---|---|
| `ConnectionStrings:DefaultConnection` | Połączenie z bazą SQL Server |
| `Jwt:Key` | Sekret do podpisywania tokenów JWT (nie jest w repozytorium – ustaw przez *user secrets*) |
| `Jwt:Issuer` | Wystawca tokenów |
| `Cors:AllowedOrigins` | Dozwolone originy frontendu (domyślnie `https://localhost:51339`) |

Zmienna środowiskowa:

```powershell
$env:OPEN_AI_API_KEY = "sk-..."
```

Przykład ustawienia sekretu JWT:

```powershell
cd projekt_inzynierski.Server
dotnet user-secrets set "Jwt:Key" "<losowy-dlugi-sekret>"
```

Frontend: adres API w `projekt_inzynierski.client/.env.development` (`VITE_API_URL`) oraz `.env.production`.

## Uruchomienie

```powershell
# 1. Baza danych
cd projekt_inzynierski.Server
dotnet ef database update

# 2. Frontend – zależności
cd ..\projekt_inzynierski.client
npm install

# 3. Backend (uruchamia też frontend przez SPA proxy)
cd ..\projekt_inzynierski.Server
dotnet run
```

Alternatywnie otwórz `projekt_inzynierski.sln` w Visual Studio i uruchom projekt serwerowy – `npm run dev` zostanie wystartowany automatycznie. Frontend działa pod `https://localhost:51339`.

Frontend osobno:

```powershell
cd projekt_inzynierski.client
npm run dev       # tryb deweloperski
npm run build     # build produkcyjny
npm run lint
```

## Testy

```powershell
dotnet test
```

## Huby SignalR

| Ścieżka | Przeznaczenie |
|---|---|
| `/hubs/achievements` | Powiadomienia o osiągnięciach |
| `/notificationHub` | Powiadomienia ogólne |
| `/hubs/game` | Rozgrywka PVP |

## Autor

Jakub Dąbrowski
