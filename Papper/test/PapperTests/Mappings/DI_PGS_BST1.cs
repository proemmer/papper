using Papper.Attributes;
using System;

namespace Insite.Customer.Data.DI_PGS_BST1
{
    
    


    

    [PlcType("UDT_MFU_PGS.MFU_Data")]
    public class UDT_MFU_PGS_MFU_Data
    {
        public Int16 SF_selected { get; set; }	//ausgewählter Schraubfall

        [ArrayBounds(1,32,0)]
        public Int16[] SF { get; set; }	//SchraubFälle je Kanal
    }

    

    [PlcType("UDT_Programm_PGS.Bauteil.Programm")]
    public class UDT_Programm_PGS_Bauteil_Programm
    {
        [ArrayBounds(1,2,0)]
        public Char[] Prg_Name { get; set; }	//Programmname
        public Int16 Anz_verschr { get; set; }	//Anzahl Verschraubungen
    }

    

    [PlcType("UDT_Programm_PGS.Bauteil")]
    public class UDT_Programm_PGS_Bauteil
    {
        
        [StringLength(60)]
        public string Bauteilbez { get; set; }	//Bezeichnung
        public Int16 Kanal_Nr { get; set; }	//Nummer des Schraubkanals
        public Int16 steckplatz { get; set; }	//Nussnummer
        public Int16 Schraubreihenfolge { get; set; }	//Schraubreihenfolge
        public Int16 SF { get; set; }	//Schraubfall für Nuss
        public Int16 Loeseschraubfall { get; set; }	//Schraubfall lösen Global

        [ArrayBounds(1,64,0)]
        public UDT_Programm_PGS_Bauteil_Programm[] Programm { get; set; }
    }

    


    [PlcType("UDT_Intf_WsPgs.SP.WStoELEM")]
    public class UDT_Intf_WsPgs_SP_WStoELEM
    {
        public bool WS_available { get; set; }	//Werkerführung vorhanden
        public bool Active { get; set; }	//Aktuell Aktiver Schritt
        public bool Release { get; set; }	//freigabe Bearbeitung
        public bool Finished { get; set; }	//Bearbeitung beendet
        public bool Reset { get; set; }	//Reset
        public bool ManOK { get; set; }	//Hand IO von HMI
        public byte StepNo { get; set; }	//Schrittnummer
    }

    

    [PlcType("UDT_Intf_WsPgs.SP.ELEMtoWS")]
    public class UDT_Intf_WsPgs_SP_ELEMtoWS
    {
        public bool Available { get; set; }	//Element vorhanden
        public bool DeSelection { get; set; }	//Abwahl von Element
        public bool InPosition { get; set; }	//Element in Position
        public bool InProcess { get; set; }	//Element im Bearbeitung
        public bool Finished { get; set; }	//Bearbeitung beendet
        public byte Info { get; set; }	//optionale ZusatzInfo -> Kanal Nummer des Elements
        public UDT_Ergebnis Result { get; set; }
    }

    

    [PlcType("UDT_Intf_WsPgs.SP")]
    public class UDT_Intf_WsPgs_SP
    {
        public UDT_Intf_WsPgs_SP_WStoELEM WStoELEM { get; set; }	//von Werkerführung
        public UDT_Intf_WsPgs_SP_ELEMtoWS ELEMtoWS { get; set; }	//zu Werkerführung
    }

    




    
    [PlcType("UDT_AI_PGS")]
    public class UDT_AI_PGS
    {
        public Int16 NussAkt { get; set; }	//aktuell entnommene Nuss
        public Int16 Kanal { get; set; }
        public Int16 Anz_versch { get; set; }	//Anzahl offener Verschraubunngen
        public bool ausgew { get; set; }	//ausgewählt an ProTool
        public bool vorhanden { get; set; }	//PGS_vorhanden
        public bool fertig { get; set; }	//PGS_Fertig
        public bool Rueckm_Hand { get; set; }	//Hand
        public bool Loesen { get; set; }	//Loesen
        public bool Ges_ende { get; set; }	//Gesamt_ende manuell
        public bool SD_Senden { get; set; }	//Anstoss Solldaten senden
        public bool SD_gesendet { get; set; }	//Solldaten gesendet
        public bool Hand { get; set; }	//Anwahl Hand
        public bool RM_Einzel_IO { get; set; }	//Rückmeldung Einzel-IO
        public bool RM_Einzel_NIO { get; set; }	//Rückmeldung Einzel-NIO
        public bool RM_Fertig { get; set; }	//Rückmeldung Fertig
        public bool RM_HIO { get; set; }	//Rückmeldung HIO
        public bool RM_Gesamt_IO { get; set; }	//Rückmeldung Gesamt IO
        public bool RM_Gesamt_NIO { get; set; }	//Rückmeldung Gesamt NIO
        public bool Start_schrauben { get; set; }	//Start_Schrauben von hand auslösen
        public Int16 NussSoll { get; set; }	//zu entnehmende Nuss

