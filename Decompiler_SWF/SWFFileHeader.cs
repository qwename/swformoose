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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;


namespace Decompiler_SWF
{
    //The Byte value type represents unsigned integers with values ranging from 0 to 255.
    //http://msdn.microsoft.com/en-us/library/system.byte(VS.71).aspx
    using UI8 = System.Byte;
    using UI16 = System.UInt16;
    using UI32 = System.UInt32;

    class SWFFileHeader
    {
        public bool m_bCompressed = false;
        public UI8 m_ui8Version = 0;
        public UI32 m_ui32FileLength = 0;
        public RECT m_rectFrameSize = null;
        public double m_dFrameRate = 0;
        public UI16 m_ui16FrameCount = 0;
        public string m_strSignature = "";
        public SWFFileHeader(Stream strmReader)
        {
            UI8 ui8SigByteOne = ReadUI8(strmReader);
            UI8 ui8SigByteTwo = ReadUI8(strmReader);
            UI8 ui8SigByteThree = ReadUI8(strmReader);       
            

            if (ui8SigByteOne == (byte)'F' || ui8SigByteOne == (byte)'C')
            {
                if (ui8SigByteOne == (byte)'C') m_bCompressed = true;
                if (ui8SigByteTwo == (byte)'W' && ui8SigByteThree == (byte)'S')
                {
                    m_ui8Version = ReadUI8(strmReader);
                    m_ui32FileLength = ReadUI32(strmReader);
                    if (m_bCompressed)
                    {
                        m_strSignature = "CWS";
                        strmReader.ReadByte();
                        strmReader.ReadByte();
                        DeflateStream zipStream = new DeflateStream(strmReader, CompressionMode.Decompress);
                        strmReader = zipStream;
                    }
                    else
                    {
                        m_strSignature = "FWS";
                    }
                    m_rectFrameSize = new RECT(strmReader);
                    m_dFrameRate = ReadFrameRate(strmReader);
                    m_ui16FrameCount = ReadUI16(strmReader);

                    strmReader.Close();
                }
                else
                {
                    throw new Exception("Invalid file format");
                }

            }
            else
            {
                throw new Exception("Invalid file format");
            }
        }

        public UI16 FrameCount
        {
            get { return m_ui16FrameCount; }
        }

        private double ReadFrameRate(Stream strmSource)
        {
            double m_dResult = 0;
            byte byTemp = (byte)strmSource.ReadByte();
            m_dResult += strmSource.ReadByte();

            for (int nIndex = 0; nIndex < 8; nIndex++)
            {
                if ((byTemp & 128) == 128)
                {
                    m_dResult += Math.Pow(2,-(nIndex+1));
                }
                byTemp <<= 1;
            }
            return m_dResult;
        }

        public double FrameRate
        {
            get { return m_dFrameRate; }
        }
        public UI32 FileLength
        {
            get { return m_ui32FileLength; }
        }
        public string Version
        {
            get { return m_ui8Version.ToString(); }
        }
        public string Signature
        {
            get { return m_strSignature; }
        }
        public UI16 ReadUI16(Stream strmSource)
        {
            UI16 ui16Result = 0;

            ui16Result |= (UInt16)strmSource.ReadByte();
            ui16Result |= (UInt16)(strmSource.ReadByte() << 8);

            return ui16Result;
        }

        private UI32 ReadUI32(Stream strmSource)
        {
            UI32 ui32Result = 0;
            ui32Result |= (UI32)strmSource.ReadByte();
            ui32Result |= (UI32)(strmSource.ReadByte() << 8);
            ui32Result |= (UI32)(strmSource.ReadByte() << 16);
            ui32Result |= (UI32)(strmSource.ReadByte() << 24);
            return ui32Result;        
        }
        public static UI8 ReadUI8(Stream strmSource)
        {            
            return (UI8) strmSource.ReadByte();
        }

        public RECT FrameSize
        {
            get { return m_rectFrameSize; }
        }

        public class RECT
        {
            byte m_byNbits = 0;
            int m_Xmin = 0;
            int m_Xmax = 0;
            int m_Ymin = 0;
            int m_Ymax = 0;
            public RECT(Stream strmReader)
            {
                int nBitcount = 0,nCurrentValue = 0,nCurrentBit = 2;
                byte byTemp = SWFFileHeader.ReadUI8(strmReader) ;
                m_byNbits = (byte) ((int)byTemp >> 3);
                byTemp &= 7;
                byTemp <<= 5;
                for (int nIndex = 0; nIndex < 4; nIndex++)
                {                    
                    while (nBitcount < m_byNbits)
                    {
                        if ((byTemp & 128) == 128)
                        {
                            nCurrentValue += 1 << (m_byNbits - nBitcount - 1);
                        }
                        byTemp <<= 1;
                        byTemp &= 255;
                        nCurrentBit--;
                        nBitcount++;
                        if (nCurrentBit < 0)
                        {
                            byTemp = SWFFileHeader.ReadUI8(strmReader);
                            nCurrentBit = 7;
                        }
                    }
                   
                    switch (nIndex)
                    {
                        case 0:
                            m_Xmin = nCurrentValue;
                            break;
                        case 1:
                            m_Xmax = nCurrentValue;
                            break;
                        case 2:
                            m_Ymin = nCurrentValue;
                            break;
                        case 3:
                            m_Ymax = nCurrentValue;
                            break;
                    }

                    nBitcount = 0;
                    nCurrentValue = 0;
                }

            }

            public int XMinInTwips
            {
                get { return m_Xmin; }
            }

            public int XMinInPixels
            {
                get { return XMinInTwips / 20; }
            }

            public int XMaxInTwips
            {
                get { return m_Xmax; }
            }

            public int XMaxInPixels
            {
                get { return XMaxInTwips / 20; }
            }

            public int YMinInTwips
            {
                get { return m_Ymin; }
            }

            public int YMinInPixels
            {
                get { return YMinInTwips / 20; }
            }

            public int YMaxInTwips
            {
                get { return m_Ymax; }
            }

            public int YMaxInPixels
            {
                get { return YMaxInTwips / 20; }
            }

            public int WidthInTwips
            {
                get { return m_Xmax - m_Xmin; }
            }

            public int WidthInPixels
            {
                get { return WidthInTwips / 20; }
            }

            public int HeightInTwips
            {
                get { return m_Ymax - m_Ymin; }
            }

            public int HeightInPixels
            {
                get { return HeightInTwips / 20; }
            }
        }
    }
}
