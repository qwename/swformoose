/*
Copyright 2015 Yixin Zhang

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

﻿using System;
using System.IO;
using System.IO.Compression;
using ComponentAce.Compression.Libs.zlib;

namespace Decompiler_SWF
{
    class ByteReader
    {
        public const int MEGA = 1048576;
        public const int KILO = 1024;

        public static UInt16 ReverseInt16(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        public byte[] DecompressData(byte[] inData)
        {
            byte[] header = getByteArray(inData, 0, 7);
            header[0] = StringToByteArray("46")[0];
            inData = getByteArray(inData, 8);
            byte[] decompressed;
            using (MemoryStream output = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(output))
            using (Stream input = new MemoryStream(inData))
            {
                CopyStream(input, outZStream);
                decompressed = output.ToArray();
            }
            byte[] final = new byte[header.Length + decompressed.Length];
            Buffer.BlockCopy(header, 0, final, 0, header.Length);
            Buffer.BlockCopy(decompressed, 0, final, header.Length, decompressed.Length);
            return final;
        }

        public byte[] CompressData(byte[] inData)
        {
            byte[] header = getByteArray(inData, 0, 7);
            header[0] = StringToByteArray("43")[0];
            inData = getByteArray(inData, 8);
            byte[] compressed;
            using (MemoryStream outMemoryStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outMemoryStream, zlibConst.Z_DEFAULT_COMPRESSION))
            using (Stream inMemoryStream = new MemoryStream(inData))
            {
                CopyStream(inMemoryStream, outZStream);
                outZStream.finish();
                compressed = outMemoryStream.ToArray();
            }
            byte[] final = new byte[header.Length + compressed.Length];
            Buffer.BlockCopy(header, 0, final, 0, header.Length);
            Buffer.BlockCopy(compressed, 0, final, header.Length, compressed.Length);
            return final;
        }

        public byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-","");
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public byte[] readBytes(byte[] b, int start, int length)
        {
            using (Stream byte2 = new MemoryStream(b))
            {
                using (BinaryReader editor = new BinaryReader(byte2))
                {
                    editor.BaseStream.Position = start;
                    return editor.ReadBytes(length);
                }
            }
        }

        public int readByte(byte[] b, int pos)
        {
            using (Stream byte2 = new MemoryStream(b))
            {
                using (BinaryReader editor = new BinaryReader(byte2))
                {
                    editor.BaseStream.Position = pos;
                    return editor.ReadByte();
                }
            }
        }

        public int readInt16(byte[] b, int pos)
        {
            using (Stream byte2 = new MemoryStream(b))
            {
                using (BinaryReader editor = new BinaryReader(byte2))
                {
                    editor.BaseStream.Position = pos;
                    //if (BitConverter.IsLittleEndian)
                        //return ReverseInt16(editor.ReadUInt16()); << only when changing back to swf data
                    return editor.ReadInt16();
                }
            }
        }

        public int readInt32(byte[] b, int pos)
        {
            using (Stream byte2 = new MemoryStream(b))
            {
                using (BinaryReader editor = new BinaryReader(byte2))
                {
                    editor.BaseStream.Position = pos;
                    //if (BitConverter.IsLittleEndian)
                        //return Reverse(BitConverter.ToInt32(b, index), 32); crapped
                    return editor.ReadInt32();
                }
            }
        }

        public byte[] getBytes(Stream b)
        {
            return ReadToEnd(b);
        }

        public byte[] getByteArray(byte[] b, int start, int end = 0x7FFFFFF)
        {
            end = (end == 0x7FFFFFF) ? b.Length - 1 : end;
            byte[] final = new byte[end - start + 1];
            for (int i = 0; i < (end - start + 1) && end < b.Length; i++)
            {
                final[i] = b[i + start];
            }
            return final;
        }

        public string getSize(byte[] by)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = by.Length;
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            return result;
        }

        public string getSizeInt(uint by)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = by;
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            return result;
        }

        public string toHex(int x)
        {
            return "0x" + x.ToString("X");
        }

        public string toASCII(byte[] b)
        {
            return System.Text.Encoding.GetEncoding(1251).GetString(b, 0, b.Length);
        }

        public int[] getDec(byte[] by, int pos = 0, int length = 1)
        {
            if (length - pos <= by.Length && length > 0 && pos >= 0)
            {
                int[] final = new int[length];
                for (int i = 0; i < length; i++)
                {
                    final[i] = by[i+pos];
                }
                return final;

            }
            return null;
        }

        public string getHex(byte[] by, int pos = 0, int length = 1)
        {
            if (length - pos <= by.Length && length > 0 && pos >= 0)
            {
                String final = "";
                for (int i = 0; i < length; i++)
                {
                    final += by[i+pos].ToString("X2") + " ";
                }
                return final;
            }
            return "Error: getHex";
        }

        public string getASCII(byte[] by, int pos = 0, int length = 1)
        {
            if (length - pos <= by.Length && length > 0 && pos >= 0)
            {
                return System.Text.Encoding.GetEncoding(1251).GetString(by, pos, length);
            }
            return "Error: getASCII";
        }

        private static byte[] ReadToEnd(Stream stream)
        {
            long originalPosition = stream.Position;
            stream.Position = 0;

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }

    }
}