        [ArrayBounds(1,16,0)]
        public bool[] Status { get; set; }
        
        [StringLength(60)]
        public string BtInfo { get; set; }	//Bauteil Information

    }

    
    [PlcType("UDT_PGS_Anw")]
    public class UDT_PGS_Anw
    {
        public bool Hand { get; set; }
        public bool Ges_ende { get; set; }
        public bool loesen { get; set; }
        public bool Dat_send { get; set; }
        public bool MFU { get; set; }
        public bool DatSendPwRequired { get; set; }	//Passwort für Daten senden erforderlich

    }

    
    [PlcType("UDT_MFU_PGS")]
    public class UDT_MFU_PGS
    {
        public UDT_MFU_PGS_MFU_Data MFU_Data { get; set; }	//Daten für MFU Betrieb PGS

    }

    

    
    [PlcType("UDT_Schrauber_In_Allg")]
    public class UDT_Schrauber_In_Allg
    {
        public bool Life { get; set; }	//Lifsignal der Schraubtechnik (getaktete Impulse 1Hz)
        public bool Neustart_durchgefuehrt { get; set; }	//System Neustart wurde durchgeführt bzw. erwacht aus Sleep Mode
        public bool Reserve_02 { get; set; }	//Res.
        public bool Reserve_03 { get; set; }	//Res.
        public bool Reserve_04 { get; set; }	//Res.
        public bool Reserve_05 { get; set; }	//Res.
        public bool Reserve_06 { get; set; }	//Res.
        public bool Reserve_07 { get; set; }	//Res.
        public byte Reserve_1 { get; set; }	//Reservebyte
        public bool ERR_01 { get; set; }	//Fehler: Störung Feldbus Master
        public bool ERR_02 { get; set; }	//Fehler: 24V Spannungsversorgung
        public bool ERR_03 { get; set; }	//Fehler: Spannungsversorgung Kühlung
        public bool ERR_04 { get; set; }	//Fehler: Übertemperatur
        public bool ERR_05 { get; set; }	//Fehler: Parameter Master
        public bool ERR_06 { get; set; }	//Fehler: Not-Aus
        public bool ERR_07 { get; set; }	//Fehler: Sicherung Anlage NIO
        public bool ERR_08 { get; set; }	//Fehler: Systemfehler (z.B. Zugriff auf Typdatei, Offsettabelle usw.)
        public bool ERR_09 { get; set; }	//Fehler: Kommunikationsfehler Master<>Schraubmodul
        public bool ERR_10 { get; set; }	//Fehler: Reserve
        public bool ERR_11 { get; set; }	//Fehler: Reserve
        public bool ERR_12 { get; set; }	//Fehler: Reserve
        public bool ERR_13 { get; set; }	//Fehler: Reserve
        public bool ERR_14 { get; set; }	//Fehler: Reserve
        public bool ERR_15 { get; set; }	//Fehler: Reserve
        public bool ERR_16 { get; set; }	//Fehler: Reserve
        public bool MSG_01 { get; set; }	//Warnung: Spannungsversorgung Mastersystem
        public bool MSG_02 { get; set; }	//Warnung: Übertemperatur Master
        public bool MSG_03 { get; set; }	//Warnung: Lüfter Master
        public bool MSG_04 { get; set; }	//Warnung: Pufferbatterie Master
        public bool MSG_05 { get; set; }	//Warnung: Spannungsversorgung Mastersystem
        public bool MSG_06 { get; set; }	//Warnung: Temperatur Schaltschrank
        public bool MSG_07 { get; set; }	//Warnung: Harddisk
        public bool MSG_08 { get; set; }	//Warnung: Reserve
        public bool MSG_09 { get; set; }	//Warnung: Reserve
        public bool MSG_10 { get; set; }	//Warnung: Reserve
        public bool MSG_11 { get; set; }	//Warnung: Reserve
        public bool MSG_12 { get; set; }	//Warnung: Reserve
        public bool MSG_13 { get; set; }	//Warnung: Reserve
        public bool MSG_14 { get; set; }	//Warnung: Reserve
        public bool MSG_15 { get; set; }	//Warnung: Reserve
        public bool MSG_16 { get; set; }	//Warnung: Reserve
        public Int16 Diag_Code { get; set; }	//Diagnose Code (kann für Systemfehler verwendet werden)

    }

    
   

    
    [PlcType("UDT_Schrauber_Input")]
    public class UDT_Schrauber_Input
    {
        public UDT_Schrauber_In_Allg Allg { get; set; }

