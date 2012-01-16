/*
 *  This file is made available under the MIT license. See LICENSE.txt for more details.
 *
 *  Copyright (C) 2012 Noam Chitayat. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TagSprite
{
    public partial class FrameLabeler : UserControl
    {
        private SpritesheetData frameFinder;
        private Bitmap image;
        private SpriteFrame currentFrame;
        private BindingSource listBinding = new BindingSource();

        public SpritesheetData FrameFinder
        {
            set
            {
                this.frameFinder = value;
                this.listBinding.DataSource = this.frameFinder.Frames;
                this.frameListBox.SelectedIndex = 0;
            }
        }

        public FrameLabeler(Bitmap image, SpritesheetData frameFinder)
        {
            InitializeComponent();
            this.image = image;
            this.frameListBox.DataSource = listBinding;

            this.frameListBox.SelectedIndexChanged += 
                (sender, e) => ChangeFrame();

            this.frameNameText.TextChanged += 
                (sender, e) => 
                { 
                    this.currentFrame.Name = this.frameNameText.Text;
                    this.listBinding.ResetCurrentItem(); 
                };

            this.frameImageBox.Paint += PaintEvent;
            this.FrameFinder = frameFinder;
            this.frameListBox.ValueMember = "Name";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.Down))
            {
                if (this.frameListBox.SelectedIndex < this.frameListBox.Items.Count - 1)
                {
                    ++this.frameListBox.SelectedIndex;
                    this.frameNameText.SelectAll();
                }
                return true;
            }
            else if (keyData == (Keys.Alt | Keys.Up))
            {
                if (this.frameListBox.SelectedIndex > 0)
                {
                    --this.frameListBox.SelectedIndex;
                    this.frameNameText.SelectAll();
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PaintEvent(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (this.currentFrame != null)
            {
                e.Graphics.DrawImage(this.image, this.frameImageBox.DisplayRectangle, currentFrame.Rect, GraphicsUnit.Pixel);
            }
        }

        private void ChangeFrame()
        {
            if (this.frameListBox.SelectedIndex < 0)
            {
                this.currentFrame = null;
            }
            else
            {
                this.currentFrame = this.frameFinder.Frames[this.frameListBox.SelectedIndex];

                this.frameImageBox.Width = currentFrame.Rect.Width;
                this.frameImageBox.Height = currentFrame.Rect.Height;

                this.frameNameText.Text = this.currentFrame.Name;
            }

            Refresh();
        }
    }
}
