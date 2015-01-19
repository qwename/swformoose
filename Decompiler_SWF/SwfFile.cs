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

namespace Decompiler_SWF
{
    public class SwfFile
    {
        public Stream file_stream;
        public byte[] file_bytes;
        public int index = 0;

        public string NAME = "";
        public string NAME_NOEX = "";
        public string PATH = "";
        public string FULL_PATH = "";
        public string SIZE = "";
        public bool COMPRESSED = false;
        public int VERSION;
        public uint RAW_SIZE;
        public int WIDTH;
        public int HEIGHT;
        public double FRAME_RATE;
        public int FRAME_COUNT;

        public SwfFile(string path)
        {
            FULL_PATH = path;
            NAME = Path.GetFileName(path);
            NAME_NOEX = Path.GetFileNameWithoutExtension(path);
            PATH = Path.GetDirectoryName(path);
        }

        public void parseHeader(Stream strm)
        {
            SWFFileHeader swf = new SWFFileHeader(strm);
            COMPRESSED = swf.m_bCompressed;
            VERSION = swf.m_ui8Version;
            RAW_SIZE = swf.FileLength;
            WIDTH = swf.m_rectFrameSize.WidthInPixels;
            HEIGHT = swf.m_rectFrameSize.HeightInPixels;
            FRAME_RATE = swf.FrameRate;
            FRAME_COUNT = swf.FrameCount;
        }

    }
}
