namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.PerfLib
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using Microsoft.ApplicationInsights.Common;

    /// <summary>
    /// Represents performance data for a performance object (category).
    /// </summary>
    internal class CategorySample
    {
        public Dictionary<int, CounterDefinitionSample> CounterTable;

        public Dictionary<string, int> InstanceNameTable;

        private readonly PerfLib library;

        /// <summary>Initializes a new instance of the <see cref="CategorySample"/> class. Instantiates a <see cref="CategorySample"/> class.</summary>
        /// <param name="data">Performance data.</param>
        /// <param name="categoryNameIndex">Category name index.</param>
        /// <param name="counterNameIndex">Counter name index.</param>
        /// <param name="library">Performance library.</param>
        public CategorySample(byte[] data, int categoryNameIndex, int counterNameIndex, PerfLib library)
        {
            if (library == null)
            {
                return;
            }

            this.library = library;

            NativeMethods.PERF_DATA_BLOCK dataBlock = new NativeMethods.PERF_DATA_BLOCK();

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                IntPtr dataRef = handle.AddrOfPinnedObject();

                Marshal.PtrToStructure(dataRef, dataBlock);

                dataRef = (IntPtr)((long)dataRef + dataBlock.HeaderLength);

                int numPerfObjects = dataBlock.NumObjectTypes;
                if (numPerfObjects == 0)
                {

                bool isMultiInstance = instanceNumber != -1;

                // Move pointer forward to end of PERF_OBJECT_TYPE
                dataRef = (IntPtr)((long)dataRef + perfObject.HeaderLength);

                CounterDefinitionSample[] samples = new CounterDefinitionSample[counterNumber];
                CounterDefinitionSample sample = null;
                this.CounterTable = new Dictionary<int, CounterDefinitionSample>(counterNumber);
                for (int index = 0; index < samples.Length; ++index)
                    NativeMethods.PERF_COUNTER_DEFINITION perfCounter = new NativeMethods.PERF_COUNTER_DEFINITION();
                    Marshal.PtrToStructure(dataRef, perfCounter);

                    samples[index] = new CounterDefinitionSample(perfCounter, instanceNumber);
                    if (perfCounter.CounterNameTitleIndex == counterNameIndex)
                    {
                        sample = samples[index];
                    }

                    dataRef = (IntPtr)((long)dataRef + perfCounter.ByteLength);
                    if (!CategorySample.IsBaseCounter(currentSampleType))
                    {
                        // We'll put only non-base counters in the table. 
                        if (currentSampleType != NativeMethods.PERF_COUNTER_NODATA)
                        {
                            this.CounterTable[samples[index].NameIndex] = samples[index];
                        }
                    }
                    else
                    {
                    throw new InvalidOperationException("Could not find the counter " + counterNameIndex.ToString(CultureInfo.InvariantCulture));
                }

                // now set up the InstanceNameTable.  
                if (!isMultiInstance)
                {
                    throw new InvalidOperationException("Single instance categories are not supported");
                }

                string[] parentInstanceNames = null;
                                       + Marshal.PtrToStringUni((IntPtr)((long)dataRef + perfInstance.NameOffset));
                    }
                    else
                    {
                        instanceName = Marshal.PtrToStringUni((IntPtr)((long)dataRef + perfInstance.NameOffset));
                    }

                    // in some cases instance names are not unique (Process), same as perfmon, so generate a unique name
                    string newInstanceName = instanceName;
                    int newInstanceNumber = 1;
                        {
                            newInstanceName = instanceName + "#" + newInstanceNumber.ToString(CultureInfo.InvariantCulture);
                            ++newInstanceNumber;
                        }
                    }

                    dataRef = (IntPtr)((long)dataRef + perfInstance.ByteLength);

                    // we only need one counter right now, to get more - use the following pattern:
                    ////foreach (CounterDefinitionSample s in samples)
        }

        private static bool IsBaseCounter(int type)
        {
            return type == NativeMethods.PERF_AVERAGE_BASE || type == NativeMethods.PERF_COUNTER_MULTI_BASE || type == NativeMethods.PERF_RAW_BASE
                   || type == NativeMethods.PERF_LARGE_RAW_BASE || type == NativeMethods.PERF_SAMPLE_BASE;
        }

        private string[] GetInstanceNamesFromIndex(int categoryIndex)
        {
            byte[] data = this.library.GetPerformanceData(categoryIndex.ToString(CultureInfo.InvariantCulture));

            GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                IntPtr dataRef = handle.AddrOfPinnedObject();

                    NativeMethods.PERF_COUNTER_DEFINITION perfCounter = new NativeMethods.PERF_COUNTER_DEFINITION();
                    Marshal.PtrToStructure(dataRef, perfCounter);
                    dataRef = (IntPtr)((long)dataRef + perfCounter.ByteLength);
                }

                string[] instanceNames = new string[instanceNumber];
                for (int i = 0; i < instanceNumber; i++)
                {
                    NativeMethods.PERF_INSTANCE_DEFINITION perfInstance = new NativeMethods.PERF_INSTANCE_DEFINITION();
                    Marshal.PtrToStructure(dataRef, perfInstance);

                    dataRef = (IntPtr)((long)dataRef + perfInstance.ByteLength);
                    dataRef = (IntPtr)((long)dataRef + Marshal.ReadInt32(dataRef));
                }

                return instanceNames;
            }
            finally
            {
                handle.Free();


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
