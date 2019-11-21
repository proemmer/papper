

using Papper.Attributes;
using System;

namespace Insite.Customer.Data.DB_AI_Texte_BST1
{
    
    

    public class UDT_AI_Texte_AI_Suchtext_MOMO
    {
        [ArrayBounds(1,4,0)]
        public Char[] Bez { get; set; }	//MOMO -> Bezeichnung (wird am Datenträger gesucht)

        [ArrayBounds(1,4,0)]
        public Char[] Daten { get; set; }	//MOMO -> Daten wird mit Datenträger verglichen
    }

    

    public class UDT_AI_Texte_AI_Suchtext
    {
        [ArrayBounds(1,10,0)]
        public Char[] PROD { get; set; }	//Produktnummer (Motorsachnummer) -> fixe Position am Datenträger

        [ArrayBounds(1,4,0)]
        public Char[] MOAR { get; set; }	//Motorart  -> fixe Position am Datenträger
        public UDT_AI_Texte_AI_Suchtext_MOMO MOMO { get; set; }
    }

    

    public class UDT_AI_Texte_AI_Anzeige
    {
        public Int16 Pos { get; set; }	//Position im Bild (von oben nach unten: 1,2,3) 0 = beliebige Position

        [ArrayBounds(1,4,0)]
        public Char[] Snr_KzBez { get; set; }	//Sachnummern-Kurzbezeichnung (wird am Datenträger gesucht)

        [ArrayBounds(1,80,0)]
        public Char[] Text { get; set; }	//Anzuzeigender Text -> Arbeitsinhalt
    }

    

    public class UDT_AI_Texte_AI
    {
        public UDT_AI_Texte_AI_Suchtext Suchtext { get; set; }
        public UDT_AI_Texte_AI_Anzeige Anzeige { get; set; }
    }


    
    public class UDT_AI_Texte
    {

        [ArrayBounds(1,50,0)]
        public UDT_AI_Texte_AI[] AI { get; set; }

    }
	
    [Mapping("DB_AI_Texte_BST1", "DB1012", 0)]
    public class DB_AI_Texte_BST1
    {
        public UDT_AI_Texte BST { get; set; }

    }

}

