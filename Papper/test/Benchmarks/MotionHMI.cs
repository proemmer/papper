using Papper.Attributes;
using System;

namespace Benchmarks.Mappings
{
    
    

    public class UDT_MotionLine_Handshake
    {
        public bool PulseTrigger { get; set; }
        public bool MotionSelected { get; set; }
        public bool Button1Pressed { get; set; }
        public bool Button2Pressed { get; set; }
    }

    

    public class UDT_MotionLine_Commands
    {
        public bool motionLockedFromOtherHmi { get; set; }	//Bewegung von anderer HMI belegt
        public bool SelectionEnabled { get; set; }	//Selektierung erlaubt
        public bool ReqSelection { get; set; }	//Bit aus Handbild Selektierung Zeile angefordert
        public bool Selected { get; set; }	//Zeile ist Selektiert
        public bool Release1 { get; set; }	//Bit aus Handbild Freigabe Bewegungsrichung 1
        public bool Release2 { get; set; }	//Bit aus Handbild Freigabe Bewegungsrichung 2
        public bool Start1 { get; set; }	//Bit aus Handbild Start Bewegungsrichung 1
        public bool Start2 { get; set; }	//Bit aus Handbild Start Bewegungsrichung 1
        public bool TakeoverRequest { get; set; }	//Request to TakeOver a Slot from another HMI
        public bool TakeoverPermit { get; set; }	//andere Hmi darf die Bewegung übernehmen
        public bool TakeoverRefuse { get; set; }	//andere Hmi darf die Bewegung nicht übernehmen
        public Int16 TakeoverTime { get; set; }	//Timeout for Takeover Process -> Time to permit / refuse
    }

    

    public class UDT_MotionLine_Txt
    {
[PlcType("WString")]        
        [StringLength(70)]
        public string Caption { get; set; }	//Name der Bewegung

[PlcType("WString")]        
        [StringLength(25)]
        public string DirectionLeft { get; set; }	//Name für die VerfahrRichtung links

[PlcType("WString")]        
        [StringLength(25)]
        public string DirectionRight { get; set; }	//Name für die VerfahrRichtung rechts

[PlcType("WString")]
        [ArrayBounds(0,15,0)]
                
        [StringLength(25)]
        public string[] Position { get; set; }
    }


    
    public class UDT_Jog_Axis
    {
        public bool aktiv { get; set; }	//Jog Betrieb ist aktiv
        public bool Preset { get; set; }	//Preset
        public bool Inkremente_vorhanden { get; set; }	//InkrementBetrieb vorhanden
        public bool Inkremente_aktiv { get; set; }	//Verfahren von eingestellten Inkrementen - aktiv
        public bool jog_freigabe { get; set; }	//Jog Betrieb freigabe -> Bewegung selektiert
        public bool Job_aktiv { get; set; }	//Rückmeldung Bearbeitung läuft
        public bool Teachen_aktiv { get; set; }	//Teachen aktivieren
        public bool Position_uebernehmen { get; set; }	//Position angefahrene übernehmen
        public bool Teachen_vorhanden { get; set; }	//Teachen vorhanden
        public bool Override_vorhanden { get; set; }	//Override betrieb vorhanden
        public bool Preset_vorhanden { get; set; }	//Preset Taste vorhanden
        public bool Jog_aktivieren { get; set; }	//Jog Betrieb aktivieren
        public Int16 Override { get; set; }
        public Int16 Inkremente { get; set; }
        public Int32 Position { get; set; }	//Anzeige Ist-Position
        public Int32 gespeicherte_Pos { get; set; }	//Anzeige gespeicherte Position
        public Int32 Preset_Pos { get; set; }	//Sollwert Preset Position
        public Int16 SpeicherPosition { get; set; }	//Position

    }

    
    public class UDT_MotionStates
    {
        public bool Moving_Status1 { get; set; }
        public bool Moving_Status2 { get; set; }
        public bool Executability1 { get; set; }
        public bool Executability2 { get; set; }
        public bool Group_Error { get; set; }
        public byte Number_of_Final_Position { get; set; }

        [ArrayBounds(0,15,0)]
        public bool[] Final_Position { get; set; }

        [ArrayBounds(0,15,0)]
        public bool[] Display_Order { get; set; }

    }

    
    public class UDT_MotionLine
    {
        public UDT_MotionLine_Handshake Handshake { get; set; }	//0 from plc  and   1 from Inax
        public UDT_MotionLine_Commands Commands { get; set; }
        public UDT_MotionStates MotionState { get; set; }
        public Int16 HmiId { get; set; }	//HMI Id with the rigth to uses this Slot
        public Int16 AccessRightReqFromHmiId { get; set; }	//HMI Id who wants the rigth
        public UDT_MotionLine_Txt Txt { get; set; }	//Handbild Texte

    }

    
    public class UDT_MotionPicture
    {
        public bool TogglePLC { get; set; }	//PLC Toggelt BIT
        public bool FlTogglePLC { get; set; }	//FlankePLC Toggelt BIT
        public bool ToggleHMI { get; set; }	//PLC Toggelt BIT
        public bool DisplaySymbolic { get; set; }	//Endlagen Anzeige symbolisch
        public bool DisplayAbsolute { get; set; }	//Endlagen Anzeige absolut
        public bool DisplayRuntimeTxt { get; set; }	//Anzeige der Runtime Texte
        public bool DisplayJog { get; set; }	//Jog-Anzeige einschalten
        public bool PageChanged { get; set; }	//Anzeige wurde im HMi umgeschaltet
        public bool NavigationLocked { get; set; }	//The safety FC locks Navigation -> no SlotChange or SiteChange
        public bool Reset { get; set; }	//Reset Motions
        public bool ResetDone { get; set; }	//Reset fertig
        public Int16 SelectedLine { get; set; }	//aktuell selektierte Zeile
        public Int16 TimeOut { get; set; }	//Connection Timeout
        public Int16 Language { get; set; }	//Sprache 0 = Default deDE / 1 enGB / 2 zhCN
        public UDT_Jog_Axis JogDisplay { get; set; }

        [ArrayBounds(0,8,0)]
        public Int16[] ActMotion_Adr { get; set; }	//Active BEW-Adressen

        [ArrayBounds(0,8,0)]
        public Int16[] Motion_Adr { get; set; }	//Zeilen Adresse = nachfolgendem "BEW_Adr" Array Index Nr.

        [ArrayBounds(0,8,0)]
        public UDT_MotionLine[] MotionLine { get; set; }

    }

    [Mapping("DB_MotionHMI", "DB12", 0)]
    public class DB_MotionHMI
    {
        public UDT_MotionPicture HMI { get; set; }

    }

}