        [ArrayBounds(1,32,0)]
        public UDT_Schrauber_In_Kanal[] Kanal { get; set; }

    }

    
    [PlcType("UDT_Schrauber_Out_Allg")]
    public class UDT_Schrauber_Out_Allg
    {
        public bool Life_Bit { get; set; }	//Life Signal der SPS (getaktete Impulse 1HZ)
        public bool Neustart { get; set; }	//Bootanforderung an Schraubersteuereung
        public bool Anlage_Ein { get; set; }	//Bei Sichtbeginn Anlage Ein=1
        public bool USV_ShutDownPC { get; set; }	//Bei 1 Signal runterfahren des Schraubrechners (wenn USV NIO)
        public bool Reserve_04 { get; set; }	//Res.
        public bool Reserve_05 { get; set; }	//Res.
        public bool Reserve_06 { get; set; }	//Res.
        public bool Reserve_07 { get; set; }	//Res.
        public byte Reserve_1 { get; set; }	//Res.

    }

    


    
    [PlcType("UDT_Schrauber_output")]
    public class UDT_Schrauber_output
    {
        public UDT_Schrauber_Out_Allg Allg { get; set; }

        [ArrayBounds(1,32,0)]
        public UDT_Schrauber_Out_Kanal[] Kanal { get; set; }

    }

    


    
    [PlcType("UDT_Ergebnis")]
    public class UDT_Ergebnis
    {
        public bool IO { get; set; }	//IO -> Bearbeitung
        public bool NIO { get; set; }	//NIO -> keine Bearbeitung
        public bool ABGW { get; set; }	//Abgewählt
        public bool SIO { get; set; }	//Teil oder komplette Abwahl A-Station
        public bool HIO { get; set; }	//Motor von Hand IO gesetzt
        public bool ZIO { get; set; }	//1.Schraubversuch NIO -> 2.Schr.Versuch IO
        public bool RIO { get; set; }	//IO Repariert
        public bool AIO { get; set; }	//Alternativ Sachnummer IO
        public bool DLF { get; set; }	//IO Ohne Bearbeitung
        public bool EIO { get; set; }	//IO mit Eingriff
        public bool WI { get; set; }	//Winkelfehler
        public bool MOM { get; set; }	//Momentfehler
        public bool MF { get; set; }	//Montagefehler
        public bool SF { get; set; }	//Schraubfehler
        public bool mit_Bearbeitung { get; set; }	//Dieses Montage/Schraubergebnis soll ausgewertet werden.

    }

    
    [PlcType("UDT_Intf_WsPgs")]
    public class UDT_Intf_WsPgs
    {
        public bool WS_available { get; set; }	//Werkerführung vorhanden
        public bool WS_Release { get; set; }	//freigabe Bearbeitung
        public bool WS_Finished { get; set; }	//Werkerführung fertig
        public Int16 LastArrayElem { get; set; }	//Letztes ArrayElement

        [ArrayBounds(100,199,0)]
        public UDT_Intf_WsPgs_SP[] SP { get; set; }	//Schrauber

    }
	
		    [PlcType("UDT_Programm_PGS")]
    public class UDT_Programm_PGS
    {

        [ArrayBounds(1,99,0)]
        public UDT_Programm_PGS_Bauteil[] Bauteil { get; set; }

    }
	
	
	
	
    [PlcType("UDT_Konfig_PGS.Kanal")]
    public class UDT_Konfig_PGS_Kanal
    {
        public bool aktiv { get; set; }	//Kanal aktiv
        public bool StandBy { get; set; }	//Kanal ist ein StandBy  (AMT: anderes SD Telegramm)
        public bool Start_setzend { get; set; }	//Start Taster setzend (einbauschrauber)
        public Int16 Nr { get; set; }	//Kanal Nummer -> bei A-Station irrelevant

        [ArrayBounds(1,18,0)]
        public byte[] Reserve { get; set; }
    }
	
	

    	
    [PlcType("FB_SS_PGS.i_ipm_dat")]
    public class FB_SS_PGS_i_ipm_dat
    {
        [ArrayBounds(1,12,0)]
        public Char[] ID { get; set; }	//Identifikationsdaten des Werkstücks -> FG-Nr, Motornummer etc. IPM-> IDENT

        [ArrayBounds(1,2,0)]
        public Char[] Seite { get; set; }	//Identifikationsdaten für "links","rechts","unten","oben" etc. IPM-> Zusatzinfo

