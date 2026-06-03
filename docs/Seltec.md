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
-  bei mehreren Agegroups wird innerhalb der Agegroup
  sortiert und dann nach aufsteigender Agegroup
  gesetzt. Zumindestend bei Seltec, bei den
  anderen Anbietern gibt es das nicht?
  Omega Ratingen schauen?
            
## Fragen zur Implementation
- entry muss eingebaut werden, damit klar wird, in welcher Agegroup ein Athlet
  startet. Denn ein Athlet kann auch hochmelden. Der Entry verbindet diese
  Infos. Bei Omega oder WA wird das nicht vorkommen, da die Felder immer
  AgeGroup rein sein werden.
  Dann kann der SortKey auch die Agegroup berücksichtigen und die Sortierung
  anpassen.
- Wie die internen Ids behandeln? AgeGroupId behalten, damit Abfrage klappt
## Tests
- bei geplanten Events ist rounddate angegeben nicht heatdate. ist rounddate
  verlässlicher und immer gesetzt?
- wird enddate der Veranstaltung immer gesetzt?