using System;
using System.IO;

namespace StreamDecoration
{
    class Decoration : Stream
    {
        private readonly Stream streamDecorated;
        public delegate void ProgressCheck(double number);
        public event ProgressCheck InProgress;
        public string accessCondition;
        public double readProgress;
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
            if (accessCondition == "pass")
            {
                double percent = readProgress;
                int cnt = 0;
                while (percent % 10 != 0)
                {
                    percent = Math.Round((double)streamDecorated.Position / streamDecorated.Length * 100, 5);
                    cnt += streamDecorated.Read(buffer, offset + cnt, count / 10);
                    if (streamDecorated.Position == streamDecorated.Length)
                    {
                        percent += 10;
                    }
                    if (percent % 10 == 0 && percent != readProgress)
                    {
                        InProgress(percent);
                        readProgress = percent;
                    }
                }
                readProgress += 1;
                return cnt;
            }
            else
            {
                throw new AccessViolationException("Wrong password! Access denied...");
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
        public void RequestAccess()
        {
            Console.WriteLine("Enter password:");
            accessCondition = Console.ReadLine();
        }
    }
    class Program
    {
        static void Main()
        {
            try
            {
                string readPath = @"c:\Books\C_Sharp\YazykProgrCSh7.pdf";
                using (Decoration decoration = new(new FileStream(readPath, FileMode.Open)))
                {
                    decoration.readProgress = -1;
                    decoration.RequestAccess();
                    decoration.InProgress += (double percent) => Console.WriteLine($"Progress is: {percent} %");
                    byte[] byteArray = new byte[decoration.Length];
                    int byteResult = 0;
                    do
                    {
                        byteResult += decoration.Read(byteArray, byteResult, byteArray.Length);
                    }
                    while (byteResult < byteArray.Length);

                    File.WriteAllBytes(@"c:\Books\C_Sharp\New.pdf", byteArray);
                }
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (AccessViolationException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
