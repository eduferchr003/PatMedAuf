#PatMedAuf - Patientenverwaltungssystem 
Die Anwendung ermöglicht: 
- strukturierte Patientenaufnahme 
- gezielte Patientensuche 
- Terminverwaltung 
- Anzeige medizinischer Bilddaten (DICOM) 
- automatische/manuelle Erstellung einer HL7 v3 Nachricht 

Ziele: 
- medizinische Stammdaten verwalten 
- standardtisierte medizinische Formate
- typische Workflows

Verwendete Technologie: 
- Framework: .NET Framework 4.x
- C#
- Datenbank: MySQL
- Visual Studio 2022

Verwendete Libraries/Extensions:
- Datenbank: MySQL.Data 
- DICOM: fo-dicom, fo-dicom.Codecs, K4os.Compression.LZ4, K4os.Compression.LZ4.Streams, K4os.Hash.xxHash

Datenbank: 
- MySQL 8.x
- Schema patmedauf
- zentrale Tabellen: patienten, patient_dicom

DICOM-Funktionalität:
- laden mehrerer DICOM-Dateien gleichzeitig 
- Anzeige von 1-4 Bildern 
- Zoom-Funktion über TrackBar
- Scrollbares Bild
- Anzeige wichtiger DICOM-Tags
- Zuordnung der Bilder zu Patienten über die SVNr

HL7 v3 - Entlassungsnachricht 
- eine HL7 v3 XML-Datei wird autmatisch erzeugt 
- Benutzer wählt den Speicherort 
- die Datei enthält: Patientendaten, Termin-Informationen, Diagnosen und Entlassungszeitpunkt 