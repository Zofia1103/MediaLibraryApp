Cel projektu: 
Aplikacja desktopowa służąca do katalogowania książek, filmów i albumów muzycznych w domowej kolekcji. Pozwala łatwo dodawać pozycje, filtrować je, oznaczać jako wypożyczone oraz generować raport tekstowy.

Opis projektu:
Aplikacja została napisana w technologii WPF (.NET) z wykorzystaniem Entity Framework Core oraz SQLite jako lekko‑wagowej bazy danych. 
Wszystkie operacje (dodawanie, usuwanie, filtrowanie) wykonują się lokalnie — dane przechowywane są w pliku media.db.
Klasa MediaContext automatycznie tworzy bazę przy pierwszym uruchomieniu (Database.EnsureCreated()).

Funkcje:
Dodawanie nowych pozycji (tytuł, autor, gatunek, typ).
Filtrowanie listy według autora.
Oznaczanie/odznaczanie pozycji jako Wypożyczone.
Usuwanie wybranych rekordów.
Generowanie raportu (MediaReport.txt) zawierającego pełną listę kolekcji.

Technologie
Entity Framework Core 8
Baza SQLite
Język C#
