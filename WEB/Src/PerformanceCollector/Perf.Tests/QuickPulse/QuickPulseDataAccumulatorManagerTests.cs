namespace Microsoft.ApplicationInsights.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.ApplicationInsights.Extensibility.Filtering;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
            CollectionConfiguration collectionConfiguration =
                new CollectionConfiguration(
                    new CollectionConfigurationInfo() { ETag = string.Empty, Metrics = new CalculatedMetricInfo[0] },
                    out errors,
                    new ClockMock());
            var accumulatorManager = new QuickPulseDataAccumulatorManager(collectionConfiguration);
            accumulatorManager.CurrentDataAccumulator.AIRequestSuccessCount = 5;
            
        {
            // ARRANGE
            CollectionConfigurationError[] errors;
            CollectionConfiguration collectionConfiguration =
                new CollectionConfiguration(
                    new CollectionConfigurationInfo() { ETag = string.Empty, Metrics = new CalculatedMetricInfo[0] },
                    out errors,
                    new ClockMock());
            var writeTasks = new List<Task>(taskCount);
            var pause = TimeSpan.FromMilliseconds(10);

            for (int i = 0; i < taskCount; i++)
            {
                var task = new Task(() =>
                {
                    Interlocked.Increment(ref accumulatorManager.CurrentDataAccumulator.AIRequestSuccessCount);
                    Interlocked.Increment(ref accumulatorManager.CurrentDataAccumulator.AIDependencyCallSuccessCount);
                });

                writeTasks.Add(task);
            }

            var completionTask = new Task(() =>
            {
                // sleep to increase the probability of more write tasks being between the two writes
                Thread.Sleep(TimeSpan.FromTicks(pause.Ticks / 2));

            // ACT
            var sample1 = accumulatorManager.CurrentDataAccumulator;

            var result = Parallel.For(0, writeTasks.Count, new ParallelOptions() { MaxDegreeOfParallelism = taskCount }, i => writeTasks[i].RunSynchronously());

            while (!result.IsCompleted)
            {
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
