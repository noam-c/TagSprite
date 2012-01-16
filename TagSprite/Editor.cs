/*
 *  This file is made available under the MIT license. See LICENSE.txt for more details.
 *
 *  Copyright (C) 2012 Noam Chitayat. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace TagSprite
{
    public partial class Editor : Form
    {
        private SpritesheetData frameFinder;
		private FrameLabeler frameLabeler;
        private string sheetDataFile;

        public Editor()
        {
            InitializeComponent();
        }

        private void importImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Import Image File";
            fDialog.Filter = "PNG Files|*.png";

            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                var imageFile = fDialog.FileName.ToString();
                sheetDataFile = imageFile.Substring(0, imageFile.Length - 3) + "eds";

                var image = new Bitmap(imageFile);

                this.frameFinder = SpritesheetData.CreateFromImage(image);
				SetFrameLabelerTab(image);
            }
        }

        private void openSheetFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Open Sheet File";
            fDialog.Filter = "EDS Files|*.eds";
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                sheetDataFile = fDialog.FileName.ToString();
                var imageFile = sheetDataFile.Substring(0, sheetDataFile.Length - 3) + "png";

                var image = new Bitmap(imageFile);

                this.frameFinder = new SpritesheetData(sheetDataFile);
				SetFrameLabelerTab(image);
            }
        }

		private void SetFrameLabelerTab(Bitmap image)
		{
			if(this.frameLabeler == null)
			{
				this.frameLabeler = new FrameLabeler(image, this.frameFinder);

				TabPage frameLabelerPage = new TabPage("Frame Labeler");
				frameLabelerPage.Controls.Add(this.frameLabeler);
	        	this.editorTabs.TabPages.Add(frameLabelerPage);
			}
			else
			{
				this.frameLabeler.FrameFinder = this.frameFinder;
			}
		}

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.frameFinder != null)
            {
                this.frameFinder.Save(sheetDataFile);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
