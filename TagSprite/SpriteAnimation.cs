/*
 *  This file is made available under the MIT license. See LICENSE.txt for more details.
 *
 *  Copyright (C) 2012 Noam Chitayat. All rights reserved.
 */
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TagSprite
{
    public class SpriteAnimation
    {
        List<SpriteFrame> frames = new List<SpriteFrame>();

        public string Name
        {
            get;
            set;
        }

        public SpriteAnimation(string name, List<SpriteFrame> frames)
        {
            this.Name = name;
            this.frames = frames;
        }

        public SpriteFrame GetFrame(int index)
        {
            return this.frames[index];
        }

        public int NumFrames
        {
            get
            {
                return this.frames.Count;
            }
        }

        public void Insert(int insertionIndex, List<SpriteFrame> frames)
        {
            this.frames.InsertRange(insertionIndex, frames);
        }

        public void Remove(List<int> indices)
        {
            foreach (var position in indices.OrderByDescending(x => x))
            {
                this.frames.RemoveAt(position);
            }
        }

        public void Swap(int pos1, int pos2)
        {
            if (pos1 >= 0 && pos1 < this.frames.Count
                && pos2 >= 0 && pos2 < this.frames.Count)
            {
                var temp = this.frames[pos1];
                this.frames[pos1] = this.frames[pos2];
                this.frames[pos2] = temp;
            }
        }

        public JObject ToJson()
        {
            return new JObject(
                new JProperty("name", this.Name),
                new JProperty("frames",
                    new JArray(from frame in frames select frame.Name)));
        }
    }
}
