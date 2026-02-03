# Files
Alle Daten zu einem Wettkampf sind in einem File/Abruf enthalten

## Glossar 


## Besonderheiten
- Bahneinteilung ist sehr genau bei Langläufen: 1-8 bedeutet erste Reihe, 
  8te Position; wird jetzt normalisiert: 1-8 -> 18 -10 -> Startpos Nr 8
- Die Ergebnisse sind eine Liste aller Disziplinen, in jeder Disziplin (Event)
  sind in den Entries alle Heats und Runden enthalten
- es gibt keinen Id für eine Unit, also Lauf, wie bei Omega. Bei technischen Disziplinen
  spielt es keine Rolle, aber bei Läufen müss die Phase und die Unit angegeben werden.
  Deswegen wird dieser bei Seltec intern erzeugt: <EventId-Round-Heat>
- es gibt ein Feld Status, dass abgefragt werden kann, ob ein Athlet den Wettkampf
  durchgeführt hat.
- AttemptSeperators gibt es nicht und werden deshalb auf [3] gesetzt, da bei Seltec 
  nur nach dem 3ten versuch sortiert wird
## Fragen zur Implementation
