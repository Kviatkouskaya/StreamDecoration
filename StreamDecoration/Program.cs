using System;
using System.IO;

namespace StreamDecoration
{
    class Decoration: Stream
    {
        private Stream streamDecorated;
        public Decoration(Stream s)
        {
            streamDecorated = s;
        }

        public override bool CanRead=> streamDecorated.CanRead;

        public override bool CanSeek => streamDecorated.CanSeek;

        public override bool CanWrite => streamDecorated.CanWrite;

        public override long Length => streamDecorated.Length;

        public override long Position { get => streamDecorated.Position; 
                                        set => _ = streamDecorated.Position; }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
    class Program
    {
        static void Main()
        {

        }
    }
}
