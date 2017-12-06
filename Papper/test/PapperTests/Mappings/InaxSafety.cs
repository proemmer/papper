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
    [Mapping("DB_Safety2", "DB16", 0)]
    public class DB_Safety
    {
        public UDT_SafeMotion SafeMotion { get; set; }

    }

    #endregion

}
