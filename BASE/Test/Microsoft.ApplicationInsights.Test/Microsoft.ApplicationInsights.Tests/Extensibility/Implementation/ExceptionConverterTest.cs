namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.TestFramework;
    using Moq;

    /// <summary>
    /// Tests of exception stack serialization.
    /// </summary>
    [TestClass]
    public class ExceptionConverterTest
    {
        [TestMethod]
        public void CallingConvertToExceptionDetailsWithNullExceptionThrowsArgumentNullException()
        {
            AssertEx.Throws<ArgumentNullException>(() => ExceptionConverter.ConvertToExceptionDetails(null, null));
        }

        [TestMethod]
        public void EmptyStringIsReturnedForExceptionWithoutStack()
        {
            var exp = new ArgumentException();

            Assert.AreEqual(string.Empty, expDetails.stack);
            Assert.AreEqual(0, expDetails.parsedStack.Count);

            // hasFullStack defaults to true.
            Assert.IsTrue(expDetails.hasFullStack);
        }

        [TestMethod]
        public void AllStackFramesAreConvertedIfSizeOfParsedStackIsLessOrEqualToMaximum()
        {
            frameMock.Setup(x => x.GetMethod()).Returns((MethodBase)null);

            External.StackFrame stackFrame = null;

            try
            {
                stackFrame = ExceptionConverter.GetStackFrame(frameMock.Object, 0);
            }
            catch (Exception e)
            {

            if (line != 0)
            {
                Assert.AreEqual(line, stack[0].line);
                Assert.AreEqual(fileName, stack[0].fileName);
            }
            else
            {
                Assert.AreEqual(0, stack[0].line);
                Assert.IsNull(stack[0].fileName);

        [TestMethod]
        public void CheckLevelCorrespondsToFrameForLongStack()
        {
            const int NumberOfStackFrames = 100;

            var exp = this.CreateException(NumberOfStackFrames - 1);

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);
            var stack = expDetails.parsedStack;

            // Checking levels for first few and last few.
            for (int i = 0; i < 10; ++i)
            {
                Assert.AreEqual(i, stack[i].level);
            }

            for (int j = NumberOfStackFrames - 1, i = 0; j > NumberOfStackFrames - 10; --j, i++)
            {
                Assert.AreEqual(j, stack[stack.Count - 1 - i].level);
            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);
            int parsedStackLength = 0;

            var stack = expDetails.parsedStack;
            for (int i = 0; i < stack.Count; i++)
            {
                parsedStackLength += (stack[i].method == null ? 0 : stack[i].method.Length)
                                     + (stack[i].assembly == null ? 0 : stack[i].assembly.Length)
                                     + (stack[i].fileName == null ? 0 : stack[i].fileName.Length);
            }
            Assert.IsTrue(parsedStackLength <= ExceptionConverter.MaxParsedStackLength);
        }

        [TestMethod]
        public void SanitizesLineNumberOnParsedStackFrame()
        {
            var stackFrame = ExceptionConverter.GetStackFrame(new System.Diagnostics.StackFrame("test", 1000001), 0);
            
            Assert.AreEqual(0, stackFrame.line);


            Assert.AreEqual(10, stackFrame.line);
        }

        [TestMethod]
            var exp = new Exception(new string('x', 5));

            ExceptionDetails expDetails = ExceptionConverter.ConvertToExceptionDetails(exp, null);

            Assert.AreEqual(5, expDetails.message.Length);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
                exception = exp;
            }

            return exception;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private void FailedFunction(int numberOfStackpoints)
        {
            if (numberOfStackpoints > 1)
            public ExceptionWithMessageOverride(string message)
                : base(message)
            {
                Message = message;
            }
            
            public override string Message { get; }
        }
    }
}

# This file contains partial code from the original project
# Some functionality may be missing or incomplete
