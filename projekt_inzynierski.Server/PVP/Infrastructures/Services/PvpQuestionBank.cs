using projekt_inzynierski.Server.PVP.Application.Interfaces;
using projekt_inzynierski.Server.PVP.Domain.Models;

namespace projekt_inzynierski.Server.PVP.Infrastructures.Services
{
    public class PvpQuestionBank : IPvpQuestionBank
    {
        private record Template(string Text, string Correct, string[] Wrong);

        private static Template Q(string text, string correct, params string[] wrong) => new(text, correct, wrong);

        private static readonly Dictionary<int, Template[]> Bank = new()
        {
            [1] = new[]
            {
                Q("Które słowo kluczowe deklaruje stałą w JavaScript?", "const", "def", "final", "static"),
                Q("Co zwróci typeof null?", "object", "null", "undefined", "number"),
                Q("Który operator porównuje wartość i typ?", "===", "==", "=", "!="),
                Q("Jak wypisać komunikat w konsoli przeglądarki?", "console.log()", "print()", "echo()", "System.out.println()"),
                Q("Jaki będzie wynik wyrażenia '5' + 3?", "'53'", "8", "NaN", "undefined"),
                Q("Która metoda tablicy dodaje element na końcu?", "push()", "pop()", "shift()", "unshift()"),
                Q("Który zapis przedstawia funkcję strzałkową?", "(a, b) => a + b", "function => (a, b)", "(a, b) -> a + b", "=> (a, b) a + b"),
                Q("Co zwróci Boolean('')?", "false", "true", "null", "undefined"),
                Q("Która metoda tworzy nową tablicę przez przekształcenie każdego elementu?", "map()", "forEach()", "push()", "splice()"),
                Q("Jak odczytać liczbę elementów tablicy arr?", "arr.length", "arr.size()", "arr.count", "len(arr)"),
                Q("Która pętla zawsze wykona się przynajmniej raz?", "do...while", "for", "while", "for...of"),
                Q("Jak zapisać komentarz jednoliniowy w JavaScript?", "// tekst", "# tekst", "-- tekst", "<!-- tekst -->")
            },
            [2] = new[]
            {
                Q("Czym jest domknięcie (closure)?", "Funkcją zachowującą dostęp do zmiennych z zakresu, w którym powstała", "Funkcją wywoływaną natychmiast po deklaracji", "Metodą zamykającą połączenie z serwerem", "Blokiem try/catch otaczającym funkcję"),
                Q("Co zwraca wyrażenie await w funkcji async?", "Wynik rozwiązanej obietnicy", "Zawsze undefined", "Nową funkcję", "Obiekt Error"),
                Q("Które zadania w Event Loop mają wyższy priorytet?", "Mikrozadania (np. Promise.then)", "setTimeout", "setInterval", "Zdarzenia DOM"),
                Q("Czym różni się let od var?", "let ma zasięg blokowy, var funkcyjny", "let można redeklarować w tym samym zakresie, var nie", "var ma zasięg blokowy", "Nie ma żadnej różnicy"),
                Q("Do czego służy Object.freeze()?", "Zapobiega modyfikacji właściwości obiektu", "Usuwa obiekt z pamięci", "Wykonuje głęboką kopię obiektu", "Wstrzymuje wykonywanie skryptu"),
                Q("Jaka jest wartość this w funkcji strzałkowej?", "Dziedziczona z otaczającego zakresu leksykalnego", "Zawsze obiekt window", "Zawsze undefined", "Obiekt, na którym wywołano funkcję"),
                Q("Co zwróci wyrażenie [] + []?", "Pusty ciąg znaków", "0", "[]", "NaN"),
                Q("Która metoda spełnia się dopiero, gdy wszystkie obietnice z listy zostaną spełnione?", "Promise.all()", "Promise.race()", "Promise.any()", "Promise.resolve()"),
                Q("Co robi operator ?? w JavaScript?", "Zwraca prawy operand, gdy lewy jest null lub undefined", "Zwraca prawy operand, gdy lewy jest fałszywy", "Porównuje wartości ze zrzutowaniem typów", "Sprawdza, czy właściwość istnieje w obiekcie"),
                Q("Do czego służy prototyp w JavaScript?", "Do dziedziczenia właściwości i metod między obiektami", "Do deklarowania typów zmiennych", "Do kompilacji kodu do bajtkodu", "Do zarządzania pamięcią"),
                Q("Co zwróci typeof NaN?", "number", "NaN", "undefined", "object"),
                Q("Czym jest hoisting?", "Przenoszeniem deklaracji na początek zakresu", "Kopiowaniem obiektu", "Asynchronicznym ładowaniem skryptów", "Usuwaniem nieużywanych zmiennych")
            },
            [3] = new[]
            {
                Q("Jak wypisać tekst na ekranie w Pythonie?", "print(\"Hello\")", "echo \"Hello\"", "console.log(\"Hello\")", "System.out.println(\"Hello\")"),
                Q("Który typ danych przechowuje pary klucz-wartość?", "dict", "list", "tuple", "set"),
                Q("Co zwróci len([1, 2, 3])?", "3", "2", "4", "1"),
                Q("Jak zapisać komentarz jednoliniowy w Pythonie?", "# tekst", "// tekst", "/* tekst */", "-- tekst"),
                Q("Który operator wykonuje dzielenie całkowite?", "//", "/", "%", "**"),
                Q("Jak zdefiniować funkcję w Pythonie?", "def moja_funkcja():", "function moja_funkcja():", "func moja_funkcja():", "fn moja_funkcja():"),
                Q("Co zwróci 2 ** 3?", "8", "6", "5", "9"),
                Q("Który z typów jest niemodyfikowalny?", "tuple", "list", "dict", "set"),
                Q("Jak sprawdzić typ zmiennej x?", "type(x)", "typeof(x)", "x.type()", "gettype(x)"),
                Q("Jak zapisać pętlę po kolekcji?", "for x in kolekcja:", "foreach x in kolekcja:", "for (x : kolekcja)", "loop x in kolekcja:"),
                Q("Do czego w Pythonie służy wcięcie kodu?", "Określa blok kodu", "Jest tylko kwestią estetyki", "Oznacza komentarz", "Deklaruje zmienną"),
                Q("Co zwróci 'abc'.upper()?", "ABC", "abc", "Abc", "cba")
            },
            [4] = new[]
            {
                Q("Do czego służy dekorator w Pythonie?", "Do zmiany zachowania funkcji bez zmiany jej kodu", "Do deklarowania typów zmiennych", "Do importowania modułów", "Do obsługi wyjątków"),
                Q("Co robi słowo kluczowe yield?", "Tworzy generator zwracający wartości kolejno", "Kończy program", "Przerywa pętlę", "Importuje moduł"),
                Q("Czym jest GIL w CPythonie?", "Globalną blokadą interpretera ograniczającą równoległe wykonywanie bajtkodu", "Biblioteką graficzną", "Menedżerem pakietów", "Mechanizmem szyfrowania"),
                Q("Co zwróci [x * x for x in range(3)]?", "[0, 1, 4]", "[1, 4, 9]", "[0, 1, 2]", "[0, 2, 4]"),
                Q("Do czego służy instrukcja with?", "Do zarządzania zasobami przez menedżer kontekstu", "Do definiowania klas", "Do tworzenia pętli", "Do dziedziczenia"),
                Q("Co oznacza *args w definicji funkcji?", "Dowolną liczbę argumentów pozycyjnych", "Wskaźnik do pamięci", "Argumenty nazwane", "Mnożenie argumentów"),
                Q("Która metoda specjalna inicjalizuje obiekt klasy?", "__init__", "__start__", "__create__", "__build__"),
                Q("Jaka jest średnia złożoność sprawdzenia x in set?", "O(1)", "O(n)", "O(log n)", "O(n^2)"),
                Q("Co zwróci bool([])?", "False", "True", "None", "Zgłosi błąd"),
                Q("Czym różni się list od tuple?", "list jest modyfikowalna, tuple nie", "tuple jest modyfikowalna, list nie", "Nie różnią się niczym", "tuple przechowuje tylko liczby"),
                Q("Co robi funkcja zip()?", "Łączy elementy kilku kolekcji w krotki", "Kompresuje plik", "Sortuje listę", "Odwraca listę"),
                Q("Który moduł służy do programowania asynchronicznego?", "asyncio", "multiasync", "syncio", "awaitlib")
            },
            [5] = new[]
            {
                Q("Który typ przechowuje liczby całkowite?", "int", "string", "bool", "char"),
                Q("Jak wypisać tekst w konsoli w C#?", "Console.WriteLine()", "print()", "console.log()", "echo()"),
                Q("Którego słowa kluczowego użyjesz do deklaracji klasy?", "class", "object", "define", "new"),
                Q("Jaki jest wynik 7 / 2 dla typu int?", "3", "3.5", "4", "2"),
                Q("Który modyfikator dostępu ogranicza dostęp do wnętrza klasy?", "private", "public", "internal", "protected"),
                Q("Jak zapisać pętlę po kolekcji?", "foreach (var x in lista)", "for x in lista", "foreach x of lista", "each (x in lista)"),
                Q("Czym jest string w C#?", "Typem referencyjnym reprezentującym tekst", "Typem liczbowym", "Tablicą liczb", "Typem logicznym"),
                Q("Który operator sprawdza równość?", "==", "=", "=>", "!"),
                Q("Jak zadeklarować tablicę pięciu liczb całkowitych?", "int[] t = new int[5];", "int t = [5];", "array<int> t(5);", "int t[5];"),
                Q("Co robi słowo kluczowe new?", "Tworzy nową instancję obiektu", "Usuwa obiekt", "Deklaruje stałą", "Importuje przestrzeń nazw"),
                Q("Który typ przechowuje wartość logiczną?", "bool", "bit", "logic", "flag"),
                Q("Jaki jest wynik 10 % 3?", "1", "3", "0", "7")
            },
            [6] = new[]
            {
                Q("Do czego służy słowo kluczowe async?", "Do oznaczania metod asynchronicznych, w których można używać await", "Do uruchamiania kodu w osobnym procesie", "Do blokowania wątku", "Do deklarowania zdarzeń"),
                Q("Co oznacza IEnumerable<T>?", "Sekwencję elementów, po której można iterować", "Tylko tablicę o stałym rozmiarze", "Słownik klucz-wartość", "Typ liczbowy"),
                Q("Co robi metoda LINQ Where?", "Filtruje elementy spełniające warunek", "Sortuje elementy", "Grupuje elementy", "Zwraca pierwszy element"),
                Q("Czym jest Dependency Injection?", "Wzorcem dostarczania zależności obiektowi z zewnątrz", "Metodą wstrzykiwania kodu SQL", "Mechanizmem kompilacji", "Sposobem dziedziczenia wielokrotnego"),
                Q("Jaka jest różnica między struct a class?", "struct jest typem wartościowym, class referencyjnym", "struct jest typem referencyjnym, class wartościowym", "Oba są typami wartościowymi", "Nie ma żadnej różnicy"),
                Q("Co robi instrukcja using dla obiektu IDisposable?", "Wywołuje Dispose po wyjściu z bloku", "Importuje bibliotekę DLL", "Tworzy nowy wątek", "Blokuje obiekt"),
                Q("Czym jest delegat?", "Typem reprezentującym referencję do metody", "Klasą bazową wyjątków", "Interfejsem kolekcji", "Atrybutem kompilatora"),
                Q("Co zwraca metoda zadeklarowana jako async Task<int>?", "Zadanie, które w wyniku da wartość int", "Wartość int bezpośrednio", "Nic", "Tablicę int"),
                Q("Które słowo kluczowe zabrania dziedziczenia po klasie?", "sealed", "abstract", "virtual", "override"),
                Q("Do czego służy operator ?. ?", "Do bezpiecznego dostępu do składowej obiektu, który może być null", "Do porównania napisów", "Do rzutowania typów", "Do łączenia kolekcji"),
                Q("Czym jest garbage collector?", "Mechanizmem automatycznie zwalniającym nieużywaną pamięć zarządzaną", "Narzędziem do kompilacji", "Debuggerem", "Menedżerem pakietów NuGet"),
                Q("Co robi słowo kluczowe virtual?", "Pozwala nadpisać metodę w klasie pochodnej", "Zabrania nadpisania metody", "Tworzy metodę statyczną", "Deklaruje interfejs")
            }
        };

        public IReadOnlyList<PvpQuestion> Pick(int gameId, int count)
        {
            if (!Bank.TryGetValue(gameId, out var templates))
            {
                return new List<PvpQuestion>();
            }

            return templates
                .OrderBy(_ => Random.Shared.Next())
                .Take(count)
                .Select((template, index) =>
                {
                    var options = template.Wrong
                        .Append(template.Correct)
                        .OrderBy(_ => Random.Shared.Next())
                        .ToArray();
                    return new PvpQuestion(index, template.Text, options, Array.IndexOf(options, template.Correct));
                })
                .ToList();
        }
    }
}
