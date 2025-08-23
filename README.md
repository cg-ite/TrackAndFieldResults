# TrackAndFieldResults
Dies ist eine Bibliothek für das Abfragen von Leichtathletik Wettkampf Ergebnissen mit C#. In der Leichtathletik gibt es mehrere Zeitmessunternehmen, die die Ergebnisse im Internet zum Teil live veröffentlichen. Mit dieser Bibliothek soll der Zugriff auf die Ergebnisse vereinfacht werden.

Geplant sind folgende Anbieter zu unterstützen:
- [x] Omega
- [x] Seltec
- [x] Athos
- [ ] Worldathletics

Bei den ersten drei Anbietern handelt es sich um Wettkampf-Ergebnis Dienste bei denen die Daten Wettkampf zentriert abgefragt werden können:
- Liste der bzw. Suche nach Wettkämpfe
- Auswahl des gewünschten Wettkampfes
- List aller Disziplinen/Events
- Auswahl desr Disziplin
- Liste der Ergebnisse pro Athlete

Worldathletics bieten den Zugriff Athletenzentriert auf die Daten der internationalen Atleten in der Form:
- Suche nach Athlet
- Auswahl Athlet
- Liste der Bestleistungen
- Liste der Progression in den Disziplinen
- Liste der Wettkampf-Ergebnisse

## Philosopie
Die einzelnen Clients werden in Anlehung an NSwag generierten Code gestaltet, da Seltec eine swagger Schnittstelle
bietet. So sind alle Clients im API gleich. Allerdings ist zu beachten, dass Seltec alle Daten zu einem Wettkampf
pro Request schickt. Omega und Atos aber einzelne Requests für Schedule und die einzelnen Events benötigen.

Die Daten der einzelnen Anbieter werden im ersten Schritt nicht verändert, so dass das Datei-Format unverändert bleibt. Dies ist beabsichtigt, damit alle Daten zur Verfügung stehen.

Es wird im Namespace `Common` eine Reihe von DTOs und extension-Methoden geben, die die Fülle an Anbieter spezifischen Daten reduzieren und besser verständlich machen. Diese Methoden sind nur eine Empfehlung von mir und enthalten jeweils nur die Daten, die ich selbst benötige.

Sie sollen die vielen Datenquellen in ein möglichst einheitliches API zusammenführen, so dass z.B. die Variablennamen für die Ahtletennammen immer gleich bezeichnet sind.

Du kannst basierend auf diesen DTO-Klassen dein eigenes API designen. Die DTO-Klassen sind partial, so dass weitere Daten einfach hinzugefügt werden können.

## Disclaimer
Dies ist keine offiziell unterstützte API-Bibliothek. Nutzung auf eingene Gefahr. Bitte die TOS der jeweiligen Anbieter beachten.
