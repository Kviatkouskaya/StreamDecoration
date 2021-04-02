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

        public void EnterPassword()
        {
            Console.WriteLine("Enter password:");
            string line = Console.ReadLine();
            if (line != "1111"||string.Empty==line)
            {
                throw new ArgumentException("\nWrong password! Acsess denied...");
            }
        }
    }
    class Program
    {
        static void Main()
        {
            try
            {
                string readPath = @"c:\Users\ollik\source\repos\DecorationPattern.txt";
                using (Decoration decoration = new(new FileStream(readPath, FileMode.Open)))
                {
                    decoration.EnterPassword();
                    byte[] byteArray = new byte[500];
                    int byteResult;
                    do
                    {
                        byteResult = decoration.Read(byteArray, 0, byteArray.Length);
                    }
                    while (byteResult != 0);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
