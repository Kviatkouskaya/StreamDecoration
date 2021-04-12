using System;
using System.IO;

namespace StreamDecoration
{
    class Decoration : Stream
    {
        private readonly Stream streamDecorated;
        public delegate void ProgressCheck(double number);
        public event ProgressCheck InProgress;
        public delegate void AccessRequest();
        public event AccessRequest AccessCheck;
        public bool accessCondition;
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
            if (streamDecorated.Position == 0)
            {
                AccessCheck();
            }
            if (accessCondition)
            {
                double percent = Math.Round((double)streamDecorated.Position / streamDecorated.Length * 100, 0);
                if (percent % 10 == 0)
                {
                    InProgress(percent);
                }
                return streamDecorated.Read(buffer, offset, count);
            }
            else
            {
                throw new AccessViolationException("Access denied...");
            }
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
            try
            {
                string readPath = @"c:\Users\ollik\source\repos\DecorationPattern.txt";
                using (Decoration decoration = new(new FileStream(readPath, FileMode.Open)))
                {
                    decoration.AccessCheck += () =>
                      {
                          Console.WriteLine("Enter password before reading:");
                          string password = Console.ReadLine();
                          _ = (password == "11") ? decoration.accessCondition = true : decoration.accessCondition = false;
                      };
                    decoration.InProgress += (double percent) => Console.WriteLine($"Progress is: {percent} %");
                    byte[] byteArray = new byte[200];
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
            catch(AccessViolationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
