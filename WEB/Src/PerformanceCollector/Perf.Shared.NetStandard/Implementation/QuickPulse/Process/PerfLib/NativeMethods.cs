namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System.Globalization;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        public const int WAIT_TIMEOUT = 0x00000102;
        
        public const int ERROR_NOT_READY = 21;
        public const int ERROR_LOCK_FAILED = 167;
        public const int ERROR_BUSY = 170;

        // file src\services\monitoring\system\diagnosticts\nativemethods.cs
        public const int PERF_SIZE_DWORD = 0x00000000;
        public const int PERF_SIZE_LARGE = 0x00000100;
        public const int PERF_SIZE_ZERO = 0x00000200;  // for Zero Length fields
                
        //
        //  select one of the following values to indicate the counter field usage
        //
        public const int PERF_TYPE_COUNTER = 0x00000400;  // an increasing numeric value
        
        //
        //  If the PERF_TYPE_COUNTER value was selected then select one of the
        //  following to indicate the type of counter
        // played to the user.
        public const int PERF_AVERAGE_BASE =
                (PERF_SIZE_DWORD | PERF_TYPE_COUNTER | PERF_COUNTER_BASE |
                PERF_DISPLAY_NOSHOW |
                0x00000002);  // for compatibility with pre-beta versions
        
        // Number of instances to which the preceding _MULTI_..._INV counter
        // applies.  Used as a factor to get the percentage.
        public const int PERF_COUNTER_MULTI_BASE =
                (PERF_SIZE_LARGE | PERF_TYPE_COUNTER | PERF_COUNTER_BASE |
                PERF_MULTI_COUNTER | PERF_DISPLAY_NOSHOW);
        
        // Indicates the data is a base for the preceding counter which should
        // not be time averaged on display (such as free space over total space.)
        public const int PERF_RAW_BASE =
                (PERF_SIZE_DWORD | PERF_TYPE_COUNTER | PERF_COUNTER_BASE |
                PERF_DISPLAY_NOSHOW |
                0x00000003);  // for compatibility with pre-beta versions

        public const int PERF_LARGE_RAW_BASE =
            // #else
            //  LPWSTR          CounterNameTitle;
            // #endif
            // so we can't use IntPtr here.

            public int CounterNameTitlePtr = 0;
            public int CounterHelpTitleIndex = 0;
            public int CounterHelpTitlePtr = 0;
            public int DefaultScale = 0;
            public int DetailLevel = 0;
            public int CounterType = 0;
            public int CounterSize = 0;
            public int CounterOffset = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_DATA_BLOCK
        {
            public int Signature1 = 0;
            public int Signature2 = 0;
            public int LittleEndian = 0;
            public int Version = 0;
            public int Revision = 0;
            public int TotalByteLength = 0;
            public int HeaderLength = 0;
            public int NumObjectTypes = 0;
            public int DefaultObject = 0;
            public SYSTEMTIME SystemTime = null;
            public int pad1 = 0;  // Need to pad the struct to get quadword alignment for the 'long' after SystemTime
            public long PerfTime = 0;
            public long PerfFreq = 0;
            public long PerfTime100nSec = 0;
            public int SystemNameLength = 0;
            public int SystemNameOffset = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_INSTANCE_DEFINITION
        {
            public int ByteLength = 0;
            public int ParentObjectTitleIndex = 0;
            public int ParentObjectInstance = 0;
            public int UniqueID = 0;
            public int NameOffset = 0;
            public int NameLength = 0;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal class PERF_OBJECT_TYPE
        {
            public long PerfFreq = 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Legacy suppression")]
        [StructLayout(LayoutKind.Sequential)]
        internal class SYSTEMTIME


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