        [ArrayBounds(1,4,0)]
        public Char[] WT_NR { get; set; }	//Werkstückträgernummer , STS-Nummer etc. IPM-> Bauteilträger-ID

        [ArrayBounds(1,2,0)]
        public Char[] Typbezeichnung { get; set; }	//Typbezeichnung für Handschraubsysteme IPM-> Typ
    }

    

    [PlcType("FB_PGS.i_ipm_dat")]
    public class FB_PGS_i_ipm_dat
    {
        [ArrayBounds(1,12,0)]
        public Char[] ID { get; set; }	//Identifikationsdaten des Werkstücks -> FG-Nr, Motornummer etc. IPM-> IDENT

        [ArrayBounds(1,2,0)]
        public Char[] Seite { get; set; }	//Identifikationsdaten für "links","rechts","unten","oben" etc. IPM-> Zusatzinfo

        [ArrayBounds(1,4,0)]
        public Char[] WT_NR { get; set; }	//Werkstückträgernummer , STS-Nummer etc. IPM-> Bauteilträger-ID

        [ArrayBounds(1,2,0)]
        public Char[] Typbezeichnung { get; set; }	//Typbezeichnung für Handschraubsysteme IPM-> Typ
    }

        [PlcType("UDT_NUKA.SchraubPunkt")]
    public class UDT_NUKA_SchraubPunkt
    {
        public Int16 Steckplatz { get; set; }	//Zuordnung Steckplatz
        public Int16 SF { get; set; }	//Schraubfall
        public Int16 SF_loesen { get; set; }	//Löseschraubfall
        public Int16 Anz_verschr { get; set; }	//Anzahl Soll verschraubungen für Nuss
        public Int16 Anz_off_Verschr { get; set; }	//Anzahl offener Verschraubungen
        public bool fertig { get; set; }	//Fertig
        public bool IO_verschraubt { get; set; }	//IO Verschraubt
        public bool Reset { get; set; }	//Rücksetzen
        public bool ManOK { get; set; }	//Hand IO
    }




    [PlcType("UDT_NUKA.Steckplatz")]
    public class UDT_NUKA_Steckplatz
    {
        public Int16 Schraubpunkt { get; set; }	//Zuordnung Schraubpunkt
        public Int16 SF { get; set; }	//Schraubfall
        public Int16 SF_loesen { get; set; }	//Löseschraubfall
        public Int16 Anz_verschr { get; set; }	//Anzahl Soll verschraubungen für Nuss
        public Int16 Schraubreihenfolge { get; set; }
        public Int16 Anz_off_Verschr { get; set; }	//Anzahl offener Verschraubungen
        public bool Nuss_entnommen { get; set; }	//Nuss entnommen
        public bool Nuss_fertig { get; set; }	//Nuss fertig
        public bool belegt { get; set; }	//Nuss auf Steckplatz vorhanden
        public bool gesperrt { get; set; }	//Steckplatz gesperrt
        public bool IO_verschraubt { get; set; }	//Steckplatz IO Verschraubt
        public bool Reset { get; set; }	//Rücksetzen
        public bool Alle_SchraubPkt_fertig { get; set; }	//alle Schraubpunkte dieser Nuss sind fertig
    }

	[PlcType("UDT_NUKA_PoolConfig.Nuka")]
    public class UDT_NUKA_PoolConfig_Nuka
    {
        public bool assigned { get; set; }	//einer BST zugeordnet
        public bool disabled { get; set; }	//nicht vorhanden -> beschalten von extern
        public bool isvirtual { get; set; }	//virtueller Nuka, kein PN Slave -> beschalten von extern
        public Int16 IO_Adr { get; set; }	//E/A Adresse (bei virtuell -> beschalten von extern)
        public Int16 slaveAdr { get; set; }	//Slave Adresse (bei virtuell -> beschalten von extern)
        public Int16 cntSockets { get; set; }	//Anzahl Nussen (bei virtuell -> beschalten von extern)
        public Int16 actBst { get; set; }	//Aktuell zugeordnete BST
    }

    

    [PlcType("UDT_NUKA_BSTConfig.BstNukaNo")]
    public class UDT_NUKA_BSTConfig_BstNukaNo
    {
        public Int16 PoolNukaNo { get; set; }	//Nummer es zugeordneten Nukas aus Pool
        public Int16 firstSocket { get; set; }	//Erste Nuss Nummer
        public Int16 lastSocket { get; set; }	//Letzte Nuss Nummer
        public Int16 Reserve { get; set; }
    }
	
	
    
    [PlcType("UDT_Konfig_PGS")]
    public class UDT_Konfig_PGS
    {
        public UDT_Konfig_PGS_Kanal Kanal { get; set; }

    }


