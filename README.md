# ChessGameSketch

# Do poprawnego działania rozwiązania należy jednocześnie uruchomić projekty ChessGameSketchApi oraz ChessGameSketchFront.

# Opis rozwiązania
Rozwiązanie składa się z następujących projektów:

- **ChessGameSketch** - zawiera logikę związaną z obliczaniem możliwych ruchów i walidacją podejmowanych akcji.
- **ChessGameSketchTest** - zawiera testy walidatora.
- **ChessGameSketchApi** - odpowiada za udostępnenie endpointów, przyjmowanie zapytań dot. ruchów i możliwych ruchów, zapisywanie pozycji figur i ew. pola en passant (bicie w przelocie).
- **ChessGameSketchFront** - odpowiada interfejs graficzny (wykonane w Angular).
Backend udostępnia figury możliwe do postawienia na planszy, dodawanie figur do planszy, wykonywanie ruchów, usuwanie figur i udostępnianie możliwych ruchów dla danej figury w kontekscie wszystkich figur na planszy.

Pionki, które dotrą na ostatnie pole, zostają automatycznie wypromowane na Hetmana.

# Algorytm walidacji ruchów:
**Nie zezwala na:**
- ruchy wykraczające poza pole gry,
- ruchy narażające króla na szacha,
- postawienie nowej figurki na innej figurze,

**Zezwala na:**
- bicie innych figur (także w przelocie),
- ruchy zgodne z zasadami

# Front end:
Komponent **app.component** odpowiada za wyświetlanie interfejsu użytkownika, wykrywanie akcji (click) i zarządzanie nowymi figurkami.

Podejmowanie akcji, generowanie widoku planszy z figurami oraz wyświetlanie wiadomości z backendu jest realizowane przez serwis game.manager.

Widok planszy (tablica pól - Tile[][]) oraz wiadomości dla użytkownika są przekazywane za pośrednictwem interfejsów Subject z **game.manager** do **app.component**.
