using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigFilesSort.Models
{
    public static class FileSorter
    {
        public static void Sort(string path, int readBufferSize, int mainBufferSize)
        {
            mainBufferSize = (mainBufferSize / 2) * 2;

            using (var FS = new FileStream(path,
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite,
                readBufferSize))
            {
                Sort(FS, mainBufferSize);
            }
        }
        public static void Sort(FileStream FS, int mainBufferSize)
        {
            var buffer = new byte[256];
            var intBuffer = new int[mainBufferSize];
            var digCount = GetCount(FS);
            FS.Position = 0;

            while (FS.Position < FS.Length)
            {
                SkipNotDigit(FS);
                var startPos = FS.Position;
                int count = ReadToBuffer(FS, intBuffer, 0, mainBufferSize);

                Array.Sort(intBuffer, 0, count);

                FS.Position = startPos;

                WriteFromBuffer(FS, intBuffer, 0, count);
            }


            int half = mainBufferSize / 2;
            int digStart = (digCount / half) * half;
            if (digCount % half == 0)
                digStart -= half;

            FS.Position = 0;
            SkipDigits(FS, digStart);
            int endCount = ReadToBuffer(FS, intBuffer, half, half);

            while (digStart >= half)
            {
                long writePos = 0;
                long readPos = 0;
                long digPos = 0;
                FS.Position = readPos;
                while (digPos < digStart)
                {
                    ReadToBuffer(FS, intBuffer, 0, half);

                    Marge(intBuffer, 0, half, endCount);

                    readPos = FS.Position;
                    FS.Position = writePos;
                    WriteFromBuffer(FS, intBuffer, 0, half);
                    writePos = FS.Position;
                    FS.Position = readPos;
                    digPos += half;
                }

                FS.Position = writePos;
                WriteFromBuffer(FS, intBuffer, half, endCount);
                for (int i = 0; i < half; i++)
                    intBuffer[i + half] = intBuffer[i];
                endCount = half;
                digStart -= half;
            }
        }


        private static int GetCount(FileStream FS)
        {
            bool d = false;
            int readRes;
            int res = 0;
            while ((readRes = FS.ReadByte()) != -1)
            {
                char с = Convert.ToChar((byte)readRes);

                if (char.IsDigit(с))
                    d = true;
                else
                if (d)
                {
                    res++;
                    d = false;
                }
            }

            if (d)
            {
                res++;
            }

            return res;
        }
        private static void SkipNotDigit(FileStream FS)
        {
            int res;
            do
            {
                res = FS.ReadByte();
            } while (res != -1 && !char.IsDigit(Convert.ToChar((byte)res)));

            FS.Position--;
        }
        private static void SkipDigits(FileStream FS, int count)
        {
            bool d = false;
            int readRes;

            while (count > 0 && (readRes = FS.ReadByte()) != -1)
            {
                char с = Convert.ToChar((byte)readRes);

                if (char.IsDigit(с))
                    d = true;
                else
                if (d)
                {
                    count--;
                    d = false;
                }
            }

        }
        private static int ReadToBuffer(FileStream FS, int[] intBuffer, int offset, int count)
        {
            int i = 0;
            var val = "";
            int readRes = -1;

            while (i < count && (readRes = FS.ReadByte()) != -1)
            {
                char с = Convert.ToChar((byte)readRes);

                if (char.IsDigit(с))
                    val += с;
                else
                if (val != "")
                {
                    intBuffer[offset + i++] = int.Parse(val);
                    val = "";
                }
            }

            if (readRes == -1 && val != "")
            {
                intBuffer[offset + i++] = int.Parse(val);
            }

            return i;
        }
        private static void WriteFromBuffer(FileStream FS, int[] intBuffer, int offset, int count)
        {
            var buffer = new byte[256];
            for (int i = offset; i < count + offset; i++)
            {
                var val = $"{intBuffer[i]}\r\n";
                int size = Encoding.UTF8.GetBytes(val, 0, val.Length, buffer, 0);
                FS.Write(buffer, 0, size);
            }
        }
        private static void Marge(int[] buffer, int aStart, int bStart, int bSize)
        {
            Array.Sort(buffer, aStart, bStart - aStart + bSize);
        }

    }
}
