using System;
using System.IO;

namespace StreamDecoration
{
    class Decoration : Stream
    {
        private Stream streamDecorated;
        public Decoration(Stream s)
        {
            streamDecorated = s;
        }

        public override bool CanRead => streamDecorated.CanRead;

        public override bool CanSeek => streamDecorated.CanSeek;

        public override bool CanWrite => streamDecorated.CanWrite;

        public override long Length => streamDecorated.Length;

        public override long Position
        {
            get => streamDecorated.Position;
            set => _ = streamDecorated.Position;
        }

        public override void Flush()
        {
            streamDecorated.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (streamDecorated.Length != streamDecorated.Position)
            {
                Console.WriteLine($"\nProgress is: {Math.Round((double)streamDecorated.Position / streamDecorated.Length * 100, 2)}%");
            }
            else
            {
                Console.WriteLine($"\nProgress is: 100%");
            }
            return streamDecorated.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return streamDecorated.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            streamDecorated.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            streamDecorated.Write(buffer, offset, count);
        }
    }
    class Program
    {
        static void Main()
        {
            string readPath = @"c:\Users\ollik\source\repos\DecorationPattern.txt";
            using (Decoration decoration = new(new FileStream(readPath, FileMode.Open)))
            {
                byte[] byteArray = new byte[500];
                int byteResult;
                do
                {
                    byteResult = decoration.Read(byteArray, 0, byteArray.Length);
                }
                while (byteResult != 0);
            }
        }
    }
}
