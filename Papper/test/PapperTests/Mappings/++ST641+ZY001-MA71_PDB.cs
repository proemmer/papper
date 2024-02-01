using Papper.Attributes;
using System;


namespace Insite.Customer.Data.___ST641_ZY001_MA71_PDB
{
    
    

    [PlcType("GROB_TESTSTOP_SBT_UDT.CONFIG")]
    public class GROB_TESTSTOP_SBT_UDT_CONFIG
    {
        public bool BRAKE_1_TEST { get; set; }	//0 = Bremse 1 nicht testen, 1 = Bremse 1 testen
        public bool BRAKE_1_WITH_FEEDBACK { get; set; }	//0 = Bremse 1 ohne Rückm., 1 = Bremse 1 mit Rückm.
        public bool BRAKE_1_RELEASE_FEEDBACK { get; set; }	//0 = Bremse 1 betätigt, 1 = Bremse 1 gelöst
        public bool BRAKE_1_EXTERNAL { get; set; }	//0 = Bremse 1 Ansteuerung über Antieb, 1 = Bremse 1 Ansteuerung über Schütz
        public bool BRAKE_2_TEST { get; set; }	//0 = Bremse 2 nicht testen, 1 = Bremse 2 testen
        public bool BRAKE_2_WITH_FEEDBACK { get; set; }	//0 = Bremse 2 ohne Rückm., 1= Bremse 2 mit Rückm.
        public bool BRAKE_2_RELEASE_FEEDBACK { get; set; }	//0 = Bremse 2 betätigt, 1 = Bremse 2 gelöst
        public bool BRAKE_2_EXTERNAL { get; set; }	//0 = Bremse 2 Ansteuerung über Antieb, 1 = Bremse 1 Ansteuerung über Schütz
    }

    

    [PlcType("GROB_TESTSTOP_SBT_UDT.STATUS")]
    public class GROB_TESTSTOP_SBT_UDT_STATUS
    {
        public bool BRAKE_1_CLOSE_REQUEST { get; set; }	//1= Bremse 1 betätigen Anforderung
        public bool BRAKE_1_ERROR { get; set; }	//Bremsentest Bremse 1 fehlgeschlagen
        public bool BRAKE_2_CLOSE_REQUEST { get; set; }	//1= Bremse 2 betätigen Anforderung
        public bool BRAKE_2_ERROR { get; set; }	//Bremsentest Bremse 2 fehlgeschlagen
        public bool BRAKETEST_REQUIRED { get; set; }	//Bremsentest erforderlich
        public bool BUSY { get; set; }	//Test wird ausgeführt
        public bool DONE { get; set; }	//Test abgeschlossen
        public bool ERROR { get; set; }	//Test Störung
        
