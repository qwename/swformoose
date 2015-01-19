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

﻿namespace Decompiler_SWF
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFile = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.txtBox = new System.Windows.Forms.RichTextBox();
            this.fileOpener = new System.Windows.Forms.OpenFileDialog();
            this.fileSaver = new System.Windows.Forms.SaveFileDialog();
            this.compressBtn = new System.Windows.Forms.Button();
            this.swfList = new System.Windows.Forms.ListBox();
            this.decompressBtn = new System.Windows.Forms.Button();
            this.clearLog = new System.Windows.Forms.Button();
            this.addBtn = new System.Windows.Forms.Button();
            this.removeBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFile
            // 
            this.openFile.Location = new System.Drawing.Point(68, 201);
            this.openFile.Name = "openFile";
            this.openFile.Size = new System.Drawing.Size(75, 23);
            this.openFile.TabIndex = 0;
            this.openFile.Text = "Open";
            this.openFile.UseVisualStyleBackColor = true;
            this.openFile.Click += new System.EventHandler(this.openFile_Click);
            // 
            // Save
            // 
            this.Save.Enabled = false;
            this.Save.Location = new System.Drawing.Point(210, 201);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 1;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // txtBox
            // 
            this.txtBox.BackColor = System.Drawing.SystemColors.Window;
            this.txtBox.EnableAutoDragDrop = true;
            this.txtBox.Location = new System.Drawing.Point(12, 12);
            this.txtBox.Name = "txtBox";
            this.txtBox.ReadOnly = true;
            this.txtBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.txtBox.Size = new System.Drawing.Size(350, 173);
            this.txtBox.TabIndex = 2;
            this.txtBox.Text = "Drag a swf/dat file here to begin, or click \"Open\" to select a file from your com" +
                "puter.";
            // 
            // fileOpener
            // 
            this.fileOpener.FileName = "fileOpened";
            this.fileOpener.Filter = "Swf files|*.swf|Dat files|*.dat";
            this.fileOpener.Title = "Open a swf file";
            // 
            // fileSaver
            // 
            this.fileSaver.Filter = "Swf files|*.swf|Dat files|*.dat";
            this.fileSaver.Title = "Save file as";
            // 
            // compressBtn
            // 
            this.compressBtn.Enabled = false;
            this.compressBtn.Location = new System.Drawing.Point(68, 249);
            this.compressBtn.Name = "compressBtn";
            this.compressBtn.Size = new System.Drawing.Size(75, 23);
            this.compressBtn.TabIndex = 3;
            this.compressBtn.Text = "Compress";
            this.compressBtn.UseVisualStyleBackColor = true;
            this.compressBtn.Click += new System.EventHandler(this.compressBtn_Click);
            // 
            // swfList
            // 
            this.swfList.FormattingEnabled = true;
            this.swfList.Location = new System.Drawing.Point(389, 12);
            this.swfList.Name = "swfList";
            this.swfList.Size = new System.Drawing.Size(298, 173);
            this.swfList.TabIndex = 4;
            // 
            // decompressBtn
            // 
            this.decompressBtn.Enabled = false;
            this.decompressBtn.Location = new System.Drawing.Point(210, 249);
            this.decompressBtn.Name = "decompressBtn";
            this.decompressBtn.Size = new System.Drawing.Size(75, 23);
            this.decompressBtn.TabIndex = 5;
            this.decompressBtn.Text = "Decompress";
            this.decompressBtn.UseVisualStyleBackColor = true;
            this.decompressBtn.Click += new System.EventHandler(this.decompressBtn_Click);
            // 
            // clearLog
            // 
            this.clearLog.Location = new System.Drawing.Point(137, 294);
            this.clearLog.Name = "clearLog";
            this.clearLog.Size = new System.Drawing.Size(75, 23);
            this.clearLog.TabIndex = 6;
            this.clearLog.Text = "Clear Text";
            this.clearLog.UseVisualStyleBackColor = true;
            this.clearLog.Click += new System.EventHandler(this.clearLog_Click);
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(427, 201);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 23);
            this.addBtn.TabIndex = 7;
            this.addBtn.Text = "Add";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // removeBtn
            // 
            this.removeBtn.Location = new System.Drawing.Point(570, 201);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(75, 23);
            this.removeBtn.TabIndex = 8;
            this.removeBtn.Text = "Remove";
            this.removeBtn.UseVisualStyleBackColor = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 329);
            this.Controls.Add(this.removeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.clearLog);
            this.Controls.Add(this.decompressBtn);
            this.Controls.Add(this.swfList);
            this.Controls.Add(this.compressBtn);
            this.Controls.Add(this.txtBox);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.openFile);
            this.Name = "Main";
            this.Text = "Swformoose Decopmiler";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button openFile;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.RichTextBox txtBox;
        private System.Windows.Forms.OpenFileDialog fileOpener;
        private System.Windows.Forms.SaveFileDialog fileSaver;
        private System.Windows.Forms.Button compressBtn;
        private System.Windows.Forms.ListBox swfList;
        private System.Windows.Forms.Button decompressBtn;
        private System.Windows.Forms.Button clearLog;
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button removeBtn;
    }
}

