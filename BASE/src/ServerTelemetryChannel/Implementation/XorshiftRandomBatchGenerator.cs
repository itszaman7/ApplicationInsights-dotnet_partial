namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Generates batches of random number using Xorshift algorithm
    /// Note: the base code is from http://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Xorshift is a well-known algorithm name")]
    internal class XorshiftRandomBatchGenerator : IRandomNumberBatchGenerator
        /// Initializes a new instance of the <see cref="XorshiftRandomBatchGenerator"/> class.
        /// </summary>
        /// <param name="seed">Random generator seed value.</param>
        public XorshiftRandomBatchGenerator(ulong seed)
        {
            this.lastX = (seed * 5073061188973594169L) + (seed * 8760132611124384359L) + (seed * 8900702462021224483L)
                         + (seed * 6807056130438027397L);

            this.lastY = Y;
            this.lastZ = Z;

        /// <summary>
        /// Generates a batch of random numbers.
        /// </summary>
        /// <param name="buffer">Buffer to put numbers in.</param>
        {
            ulong x = this.lastX;
            ulong y = this.lastY;
            ulong z = this.lastZ;
            ulong w = this.lastW;
            for (int i = 0; i < count; i++)
            {
                ulong t = x ^ (x << 11);
                x = y;
                y = z;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                buffer[index + i] = w;
            }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
