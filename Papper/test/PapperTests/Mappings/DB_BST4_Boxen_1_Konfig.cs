using Papper.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Papper.Tests.Mappings
{


    public class UDT_Boxen_Fach
    {
        public bool aktiv { get; set; }	//Fach aktiviert
        public bool Out { get; set; }	//Zustand Ausgang
        public bool fertig { get; set; }	//Fertigmeldung Fach
        public bool angew { get; set; }	//Fach angewaehlt
        public bool HIO { get; set; }	//M-ID: Hand-IO
        public bool ABGW { get; set; }	//M-ID: Montageschritt abgewählt
        public bool geschlossen { get; set; }	//Box geschlossen
        public bool geoeffnet_Eingriff { get; set; }	//Box geöffnet oder Eingriff
        public Int16 M_ID { get; set; }	//Montageschritt-ID
    }



    public class UDT_Boxen_UNIV_Fach_Ausw
    {
        public UDT_DatenAusw_Univ Auswertungen { get; set; }
    }



    public class UDT_Boxen_UNIV_Fach_Ergeb
    {
        public UDT_DatenErgeb_Univ Data { get; set; }
    }




    public class UDT_Boxen
    {
        public bool vorhanden { get; set; }	//Regal ist vorhanden (PLC)
        [ReadOnly(true)]
        public bool fertig { get; set; }	//Fertigmeldung Gesamt Regal
        [ReadOnly(true)]
        public bool aktiv { get; set; }	//Regal aktiv

        [ArrayBounds(1, 4, 0)]
        public UDT_Boxen_Fach[] Fach { get; set; }
        public bool Hand_IO { get; set; }   //Regal Hand IO

        [StringLength(20)]
        public string Headline { get; set; }	//editierbares Beschreibungsfeld im Bildkopf

        [ArrayBounds(1, 4, 0)]
        public UDT_Boxen_UNIV_Fach_Ausw[] UNIV_Fach_Ausw { get; set; }	//Universal Auswertung
        [ReadOnly(true)]

        [ArrayBounds(1, 4, 0)]
        public UDT_Boxen_UNIV_Fach_Ergeb[] UNIV_Fach_Ergeb { get; set; }	//ergebnisse der Universal Auswertung

    }

    [Mapping("DB_BST4_Boxen_1_Konfig", "DB4046", 0)]
    public class DB_BST4_Boxen_1_Konfig
    {
        public UDT_Boxen Boxen { get; set; }

    }
}
