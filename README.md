# CodeOdyssey

Platforma e-learningowa do nauki programowania (m.in. C#) z grywalizacją i wsparciem AI. Projekt inżynierski: backend w ASP.NET Core (.NET 9) i frontend w React + TypeScript.

## Demo

[![Demo aplikacji CodeOdyssey](https://img.youtube.com/vi/oWyKugnDe1Q/maxresdefault.jpg)](https://www.youtube.com/watch?v=oWyKugnDe1Q)

Pełne demo aplikacji: [obejrzyj na YouTube](https://www.youtube.com/watch?v=oWyKugnDe1Q). Projekt działa lokalnie, nie ma wersji online.

## Najważniejsze funkcje

- **Kursy i lekcje**: teoria, quizy i ćwiczenia praktyczne, śledzenie postępów
- **Edytor kodu i uruchamianie kodu**: Monaco Editor + `CodeRunner`
- **Asystent AI** (OpenAI): czat, code review rozwiązań, generowanie treści, adaptacyjna nauka
- **Osiągnięcia i odznaki**: system reguł (serie nauki, liczba lekcji i zadań, wyniki quizów), powiadomienia w czasie rzeczywistym (SignalR)
- **Wyzwania**, w tym tygodniowe
- **PVP**: pojedynki graczy z kolejką matchmakingu (SignalR)
- **Sklep premium** i **panel administratora**
- **Uwierzytelnianie**: JWT w ciasteczku HttpOnly, rejestracja wieloetapowa

## Co warto zobaczyć w kodzie

- **MediatR**: logowanie i rejestracja jako komendy (`Users/Application/Commands` + `Handlers`), kontroler wysyła je bezpośrednio przez `IMediator`.
- **Zdarzenia domenowe**: `LessonEvaluatedEvent` uruchamia sugestię kursu powtórzeniowego, a `AchievementUnlockedEvent` wysyła powiadomienie przez SignalR. Każde ma własny `INotificationHandler`.
- **Układ modułowy**: każdy moduł ma warstwy `Api` / `Application` / `Domain` / `Infrastructures`.
- **Bezpieczeństwo logowania**: hashowanie z rehashowaniem, wyrównany czas odpowiedzi dla nieznanego e-maila, rate limiting na endpointach `auth`.
- **Testy**: xUnit (jednostkowe i integracyjne), projekt `Tests/`.

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
