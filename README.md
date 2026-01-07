# Zuul

Een simpele textadventure met een moderne TUI (Text User Interface).

## Het spel spelen

**Stap 1: Installeer dotnet**

Installeer de laatste versie van [dotnet](https://dotnet.microsoft.com/en-us/download).
Of installeer de LTS versie (Long Term Support).

**Stap 2: Open een terminal**

Open deze folder in de terminal.
De folder bevat het Zuul.csproj bestand.

**Stap 3: Run het spel**

Typ dit commando:

```
dotnet run
```

Het spel start nu.
Veel plezier!

---

## 🎮 Hoe te spelen

### TUI Menu Systeem

Het spel gebruikt een interactief menu-systeem met toetsenbord navigatie:

- **Druk op een willekeurige toets** om het commandomenu te openen
- **↑/↓ Pijltjestoetsen**: Navigeer door de beschikbare commando's
- **SPATIEBALK of ENTER**: Selecteer het gemarkeerde commando
- **ESC**: Annuleer de selectie en ga terug

### Beschikbare Commando's

Het menu toont alle geldige commando's met beschrijvingen:

| Commando | Beschrijving |
|----------|--------------|
| `help` | Toon hulp informatie |
| `go` | Verplaats naar een andere kamer (vraagt om een richting) |
| `quit` | Sluit het spel af |
| `look` | Kijk rond in de huidige kamer |
| `health` | Controleer je huidige gezondheid |

**Let op**: Wanneer je het `go` commando selecteert, word je gevraagd om een richting in te voeren (bijv. north, south, east, west, up, down).

---

## 🏗️ Architectuur en Code Kwaliteit

### Type-Safe Command Systeem

Het project gebruikt moderne C# patterns voor betere code kwaliteit:

#### Enum-based Commands (`CommandType.cs`)
- **Geen string vergelijkingen meer**: Alle commando's zijn gedefinieerd als `CommandType` enum
- **Compile-time veiligheid**: Typefouten worden tijdens compilatie gevonden
- **Betere prestaties**: Enum switches zijn sneller dan string switches
- **IntelliSense ondersteuning**: IDE kan alle geldige commando's suggereren

```csharp
public enum CommandType
{
    Unknown,
    Help,
    Go,
    Quit,
    Look,
    Health
}
```

#### Struct-based Command (`Command.cs`)
- **Readonly struct**: Immutable en efficiënt in geheugengebruik
- **Type-safe parameters**: Gebruikt `CommandType` in plaats van strings
- **Built-in validatie**: Methoden om te controleren of parameters vereist/aanwezig zijn

```csharp
public readonly struct Command
{
    public CommandType Type { get; init; }
    public string Parameter { get; init; }
}
```

#### Extension Methods (`CommandTypeExtensions`)
- **Centraal beheer**: Alle command-gerelateerde logica op één plek
- **String conversie**: Veilige conversie tussen strings en enums
- **Metadata**: Beschrijvingen en validatie regels per commando type

### Component Overzicht

```
┌─────────────────┐
│   MenuSystem    │  TUI met arrow key navigatie
└────────┬────────┘
         │
┌────────▼────────┐
│     Parser      │  Interpreteert gebruikersinvoer
└────────┬────────┘
         │
┌────────▼────────┐
│  CommandLibrary │  Beheert geldige CommandTypes
└────────┬────────┘
         │
┌────────▼────────┐
│      Game       │  Verwerkt commando's met enum switch
└─────────────────┘
```

### Voordelen van deze architectuur

1. **Onderhoudbaarheid**: Nieuwe commando's toevoegen is eenvoudig en veilig
2. **Testbaarheid**: Enums zijn gemakkelijk te testen zonder string magic
3. **Prestaties**: Geen string allocaties of vergelijkingen in de game loop
4. **Veiligheid**: Compile-time controle voorkomt runtime fouten
5. **Leesbaarheid**: Code is zelf-documenterend met enums en structs

---

## 📁 Project Structuur

```
Zuul/
├── src/
│   ├── Command.cs          # Readonly struct voor commando representatie
│   ├── CommandType.cs      # Enum definitie + extension methods
│   ├── CommandLibrary.cs   # Beheert geldige commando types
│   ├── Game.cs            # Hoofd game logica met enum switching
│   ├── MenuSystem.cs      # TUI menu voor command selectie
│   ├── Parser.cs          # Input parsing met menu integratie
│   ├── Player.cs          # Speler status en inventaris
│   └── Room.cs            # Kamer definities en uitgangen
├── Zuul.csproj
└── README.md
```

---

## 🔧 Ontwikkeling

### Bouwen

```bash
dotnet build
```

### Uitvoeren

```bash
dotnet run
```

### Een nieuw commando toevoegen

1. Voeg het commando toe aan de `CommandType` enum
2. Voeg metadata toe in `CommandTypeExtensions` (string mapping, beschrijving, validatie)
3. Implementeer de handler in `Game.ProcessCommand()`
4. Klaar! Het menu wordt automatisch bijgewerkt
