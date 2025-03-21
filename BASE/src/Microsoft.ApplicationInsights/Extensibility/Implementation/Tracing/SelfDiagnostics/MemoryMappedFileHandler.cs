namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.SelfDiagnostics
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.IO.MemoryMappedFiles;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// MemoryMappedFileHandler open a MemoryMappedFile of a certain size at a certain file path.
    /// The class provides a stream object with proper write position.
    /// The stream is cached in ThreadLocal to be thread-safe.
    /// </summary>
    internal class MemoryMappedFileHandler : IDisposable
    {
        public static readonly byte[] MessageOnNewFile = Encoding.UTF8.GetBytes("Successfully opened file.\n");

        /// <summary>
        /// memoryMappedFileCache is a handle kept in thread-local storage as a cache to indicate whether the cached
        /// viewStream is created from the current m_memoryMappedFile.
        /// </summary>
        private readonly ThreadLocal<MemoryMappedFile> memoryMappedFileCache = new ThreadLocal<MemoryMappedFile>(true);
        private readonly ThreadLocal<MemoryMappedViewStream> viewStream = new ThreadLocal<MemoryMappedViewStream>(true);

#pragma warning disable CA2213 // Disposed in CloseLogFile, which is called in Dispose
        private volatile FileStream underlyingFileStreamForMemoryMappedFile;
        private volatile MemoryMappedFile memoryMappedFile;
#pragma warning restore CA2213 // Disposed in CloseLogFile, which is called in Dispose

        private bool disposedValue;

        private string logDirectory;  // Log directory for log files
        private int logFileSize;  // Log file size in bytes
        private long logFilePosition;  // The logger will write into the byte at this position

        public string LogDirectory { get => this.logDirectory; set => this.logDirectory = value; }

        public int LogFileSize { get => this.logFileSize; private set => this.logFileSize = value; }

        public string CurrentFilePath => this.underlyingFileStreamForMemoryMappedFile?.Name;

        /// <summary>
                // (https://referencesource.microsoft.com/#system.core/System/IO/MemoryMappedFiles/MemoryMappedFile.cs,148)
                // and [.NET Core]
                // (https://github.com/dotnet/runtime/blob/master/src/libraries/System.IO.MemoryMappedFiles/src/System/IO/MemoryMappedFiles/MemoryMappedFile.cs#L152)
                // The parameter for FileAccess is different in type but the same in rules, both are Read and Write.
                // The parameter for FileShare is different in values and in behavior.
                // .NET Framework doesn't allow sharing but .NET Core allows reading by other programs.
                // The last two parameters are the same values for both frameworks.
                // [1]: https://docs.microsoft.com/dotnet/api/system.io.memorymappedfiles.memorymappedfile.createfromfile?view=net-5.0#System_IO_MemoryMappedFiles_MemoryMappedFile_CreateFromFile_System_String_System_IO_FileMode_System_String_System_Int64_
                // [2]: https://docs.microsoft.com/dotnet/api/system.io.memorymappedfiles.memorymappedfile.createfromfile?view=net-5.0#System_IO_MemoryMappedFiles_MemoryMappedFile_CreateFromFile_System_IO_FileStream_System_String_System_Int64_System_IO_MemoryMappedFiles_MemoryMappedFileAccess_System_IO_HandleInheritability_System_Boolean_
                this.underlyingFileStreamForMemoryMappedFile =
                    new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 0x1000, FileOptions.None);

                // The parameter values for MemoryMappedFileSecurity, HandleInheritability and leaveOpen are the same
                // values for .NET Framework and .NET Core:
                // https://referencesource.microsoft.com/#system.core/System/IO/MemoryMappedFiles/MemoryMappedFile.cs,172
                // https://github.com/dotnet/runtime/blob/master/src/libraries/System.IO.MemoryMappedFiles/src/System/IO/MemoryMappedFiles/MemoryMappedFile.cs#L168-L179
                this.memoryMappedFile = MemoryMappedFile.CreateFromFile(
                    this.underlyingFileStreamForMemoryMappedFile,
                    null,
                    fileSize,
                    // default value for MemoryMappedFileSecurity.
                    // https://docs.microsoft.com/dotnet/api/system.io.memorymappedfiles.memorymappedfile.createfromfile?view=netframework-4.5.2
                    // .NET Core simply doesn't support this parameter.
                    null,
#endif
                    HandleInheritability.None,
                    false);
                this.logDirectory = logDirectory;
                this.logFileSize = fileSize;
                this.logFilePosition = 0;
        public void CloseLogFile()
        {
            MemoryMappedFile mmf = Interlocked.CompareExchange(ref this.memoryMappedFile, null, this.memoryMappedFile);
            if (mmf != null)
            {
                // Each thread has its own MemoryMappedViewStream created from the only one MemoryMappedFile.
                // Once worker thread closes the MemoryMappedFile, all the ViewStream objects should be disposed
                // properly.
                foreach (MemoryMappedViewStream stream in this.viewStream.Values)
                {
        {
            var dateTimeStamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);

            var currentProcess = Process.GetCurrentProcess();
            var processFileName = Path.GetFileName(currentProcess.MainModule.FileName);
            var processId = currentProcess.Id;

            return $"{dateTimeStamp}.{processFileName}.{processId}.log";
        }

        /// <summary>
        /// Try to get the log stream which is seeked to the position where the next line of log should be written.
        /// </summary>
        /// <param name="byteCount">The number of bytes that need to be written.</param>
        /// <param name="stream">When this method returns, contains the Stream object where `byteCount` of bytes can be written.</param>
        /// <param name="availableByteCount">The number of bytes that is remaining until the end of the stream.</param>
        /// <returns>Whether the logger should log in the stream.</returns>
        private bool TryGetLogStream(int byteCount, out Stream stream, out int availableByteCount)
        {
            if (this.memoryMappedFile == null)
                        endPosition %= this.logFileSize;
                    }
                }
                while (beginPosition != Interlocked.CompareExchange(ref this.logFilePosition, endPosition, beginPosition));
                availableByteCount = (int)(this.logFileSize - beginPosition);
                cachedViewStream.Seek(beginPosition, SeekOrigin.Begin);
                stream = cachedViewStream;
                return true;
            }
            catch (Exception)
        }

        /// <summary>
        /// Get a MemoryMappedViewStream for the MemoryMappedFile object for the current thread.
        /// If no MemoryMappedFile is created yet, return null.
        /// </summary>
        /// <returns>A MemoryMappedViewStream for the MemoryMappedFile object.</returns>
        /// <exception cref="System.UnauthorizedAccessException">Thrown when access to the memory-mapped file is unauthorized.</exception>
        /// <exception cref="System.NullReferenceException">Thrown in a race condition when the memory-mapped file is closed after null check.</exception>
        private MemoryMappedViewStream GetStream()
        {
            if (this.memoryMappedFile == null)
            {
                return null;
            }

            var cachedViewStream = this.viewStream.Value;
            // Each thread creates a new MemoryMappedViewStream the next time it tries to retrieve it.
            // Whether the MemoryMappedViewStream is obsolete is determined by comparing the current
            // MemoryMappedFile object with the MemoryMappedFile object cached at the creation time of the
            // MemoryMappedViewStream.
            if (cachedViewStream == null || this.memoryMappedFileCache.Value != this.memoryMappedFile)
            {
                // Race condition: The code might reach here right after the worker thread sets memoryMappedFile
                // to null in CloseLogFile().
                // In this case, let the NullReferenceException be caught and fail silently.
                // By design, all events captured will be dropped during a configuration file refresh if
                this.viewStream.Value = cachedViewStream;
                this.memoryMappedFileCache.Value = this.memoryMappedFile;
            }

            return cachedViewStream;
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
