Spieler
- Figur
- Movement
- Interkation mit Holzsammelstelle & Rigidbody-Stuff für Hebel
Holzsammelstelle
- Position zu der Spieler hingehen und interagieren kann um nach kleiner X Zeit ein Stück Holz in der Hand zu halten

Cart
- Wand & Hebel
    - Rotation muss auf 4 Richtungen snappen
- Feuer
    - aktuelle & maximale Brennstärke
    - bleed

Schienen
- Spline

Wind
- Emitter ist Ebene, Punkt des Feuers auf Ebene, Raycast zurück -> binär im Wind/nicht im Wind
Maschienen
- Position auf der Schiene an der gewartet wird und das Feuer bekommt Schaden bis Minimalwert für X Zeit

Erkundungs-Radius
- Es ist wirklich einfach zu dunkel um iwas zu sehen, selbst Schuld du Opfer
- In Dunkelheit sollte keine Interaktion mit Resourcen möglich sein
-> Gib Resourcen FireInteractor und berechne Entfernung etc

GameManager
- Win-Condition: Feuer erreicht das Ende
- Lose-Events: Feuer geht aus [|| Abgründe]
- Fortschritt des Carts auf der Strecke
    - Schienenstrecken, aber auch Maschienen

Welt:
Maßstab - Cart 1x1 Unity-Units, Schiene ca 1 Unit breit

Treibstoff/-veredelung
Umgebungsobjekte zum Schieben?

Ziel eines Levels:
Feuer fährt zum Ende der Schiene

Levelaufbau:
- Lange lange Schiene
- Spieler kann nur kleinen Teil des Gesamten überblicken.
- Auf der Strecke gibt es immer wieder
    - Maschienen, die Feuer-Energie verbrauchen
    - Resourcen-Sammelstellen
    - Maschienen, die Resourcen zu besserem Treibstoff veredeln
- Feuerwagen fährt kontinuierlich weiter (wird an Maschienen für X Zeit gestoppt)

Notes:
- Wand kann per Hebel am Wagen durch physisches Gegenlaufen gedreht werden
- Stärke des Feuers bestimmt die Reichweite des sichtbaren Bereichs um den Wagen herum, denn die Welt ist dunkel
- 