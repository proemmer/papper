

using Papper.Attributes;
using System;

namespace Insite.Customer.Data.___ST000_SCREW_GS_PDB
{
    

    
    [PlcType("SCREW_GS_PROCESS_POINT_STATE_UDT")]
    public class SCREW_GS_PROCESS_POINT_STATE_UDT
    {
        public Int16 PT_ID { get; set; }
        
        [StringLength(254)]
        public string PT_NAME { get; set; }
        public Int16 SST_NR { get; set; }
        public Int16 SPINDLE_NR { get; set; }
        
        [StringLength(254)]
        public string GROUP_ID { get; set; }
        public bool STOP { get; set; }
        public bool STARTED { get; set; }
        public bool COMPLETE { get; set; }
        public bool RESULT_OK { get; set; }
        public bool RESULT_NOT_OK { get; set; }
        public bool SET_IO { get; set; }
        public bool SOLVE_AFTER_RESULT_NOT_OK { get; set; }
        public Int16 SOLVE_PROGRAM_NR { get; set; }
        public bool SOLVE { get; set; }
        public bool SOLVE_DONE { get; set; }
        public bool SOLVE_IO { get; set; }
        public bool SPINDLE_IS_BACK { get; set; }
        public bool SPINDLE_IS_FORWARD { get; set; }
        public Int16 MONITOR_SET_TIME { get; set; }

    }

    
    [PlcType("SCREW_GS_PROCESS_SUB_DECLARATION_UDT")]
    public class SCREW_GS_PROCESS_SUB_DECLARATION_UDT
    {
        public Int16 PROGRAM_NR { get; set; }
        public Int16 ATTEMPT_SET_COUNTER { get; set; }
        public Int16 ATTEMPT_COUNTER_RUN { get; set; }
        public bool SYNCHRONIZE { get; set; }
        public bool RELEASE_THIS_SUB { get; set; }
        public bool RELEASE_NEXT_SUB { get; set; }
        public bool START { get; set; }
        public bool STARTED { get; set; }
        public bool COMPLETE { get; set; }
        public bool RESULT_OK { get; set; }
        public bool RESULT_NOT_OK { get; set; }
        public Int16 MONITOR_SET_RELEASE_THIS_SUB { get; set; }

    }

    
    [PlcType("SCREW_GS_PROCESS_POINT_DECLARATION_HMI_UDT")]
    public class SCREW_GS_PROCESS_POINT_DECLARATION_HMI_UDT
    {
        public SCREW_GS_PROCESS_POINT_STATE_UDT STATE { get; set; }
        public SCREW_GS_PROCESS_SUB_DECLARATION_UDT SUB_PG_0 { get; set; }
        public SCREW_GS_PROCESS_SUB_DECLARATION_UDT SUB_PG_1 { get; set; }
        public SCREW_GS_PROCESS_SUB_DECLARATION_UDT SUB_PG_2 { get; set; }
        public SCREW_GS_PROCESS_SUB_DECLARATION_UDT SUB_PG_3 { get; set; }
        public SCREW_GS_PROCESS_SUB_DECLARATION_UDT SUB_PG_4 { get; set; }
        public SCREW_GS_PROCESS_SUB_DECLARATION_UDT SUB_PG_5 { get; set; }

    }

    
    [PlcType("SCREW_GS_PROCESS_TYPEN_UDT")]
    public class SCREW_GS_PROCESS_TYPEN_UDT
    {
        public Int16 TYPE_NR { get; set; }
        
        [StringLength(254)]
        public string TYPE_NAME { get; set; }

        [ArrayBounds(0,10,0)]
        public SCREW_GS_PROCESS_POINT_DECLARATION_HMI_UDT[] POINT { get; set; }

    }

    [Mapping("++ST000_SCREW_GS_PDB", "++ST000_SCREW_GS_PDB", 0)]
    public class ___ST000_SCREW_GS_PDB
    {

        [ArrayBounds(0,7,0)]
        public SCREW_GS_PROCESS_TYPEN_UDT[] SCREW_TYPES_STORE { get; set; }

    }


}

