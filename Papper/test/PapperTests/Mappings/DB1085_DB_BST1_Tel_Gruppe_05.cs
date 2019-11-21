

using Papper.Attributes;
using System;

namespace Insite.Customer.Data.DB_BST1_Tel_Gruppe_05
{
    

    
    public class UDT_Telefon
    {
        [ReadOnly(true)]
        public Int16 Auswahl_Text { get; set; }	//1 für 1. Zeile, ... 10 für 10. Zeile angewählt
        [ReadOnly(true)]
        public Int16 Status { get; set; }	//1 = wird bearbeitet; 2 = Quit IO 3 = Quit NIO

        [ArrayBounds(1,10,0)]
                
        [StringLength(35)]
        public string[] Text { get; set; }	//Texte Zeilen 1 - 10 Länge 35
        
        [StringLength(10)]
        public string Ruftyp { get; set; }

[PlcType("Date")]
        [ArrayBounds(1,10,0)]
        public DateTime[] Datum { get; set; }

[PlcType("TimeOfDay")]
        [ArrayBounds(1,10,0)]
        public TimeSpan[] Zeit { get; set; }

        [ArrayBounds(1,10,0)]
        public Int16[] Tel_lfd_Nr { get; set; }	//Laufende Nummer des Telegramm

    }

    [Mapping("DB_BST1_Tel_Gruppe_05", "DB1085", 0)]
    public class DB_BST1_Tel_Gruppe_05
    {
        public UDT_Telefon tel { get; set; }

    }

}