	 [PlcType("UDT_Schrauber_In_Kanal")]
    public class UDT_Schrauber_In_Kanal
    {
        public bool Kanal_BB { get; set; }	//Kanal ist Betriebsbereit
        public bool Schraubfall_uebernommen { get; set; }	//Schraubfallanwahl wurde übernommen / => kann gestartet werden
        public bool Start_Schrauber_rangiert { get; set; }	//Startsignal Schrauber an SPS durchrangiert
        public bool SS_loesen_rangiert { get; set; }	//Schlüsselschalter "Lösen" an SPS durchrangiert
        public bool Prozess_laeuft { get; set; }	//Schraubprozess in Bearbeitung
        public bool Schraubablauf_Ende { get; set; }	//(SE) Schraubablaufende erreicht (invertiert Signal „Ablauf Ende“)
        public bool BA_Auto_rangiert { get; set; }	//Betriebsart "Hand/Auto" an SPS durchrangiert (Hand=0/Auto=1)
        public bool MFU_rangiert { get; set; }	//Anwahl MFU an SPS durchrangiert => Unterdrückung IO Zählung
        public bool Parametrierung_angef { get; set; }	//Nach fertig bearbeitet, wird der Start solange gesperrt bis die neue Parametrie
        public bool Spindel_IO { get; set; }	//Ergebnis Spindel IO verschraubt
        public bool Spindel_NIO { get; set; }	//Ergebnis Spindel NIO verschraubt
        public bool Winkelfehler { get; set; }	//Winkelfehler
        public bool Momentenfehler { get; set; }	//Momentenfehler
        public bool Werkerfreigabe { get; set; }	//Bei Schraubsp.: 1, wenn Spindel fertig,Bei GWK: 1, wenn „grüne Taste“ gedrückt
        public bool keine_Bearbeitung { get; set; }	//keine Bearbeitung (SIO)
        public bool Spindel_abgewaehlt { get; set; }	//Spindel abgewählt
        public bool Luftverschraubung { get; set; }	//(NA) Voranzug wurde nicht IO abgeschlossen
        public bool GST { get; set; }	//Fügemodul, Grundstellung
        public bool ZwiPos { get; set; }	//Fügemodul Zwischenposition
        public bool IST_GWK { get; set; }	//Spindel ist Knickschlüssel
        public bool FesteSchraubeErkannt { get; set; }	//DoubleHit -> angezogene Schraube erkannt (Ausgang 43)
        public bool Ausgang_44 { get; set; }	//Ausgang 44 aus Schraubprogramm
        public bool Ausgang_45 { get; set; }	//Ausgang 45 aus Schraubprogramm
        public bool Ausgang_46 { get; set; }	//Ausgang 46 aus Schraubprogramm
        public byte Reserve_byte { get; set; }	//Reserve
        public bool ERR_01 { get; set; }	//Fehler 24V Spannungsversorgung
        public bool ERR_02 { get; set; }	//Fehler Einspeisung Leistungsteil
        public bool ERR_03 { get; set; }	//Fehler Einspeisung Steuerteil
        public bool ERR_04 { get; set; }	//Fehler Übertemperatur
        public bool ERR_05 { get; set; }	//Fehler Sicherung Anlage n.i.O
        public bool ERR_06 { get; set; }	//Fehler Störung FeldBus Slave
        public bool ERR_07 { get; set; }	//Fehler MSR Board
        public bool ERR_08 { get; set; }	//Fehler bei Schraubablauf
        public bool ERR_09 { get; set; }	//Fehler in Programmliste
        public bool ERR_10 { get; set; }	//Kommunikationsfehler Master  <> Schraubmodul
        public bool ERR_11 { get; set; }	//IPM Spooler 100% voll
        public bool ERR_12 { get; set; }	//Angewählte Schraubfallnummer fehlt
        public bool ERR_13 { get; set; }	//Fügemodul, Fahrbereichsgrenze Max überschritten. (Endlage oben)
        public bool ERR_14 { get; set; }	//Fügemodul, Fahrbereichsgrenze Min unterschritten. (Endlage unten)
        public bool ERR_15 { get; set; }	//Sicherheitsmoment/-kraft wurde überschritten
        public bool ERR_16 { get; set; }	//Reserve
        public UInt16 ERR_RES_Word { get; set; }	//Reserve
        public bool MSG_01 { get; set; }	//Warnung Kopplung IPM gestört
        public bool MSG_02 { get; set; }	//Reserve
        public bool MSG_03 { get; set; }	//Reserve
        public bool MSG_04 { get; set; }	//Warnung Auslastung Endstufe
        public bool MSG_05 { get; set; }	//Warnung Auslastung Motor
        public bool MSG_06 { get; set; }	//Warnmeldung Systemspannungen Schraubmodul
        public bool MSG_07 { get; set; }	//Warnmeldung Messsystem allg. (Aufnehmer)
        public bool MSG_08 { get; set; }	//Warnmeldung Spindel allg. (Motor / Getriebe)
        public bool MSG_09 { get; set; }	//Warnmeldung Aufnehmer Wartungszyklus notwendig
        public bool MSG_10 { get; set; }	//Warnmeldung Spindel (Motor / Getriebe) Wartungszyklus notwendig
        public bool MSG_11 { get; set; }	//IPM Spooler 80% voll
        public bool MSG_12 { get; set; }	//Fügemodul, Maximale Anzahl Lastzyklen erreicht
        public bool MSG_13 { get; set; }	//Reserve
        public bool MSG_14 { get; set; }	//Reserve
        public bool MSG_15 { get; set; }	//Reserve
        public bool MSG_16 { get; set; }	//Reserve
        public UInt16 MSG_RES_Word { get; set; }	//Reserve
        public Int16 Diag_Code { get; set; }	//Spindelbezogener Diagnose Coder der einzelnen Hersteller

    }


