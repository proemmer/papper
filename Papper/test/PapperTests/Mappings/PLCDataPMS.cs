
using Papper.Attributes;
using System;

namespace PMSComponentHost.VTagStorerLoader
{
    public class UDTTemplateGeneral_ColdTestTA010
    {
        [ArrayBounds(1,8,0)]
        public Char[] MachineProgram { get; set; }	//MAPO

        [ArrayBounds(1,8,0)]
        public Char[] TestProgram { get; set; }	//PRUE

        [ArrayBounds(1,8,0)]
        public Char[] Spare1 { get; set; }

        [ArrayBounds(1,8,0)]
        public Char[] Spare2 { get; set; }

        [ArrayBounds(1,8,0)]
        public Char[] Spare3 { get; set; }

        [ArrayBounds(1,8,0)]
        public Char[] Spare4 { get; set; }

        [ArrayBounds(1,22,0)]
        public byte[] Spare { get; set; }
    }

    

    public class UDTTemplateGeneral_StatusStation
    {
        public UDTStatusProc RV015 { get; set; }
        public UDTStatusProc RV055 { get; set; }
        public UDTStatusProc RV065 { get; set; }
        public UDTStatusProc RV0751 { get; set; }
        public UDTStatusProc RV0752 { get; set; }
        public UDTStatusProc RV0753 { get; set; }
        public UDTStatusProc RV085 { get; set; }
        public UDTStatusProc FV015 { get; set; }
        public UDTStatusProc FV035 { get; set; }
        public UInt16 Spare_18 { get; set; }
        public UDTStatusProc RV005 { get; set; }
        public UDTStatusProc RM005 { get; set; }
        public UDTStatusProc RM010 { get; set; }
        public UDTStatusProc RM020 { get; set; }
        public UDTStatusProc RM030 { get; set; }
        public UDTStatusProc RM040 { get; set; }
        public UDTStatusProc RM050 { get; set; }
        public UDTStatusProc RM060 { get; set; }
        public UDTStatusProc RM070 { get; set; }
        public UDTStatusProc RM080 { get; set; }
        public UInt16 Spare_38 { get; set; }
        public UDTStatusProc FM010 { get; set; }
        public UDTStatusProc FM020 { get; set; }
        public UDTStatusProc FM030 { get; set; }
        public UInt16 Spare_46 { get; set; }
        public UDTStatusProc FM050 { get; set; }
        public UInt16 Spare_50 { get; set; }
        public UInt16 Spare_52 { get; set; }
        public UDTStatusProc TA010 { get; set; }
        public UDTStatusProc TA030 { get; set; }
        public UDTStatusProc EM010 { get; set; }

        [ArrayBounds(1,8,0)]
        public byte[] SpareTo080 { get; set; }
    }

    

    public class UDT_PMS_data_header
    {
        [ArrayBounds(1,32,0)]
        public Char[] DataDesc { get; set; }	//Describes the content of the data block.
        public Int16 length { get; set; }	//  Valid length of the data block in bytes
    }


    
    public class UDTEnableControlBits
    {
        public bool AckOkInStation { get; set; }	//DISPLAY BUTTON "ACK OK" IN STATION
        public bool RepeatInStation { get; set; }	//DISPLAY BUTTON "REPEAT" IN STATION
        public bool Spare_02 { get; set; }
        public bool AutoChangeScreen { get; set; }	//ENABLE AUTOMATIC CHANGE IN ACTUAL PROCESS SCREEN
        public bool AckOkInRepair { get; set; }	//DISPLAY BUTTON "ACK OK" IN REPAIR AREA
        public bool RepeatInRepair { get; set; }	//DISPLAY BUTTON "REPEAT" IN REPAIR AREA
        public bool Spare_06 { get; set; }
        public bool Spare_07 { get; set; }
        public byte Spare_1 { get; set; }

    }

    
    public class UDTStatusProc
    {
        public bool IO { get; set; }	//In Ordnung
        public bool NIO { get; set; }	//Nicht in Ordnung
        public bool HIO { get; set; }	//Hand in Ordnung
        public bool EIO { get; set; }	//Eingriff in Ordnung
        public bool ABGW { get; set; }	//Abgewählt
        public bool ZIO { get; set; }	//Beim 2. Versuch in Ordnung
        public bool RIO { get; set; }	//Nach Reparatur in Ordnung
        public bool AIO { get; set; }	//Mit Änderung in Ordnung
        public bool HMI_Started { get; set; }	//Started = Yellow
        public bool HMI_Nok { get; set; }	//Nok     = Red
        public bool HMI_Ok { get; set; }	//Ok      = Green

    }

    
    public class UDTTemplateOperationData
    {

        public UDTTemplateOperationData()
        {

        }

        [ArrayBounds(1,40,0)]
        public Char[] OperationText { get; set; }	//Text Englisch