        [StringLength(254)]
        public string DIAG { get; set; }	//Diagnosestatus
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.GENERAL", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_GENERAL
    {
        [NotAccessible(true)]
        public Int16 PART_TYPE { get; set; }	//Typ
        [NotAccessible(true)]
        public Int16 UNIT { get; set; }	//1: Millimeter mm 2: Grad °
        [NotAccessible(true)]
        public bool INTERPOLATE { get; set; }	//0: Positionierachse; 1: Interpolierte Achse
        [NotAccessible(true)]
        public bool MODULO { get; set; }	//Moduloachse
        [NotAccessible(true)]
        public double MODULO_VALUE { get; set; }	//Übersprung Moduloachse
        [NotAccessible(true)]
        public bool THERM_SENSOR { get; set; }	//Rückmeldung Temperatursensor des Bremswiderstandes ok - nur bei 
        [NotAccessible(true)]
        public bool MASTER_SYNC_ACTIVE { get; set; }	//Dies ist eine Masterachse - alle Slaves die sich auf diese Achse koppeln können verodern (Basic.Feedback_Ax.Slave_in_Sync)
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.JOG.RETRACT", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_JOG_RETRACT
    {
        [NotAccessible(true)]
        public bool ILOCK { get; set; }	//Joggen Verriegelung
        [NotAccessible(true)]
        public bool REQ { get; set; }	//Joggen angefordert von HMI
        [NotAccessible(true)]
        public Int16 ILOCK_ADD { get; set; }	//Interne Laufvariable für mehrere FCs zu einem Befehl
        [NotAccessible(true)]
        public UInt16 DIAG_ID { get; set; }	//Stördiagnose weiterleiten
        [NotAccessible(true)]
        public bool BYPASS_ACTIVE { get; set; }	//Bypass aktiv
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe für HMI
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.JOG.ADVANCE", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_JOG_ADVANCE
    {
        [NotAccessible(true)]
        public bool ILOCK { get; set; }	//Joggen Verriegelung
        [NotAccessible(true)]
        public bool REQ { get; set; }	//Joggen angefordert von HMI
        [NotAccessible(true)]
        public Int16 ILOCK_ADD { get; set; }	//Interne Laufvariable für mehrere FCs zu einem Befehl
        [NotAccessible(true)]
        public UInt16 DIAG_ID { get; set; }	//Stördiagnose weiterleiten
        [NotAccessible(true)]
        public bool BYPASS_ACTIVE { get; set; }	//Bypass aktiv
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe für HMI
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.JOG", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_JOG
    {
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_JOG_RETRACT RETRACT { get; set; }	//Zurück
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_JOG_ADVANCE ADVANCE { get; set; }	//Vor
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.REF", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_REF
    {
        [NotAccessible(true)]
        public bool CONFIG_REF_ENCODER { get; set; }	//0 = Achse; 1 = Externer Geber
        [NotAccessible(true)]
        public bool CONFIG_INC_ENCODER { get; set; }	//0 = Absolutwertgeber; 1 = Inkrementalgeber
        [NotAccessible(true)]
        public double CONFIG_REF_POS_VALUE { get; set; }	//Referenzierposition
        [NotAccessible(true)]
        public bool REF_VALUE_FROM_HMI { get; set; }	//Referenzierwert kommt von HMI Eingabe
        [NotAccessible(true)]
        public bool ILOCK_REF { get; set; }	//Referenzieren Verriegelung
        [NotAccessible(true)]
        public bool REF_REQ { get; set; }	//Referenzieren angefordert von HMI
        [NotAccessible(true)]
        public bool REF_DONE { get; set; }	//Referenzieren erfolgreich abgeschlossen
        [NotAccessible(true)]
        public Int16 ILOCK_ADD { get; set; }	//Interne Laufvariable für mehrere FCs zu einem Befehl
        [NotAccessible(true)]
        public UInt16 DIAG_ID { get; set; }	//Stördiagnose weiterleiten
        [NotAccessible(true)]
        public UInt16 DIAG_INFO { get; set; }	//Werkerhinweis
        [NotAccessible(true)]
        public bool BYPASS_ACTIVE { get; set; }	//Bypass aktiv
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe für HMI
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.SAFE_REF", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_SAFE_REF
    {
        [NotAccessible(true)]
        public bool EXIST { get; set; }	//Sichere Referenzierung vorhanden
        [NotAccessible(true)]
        public double CONFIG_REF_POS_VALUE { get; set; }	//Referenzierposition
        [NotAccessible(true)]
        public bool ILOCK_REF { get; set; }	//Referenzieren Verriegelung
        [NotAccessible(true)]
        public bool REF_REQ { get; set; }	//Referenzieren angefordert von HMI
        [NotAccessible(true)]
        public bool REF_DONE { get; set; }	//Referenzieren erfolgreich abgeschlossen
        [NotAccessible(true)]
        public Int16 ILOCK_ADD { get; set; }	//Interne Laufvariable für mehrere FCs zu einem Befehl
        [NotAccessible(true)]
        public UInt16 DIAG_ID { get; set; }	//Stördiagnose weiterleiten
        [NotAccessible(true)]
        public UInt16 DIAG_INFO { get; set; }	//Werkerhinweis
        [NotAccessible(true)]
        public bool BYPASS_ACTIVE { get; set; }	//Bypass aktiv
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe für HMI
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.TEMP_COMP", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_TEMP_COMP
    {
        [NotAccessible(true)]
        public bool EXIST { get; set; }	//Temperaturkompensation vorhanden
        [NotAccessible(true)]
        public Int16 DIR { get; set; }	//Temperaturkompensation: Kompensationsrichtung
        [NotAccessible(true)]
        public double FIX_POINT { get; set; }	//Temperaturkompensation: Fixpunkt
        [NotAccessible(true)]
        public double END_POINT { get; set; }	//Temperaturkompensation: Endpunkt
        [NotAccessible(true)]
        public double SENSOR { get; set; }	//Temperaturkompensation: Messwert
        [NotAccessible(true)]
        public double ERR_LIMIT { get; set; }	//Temperaturkompensation: Maximale Abweichung Messwert innerhalb 100ms
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.LOCKING_PIN", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_LOCKING_PIN
    {
        [NotAccessible(true)]
        public bool EXIST { get; set; }	//vorhanden
        [NotAccessible(true)]
        public bool PIN_IN_REST { get; set; }	//Absteckbolzen vorhanden
        [NotAccessible(true)]
        
        [StringLength(30)]
        public string BMK { get; set; }	//BMK des Sensor einfügen
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.BELT", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_BELT
    {
        [NotAccessible(true)]
        public Int16 EXIST { get; set; }	//0: nicht vorhanden, 1:Riemenbruchkontrolle, 2:Schleppkettenüberwachung
        [NotAccessible(true)]
        public bool OK { get; set; }	//Rückmeldung i.O.
        [NotAccessible(true)]
        
        [StringLength(30)]
        public string BMK { get; set; }	//BMK des Sensor einfügen
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.OVERRIDE.HANDSHAKE_INTERN", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_OVERRIDE_HANDSHAKE_INTERN
    {
        [NotAccessible(true)]
        public bool MASTER_ACTIVE { get; set; }	//Globaler Override Diese Achse ist der Master
        [NotAccessible(true)]
        public bool FORCE_OVERRIDE { get; set; }	//Globaler Override schreiben
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.OVERRIDE", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_OVERRIDE
    {
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe für HMI
        [NotAccessible(true)]
        public Int16 VALUE { get; set; }	//Override in 0-100%
        [NotAccessible(true)]
        public bool GLOB_OVERRIDE { get; set; }	//Globaler Override angewählt
        [NotAccessible(true)]
        public bool START_WITH_GLOB_OVERRIDE { get; set; }	//Override wird Global betrachtet
        [NotAccessible(true)]
        public bool EXTERN_FORCE { get; set; }	//Value übernehmen
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_OVERRIDE_HANDSHAKE_INTERN HANDSHAKE_INTERN { get; set; }	//Handshake Globaler Override
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.VEL", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_VEL
    {
        [NotAccessible(true)]
        public double JOG { get; set; }	//Tippbetrieb (mm/s bzw. °/s)
        [NotAccessible(true)]
        public double SLS { get; set; }	//Sichere Geschwindigkeit (mm/s bzw. °/s)
        [NotAccessible(true)]
        public double REG { get; set; }	//Normaler Betrieb (Maximalgeschwindigkeit in mm/s bzw. °/s)
        [NotAccessible(true)]
        public Int16 DIGITS_AFTER_POINT { get; set; }	//Nachkommastellen
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.RAMP", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_RAMP
    {
        [NotAccessible(true)]
        public Int16 ACCELERATION { get; set; }	//Beschleunigung  (mm/s² bzw. °/s²)
        [NotAccessible(true)]
        public Int16 DECELERATION { get; set; }	//Verzögerung (mm/s² bzw. °/s²)
        [NotAccessible(true)]
        public double JERK { get; set; }	//Ruck (mm/s² bzw. °/s²)
        [NotAccessible(true)]
        public Int16 JERK_FACTOR { get; set; }	//Ruckfaktor
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.SBT.ERROR", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_SBT_ERROR
    {
        [NotAccessible(true)]
        public bool CALLCOUNTER { get; set; }	//Callcounter
        [NotAccessible(true)]
        public bool PARAMETER { get; set; }	//Parameter
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.SBT", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_SBT
    {
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe von HMI
        [NotAccessible(true)]
        public bool START_FROM_PHASE { get; set; }	//Ansteuerung Automatik
        [NotAccessible(true)]
        public bool MANUAL_BRAKE_TEST_REQUEST { get; set; }	//Manuelle Bremsentest Anforderung
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_SBT_ERROR ERROR { get; set; }	//Störungen
        [NotAccessible(true)]
        public GROB_TESTSTOP_SBT_UDT IN { get; set; }
        [NotAccessible(true)]
        public GROB_TESTSTOP_SBT_DATA_UDT OUT { get; set; }
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.SW_LIMIT", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_SW_LIMIT
    {
        [NotAccessible(true)]
        public bool REQUEST_IGNORE { get; set; }	//Software-Endschalter deaktivieren
        [NotAccessible(true)]
        public bool IGNORE_ACTIVE { get; set; }	//Software-Endschalter inaktiv
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.TORQUE_LIMIT", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_TORQUE_LIMIT
    {
        [NotAccessible(true)]
        public bool REDUCE { get; set; }	//Angefordert
        [NotAccessible(true)]
        public bool SET_ERROR { get; set; }	//Status 1 stoppt die Achse bei Überlast
        [NotAccessible(true)]
        public Int16 VALUE { get; set; }	//Prozentualer Wert
        [NotAccessible(true)]
        public bool ACTIVE { get; set; }	//Aktiv
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.TORQUE_MONITORING", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_TORQUE_MONITORING
    {
        [NotAccessible(true)]
        public bool REDUCE { get; set; }	//Angefordert
        [NotAccessible(true)]
        public bool SET_ERROR { get; set; }	//Status 1 stoppt die Achse bei Überlast
        [NotAccessible(true)]
        public Int16 VALUE { get; set; }	//Prozentualer Wert
        [NotAccessible(true)]
        public bool ACTIVE { get; set; }	//Aktiv
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.POS_DESYNC", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_POS_DESYNC
    {
        [NotAccessible(true)]
        public bool MODE_REQUEST { get; set; }	//Sondermode anwählen (Command)
        [NotAccessible(true)]
        public bool MODE_SELECTED { get; set; }	//Sondermode angewählt (Feedback)
        [NotAccessible(true)]
        public bool ENABLE_DESYNC_TO_POS { get; set; }	//Antrieb synchron (Feedback)
        [NotAccessible(true)]
        public bool START_DESYNC_TO_POS { get; set; }	//Fliegend auskoppeln und Positionieren (Command)
        [NotAccessible(true)]
        public bool IN_DESYNC_POS { get; set; }	//In Position (Feedback)
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.ERROR", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_ERROR
    {
        [NotAccessible(true)]
        public bool CONF_BASIC { get; set; }	//Konfigurationsfehler - keine Zuordnung möglich
    }

    

    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT.FEEDBACK_AX", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT_FEEDBACK_AX
    {
        [NotAccessible(true)]
        public bool READY_FOR_OPERATION { get; set; }	//Betriebsbereit
        [NotAccessible(true)]
        public double POSITION { get; set; }	//Istposition
        [NotAccessible(true)]
        public double TARGET_POSITION { get; set; }	//Sollposition
        [NotAccessible(true)]
        public Int16 VELOCITY { get; set; }	//Drehzahl
        [NotAccessible(true)]
        public double JERK { get; set; }	//Ruck
        [NotAccessible(true)]
        public Int16 TORQUE { get; set; }	//Drehmoment
        [NotAccessible(true)]
        public bool TORQUE_LIMIT { get; set; }	//Drehmomentreduzierung aktiv
        [NotAccessible(true)]
        public bool STANDSTILL { get; set; }	//Motorstillstand aktiv
        [NotAccessible(true)]
        public UInt32 STATUS_FAULT { get; set; }	//Anzeige Fehlercode / aktueller FCB wenn kein Fehler/Warnung ansteht
        [NotAccessible(true)]
        public bool SLAVE_IN_SYNC { get; set; }	//Diese Achse ist ein Slave und aktuell auf einen Master gekoppelt
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT.TOUCHPROBE", true)]
    public class GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT_TOUCHPROBE
    {
        [NotAccessible(true)]
        public bool ENABLE { get; set; }	//Freigabe
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe von HMI aus starten
        [NotAccessible(true)]
        public bool RESET { get; set; }	//Starttrigger ablöschen
        [NotAccessible(true)]
        public bool START { get; set; }	//Sensor scharf stellen
        [NotAccessible(true)]
        public Int16 SELECT_SENSOR { get; set; }	//Angewählter Sensor (Default: 1 wenn nur ein Sensor verbaut)
        [NotAccessible(true)]
        public double OFFSET { get; set; }	//Relativposition nach Triggerereignis
        [NotAccessible(true)]
        public bool FLAG { get; set; }	//0: fallende Flanke gesucht 1: steigende Flanke gesucht
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT.CHANGE_MASTER", true)]
    public class GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT_CHANGE_MASTER
    {
        [NotAccessible(true)]
        public bool ENABLE { get; set; }	//Freigabe
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe von HMI aus starten
        [NotAccessible(true)]
        public Int16 NUMBER { get; set; }	//Gewählter Master
        [NotAccessible(true)]
        public bool CHANGE_START { get; set; }	//Master wechseln
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT.GEARING", true)]
    public class GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT_GEARING
    {
        [NotAccessible(true)]
        public bool EXIST { get; set; }	//Vorhanden
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe von HMI aus starten
        [NotAccessible(true)]
        public bool START { get; set; }	//Starten
        [NotAccessible(true)]
        public bool STOP { get; set; }	//Stoppen
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTEND_FEEDBACK_UDT.CHANGE_MASTER", true)]
    public class GROB_DRIVE_SEW_C_EXTEND_FEEDBACK_UDT_CHANGE_MASTER
    {
        [NotAccessible(true)]
        public Int16 ACTIVE_MASTER { get; set; }	//Aktueller Master
        [NotAccessible(true)]
        public bool MASTER_CHANGED { get; set; }	//Master wurde geändert (Entprellt 500ms)
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTEND_FEEDBACK_UDT.GEARING", true)]
    public class GROB_DRIVE_SEW_C_EXTEND_FEEDBACK_UDT_GEARING
    {
        [NotAccessible(true)]
        public bool ACTIVE { get; set; }	//Aktiv
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.OUT.GENERAL")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_GENERAL
    {
        public Int16 ACTIVE_PIC { get; set; }	//Aktives Bild: 1: Cam; 2: Touchprobe; 3: Master; 4:Gearing
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.OUT.CONF")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_CONF
    {
        public bool CAM { get; set; }	//Kurvenscheibe
        public bool TOUCHPROBE { get; set; }	//Touchprobe
        public bool CHANGE_MASTER { get; set; }	//Masterquellenumschaltung
        public bool GEARING { get; set; }	//Gearing
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.OUT.CAM")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_CAM
    {
        public bool ENABLE_HMI { get; set; }	//Freigabe HMI
        
        [StringLength(254)]
        public string ILOCK_MISSING { get; set; }	//Werkerhinweis
        public bool ACTIVE_POS { get; set; }	//Befehl wird angesteuert
        public bool ENABLE_SYNC { get; set; }	//Freigabe
        public bool IN_SYNC { get; set; }	//Gestartet
        public Int16 CAM_NUMBER { get; set; }	//Kurvennummer (1:mechanische Kurve; 2..:berechnete Kurven)
        public bool EXTEND_VISIBLE { get; set; }	//Erweiterte Parameter anzeigen
        public Int32 X_OFFSET { get; set; }	//X - Offsetwert
        public Int32 Y_OFFSET { get; set; }	//Y - Offsetwert
        public Int16 VELOCITY_PHASE_SHIFT { get; set; }	//Geschwindigkeit für Phasenverschiebung
        public Int16 ACCELERATION_PHASE_SHIFT { get; set; }	//Beschleunigung für Phasenverschiebung
        public Int16 DELAY_PHASE_SHIFT { get; set; }	//Verzögerung für Phasenverschiebung
        public double JERK_PHASE_SHIFT { get; set; }	//Ruck für Phasenverschiebung
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.OUT.TOUCHPROBE")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_TOUCHPROBE
    {
        public bool ENABLE_MOVE { get; set; }	//Freigabe Befehl starten
        public bool ENABLE_EDIT { get; set; }	//Freigabe Werte ändern
        public GROB_DRIVE_SEW_C_EXTEND_TOUCHPROBE_FEEDBACK_UDT PARAMETER { get; set; }
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.OUT.CHANGE_MASTER")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_CHANGE_MASTER
    {
        public bool ENABLE_SYNC { get; set; }	//Freigabe Befehl starten
        public bool ENABLE_EDIT { get; set; }	//Freigabe Werte ändern
        public bool ENABLE_SAVE { get; set; }	//Freigabe Speichern
        public Int16 ACTIVE_MASTER { get; set; }	//Aktiver Master
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.OUT.GEARING")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_GEARING
    {
        public bool ENABLE_GEARING { get; set; }	//Freigabe Befehl starten
        public bool ACTIVE { get; set; }	//Gearing aktiv
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.OUT")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT
    {
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_GENERAL GENERAL { get; set; }	//Allgemein
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_CONF CONF { get; set; }	//Konfiguration
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_CAM CAM { get; set; }	//Positionieren
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_TOUCHPROBE TOUCHPROBE { get; set; }	//Touchprobe
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_CHANGE_MASTER CHANGE_MASTER { get; set; }	//Masterquellenumschaltung
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT_GEARING GEARING { get; set; }	//Gearing
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.IN.GENERAL")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_GENERAL
    {
        public bool OPEN_CAM { get; set; }	//Öffne Kurvenscheiben
        public bool OPEN_TOUCHPROBE { get; set; }	//Öffne Touchprobe
        public bool OPEN_MASTER { get; set; }	//Öffne Masterquellenumschaltung
        public bool OPEN_GEARING { get; set; }	//Öffne Gearing
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.IN.CAM")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_CAM
    {
        public bool COMMAND_SYNC { get; set; }	//Befehl Aufkoppeln
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.IN.TOUCHPROBE")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_TOUCHPROBE
    {
        public bool START { get; set; }	//Start gedrückt
        public bool COMMAND_SAVE { get; set; }	//Speichern gedrückt
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.IN.CHANGE_MASTER")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_CHANGE_MASTER
    {
        public bool START { get; set; }	//Start gedrückt
        public bool COMMAND_SAVE { get; set; }	//Speichern gedrückt
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.IN.GEARING")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_GEARING
    {
        public bool START { get; set; }	//Start gedrückt
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.IN")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN
    {
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_GENERAL GENERAL { get; set; }	//Allgemein
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_CAM CAM { get; set; }	//Positionieren
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_TOUCHPROBE TOUCHPROBE { get; set; }	//Touchprobe
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_CHANGE_MASTER CHANGE_MASTER { get; set; }	//Masterquellenumschaltung
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN_GEARING GEARING { get; set; }	//Gearing
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.INOUT.TOUCHPROBE")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_INOUT_TOUCHPROBE
    {
        public Int16 SELECT_SENSOR { get; set; }	//Angewählter Sensor
        public double OFFSET { get; set; }	//Offset nach Touchprobeereignis
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.INOUT.CHANGE_MASTER")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_INOUT_CHANGE_MASTER
    {
        public Int16 NUMBER { get; set; }	//Nummer
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT.INOUT")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_INOUT
    {
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_INOUT_TOUCHPROBE TOUCHPROBE { get; set; }	//Touchprobe
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_INOUT_CHANGE_MASTER CHANGE_MASTER { get; set; }	//Masterquellenumschaltung
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.CONF", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_CONF
    {
        [NotAccessible(true)]
        public bool RESET { get; set; }	//Konfiguration wird neu gelesen
        [NotAccessible(true)]
        public bool TOUCHPROBE { get; set; }	//Touchprobe
        [NotAccessible(true)]
        public bool CHANGE_MASTER { get; set; }	//Masterquellenumschaltung
        [NotAccessible(true)]
        public Int16 CAMMING { get; set; }	//Kurvenscheiben 0: deaktiviert; 1:Basic; 2:Advanced
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.REQUEST", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_REQUEST
    {
        [NotAccessible(true)]
        public bool MODE_TOUCHPROBE { get; set; }	//Touchprobe
        [NotAccessible(true)]
        public Int16 DOWNLOAD_PARAMETER { get; set; }	//Parameter von HMI runterladen 0:kein Download, 1: Cam; 2: Touchprobe; 3: Master
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.GENERAL", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_GENERAL
    {
        [NotAccessible(true)]
        public bool AX_SYNC { get; set; }	//Sync aktiv
        [NotAccessible(true)]
        public double POSITION { get; set; }	//Position
        [NotAccessible(true)]
        public bool STANDSTILL { get; set; }	//Stillstand
        [NotAccessible(true)]
        public bool MODE_MAN { get; set; }	//Betriebsart Hand
        [NotAccessible(true)]
        public bool ACK_PULSE { get; set; }	//Störung quittieren
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.TOUCHPROBE", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_TOUCHPROBE
    {
        [NotAccessible(true)]
        public double TOLERANCE { get; set; }	//Positioniertoleranz
        [NotAccessible(true)]
        public bool DI04 { get; set; }	//Rückmeldung digitaler Eingang
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.TORQUE.LIMIT", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_TORQUE_LIMIT
    {
        [NotAccessible(true)]
        public bool REDUCE { get; set; }	//Angefordert
        [NotAccessible(true)]
        public bool SET_ERROR { get; set; }	//Status 1 stoppt die Achse bei Überlast
        [NotAccessible(true)]
        public Int16 VALUE { get; set; }	//Prozentualer Wert
        [NotAccessible(true)]
        public bool ACTIVE { get; set; }	//Aktiv
        [NotAccessible(true)]
        public bool WARNING { get; set; }	//Warnung aktiv
        [NotAccessible(true)]
        public bool ERROR { get; set; }	//Störung aktiv
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.TORQUE.MONITORING", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_TORQUE_MONITORING
    {
        [NotAccessible(true)]
        public bool REDUCE { get; set; }	//Angefordert
        [NotAccessible(true)]
        public bool SET_ERROR { get; set; }	//Status 1 stoppt die Achse bei Überlast
        [NotAccessible(true)]
        public Int16 VALUE { get; set; }	//Prozentualer Wert
        [NotAccessible(true)]
        public bool ACTIVE { get; set; }	//Aktiv
        [NotAccessible(true)]
        public bool WARNING { get; set; }	//Warnung aktiv
        [NotAccessible(true)]
        public bool ERROR { get; set; }	//Störung aktiv
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.TORQUE", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_TORQUE
    {
        [NotAccessible(true)]
        public Int16 ACTUAL_VALUE { get; set; }	//Aktueller Wert
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_TORQUE_LIMIT LIMIT { get; set; }	//Drehmomentreduzierung
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_TORQUE_MONITORING MONITORING { get; set; }	//Drehmomentüberwachung
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.MASTER", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_MASTER
    {
        [NotAccessible(true)]
        public bool READY_SYNC { get; set; }	//Freigabe zum Einkoppeln
        [NotAccessible(true)]
        public Int16 NUMBER { get; set; }	//Master
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.GEARING", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_GEARING
    {
        [NotAccessible(true)]
        public bool ENABLE { get; set; }	//Existiert
        [NotAccessible(true)]
        public bool START { get; set; }	//Gearing starten
        [NotAccessible(true)]
        public bool ACTIVE { get; set; }	//Gearing ist aktiv
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe für HMI
        [NotAccessible(true)]
        public bool BUTTON_HMI_PRESSED { get; set; }	//Rückmeldung von HMI
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.CAMMING", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_CAMMING
    {
        [NotAccessible(true)]
        public bool SELECT_MODE { get; set; }	//Mode anwählen
        [NotAccessible(true)]
        public bool MODE_ACTIVE { get; set; }	//Camming ist aktiv
        [NotAccessible(true)]
        public bool SELECT_START { get; set; }	//Camming starten
        [NotAccessible(true)]
        public bool IN_SYNC_ACTIVE { get; set; }	//Achse ist aufgekoppelt - alle anderen Funktionen sperren
        [NotAccessible(true)]
        public bool IN_POS { get; set; }	//Slaveachse ist eingekuppelt und synchron zur Masterachse
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.HMI.INTERFACE", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_HMI_INTERFACE
    {
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_CAM_11PD_PLC_IN_UDT PLC_IN_CAM { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_CAM_11PD_PLC_OUT_UDT PLC_OUT_CAM { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_MASTER_1PD_PLC_IN_UDT PLC_IN_MASTER { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_MASTER_1PD_PLC_OUT_UDT PLC_OUT_MASTER { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_TOUCHPROBE_2PD_PLC_IN_UDT PLC_IN_TOUCHPROBE { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_TOUCHPROBE_2PD_PLC_OUT_UDT PLC_OUT_TOUCHPROBE { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_TORQUE_LIMIT_PLC_IN_UDT PLC_IN_TORQUE { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_TORQUE_LIMIT_PLC_OUT_UDT PLC_OUT_TORQUE { get; set; }
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.HMI.GENERAL", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_HMI_GENERAL
    {
        [NotAccessible(true)]
        public Int16 PAGE_INDEX { get; set; }	//Aktuelles Registerblatt
        [NotAccessible(true)]
        public Int16 LINE_INDEX { get; set; }	//Aktuelle Zeile
        [NotAccessible(true)]
        public UInt16 LANGUAGE { get; set; }	//Aktuelle Sprache für Werkerhinweise
    }

    

    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT.HMI", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT_HMI
    {
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_HMI_INTERFACE INTERFACE { get; set; }	//Schnittstelle
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT DATA { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_HMI_GENERAL GENERAL { get; set; }
    }


    
    [PlcType("GROB_TESTSTOP_SBT_UDT")]
    public class GROB_TESTSTOP_SBT_UDT
    {
        public bool START { get; set; }	//Starten
        public GROB_TESTSTOP_SBT_UDT_CONFIG CONFIG { get; set; }	//Konfiguration
        public GROB_TESTSTOP_SBT_UDT_STATUS STATUS { get; set; }	//Status und Diagnose

    }

    
    [PlcType("GROB_TESTSTOP_SBT_ERROR_UDT")]
    public class GROB_TESTSTOP_SBT_ERROR_UDT
    {
        public bool BRAKETEST_PARAMETER { get; set; }	//Störung: Bremsentest-Parameter
        public bool BRAKETEST_SEQUENCE_FAULT { get; set; }	//Störung: Ablaufstörung Teststopp/Bremsentest
        public bool BRAKETEST_INT_BRAKE { get; set; }	//Störung: Bremsentest interne Bremse fehlgeschlagen
        public bool BRAKETEST_EXT_BRAKE { get; set; }	//Störung: Bremsentest externe Bremse fehlgeschlagen

    }

    
    [PlcType("GROB_TESTSTOP_SBT_POPUP_WARN_UDT")]
    public class GROB_TESTSTOP_SBT_POPUP_WARN_UDT
    {
        public bool DO_NOT_OPEN_SAFEGUARD { get; set; }	//Bremsentest/Teststopp aktiv => Schutzeinrichtung nicht unterbrechen/öffnen
        public bool SAFE_GUARD_NOT_OPEN { get; set; }	//Bremsentest/Teststopp angefordert => Schutzeinrichtung kann nicht geöffnet/entriegelt werden (Leistung ein oder E2 erforderlich)
        public bool BRAKETEST_IMMEDIATELY { get; set; }	//Schutzeinrichtung für Bremsentest/Teststopp schließen oder quittieren
        public bool BRAKETEST_NIO_POWEROFF { get; set; }	//Bremsentest nio => Leistung aus (Bremse/Antrieb austauschen)
        public bool BRAKETEST_ONLY_IN_HOME { get; set; }	//Bremsentest/Teststopp ist nur in Grundstellung möglich

    }

    
    [PlcType("GROB_TESTSTOP_SBT_DATA_UDT", true)]
    public class GROB_TESTSTOP_SBT_DATA_UDT
    {
        [NotAccessible(true)]
        public bool BRAKETEST_REQUEST { get; set; }	//Bremsentest Anforderung
        [NotAccessible(true)]
        public bool BRAKETEST_REQUEST_WITH_PRECONDITIONS { get; set; }	//Bremsentest Anforderung mit Vorbedingungen
        [NotAccessible(true)]
        public bool BRAKETEST_PRECONDITIONS_NOK { get; set; }	//Bremsentest Vorbedingungen NOK
        [NotAccessible(true)]
        public bool BRAKETEST_IMMEDIATELY { get; set; }	//Sofortausführung Bremsentest
        [NotAccessible(true)]
        public bool BRAKETEST_ACTIVE { get; set; }	//Bremsentest ist zur Zeit aktiv
        [NotAccessible(true)]
        public bool SAFEGUARD_BLOCKED { get; set; }	//Öffnen/Entriegeln der Schutzteinrichtung ist geblockt
        [NotAccessible(true)]
        public GROB_TESTSTOP_SBT_ERROR_UDT ERROR { get; set; }
        [NotAccessible(true)]
        public GROB_TESTSTOP_SBT_POPUP_WARN_UDT POPUP { get; set; }
        [NotAccessible(true)]
        public GROB_TESTSTOP_SBT_POPUP_WARN_UDT WARNING { get; set; }

    }

    
    [PlcType("GROB_DRIVE_SEW_C_BASIC_UDT", true)]
    public class GROB_DRIVE_SEW_C_BASIC_UDT
    {
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_GENERAL GENERAL { get; set; }	//Generell
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_JOG JOG { get; set; }	//Joggen
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_REF REF { get; set; }	//Referenzieren
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_SAFE_REF SAFE_REF { get; set; }	//Sicher Referenzieren
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_TEMP_COMP TEMP_COMP { get; set; }	//Temperaturkompensation
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_LOCKING_PIN LOCKING_PIN { get; set; }	//Absteckbolzen
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_BELT BELT { get; set; }	//Riemenbruchkontrolle/Schleppkettenüberwachung
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_OVERRIDE OVERRIDE { get; set; }	//Geschwindigkeit prozentual
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_VEL VEL { get; set; }	//Geschwindigkeit
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_RAMP RAMP { get; set; }	//Rampen
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_SBT SBT { get; set; }	//Teststop
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_SW_LIMIT SW_LIMIT { get; set; }	//Software-Endschalter
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_TORQUE_LIMIT TORQUE_LIMIT { get; set; }	//Drehmomentreduzierung
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_TORQUE_MONITORING TORQUE_MONITORING { get; set; }	//Drehmomentüberwachung
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_POS_DESYNC POS_DESYNC { get; set; }	//Sondermode für Positionierung nach Synchronlauf
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_ERROR ERROR { get; set; }	//Störung - Interner Handshake, bitte nicht benutzen
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT_FEEDBACK_AX FEEDBACK_AX { get; set; }	//Rückmeldung Achse

    }

    
    [PlcType("GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT", true)]
    public class GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT
    {
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT_TOUCHPROBE TOUCHPROBE { get; set; }	//Touchprobe
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT_CHANGE_MASTER CHANGE_MASTER { get; set; }	//Master/Slave
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT_GEARING GEARING { get; set; }	//Gearing

    }

    
    [PlcType("GROB_DRIVE_SEW_C_EXTEND_TOUCHPROBE_FEEDBACK_UDT")]
    public class GROB_DRIVE_SEW_C_EXTEND_TOUCHPROBE_FEEDBACK_UDT
    {
        public bool ACTIVE { get; set; }	//Touchprobesuche hat begonnen 
        public Int16 SELECTED_SENSOR { get; set; }	//Angewählter Sensor (Default 0 wenn keine Sensorumschaltung vorhanden)
        public bool TRIGGER_FOUND { get; set; }	//Aktiv
        public bool TRIGGER_HIGH_FOUND { get; set; }	//Rückmeldung positive Flanke
        public bool TRIGGER_LOW_FOUND { get; set; }	//Rückmeldung negative Flanke
        public bool NO_TRIGGER_DETECTED { get; set; }	//Sollposition errreicht ohne Touchprobeereignis 
        public Int16 TRIGGER_COUNTER { get; set; }	//Triggerzähler, Erhöhng um 1 bei jedem Triggerereignis
        public double VALUE { get; set; }	//Erfasste Achsposition bei Touchprobereignis
        public bool IN_POS_DONE { get; set; }	//Achse auf Position mit Offset

    }

    
    [PlcType("GROB_DRIVE_SEW_C_EXTEND_FEEDBACK_UDT", true)]
    public class GROB_DRIVE_SEW_C_EXTEND_FEEDBACK_UDT
    {
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTEND_TOUCHPROBE_FEEDBACK_UDT TOUCHPROBE { get; set; }
        public GROB_DRIVE_SEW_C_EXTEND_FEEDBACK_UDT_CHANGE_MASTER CHANGE_MASTER { get; set; }	//Masterquellenumschaltung
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTEND_FEEDBACK_UDT_GEARING GEARING { get; set; }	//Gearing

    }

    
    [PlcType("GROB_DRIVE_SEW_C_CAM_11PD_PLC_IN_UDT")]
    public class GROB_DRIVE_SEW_C_CAM_11PD_PLC_IN_UDT
    {
        public bool I_X10_8_RESERVED { get; set; }	//Reserve
        public bool I_X10_9_RESERVED { get; set; }	//Reserve
        public bool I_X10_10_RESERVED { get; set; }	//Reserve
        public bool I_X10_11_RESERVED { get; set; }	//Reserve
        public bool I_X10_12_RESERVED { get; set; }	//Reserve
        public bool I_X10_13_RESERVED { get; set; }	//Reserve
        public bool I_X10_14_RESERVED { get; set; }	//Reserve
        public bool I_X10_15_RESERVED { get; set; }	//Reserve
        public bool I_X10_0_ADJUST_TO_MASTER_ACTIVE { get; set; }	//Adjust to Master aktiv
        public bool I_X10_1_ADJUST_TO_MASTER_DONE { get; set; }	//Adjust to Master abgeschlossen
        public bool I_X10_2_X_OFFSET_ACTIVE { get; set; }	//X - Offsetbearbeitung aktiv (Phasenverschiebung)
        public bool I_X10_3_X_OFFSET_DONE { get; set; }	//X - Offsetbearbeitung abgeschlossen (Phasenverschiebung)
        public bool I_X10_4_Y_OFFSET_ACTIVE { get; set; }	//Y - Offsetbearbeitung aktiv (Amplitudenverschiebung)
        public bool I_X10_5_Y_OFFSET_DONE { get; set; }	//Y - Offsetbearbeitung abgeschlossen (Amplitudenverschiebung)
        public bool I_X10_6_RESERVED { get; set; }	//Reserve
        public bool I_X10_7_RESERVED { get; set; }	//Reserve
        public Int16 I_W11_ACTIVE_CAM { get; set; }	//Aktuelle Kurvennummer / Stützpunkttabelle
        public Int16 I_W12_CAM_STATUS { get; set; }	//Camming - Status
        public Int32 I_W13_W14_Y_OFFSET { get; set; }	//Anwedereinheit
        public Int32 I_W15_W16_X_OFFSET { get; set; }	//Anwedereinheit
        public Int16 I_W17_RESERVED { get; set; }	//Reserve
        public Int16 I_W18_RESERVED { get; set; }	//Reserve
        public Int16 I_W19_RESERVED { get; set; }	//Reserve
        public Int16 I_W20_RESERVED { get; set; }	//Reserve

    }

    
    [PlcType("GROB_DRIVE_SEW_C_CAM_11PD_PLC_OUT_UDT")]
    public class GROB_DRIVE_SEW_C_CAM_11PD_PLC_OUT_UDT
    {
        public bool Q_X10_8_RESERVED { get; set; }	//Reserve
        public bool Q_X10_9_RESERVED { get; set; }	//Reserve
        public bool Q_X10_10_RESERVED { get; set; }	//Reserve
        public bool Q_X10_11_RESERVED { get; set; }	//Reserve
        public bool Q_X10_12_RESERVED { get; set; }	//Reserve
        public bool Q_X10_13_RESERVED { get; set; }	//Reserve
        public bool Q_X10_14_RESERVED { get; set; }	//Reserve
        public bool Q_X10_15_RESERVED { get; set; }	//Reserve
        public bool Q_X10_0_ADJUST_TO_MASTER { get; set; }	//Adjust to Master
        public bool Q_X10_1_START_X { get; set; }	//Starte X - Offsetbearbeitung (Phasenverschiebung)
        public bool Q_X10_2_START_Y { get; set; }	//Starte Y - Offsetbearbeitung (Amplitudenverschiebung)
        public bool Q_X10_3_SET_X { get; set; }	//Setze X - Offset
        public bool Q_X10_4_SET_Y { get; set; }	//Setze Y - Offset
        public bool Q_X10_5_RESERVED { get; set; }	//Reserve
        public bool Q_X10_6_RESERVED { get; set; }	//Reserve
        public bool Q_X10_7_RESERVED { get; set; }	//Reserve
        public Int16 Q_W11_CAM_NUMBER { get; set; }	//Kurvennummer
        public Int16 Q_W12_RESERVED { get; set; }	//Reserve
        public Int32 Q_W13_W14_Y_OFFSET { get; set; }	//Y - Offsetwert
        public Int32 Q_W15_W16_X_OFFSET { get; set; }	//X - Offsetwert
        public Int16 Q_W17_VELOCITY_PHASE_SHIFT { get; set; }	//Geschwindigkeit für Phasenverschiebung
        public Int16 Q_W18_ACCELERATION_PHASE_SHIFT { get; set; }	//Beschleunigung für Phasenverschiebung
        public Int16 Q_W19_DELAY_PHASE_SHIFT { get; set; }	//Verzögerung für Phasenverschiebung
        public Int16 Q_W20_JERK_PHASE_SHIFT { get; set; }	//Ruck für Phasenverschiebung

    }

    
    [PlcType("GROB_DRIVE_SEW_C_MASTER_1PD_PLC_IN_UDT")]
    public class GROB_DRIVE_SEW_C_MASTER_1PD_PLC_IN_UDT
    {
        [AliasName("I_X1_0-7_ACTIVE_MASTER")]
        [SymbolicAccessName("I_X1_0-7_ACTIVE_MASTER")]
        public byte I_X1_0_Minus_7_ACTIVE_MASTER { get; set; }	//Aktuelle Masternummer
        public bool I_X2_0_MASTER_CHANGED { get; set; }	//"1": Master umgeschalten
        public bool I_X2_1_RESERVED { get; set; }	//Reserviert
        public bool I_X2_2_RESERVED { get; set; }	//Reserviert
        public bool I_X2_3_RESERVED { get; set; }	//Reserviert
        public bool I_X2_4_RESERVED { get; set; }	//Reserviert
        public bool I_X2_5_RESERVED { get; set; }	//Reserviert
        public bool I_X2_6_RESERVED { get; set; }	//Reserviert
        public bool I_X2_7_RESERVED { get; set; }	//Reserviert

    }

    
    [PlcType("GROB_DRIVE_SEW_C_MASTER_1PD_PLC_OUT_UDT")]
    public class GROB_DRIVE_SEW_C_MASTER_1PD_PLC_OUT_UDT
    {
        [AliasName("Q_X1_0-7_MASTER_NUMBER")]
        [SymbolicAccessName("Q_X1_0-7_MASTER_NUMBER")]
        public byte Q_X1_0_Minus_7_MASTER_NUMBER { get; set; }	//Masternummer (Zuordnung erfolgt im IEC-Programm)
        public bool Q_X2_0_CHANGE_MASTER { get; set; }	//Master umschalten
        public bool Q_X2_1_RESERVED { get; set; }	//Reserviert
        public bool Q_X2_2_RESERVED { get; set; }	//Reserviert
        public bool Q_X2_3_RESERVED { get; set; }	//Reserviert
        public bool Q_X2_4_RESERVED { get; set; }	//Reserviert
        public bool Q_X2_5_RESERVED { get; set; }	//Reserviert
        public bool Q_X2_6_RESERVED { get; set; }	//Reserviert
        public bool Q_X2_7_RESERVED { get; set; }	//Reserviert

    }

    
    [PlcType("GROB_DRIVE_SEW_C_TOUCHPROBE_2PD_PLC_IN_UDT", true)]
    public class GROB_DRIVE_SEW_C_TOUCHPROBE_2PD_PLC_IN_UDT
    {
        [NotAccessible(true)]
        public bool I_X12_8_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_9_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_10_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_11_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_12_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_13_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_14_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_15_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_0_TOUCHPROBE_ACTIVE { get; set; }	//Funktion aktiviert, Auf Triggerereignis warten
        [NotAccessible(true)]
        public bool I_X12_1_TRIGGER_FOUND { get; set; }	//Trigger aktiviert
        [NotAccessible(true)]
        public bool I_X12_2_NO_TRIGGER_DETECTED { get; set; }	//Kein Trigger innerhalb des Bereiches gefunden
        [NotAccessible(true)]
        public bool I_X12_3_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_4_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_5_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_6_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool I_X12_7_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public Int16 I_W13_TOUCHPROBE_COUNTER { get; set; }	//Triggerzähler, Erhöhung um 1 bei jedem Triggerereignis
        [NotAccessible(true)]
        public Int32 I_W14_W15_DETECTED_VALUE { get; set; }	//Erfasster Wert der Anwendereinheit

    }

    
    [PlcType("GROB_DRIVE_SEW_C_TOUCHPROBE_2PD_PLC_OUT_UDT", true)]
    public class GROB_DRIVE_SEW_C_TOUCHPROBE_2PD_PLC_OUT_UDT
    {
        [NotAccessible(true)]
        public bool Q_X12_8_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_9_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_10_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_11_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_12_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_13_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_14_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_15_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_0_ACTIVATE { get; set; }	//Touchprobe aktivieren
        [NotAccessible(true)]
        public bool Q_X12_1_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_2_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_3_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_4_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_5_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_6_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public bool Q_X12_7_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public UInt16 Q_X13_RESERVED { get; set; }	//Reserve
        [NotAccessible(true)]
        public Int32 Q_W14_W15_OFFSET { get; set; }	//Restwegpositionierung

    }

    
    [PlcType("GROB_DRIVE_SEW_C_TORQUE_LIMIT_PLC_IN_UDT")]
    public class GROB_DRIVE_SEW_C_TORQUE_LIMIT_PLC_IN_UDT
    {
        public bool I_X10_8_RESERVED { get; set; }	//Reserve 8
        public bool I_X10_9_RESERVED { get; set; }	//Reserve 9
        public bool I_X10_10_RESERVED { get; set; }	//Reserve 10
        public bool I_X10_11_RESERVED { get; set; }	//Reserve 11
        public bool I_X10_12_RESERVED { get; set; }	//Reserve 12
        public bool I_X10_13_RESERVED { get; set; }	//Reserve 13
        public bool I_X10_14_RESERVED { get; set; }	//Reserve 14
        public bool I_X10_15_RESERVED { get; set; }	//Reserve 15
        public bool I_X10_0_TORQUE_LIMIT_PD { get; set; }	//Drehmomentgrenze über PD aktive
        public bool I_X10_1_MONITORING_INACTIVE { get; set; }	//Drehzahl- und Schleppfehlerüberwachung
        public bool I_X10_2_TORQUE_LIMIT_REACHED { get; set; }	//Drehmomentgrenze erreicht
        public bool I_X10_3_SPEED_MONITORING_INACTIVE { get; set; }	//Drehzahlüberwachung inaktiv
        public bool I_X10_4_RESERVED { get; set; }	//Reserve 4
        public bool I_X10_5_RESERVED { get; set; }	//Reserve 5
        public bool I_X10_6_RESERVED { get; set; }	//Reserve 6
        public bool I_X10_7_RESERVED { get; set; }	//Reserve 7
        public Int16 I_W11_ACTUAL_TORQUE { get; set; }	//Istdrehmoment [0,1%]

    }

    
    [PlcType("GROB_DRIVE_SEW_C_TORQUE_LIMIT_PLC_OUT_UDT")]
    public class GROB_DRIVE_SEW_C_TORQUE_LIMIT_PLC_OUT_UDT
    {
        public bool Q_X10_8_RESERVED { get; set; }	//Reserve 8
        public bool Q_X10_9_RESERVED { get; set; }	//Reserve 9
        public bool Q_X10_10_RESERVED { get; set; }	//Reserve 10
        public bool Q_X10_11_RESERVED { get; set; }	//Reserve 11
        public bool Q_X10_12_RESERVED { get; set; }	//Reserve 12
        public bool Q_X10_13_RESERVED { get; set; }	//Reserve 13
        public bool Q_X10_14_RESERVED { get; set; }	//Reserve 14
        public bool Q_X10_15_RESERVED { get; set; }	//Reserve 15
        public bool Q_X10_0_ACTIVATE_TORQUE_LIMIT { get; set; }	//Drehmomentgrenze über PD aktivieren
        public bool Q_X10_1_DEACTIVATE_MONITORING { get; set; }	//Drehzahl- und Schleppfehlerüberwachung deaktivieren
        public bool Q_X10_2_RESERVED { get; set; }	//Reserve 2
        public bool Q_X10_3_DEACTIVATE_SPEED_MONITORING { get; set; }	//Deaktivieren der Drehzahlüberwachung
        public bool Q_X10_4_RESERVED { get; set; }	//Reserve 4
        public bool Q_X10_5_RESERVED { get; set; }	//Reserve 5
        public bool Q_X10_6_RESERVED { get; set; }	//Reserve 6
        public bool Q_X10_7_RESERVED { get; set; }	//Reserve 7
        public Int16 Q_W11_SET_MAXIMUM_TORQUE { get; set; }	//Maximum torque

    }

    
    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT")]
    public class GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT
    {
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_OUT OUT { get; set; }	//An HMI
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_IN IN { get; set; }	//Von HMI
        public GROB_DRIVE_SEW_C_EXTENDS_DATA_HMI_UDT_INOUT INOUT { get; set; }	//Handshake

    }

    
    [PlcType("GROB_DRIVE_SEW_C_EXTENDS_UDT", true)]
    public class GROB_DRIVE_SEW_C_EXTENDS_UDT
    {
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_CONF CONF { get; set; }	//Konfiguration
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_REQUEST REQUEST { get; set; }	//Anfoderungen
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_GENERAL GENERAL { get; set; }	//Generell
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_TOUCHPROBE TOUCHPROBE { get; set; }	//Touchprobe
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_TORQUE TORQUE { get; set; }	//Drehmoment
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_MASTER MASTER { get; set; }	//Master/Slave
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_GEARING GEARING { get; set; }	//Gearing
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_CAMMING CAMMING { get; set; }	//Kurvenscheiben
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTEND_COMMAND_UDT COMMAND_EXTENDS { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTEND_FEEDBACK_UDT FEEDBACK_EXTENDS { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT_HMI HMI { get; set; }	//HMI Anzeige

    }

    
    [PlcType("GROB_DRIVE_SEW_C_POS_CMD_UDT", true)]
    public class GROB_DRIVE_SEW_C_POS_CMD_UDT
    {
        [NotAccessible(true)]
        public bool ENABLE { get; set; }	//Programm freigegeben
        [NotAccessible(true)]
        public bool ANY_TYPE { get; set; }	//Position Werkstücktyp unabhängig
        [NotAccessible(true)]
        public byte MODE { get; set; }	//1=Absolut 2 = Relativ
        [NotAccessible(true)]
        public bool MOVE { get; set; }	//Position anfordern
        [NotAccessible(true)]
        public bool ILOCK { get; set; }	//Verriegelung
        [NotAccessible(true)]
        public Int16 ILOCK_ADD { get; set; }	//Interne Laufvariable für mehr Interlock FCs
        [NotAccessible(true)]
        public bool ACTIVE_LOCKED { get; set; }	//Ausführung durch Verriegelung gesperrt
        [NotAccessible(true)]
        public bool ACTIVE { get; set; }	//Wird ausgeführt
        [NotAccessible(true)]
        public bool IN_POS_ABS { get; set; }	//Achse ist in Position ( Absolut)
        [NotAccessible(true)]
        public bool IN_POS_REL { get; set; }	//Achse ist in Position ( Relativ)
        [NotAccessible(true)]
        public bool IN_DYN_SPEED { get; set; }	//Achse ist in Bereich für Geschwindigkeitsänderung
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//HMI freigeben
        [NotAccessible(true)]
        public byte EXTEND { get; set; }	//Parametererweiterung
        [NotAccessible(true)]
        public UInt16 DIAG_ID { get; set; }	//Stördiagnose weiterleiten
        [NotAccessible(true)]
        public UInt16 DIAG_INFO { get; set; }	//Werkerhinweis
        [NotAccessible(true)]
        public bool TARGET_POS_PHASE { get; set; }	//Zielposition in der Phase

    }

    
    [PlcType("GROB_DRIVE_SEW_C_POS_BASIC_UDT", true)]
    public class GROB_DRIVE_SEW_C_POS_BASIC_UDT
    {

        [ArrayBounds(0,32,0)]
        public double[] POSITION { get; set; }	//Zielposition
        [NotAccessible(true)]
        public double TOLERANCE { get; set; }	//Zieltoleranz (+/-Position)

    }

    
    [PlcType("GROB_DRIVE_SEW_C_POS_EXTEND_UDT", true)]
    public class GROB_DRIVE_SEW_C_POS_EXTEND_UDT
    {
        [NotAccessible(true)]
        public double VELOCITY { get; set; }	//Geschwindigkeit [LU/min]
        [NotAccessible(true)]
        public Int16 ACCELERATION { get; set; }	//Beschleunigung [LU/s^2]
        [NotAccessible(true)]
        public Int16 DECELERATION { get; set; }	//Verzögerung [LU/s^2]
        [NotAccessible(true)]
        public Int16 DIRECTION { get; set; }	//Richtung abs (1= positive Richtung, 2= negative Richtung, 3= kürzeseter Weg)  rel(1)
        [NotAccessible(true)]
        public double JERK { get; set; }	//Vorgegebene Ruckzeit in [ms]
        [NotAccessible(true)]
        public bool POS_CHANGE_ON_THE_FLY { get; set; }	//kontinuierliche Sollwertübernahme Position - Nur Positionierachse
        [NotAccessible(true)]
        public bool VEL_CHANGE_ON_THE_FLY { get; set; }	//kontinuierliche Sollwertübernahme Geschwindigkeit
        [NotAccessible(true)]
        public bool DYN_SPEED_ACT { get; set; }	//Geschwindigkeitsumschaltung freigegeben
        [NotAccessible(true)]
        public double DYN_SPEED_VALUE { get; set; }	//Geschwindigkeitsumschaltung Wert
        [NotAccessible(true)]
        public double DYN_SPEED_AREA_LOW { get; set; }	//Geschwindigkeitsumschaltung Bereich unten
        [NotAccessible(true)]
        public double DYN_SPEED_AREA_UP { get; set; }	//Geschwindigkeitsumschaltung Bereich oben

    }

    
    [PlcType("GROB_DRIVE_SEW_C_AREA_CMD_UDT", true)]
    public class GROB_DRIVE_SEW_C_AREA_CMD_UDT
    {
        [NotAccessible(true)]
        public bool ENABLE { get; set; }	//Programm freigegeben
        [NotAccessible(true)]
        public bool ANY_TYPE { get; set; }	//Bereich Werkstücktyp unabhängig
        [NotAccessible(true)]
        public bool IN_AREA { get; set; }	//In Bereich
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//HMI freigeben

    }

    
    [PlcType("GROB_DRIVE_SEW_C_AREA_BASIC_UDT", true)]
    public class GROB_DRIVE_SEW_C_AREA_BASIC_UDT
    {
        [NotAccessible(true)]

        [ArrayBounds(0,32,0)]
        public double[] UP { get; set; }	//Bereich Obergrenze
        [NotAccessible(true)]

        [ArrayBounds(0,32,0)]
        public double[] LOW { get; set; }	//Bereich Untergrenze

    }

    
    [PlcType("GROB_DRIVE_SEW_C_CAM_CMD_UDT", true)]
    public class GROB_DRIVE_SEW_C_CAM_CMD_UDT
    {
        [NotAccessible(true)]
        public bool ENABLE { get; set; }	//Freigabe
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//Freigabe für HMI
        [NotAccessible(true)]
        public bool ILOCK { get; set; }	//Verriegelung
        [NotAccessible(true)]
        public Int16 ILOCK_ADD { get; set; }	//Interne Laufvariable für mehr Interlock FCs
        [NotAccessible(true)]
        public UInt16 DIAG_ID { get; set; }	//Stördiagnose weiterleiten
        [NotAccessible(true)]
        public UInt16 DIAG_INFO { get; set; }	//Werkerhinweis
        [NotAccessible(true)]
        public bool ACTIVE_LOCKED { get; set; }	//Ausführung durch Verriegelung gesperrt
        [NotAccessible(true)]
        public bool ADJUST_TO_MASTER { get; set; }	//Wird vorbereitet (Rückmeldung)
        [NotAccessible(true)]
        public bool IN_SYNC { get; set; }	//Synchronisation aktiv (Rückmeldung)
        [NotAccessible(true)]
        public bool SYNC { get; set; }	//Aufsynchronisieren (Startbefehl)
        [NotAccessible(true)]
        public bool DESYNC { get; set; }	//Absynchronisieren (Stoppbefehl)

    }

    
    [PlcType("GROB_DRIVE_SEW_C_CAMMING_BASIC_UDT", true)]
    public class GROB_DRIVE_SEW_C_CAMMING_BASIC_UDT
    {
        [NotAccessible(true)]
        public Int16 CAM_NUMBER { get; set; }	//Kurvennummer (1:mechanische Kurve; 2..:berechnete Kurven)
        [NotAccessible(true)]
        public Int16 ADVANCED { get; set; }	//Erweiterte Parameter

    }

    
    [PlcType("GROB_DRIVE_SEW_C_CAMMING_ADVANCED_UDT", true)]
    public class GROB_DRIVE_SEW_C_CAMMING_ADVANCED_UDT
    {
        [NotAccessible(true)]
        public bool START_X { get; set; }	//Starte X - Offsetbearbeitung (Phasenverschiebung)
        [NotAccessible(true)]
        public bool START_Y { get; set; }	//Starte Y - Offsetbearbeitung (Amplitudenverschiebung)
        [NotAccessible(true)]
        public bool SET_X { get; set; }	//Setze X - Offset
        [NotAccessible(true)]
        public bool SET_Y { get; set; }	//Setze Y - Offset
        [NotAccessible(true)]
        public Int32 X_OFFSET { get; set; }	//X - Offsetwert
        [NotAccessible(true)]
        public Int32 Y_OFFSET { get; set; }	//Y - Offsetwert
        [NotAccessible(true)]
        public Int16 VELOCITY_PHASE_SHIFT { get; set; }	//Geschwindigkeit für Phasenverschiebung
        [NotAccessible(true)]
        public Int16 ACCELERATION_PHASE_SHIFT { get; set; }	//Beschleunigung für Phasenverschiebung
        [NotAccessible(true)]
        public Int16 DELAY_PHASE_SHIFT { get; set; }	//Verzögerung für Phasenverschiebung
        [NotAccessible(true)]
        public Int16 JERK_PHASE_SHIFT { get; set; }	//Ruck für Phasenverschiebung

    }

    
    [PlcType("GROB_DRIVE_SEW_C_VEL_CMD_UDT", true)]
    public class GROB_DRIVE_SEW_C_VEL_CMD_UDT
    {
        [NotAccessible(true)]
        public bool ENABLE { get; set; }	//Programm freigegeben
        [NotAccessible(true)]
        public bool ANY_TYPE { get; set; }	//Geschwindigkeit Werkstücktyp unabhängig
        [NotAccessible(true)]
        public bool MOVE { get; set; }	//Geschwindigkeit anfordern
        [NotAccessible(true)]
        public bool ILOCK { get; set; }	//Verriegelung
        [NotAccessible(true)]
        public bool ACTIVE_LOCKED { get; set; }	//Ausführung durch Verriegelung gesperrt
        [NotAccessible(true)]
        public bool ACTIVE { get; set; }	//Wird ausgeführt
        [NotAccessible(true)]
        public bool IN_VEL { get; set; }	//Achse ist auf Zielgeschwindigkeit
        [NotAccessible(true)]
        public bool ENABLE_HMI { get; set; }	//HMI freigeben
        [NotAccessible(true)]
        public byte EXTEND { get; set; }	//Parametererweiterung
        [NotAccessible(true)]
        public UInt16 DIAG_ID { get; set; }	//Stördiagnose weiterleiten
        [NotAccessible(true)]
        public UInt16 DIAG_INFO { get; set; }	//Werkerhinweis
        [NotAccessible(true)]
        public Int16 ILOCK_ADD { get; set; }	//Interne Laufvariable für mehr Interlock FCs

    }

    
    [PlcType("GROB_DRIVE_SEW_C_VEL_BASIC_UDT", true)]
    public class GROB_DRIVE_SEW_C_VEL_BASIC_UDT
    {
        [NotAccessible(true)]

        [ArrayBounds(0,32,0)]
        public double[] VEL { get; set; }	//Zielgeschwindigkeit
        [NotAccessible(true)]
        public double TOLERANCE { get; set; }	//Zieltoleranz (+/- Geschwindigkeit)
        [NotAccessible(true)]
        public Int16 DIRECTION { get; set; }	//Richtung (1= positive Richtung, 2= negative Richtung)

    }

    
    [PlcType("GROB_DRIVE_SEW_C_VEL_EXTEND_UDT", true)]
    public class GROB_DRIVE_SEW_C_VEL_EXTEND_UDT
    {
        [NotAccessible(true)]
        public Int16 ACCELERATION { get; set; }	//Beschleunigung [LU/s^2]
        [NotAccessible(true)]
        public Int16 DECELERATION { get; set; }	//Verzögerung [LU/s^2]
        [NotAccessible(true)]
        public double JERK { get; set; }	//Ruck
        [NotAccessible(true)]
        public bool VEL_CHANGE_ON_THE_FLY { get; set; }	//kontinuierliche Sollwertübernahme Geschwindigkeit

    }

    [Mapping("++ST641+ZY001-MA71_PDB", "DB114", 0)]
    public class ___ST641_ZY001_MA71_PDB
    {
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_BASIC_UDT BASIC { get; set; }
        [NotAccessible(true)]
        public GROB_DRIVE_SEW_C_EXTENDS_UDT EXTENDS { get; set; }
        [NotAccessible(true)]

        [ArrayBounds(0, 8, 0)]
        public GROB_DRIVE_SEW_C_POS_CMD_UDT[] POS_CMD { get; set; }

        [ArrayBounds(0, 8, 0)]
        public GROB_DRIVE_SEW_C_POS_BASIC_UDT[] POS_BASIC { get; set; }

        [ArrayBounds(0, 1, 0)]
        public GROB_DRIVE_SEW_C_POS_EXTEND_UDT[] POS_EXTEND { get; set; }
        [NotAccessible(true)]

        [ArrayBounds(0, 1, 0)]
        public GROB_DRIVE_SEW_C_AREA_CMD_UDT[] AREA_CMD { get; set; }

        [ArrayBounds(0, 1, 0)]
        public GROB_DRIVE_SEW_C_AREA_BASIC_UDT[] AREA_BASIC { get; set; }
        [NotAccessible(true)]

        [ArrayBounds(0, 1, 0)]
        public GROB_DRIVE_SEW_C_CAM_CMD_UDT[] CAM_CMD { get; set; }
        [NotAccessible(true)]

        [ArrayBounds(0, 1, 0)]
        public GROB_DRIVE_SEW_C_CAMMING_BASIC_UDT[] CAM_BASIC { get; set; }
        [NotAccessible(true)]

        [ArrayBounds(0, 1, 0)]
        public GROB_DRIVE_SEW_C_CAMMING_ADVANCED_UDT[] CAM_EXTEND { get; set; }
        [NotAccessible(true)]

        [ArrayBounds(0, 1, 0)]
        public GROB_DRIVE_SEW_C_VEL_CMD_UDT[] VEL_CMD { get; set; }

        [ArrayBounds(0, 1, 0)]
        public GROB_DRIVE_SEW_C_VEL_BASIC_UDT[] VEL_BASIC { get; set; }

        [ArrayBounds(0, 1, 0)]
        public GROB_DRIVE_SEW_C_VEL_EXTEND_UDT[] VEL_EXTEND { get; set; }

    }


}