    [PlcType("UDT_Schrauber_Out_Kanal")]
    public class UDT_Schrauber_Out_Kanal
    {

        [ArrayBounds(1,12,0)]
        public Char[] ID { get; set; }	//Identifikationsdaten des Werkstücks -> FG-Nr, Motornummer etc. IPM-> IDENT

        [ArrayBounds(1,2,0)]
        public Char[] Seite { get; set; }	//Identifikationsdaten für "links","rechts","unten","oben" etc. IPM-> Zusatzinfo

        [ArrayBounds(1,4,0)]
        public Char[] WT_NR { get; set; }	//Werkstückträgernummer , STS-Nummer etc. IPM-> Bauteilträger-ID

        [ArrayBounds(1,2,0)]
        public Char[] Typbezeichnung { get; set; }	//Typbezeichnung für Handschraubsysteme IPM-> Typ
        public Int16 Schraubstelle { get; set; }	//Schraubstellennummer bei A-Station , Nussnummer bei Handarbeitsplatz
        public Int16 Schraubfallnr { get; set; }	//Schraubablauf der Schraubersteuerung (auch als Schraubprogramm bezeichnet)
        public bool BA_Auto { get; set; }	//Betriebsart an Schrauber , "Hand = 0", "Automatik =1"
        public bool Quitt { get; set; }	//setzt alle kanalbezog. Fehler dieses Kanals und alle allg. Fehler zurück
        public bool MFU { get; set; }	//Anwahl MFU an Schrauber  Einsatz HVO Handschraubtechnik
        public bool Messung_Stop { get; set; }	//Externer Stop, Satz im Programm beenden, z.B Schraube finden über SPS-Zeit
        public bool Synchronisation { get; set; }	//Wenn ein Kanal in einer synchronisierten Gruppe nicht verschrauben soll,
        public bool Spindel_Abwahl { get; set; }	//Einzelne An/Abwahl jeder Spindel (Abwahl=1)
        public bool Spindel_Start { get; set; }	//Schraubspindel wird gestartet
        public bool Daten_gueltig { get; set; }	//Daten auf der Schnittstelle sind gültig
        public bool Test_unterdruecken { get; set; }	//Unterdrückung Systemtests auf MSR-Board
        public bool Paramtrierfreigabe { get; set; }	//Freigabe Prametrierung uebernehmen
        public bool Freifahrbit { get; set; }	//Fügemodul freifahren
        public bool Tippen_plus { get; set; }	//Fügemodul, Tippbetrieb
        public bool Tippen_minus { get; set; }	//Fügemodul, Tippbetrieb
        public bool Modusanwahl { get; set; }	//=1…mit Einzelspielmessung, =0…ohne Einzelspielmessung
        public bool Reserve_06 { get; set; }	//Res.
        public bool Reserve_07 { get; set; }	//Res.

    }
	
    
    [PlcType("FB_SS_PGS", true)]
    public class FB_SS_PGS
    {
        public bool i_Stoer_quit { get; set; }	//Störung quittieren
        public bool i_Daten_vorh { get; set; }	//Daten von Datenträger vorhanden
        public bool i_auto { get; set; }	//Automatik angewählt
        public bool i_Frg_Schrauben { get; set; }
        public bool i_Vorw_loesen { get; set; }	//Loesen von HMI angewählt
        public bool i_Vorw_MFU { get; set; }	//Vorwahl MFU
        public bool i_Param_freigabe { get; set; }	//Paramtrierfreigabe
        public bool i_Start_speichernd { get; set; }
        public FB_SS_PGS_i_ipm_dat i_ipm_dat { get; set; }	//Motordaten für IPM
        public Int16 i_Nuss { get; set; }	//Nussnummer
        public Int16 i_Schraubfall { get; set; }	//Schraubfall schrauben
        public Int16 i_Schraubfall_loesen { get; set; }	//Schraubfall lösen
        public UDT_Schrauber_In_Kanal i_Kanal_input { get; set; }
        public UDT_Schrauber_Out_Kanal o_Kanal_output { get; set; }
        public bool o_falsche_drehrichtung { get; set; }
        public bool o_param_anforderung { get; set; }
        public bool io_ergebnis_IO { get; set; }
        public bool io_ergebnis_NIO { get; set; }
        public bool io_3_mal_NIO { get; set; }	//drei mal in Folge NIO-> Spindel sperren
        public bool io_ergebnis_geloest { get; set; }
        public bool io_SicherheitsMoment { get; set; }
        public bool io_DoubleHit { get; set; }	//angezogene Schrauber erkannt
        public bool Spindel_starten { get; set; }	//Spindel starten
        public bool Schrauben_gestartet { get; set; }
        public bool Fert_Low_Prz_lft_high { get; set; }	//IO,NIO,Fertig von Schrauber reset, Prozess läuft high
        public bool Prozess_beendet { get; set; }	//Prozess läuft Schrauber low
        public bool IO_NIO_Zuweisen { get; set; }	//IO, NIO -Ergebnisse zuweisen
        public bool HM_Start_von_Schrauber { get; set; }
        public bool FL_Start_von_Schrauber { get; set; }
        public bool frg_loesen { get; set; }
        public bool Schraubfall_bernommen { get; set; }
        public bool letz_erg_NIO { get; set; }
        public bool FL_SF_uebernommen { get; set; }
        public Int16 NIO_Zaehler { get; set; }
        public Int16 akt_Schraubfall { get; set; }
        public Int16 last_Schraubfall { get; set; }

    }

    
    [PlcType("FB_Stoer_Spindel", true)]
    public class FB_Stoer_Spindel
    {
        public bool i_stoer_quitt { get; set; }
        public UDT_Schrauber_In_Kanal i_Eing_Spindel { get; set; }
        public bool i_Kanal_vorh { get; set; }	//Kanal vorhanden und nicht abgewählt
        public Int16 i_kanal_nr { get; set; }
        public bool io_collect_error { get; set; }

