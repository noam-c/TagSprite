/*
 *  This file is made available under the MIT license. See LICENSE.txt for more details.
 *
 *  Copyright (C) 2012 Noam Chitayat. All rights reserved.
 */
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace TagSprite
{
    public class SpritesheetData
    {
        private List<SpriteFrame> frames;
        private List<SpriteAnimation> animations = new List<SpriteAnimation>();
        private Dictionary<string, SpriteFrame> frameMap = new Dictionary<string, SpriteFrame>();

        private SpritesheetData(Bitmap bitmap)
        {
            this.LoadFromImage(bitmap);
        }

        public static SpritesheetData CreateFromImage(Bitmap bitmap)
        {
            return new SpritesheetData(bitmap);
        }

        public SpritesheetData(string filename)
        {
            this.LoadFromData(filename);
        }

        private void LoadFromData(string filename)
        {
            JObject data;
            using(var textReader = new StreamReader(filename))
            using(var reader = new JsonTextReader(textReader))
            {
                data = JObject.Load(reader);
            }

            this.frames = data.SelectToken("frames").Select(frame => new SpriteFrame(frame)).ToList();
            foreach(var frame in this.frames)
            {
                var name = frame.Name;
                if (name != SpriteFrame.UNTITLED_LABEL)
                {
                    this.frameMap[name] = frame;
                }
            }

            JToken animationList;
            if(data.TryGetValue("animations", out animationList))
            {
                this.animations = animationList.Select(
                    animation =>
                    {
                        var name = (string)animation["name"];
                        var frameList = animation["frames"].Select(frameName => frameMap[(string)frameName]).ToList();
                        return new SpriteAnimation(name, frameList);
                    }).ToList();
            }
        }

        private void LoadFromImage(Bitmap bitmap)
        {
            var frameList = new List<SpriteFrame>();
            var pixelMappings = new Dictionary<Point, SpriteFrame>();

            //iterate through image and create frames
            for (int y = 0; y < bitmap.Height; ++y)
            {
                for (int x = 0; x < bitmap.Width; ++x)
                {
                    var currPixel = new Point(x, y);
                    if (bitmap.GetPixel(x, y).A < 80)
                    {
                        //if this pixel is transparent, it's not part of a region, so let's do the next one
                        continue;
                    }

                    var neighbours = new List<Point>(4);

                    neighbours.Add(new Point(x - 1, y - 1));
                    neighbours.Add(new Point(x, y - 1));
                    neighbours.Add(new Point(x + 1, y - 1));
                    neighbours.Add(new Point(x - 1, y));

                    foreach (var pixel in neighbours)
                    {
                        if (!pixelMappings.ContainsKey(pixel))
                        {
                            // This neighbour pixel is transparent, so we'll ignore it
                            continue;
                        }

                        if (pixelMappings.ContainsKey(currPixel))
                        {
                            var currentFrame = pixelMappings[currPixel];
                            if (pixelMappings[pixel] != pixelMappings[currPixel])
                            {
                                //if the pixel above and the pixel to the left are distinct regions, the current pixel means they're the same region
                                var neighbourFrame = pixelMappings[pixel];

                                //merge the regions together, removing one altogether and relabeling all the other entries in that list
                                currentFrame.Merge(neighbourFrame);
                                foreach (var pixelToMerge in neighbourFrame.Pixels)
                                {
                                    pixelMappings[pixelToMerge] = currentFrame;
                                }

                                //remove the old frame entry
                                frameList.Remove(neighbourFrame);
                            }
                        }
                        else
                        {
                            //if this neighbour pixel is part of a region, the current pixel also is
                            pixelMappings[currPixel] = pixelMappings[pixel];
                        }
                    }

                    if (!pixelMappings.ContainsKey(currPixel))
                    {
                        //if the previous pixels aren't in regions, then this pixel is part of a new region we need to create
                        var newFrame = new SpriteFrame();
                        pixelMappings[currPixel] = newFrame;
                        frameList.Add(newFrame);
                    }

                    if (pixelMappings.ContainsKey(currPixel))
                    {
                        pixelMappings[currPixel].InsertPixel(currPixel);
                    }
                }
            }

            this.frames = frameList.Where(frame => frame != null).OrderBy(frame => frame.Rect.Left / 10).OrderBy(frame => frame.Rect.Top / 10).ToList();
        }

        public int AnimationCount
        {
            get
            {
                return this.animations.Count;
            }
        }

        public IList<SpriteFrame> Frames
        {
            get
            {
                return this.frames;
            }
        }

        public SpriteFrame GetFrame(string frameName)
        {
            if (this.frames == null)
            {
                return null;
            }

            return this.frameMap[frameName];
        }

        public SpriteAnimation GetAnimation(int index)
        {
            return this.animations[index];
        }

        public int CreateNewAnimation()
        {
            animations.Add(new SpriteAnimation(SpriteFrame.UNTITLED_LABEL, new List<SpriteFrame>()));
            return animations.Count - 1;
        }

        public void Save(string filename)
        {
            JObject data = new JObject(
                // Serialize the frames
                new JProperty("frames", new JArray(from frame in frames select frame.ToJson())));
                
            // Serialize the animations if there are any to serialize
            if(this.animations != null && this.animations.Count > 0)
            {
                data.Add(new JProperty("animations", new JArray(from anim in animations select anim.ToJson())));
            }
                
            using (var streamWriter = new StreamWriter(filename))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                writer.Formatting = Formatting.Indented;
                data.WriteTo(writer);
            }
        }
    }
}