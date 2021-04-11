using System;
using System.IO;

namespace StreamDecoration
{
    class Decoration : Stream
    {
        private readonly Stream streamDecorated;
        public delegate void ProgressCheck(double number);
        public event ProgressCheck InProgress;
        public double percent;

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
            //CheckPassword();
            percent = Math.Round((double)streamDecorated.Position / streamDecorated.Length * 100, 0);
            if (percent % 10 == 0)
            {
                InProgress(percent);
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

        /*
         public static void CheckPassword()
         {
             Console.WriteLine("Enter password:");
             string line = Console.ReadLine();
             if (line != "11" || string.Empty == line)
             {
                 throw new ArgumentException("\nWrong password! Acsess denied...");
             }
         }
        */
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
                    decoration.InProgress += (double x) => Console.WriteLine($"Progress is: {decoration.percent} %");
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
        }
    }
}