        [ArrayBounds(1,32,0)]
        public bool[] Stoerung { get; set; }

    }

    
    [PlcType("FB_Meld_Spindel", true)]
    public class FB_Meld_Spindel
    {
        public bool i_Kanal_vorh { get; set; }
        public UDT_Schrauber_In_Kanal i_Eing_Spindel { get; set; }
        public Int16 i_kanal_nr { get; set; }
        public bool io_collect_warning { get; set; }

        [ArrayBounds(1,32,0)]
        public bool[] Meldung { get; set; }

    }

    
    [PlcType("FB_PGS", true)]
    public class FB_PGS
    {
        public Int16 i_BST_Nr { get; set; }
        public bool i_WT_IN_POS { get; set; }
        public bool i_Frg_Bearb { get; set; }	//Freigabe Bearbeitung
        public bool i_frg_schrauben { get; set; }	//Freigabe aus Schraubreihenfolge
        public UDT_Konfig_PGS i_PGS { get; set; }
        public bool i_anlage_ein { get; set; }
        public bool i_Stoer_quit { get; set; }	//Störung quittieren
        public bool i_InfoSend { get; set; }	//MotorInfo an IPM versenden
        public Int16 i_MSID_OffsNr { get; set; }	//Montegaschritt ID Offset -> MsId für Ipm = i_MSID_OffsNr + Kanal Nr.
        public FB_PGS_i_ipm_dat i_ipm_dat { get; set; }
        public bool o_Fehler_SrPrg_NIO { get; set; }	//Schraubprogramm wurde nicht gefunden
        [NotAccessible(true)]
        public UDT_AI_PGS io_AI_PGS { get; set; }
        [NotAccessible(true)]
        public UDT_PGS_Anw io_Anw_PGS { get; set; }
        [NotAccessible(true)]
        public UDT_MFU_PGS io_PgsMfu { get; set; }
        [NotAccessible(true)]
        public UDT_Programm_PGS io_Ablauf_PGS { get; set; }
        [NotAccessible(true)]
        public UDT_Schrauber_Input io_Schrauber_in { get; set; }
        [NotAccessible(true)]
        public UDT_Schrauber_output io_Schrauber_out { get; set; }
        [NotAccessible(true)]
        public UDT_NUKA io_Zustand_NUKA { get; set; }
        [NotAccessible(true)]
        public UDT_Intf_WsPgs io_WorkSteps { get; set; }
        public Int16 io_akt_Schr_Reihenfolge { get; set; }
        public Int16 io_akt_Schraubpunkt { get; set; }
        public FB_SS_PGS Schnittstelle_PGS { get; set; }
        public FB_Stoer_Spindel Stoer_Spindel { get; set; }
        public FB_Meld_Spindel Meld_Spindel { get; set; }
        public Int16 STATUS_PGS { get; set; }
        public Int16 akt_SF { get; set; }
        public Int16 letzter_Schraubpunkt { get; set; }
        public Int16 gestarteteNuss { get; set; }
        public Int16 actWorkStep { get; set; }
        public bool Verschr_IO { get; set; }
        public bool Verschr_NIO { get; set; }
        public bool drei_x_NIO { get; set; }
        public bool Verschr_geloest { get; set; }
        public bool HM_WT_IN_POS { get; set; }
        public bool HM_Verschr_IO { get; set; }
        public bool Werkerfreigabe { get; set; }
        public bool MFU_aktiv { get; set; }
        public bool SicherheitsMoment { get; set; }
        public bool DoubleHit { get; set; }
        public bool SpindelGestartet { get; set; }
        public bool MsId_gesendet { get; set; }
        public bool Reset_OK { get; set; }

    }
	
