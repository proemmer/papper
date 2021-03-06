[![insite MyGet Build Status](https://www.myget.org/BuildSource/Badge/insite?identifier=1311b225-6a23-4112-834f-931a82266bff)](https://www.myget.org/)

# Papper
Plc Data Mapper is a library to map C# classes to plc blocks to get symbolic access to the plc.

<!-- TOC -->

- [Papper](#Papper)
- [NuGet](#nuget)
- [Description](#description)
- [Sample-Code](#sample-code)
- [Release Notes](#release-notes)

<!-- /TOC -->

# NuGet

    PM>  Install-Package Papper

# Description

Papper could be used with any S7 library, because it's a top level component. It convert's the given command to read or write commands for the S7 library.


# Sample-Code


To access the data you fist have to declare a class with a mapping attribute like the following one:
<pre><code>
using Papper.Attributes;
using System;

namespace UnitTestSuit.Mappings
{
    #region Safety
    public class UDT_SafeMotionHeader_States
    {
        public bool ChecksumInvalid { get; set; }
        public bool UpdateRequested { get; set; }
    }

    public class UDT_SafeMotionHeader_Commands
    {
        public bool UpdateAllowed { get; set; }
        public bool AllSlotsLocked { get; set; }
    }

    public class UDT_SafeMotionSlot_Commands
    {
        public bool TakeoverPermitted { get; set; }
        public bool TakeoverRefused { get; set; }
    }

    public class UDT_SafeMotionSlot_Handshake
    {
        public bool MotionSelected { get; set; }
        public bool Button1Pressed { get; set; }
        public bool Button2Pressed { get; set; }
        public Int16 HandshakeTime { get; set; }
    }

    public class UDT_SafeMotionSlot_Motion
    {
        public bool ManualEnable1 { get; set; }
        public bool ManualEnable2 { get; set; }
        public bool ManualOperation1 { get; set; }
        public bool ManualOperation2 { get; set; }
    }

    public class UDT_SafeMotionHeader
    {
        public DateTime Generated { get; set; }
        public Int16 NumberOfActiveSlots { get; set; }
        public UDT_SafeMotionHeader_States States { get; set; }
        public UDT_SafeMotionHeader_Commands Commands { get; set; }

    }

    public class UDT_SafeMotionSlot
    {
        public Int16 SafeSlotVersion { get; set; }
        public byte SlotId { get; set; }
        public DateTime UnitTimestamp { get; set; }
        public UInt16 UnitChecksum { get; set; }
        public Int16 AggregateDBNummer { get; set; }
        public Int16 AggregateOffset { get; set; }
        public UInt32 HmiId { get; set; }
        public UInt32 AccessRightReqFromHmiId { get; set; }
        public UDT_SafeMotionSlot_Commands Commands { get; set; }
        public UDT_SafeMotionSlot_Handshake Handshake { get; set; }
        public UDT_SafeMotionSlot_Motion Motion { get; set; }

    }

    public class UDT_SafeMotion
    {
        public UDT_SafeMotionHeader Header { get; set; }

        [ArrayBounds(0, 254)]
        public UDT_SafeMotionSlot[] Slots { get; set; }

    }

    [Mapping("DB_Safety", "DB15", 0)]
    public class DB_Safety
    {
        public UDT_SafeMotion SafeMotion { get; set; }

    }

    #endregion

}

</code></pre>


The following code snipet is a small sample for the usage of papper:
<pre><code>
var _papper = new PlcDataMapper(960);

_papper.OnRead += Papper_OnRead;
_papper.OnWrite += Papper_OnWrite;

_papper.AddMapping(typeof(DB_Safety));


Dictionary<string,object> result = _papper.Read(mapping, "SafeMotion.Slots[100].UnitChecksum");

result.Value = 100;

_papper.Write(mapping, result);

</code></pre>


The interface to the used S7 library have to handle the following calls
<pre><code>
private static byte[] Papper_OnRead(string selector, int offset, int length)
{
    //call s7 library to read data
}

private static bool Papper_OnWrite(string selector, int offset, byte[] data, byte bitMask = 0)
{
    //call s7 library to write data
}
</code></pre>




# Absolut addressing
Syntax:


[Selector].[TYPE][OFFSET],[CountOfData]

Bool
[Selector].[TYPE][OFFSET_Byte].[OFFSET_Bit],[CountOfData]

String
[Selector].[TYPE][OFFSET_Stringlength][_CountOfData]


## Selector

IB: Input Area
FB: Flag Area
QB: Output Area
TM: Timer Area
CT: Counter Area
DB[Number]: DataBlock Area

## Type

* Boolean: 	X or BIT
* Byte:    	B or BYTE
* Char:    	C or CHAR
* Date:    	DATE
* DateTime:	DT or DATETIME
* DInt:    	DI or DINT
* DWord:   	DW or DWORD
* Int:     	I or INT
* LDateTime:LDT or LDATETIME
* LInt:		LI or LINT
* Time:		LT or LTIME
* LWord:	LW or LWORD
* Real:		R or REAL
* S5Time:	TIMEBCD
* Counter:  CT or COUNT
* SInt:		SI or SINT
* String:   S or STRING
* Time:     T or TIme
* TimeOfDay:TOD
* UDInt:	UDI or UDINT
* UInt: 	UI or UINT
* ULInt:	ULI or ULINT
* USInt:	USI or USINT
* WChar:	WC or WCHAR
* Word:     W or WORD
* WString:  WS or WSTRING 




# Release Notes
* 1.0.6:
    * migrated to VS2017 and C#7.
    * Refactoring
