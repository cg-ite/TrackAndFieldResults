# Files
- competitions: alle keys
- details: Name, Beschreibung und Ort
- schedule: Tage, Events, Units
- event: Ergebnisse, Athleten

## Glossar 
- competition: Wettkampf mit allen Disziplinen
- event: Disziplin
- phase: RND - Round 1, SMF - Semifinals, FNL - Finals, oder Disziplin im Mehrkampf
- unit: Lauf (Heat 1, Semifinal 1, ...) oder bei technischen Disziplinen Gruppe/Riege (Group A)

- ListEvents: alle Events aber ohne Uhrzeit
- Units: Alle Disziplinen mit Details wie Uhrzeit, Sieger, Übersetzungen, ...

## Besonderheiten
- Startlist-Hashtab verändert sich während des Wettkmapfes, wenn umsortiert wird. Bei
  Horizontal-Sprüngen und Würfen kann die erste Startliste nach dem Wettkampf nicht mehr
  aus dem json abgerufen werden. Sie gibt es nur noch als pdf. Genau genommen kann nach dem
  Wettkampf keine Startliste mehr berechnet werden, da die initiale Startpos fehlt. Diese
  ist bei Weitengleichheit aber notwendig.
  Bei Vertikal-Sprügen kann sie vorher und nachher abgerufen werden.
- Die Startlist bei Läufen enthält die Bahn des Laufes. Bei Läufen ohne Bahnen > 1500m ist 
  trotzdem eine Startreihenfolge (Aufstellung an Linie) gesetzt
- In stats steht auch der milisekunden Anteil der Leistung bei Läufen
- AttemptSeperators sind standardmäßig [3,5]; mal sehen wie es mit dem neuen System ist
- Event ID: CompID dann nach _ATH kommt M/W, dann bis zu 8 chars der Shortcode; Bei DECATH oder HEPTATH dann 10 frei bzw Agegroup und dann 4 für Disziplin

## Fragen zur Implementation
- Startlist index bei 1 oder 0 beginnen?
- Wie mit Läufen umgehen, da ja immer eigene Datei? Gesamtübersicht erstellen?
  Startlist mit mehreren 1. Pos?
- gibt es abfinale als phase?
- gibt es einen Event Status? Started, Finished?
- gibt es überhaupt agegroups?