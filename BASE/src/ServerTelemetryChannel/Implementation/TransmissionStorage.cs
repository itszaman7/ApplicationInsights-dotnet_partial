namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Microsoft.ApplicationInsights.Channel;

    using TaskEx = System.Threading.Tasks.Task;

    internal class TransmissionStorage : IDisposable
    {
        internal const string TemporaryFileExtension = ".tmp";
        internal const string TransmissionFileExtension = ".trn";
        internal const int DefaultCapacityKiloBytes = 50 * 1024;

        private readonly ConcurrentDictionary<string, string> badFiles;
        private readonly ConcurrentQueue<IPlatformFile> files;
        private readonly object loadFilesLock;

        private IPlatformFolder folder;
        private long capacity = DefaultCapacityKiloBytes * 1024;
        private long size;
        private bool sizeCalculated;
        private Random random = new Random();
        private Timer clearBadFiles;
        // Storage dequeue is not permitted with FlushAsync
        // When this counter is set, it blocks storage dequeue
        private long flushAsyncInProcessCounter = 0;

        public TransmissionStorage()
        {
            this.files = new ConcurrentQueue<IPlatformFile>();
            this.badFiles = new ConcurrentDictionary<string, string>();
            TimeSpan clearBadFilesInterval = new TimeSpan(29, 0, 0); // Arbitrarily aligns with IIS restart policy of 29 hours in case IIS isn't restarted.
            this.clearBadFiles = new Timer((o) => this.badFiles.Clear(), null, clearBadFilesInterval, clearBadFilesInterval);
            this.loadFilesLock = new object();
            this.sizeCalculated = false;
        }

        /// <summary>
        /// Gets or sets the total amount of disk space, in bytes, allowed for storing transmission files.
        /// </summary>
        public virtual long Capacity
        {
            get
            {
                return this.capacity;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.capacity = value;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public virtual void Initialize(IApplicationFolderProvider applicationFolderProvider)
        {
            if (applicationFolderProvider == null)
            {
                throw new ArgumentNullException(nameof(applicationFolderProvider));
            }

            this.folder = applicationFolderProvider.GetApplicationFolder();
        }

        public virtual bool Enqueue(Func<Transmission> transmissionGetter)
        {
            if (this.folder == null)
            {
                return false;
            }

            this.EnsureSizeIsCalculated();

            if (this.size < this.Capacity)
            {
                var transmission = transmissionGetter();
                if (transmission == null)
                {
                    return false;
                }

                    {
                        file = this.GetOldestTransmissionFileOrNull();
                        if (file == null)
                        {
                            return null; // Because there are no more transmission files.
                        }

                        long fileSize;
                        Transmission transmission = LoadFromTransmissionFile(file, out fileSize);
                        if (transmission != null)
                }
                catch (IOException ioe)
                {
                    // This exception can happen when one thread runs out of files to process and reloads the list while another
                    // thread is still processing a file and has not deleted it yet thus allowing it to get in the list again.
                    TelemetryChannelEventSource.Log.TransmissionStorageDequeueIOError(file.Name, ioe.ToString());
                    Thread.Sleep(this.random.Next(1, 100)); // Sleep for random time of 1 to 100 milliseconds to try to avoid future timing conflicts.
                    continue; // It may be because another thread already loaded this file, we don't know yet.
                }
            }
        }

        internal void IncrementFlushAsyncCounter()
        {
            Interlocked.Increment(ref this.flushAsyncInProcessCounter);
        }

        internal void DecrementFlushAsyncCounter()
        {
            {
                // The ingestion service rejects anything older than 2 days.
                if (file.DateCreated > DateTimeOffset.Now.AddDays(-2)) 
                {
                    ChangeFileExtension(file, TemporaryFileExtension);
                    transmission = LoadFromTemporaryFile(file, out fileSize);
                }
                else
                {
                    TelemetryChannelEventSource.Log.TransmissionStorageFileExpired(file.Name, file.DateCreated.ToString(CultureInfo.InvariantCulture));

        private static long SaveTransmissionToFile(Transmission transmission, IPlatformFile file)
        {
            using (Stream stream = file.Open())
            {
                transmission.Save(stream);
                return stream.Length;
            }
        }

        private IPlatformFile CreateTemporaryFile()
        {
            string temporaryFileName = GetUniqueFileName(TemporaryFileExtension);
            return this.folder.CreateFile(temporaryFileName);
        }

        private IEnumerable<IPlatformFile> GetTransmissionFiles()
        {
            IEnumerable<IPlatformFile> newFiles = this.folder.GetFiles();
            newFiles = newFiles.Where(f => f.Extension == TransmissionFileExtension);
            return newFiles;
        }

        private IPlatformFile GetOldestTransmissionFileOrNull()
        {
            IPlatformFile file;
            if (!this.files.TryDequeue(out file))
            {
                this.LoadFilesOrderedByDateFromFolder();
                this.files.TryDequeue(out file);
        }

        private void LoadFilesOrderedByDateFromFolder()
        {
            if (this.files.IsEmpty)
            {
                lock (this.loadFilesLock)
                {
                    if (this.files.IsEmpty)
                    {
        {
            if (!this.sizeCalculated)
            {
                lock (this.loadFilesLock)
                {
                    if (!this.sizeCalculated)
                    {
                        try
                        {
                            var storageFiles = this.GetTransmissionFiles();

        private void Dispose(bool disposing)
        {
            if (disposing && this.clearBadFiles != null)
            {
                this.clearBadFiles.Dispose();
                this.clearBadFiles = null;
            }
        }
    }


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
