namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse.Helpers
{
    using System;
    using System.Threading;

    /// <summary>
    /// Quota tracker to support throttling telemetry item collection.
    /// </summary>
    internal class QuickPulseQuotaTracker
    {
        private readonly float quotaAccrualRatePerSec;

        private readonly DateTimeOffset startedTrackingTime;

        private readonly Clock timeProvider;

        private float currentQuota;

        private float maxQuota;

        private long lastQuotaAccrualFullSeconds;
        
        public QuickPulseQuotaTracker(Clock timeProvider, float maxQuota, float startQuota, float? quotaAccrualRatePerSec = null)
        {
        }

        private bool UseQuota()
        {
            var spin = new SpinWait();

            while (true)
            {
                float originalValue = Interlocked.CompareExchange(ref this.currentQuota, 0, 0);
                

                spin.SpinOnce();
            }
        }

        private void AccrueQuota(long currentTimeFullSeconds)

                long fullSecondsSinceLastQuotaAccrual = currentTimeFullSeconds - lastQuotaAccrualFullSecondsLocal;

                // fullSecondsSinceLastQuotaAccrual <= 0 means we're in a second for which some thread has already updated this.lastQuotaAccrualFullSeconds
                if (fullSecondsSinceLastQuotaAccrual > 0)
                {
                    // we are in a new second (possibly along with a bunch of competing threads, some of which might actually be in different (also new) seconds)
                    // only one thread will succeed in updating this.lastQuotaAccrualFullSeconds
                    long newValue = lastQuotaAccrualFullSecondsLocal + fullSecondsSinceLastQuotaAccrual;

                    {
                        // we have updated this.lastQuotaAccrualFullSeconds, now increase the quota value
                        this.IncreaseQuota(fullSecondsSinceLastQuotaAccrual);

                        break;
                    }
                    else if (valueBeforeExchange >= newValue)
                    {
                        // a thread that was in a later (or same) second has beaten us to updating the value
                        // we don't have to do anything since the time that has passed between the previous
                        // update and this thread's current time has already been accounted for by that other thread
                        break;
                    }
                    else
                    {
                        // a thread that was in an earlier second (but still a later one compared to the previous update) has beaten us to updating the value
                        // we have to repeat the attempt to account for the time that has passed since
                    }
                }
                else
                {
                    // we're within a second that has already been accounted for, do nothing
                    break;
                }

                spin.SpinOnce();
            }
        }

        private void IncreaseQuota(long seconds)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
