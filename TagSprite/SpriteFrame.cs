/*
 *  This file is made available under the MIT license. See LICENSE.txt for more details.
 *
 *  Copyright (C) 2012 Noam Chitayat. All rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace TagSprite
{
    public class SpriteFrame
    {
        // NOTE: If this value must be changed, the UNTITLED_LINE string in the game's Spritesheet class must also change to be in sync 
        // As well, any .eds files with the old value must be updated to use the new value instead
        public static readonly string UNTITLED_LABEL = TagSpriteApp.Properties.Settings.Default.BlankLineLabel;

        public string Name
        {
            get;
            set;
        }

        public Rectangle Rect
        {
            get;
            private set;
        }

        public List<Point> Pixels = new List<Point>();

        public SpriteFrame()
        {
            this.Name = UNTITLED_LABEL;
        }

        public SpriteFrame(string name, Rectangle rect)
        {
            this.Name = name;
            this.Rect = rect;
        }

        public SpriteFrame(JToken data)
        {
            this.Name = (string)data["name"];
            var leftEdge = (int)data["left"];
            var topEdge = (int)data["top"];
            var rightEdge = (int)data["right"];
            var bottomEdge = (int)data["bottom"];

            this.Rect = new Rectangle(leftEdge, topEdge, rightEdge - leftEdge, bottomEdge - topEdge);
        }

        public JObject ToJson()
        {
            return new JObject(
                new JProperty("name", this.Name),
                new JProperty("left", this.Rect.Left),
                new JProperty("top", this.Rect.Top),
                new JProperty("right", this.Rect.Right),
                new JProperty("bottom", this.Rect.Bottom));
        }

        public void Merge(SpriteFrame other)
        {
            this.Pixels.AddRange(other.Pixels);
            if (this.Rect.IsEmpty)
            {
                this.Rect = other.Rect;
            }
            else if (!other.Rect.IsEmpty)
            {
                this.Rect = Rectangle.Union(this.Rect, other.Rect);
            }
        }

        public void InsertPixel(Point pixel)
        {
            this.Pixels.Add(pixel);
            var pixelRectangle = new Rectangle(pixel, new Size(1, 1));
            this.Rect = !this.Rect.IsEmpty ? Rectangle.Union(this.Rect, pixelRectangle) : pixelRectangle;
        }
    }
}
