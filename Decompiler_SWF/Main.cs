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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Decompiler_SWF
{
    public partial class Main : Form
    {
        private ByteReader r = new ByteReader();
        private bool VERBOSE = true;
        List<string> _swf = new List<string>();
        Dictionary<string, SwfFile> _files = new Dictionary<string, SwfFile>();
        public SwfFile current;

        public Main(string[] args)
        {
            InitializeComponent();
            this.DragDrop += new DragEventHandler(Main_DragDrop);
            this.DragEnter += new DragEventHandler(Main_DragEnter);
            swfList.SelectedIndexChanged += new EventHandler(swfList_SelectedIndexChanged);
            if (args.Length > 0)
            {
                foreach (string s in args)
                {
                    using (FileStream file = File.OpenRead(s))
                    {
                        newSwfFile(s, file);
                    }
                }
            }
        }

        private void swfList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (swfList.Items.Count > 0)
            {
                if (current != _files[_swf[swfList.SelectedIndex]])
                {
                    current = _files[_swf[swfList.SelectedIndex]];
                    outPutFileInfo(current);
                }
                else
                    outPutFileInfo(current);
            }
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effect = DragDropEffects.Copy;
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                outputFileInfo(files);
            }
        }

        private void outputFileInfo(string[] files)
        {
            foreach (string file in files)
            {
                if (Path.GetExtension(file) == ".swf" || Path.GetExtension(file) == ".dat")
                {
                    FileInfo fileInfo = new FileInfo(file);
                    using (FileStream bytes = fileInfo.OpenRead())
                    {
                        newSwfFile(fileInfo.FullName, bytes);
                    }
                    //outPutFileInfo(current);
                }
            }
        }

        public void newSwfFile(string path, Stream fileStream)
        {
            if (_files.ContainsKey(path))
            {
                _files.Remove(path);
                _swf.Remove(path);
                _files.Add(path, new SwfFile(path));
            }
            else
                _files.Add(path, new SwfFile(path));
            current = _files[path];
            current.file_stream = fileStream;
            current.file_bytes = r.getBytes(fileStream);
            if (r.getASCII(current.file_bytes, 0, 3) != "FWS" && r.getASCII(current.file_bytes, 0, 3) != "CWS")
            {
                log("Invalid swf file.");
                return;
            }
            current.parseHeader(current.file_stream);
            current.SIZE = r.getSizeInt(current.RAW_SIZE);
            updateBtns();
            fileSaver.FileName = current.NAME_NOEX + "_new";
            Save.Enabled = true;
            _swf.Add(path);
            swfList.Items.Add(path);
            swfList.SetSelected(swfList.Items.Count - 1, true);
        }

        private void updateBtns()
        {
            if (current.COMPRESSED)
            {
                decompressBtn.Enabled = true;
                compressBtn.Enabled = false;
            }
            else
            {
                compressBtn.Enabled = true;
                decompressBtn.Enabled = false;
            }
        }

        private void openFile_Click(object sender, EventArgs e)
        {
            DialogResult result = fileOpener.ShowDialog();
            if (result == DialogResult.OK)
            {
                using (Stream fileBytes = fileOpener.OpenFile())
                {
                    newSwfFile(fileOpener.FileName, fileBytes);
                }
                //outPutFileInfo(current);
                /*
                 * Multiple Files?
                 * foreach (String file in fileOpener.FileNames)
                {
                    log(file);
                }*/
            }
            else if (result == DialogResult.Cancel)
                log("Operation Cancelled.");
        }

        public void outPutFileInfo(SwfFile f)
        {
            clear();
            updateBtns();
            log("");
            log(f.FULL_PATH);
            log("File Size: " + f.SIZE.ToString());
            log("File compressed: " + f.COMPRESSED.ToString());
            log("Version Number: " + f.VERSION.ToString());
            log("Swf file size: " + r.getSizeInt(f.RAW_SIZE) + " (" + f.RAW_SIZE.ToString() + " B)");
            log("Frame width: " + f.WIDTH.ToString());
            log("Frame height: " + f.HEIGHT.ToString());
            log("Frame rate: " + f.FRAME_RATE.ToString());
            log("Frame count: " + f.FRAME_COUNT.ToString());
        }

        public void saveBytes(byte[] b, string path = null)
        {
            if (path == null)
            {
                path = Directory.GetCurrentDirectory() + "\\" + current.NAME_NOEX + "_new";
                int i = 1;
                while (File.Exists(path + i.ToString()))
                    i++;
                path += i.ToString() + ".swf";
            }
            File.WriteAllBytes(path, current.file_bytes);
        }

        private string readByte()
        {
            string s = (r.readByte(current.file_bytes, current.index)).ToString();
            current.index += 1;
            return s;
        }

        private string readBytes(int length, int start = -1)
        {
            start = (start == -1) ? current.index : start;
            string s = r.ByteArrayToString(r.readBytes(current.file_bytes, start, length));
            current.index += length;
            return s;
        }

        private string readInt16()
        {
            string s = (r.readInt16(current.file_bytes, current.index)).ToString();
            current.index += 2;
            return s;
        }

        private string readInt32()
        {
            string s = (r.readInt32(current.file_bytes, current.index)).ToString();
            current.index += 4;
            return s;
        }

        private void log(String message)
        {
            if (VERBOSE)
                txtBox.AppendText(message + "\n");
        }

        private void clear()
        {
            txtBox.Clear();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            DialogResult result = fileSaver.ShowDialog();
            if (result == DialogResult.OK)
            {
                saveBytes(current.file_bytes, fileSaver.FileName);
                log("File saved.");
            }
        }

        private void compressBtn_Click(object sender, EventArgs e)
        {
            if (!current.COMPRESSED)
            {
                updateBtns();
                current.COMPRESSED = true;
                current.file_bytes = r.CompressData(current.file_bytes);
                outPutFileInfo(current);
                log("File compressed successfully.");
            }
        }

        private void decompressBtn_Click(object sender, EventArgs e)
        {
            if (current.COMPRESSED)
            {
                updateBtns();
                current.COMPRESSED = false;
                current.file_bytes = r.DecompressData(current.file_bytes);
                outPutFileInfo(current);
                log("File decompressed successfully.");
            }
        }

        private void clearLog_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {

        }

        private void removeBtn_Click(object sender, EventArgs e)
        {
            return;
            int selected = swfList.SelectedIndex;
            if (selected >= 0)
            {
                clear();
                _files.Remove(_swf[selected]);
                _swf.RemoveAt(selected);
                swfList.Items.RemoveAt(selected);
                if (swfList.Items.Count == 0)
                    Save.Enabled = false;
            }
        }
    }
}