	[PlcType("UDT_NUKA")]
    public class UDT_NUKA
    {

        [ArrayBounds(100,199,0)]
        public UDT_NUKA_SchraubPunkt[] SchraubPunkt { get; set; }

        [ArrayBounds(0,128,0)]
        public UDT_NUKA_Steckplatz[] Steckplatz { get; set; }	//größte mögliche NussNr. 128 (Nuka 12 & Nuss 8 aus NukaPool)
        public Int16 Nuss_soll { get; set; }	//zu entnehmende Nuss
        public bool nusswechsel { get; set; }	//Flanke Nusswechsel

    }

    
    [PlcType("UDT_NUKA_PoolConfig")]
    public class UDT_NUKA_PoolConfig
    {

        [ArrayBounds(1,12,0)]
        public UDT_NUKA_PoolConfig_Nuka[] Nuka { get; set; }

    }

    
    [PlcType("UDT_NUKA_BSTConfig")]
    public class UDT_NUKA_BSTConfig
    {
        public bool Nuka4BstActive { get; set; }

        [ArrayBounds(0,12,0)]
        public UDT_NUKA_BSTConfig_BstNukaNo[] BstNukaNo { get; set; }	//0 = Default Nuka (ohne Pool); 1-12 = FremdNuka aus Pool

    }

    
    [PlcType("FB_NUKA", true)]
    public class FB_NUKA
    {
        public bool i_lampentest { get; set; }
        public UDT_NUKA_PoolConfig i_NukaPool { get; set; }
        public UDT_NUKA_BSTConfig i_BstNukas { get; set; }
        [NotAccessible(true)]

        [ArrayBounds(1,4,0)]
        public UDT_NUKA[] io_Zustand_nuka { get; set; }
        public Int16 io_akt_Schr_Reihenfolge { get; set; }

        [ArrayBounds(1,128,0)]
        public bool[] SP_Nuss_gesteckt { get; set; }	//Zustand Nuss gespeichert für Flankenerkennung (128 = größte mögl. NussNummer au
        public UDT_NUKA Zust_Nuka_ges { get; set; }
        
        [StringLength(4)]
        public string NussSollStr { get; set; }

    }

    [Mapping("DI_PGS_BST1", "DB1155", 0)]
    public class DI_PGS_BST1
    {
        public FB_PGS PGS_1 { get; set; }
        public FB_PGS PGS_2 { get; set; }
        public FB_PGS PGS_3 { get; set; }
        public FB_PGS PGS_4 { get; set; }

        [ArrayBounds(1,10,0)]
        public Char[] SchraubPrg { get; set; }

        [ArrayBounds(1,4,0)]
        public UDT_NUKA[] Zustand_Nuka { get; set; }
        public FB_NUKA Nuka { get; set; }
        public Int16 Akt_reihenfolge { get; set; }
        public bool Stoer_Schr_PRG_PGS1 { get; set; }
        public bool Stoer_Schr_PRG_PGS2 { get; set; }
        public bool Stoer_Schr_PRG_PGS3 { get; set; }
        public bool Stoer_Schr_PRG_PGS4 { get; set; }
        public bool PgsDataCopyDone { get; set; }

    }


}