        [ArrayBounds(1,40,0)]
        public Char[] OperationText_2lang { get; set; }	//Text second language
        public Int16 NoAssemblyStep { get; set; }
        public byte OperationType { get; set; }
        public byte ProgramNo { get; set; }
        public byte SocketNo { get; set; }
        public byte SetpointSelection { get; set; }
        public byte PatternNo { get; set; }
        public byte ToolNo { get; set; }
        public byte ProgramNoUntighten { get; set; }
        public byte NoRerunNOK { get; set; }
        public byte ProgramNoRepair { get; set; }
        public byte ToolNoRepair { get; set; }
        public UDTEnableControlBits EnableControlBits { get; set; }
        public UInt16 Spare_56 { get; set; }

        [ArrayBounds(1,40,0)]
        public Char[] Traceability { get; set; }

        [ArrayBounds(1,10,0)]
        public Char[] PartNo { get; set; }

        [ArrayBounds(1,10,0)]
        public Char[] PartNoOption_1 { get; set; }

        [ArrayBounds(1,10,0)]
        public Char[] PartNoOption_2 { get; set; }

        [ArrayBounds(1,10,0)]
        public Char[] PartNoOption_3 { get; set; }
        public UInt16 Spare_160 { get; set; }
        public UInt32 Spare_162 { get; set; }
        public UDTStatusProc Status { get; set; }
        public UInt32 SingleInfoOk { get; set; }
        public byte ReachedNumbersOfOk { get; set; }
        public byte Spare_177 { get; set; }
        public UInt32 SingleInfoNok { get; set; }
        public UInt16 Spare_182 { get; set; }
        public UInt32 Spare_196 { get; set; }

    }

    
    public class UDTTemplateGeneral
    {

        [ArrayBounds(1,40,0)]
        public Char[] EngineNo { get; set; }	//MONR

        [ArrayBounds(1,10,0)]
        public Char[] Spare { get; set; }	//Teilesachnummer

        [ArrayBounds(1,4,0)]
        public Char[] EngineType { get; set; }	//Engine Type

        [ArrayBounds(1,6,0)]
        public Char[] VehiclePlatform { get; set; }	//Fahrzeug Plattform

        [ArrayBounds(1,4,0)]
        public Char[] EnginePlatform { get; set; }	//Motor Plattform

        [ArrayBounds(1,10,0)]
        public Char[] EngineID { get; set; }	//Teilesachnummer
        public UInt32 Spare42 { get; set; }
        public UInt32 Spare46 { get; set; }

        [ArrayBounds(1,4,0)]
        public Char[] ABTE { get; set; }	//Customer identifier

        [ArrayBounds(1,4,0)]
        public Char[] KUCO { get; set; }	//Engine code

        [ArrayBounds(1,14,0)]
        public Char[] SNMU { get; set; }	//KU-Material No.

        [ArrayBounds(1,8,0)]
        public Char[] ABE { get; set; }	//Second Row of block marking

        [ArrayBounds(1,2,0)]
        public Char[] CheckSum { get; set; }	//Checksum of label (MONR + KUCO)

        [ArrayBounds(1,10,0)]
        public Char[] LPNR { get; set; }	//Lieferplannummer
        public UInt16 Spare_92 { get; set; }
        public Int16 OilVolume { get; set; }	//Oil volume to fill (1/1000 mL)
        public Int16 Prog_No_TA030 { get; set; }	//Programmnummer TA030
        public UDTTemplateGeneral_ColdTestTA010 ColdTestTA010 { get; set; }
        public UDTTemplateGeneral_StatusStation StatusStation { get; set; }

        [ArrayBounds(1,7,0)]
        public Char[] GradeMB_ZKGH { get; set; }	//Grade main bearing in ZKGH

        [ArrayBounds(1,7,0)]
        public Char[] GradeMB_MBC { get; set; }	//Grade main bearing in maine bearing caps
        public Int16 PistonProtusionValue { get; set; }	//Type of sealing
        public UInt16 OilVolumeFilled { get; set; }	//Oil volume filled in FM050 (1/1000 mL)
        public Int16 oiling_counter_TA030 { get; set; }

        [ArrayBounds(292,499,0)]
        public byte[] Spare292_499 { get; set; }

    }

    
    public class UDT_PMS_data
    {
        public UDT_PMS_data_header header { get; set; }
        public UDTTemplateGeneral General { get; set; }	//General engine Data

        [ArrayBounds(1,30,0)]
        public UDTTemplateOperationData[] Operation { get; set; }	//Operations

    }

    public class PLCDataPMS
    {
        public static PLCDataPMS CreateInstance()
        {
            // TODO: alloc all arrays etc.
            return new PLCDataPMS();
        }

        public UDT_PMS_data data { get; set; }

    }

}

