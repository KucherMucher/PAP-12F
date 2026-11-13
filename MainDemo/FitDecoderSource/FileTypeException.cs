using System;
using System.Collections.Generic;
using System.Text;

namespace FitDecoderSource
{
    public class FileTypeException : Exception
    {
        public FileTypeException() { }

        public FileTypeException(string message) : base(message) { }

        public FileTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
