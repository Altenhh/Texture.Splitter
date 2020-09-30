using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using SixLabors.ImageSharp;

namespace Texture.Splitter.SpriteSheets
{
    public class SpriteSheet
    {
        public Frame[] Frames { get; set; }
        public Metadata Metadata { get; set; }

        public static SpriteSheet LoadSpriteSheet(Plist plist)
        {
            var spriteSheet = new SpriteSheet();
            var frames = new List<Frame>();
            var metadata = new Metadata();

            Plist plistMetadata = plist["metadata"];
            Plist plistFrames = plist["frames"];

            #region Frames
            foreach (var (name, pFrame) in plistFrames)
            {
                var spOffset = convertVector<float>((string) pFrame["spriteOffset"]).ToArray();
                var spSize = convertVector<int>((string) pFrame["spriteSize"]).ToArray();
                var spSourceSize = convertVector<int>((string) pFrame["spriteSourceSize"]).ToArray();
                var spTextureRect = convertVector<int>((string) pFrame["textureRect"]).ToArray();
                List<object> spAliases = pFrame["aliases"];

                frames.Add(new Frame
                {
                    SpriteName = name,
                    Aliases = spAliases.ToArray(),
                    Offset = new Vector2(spOffset[0], spOffset[1]),
                    Size = new Size(spSize[0], spSize[1]),
                    SourceSize = new Size(spSourceSize[0], spSourceSize[1]),
                    TextureRect = new Rectangle(spTextureRect[0], spTextureRect[1], spTextureRect[2], spTextureRect[3]),
                    Rotated = pFrame["textureRotated"]
                });
            }

            spriteSheet.Frames = frames.ToArray();
            #endregion

            #region Metadata
            metadata.Format = plistMetadata["format"];
            metadata.PixelFormat = plistMetadata["pixelFormat"];
            metadata.PremultiplyAlpha = plistMetadata["premultiplyAlpha"];

            var metaSize = convertVector<int>((string) plistMetadata["size"]).ToArray();
            metadata.Size = new Size(metaSize[0], metaSize[1]);

            metadata.FileName = plistMetadata["textureFileName"];

            spriteSheet.Metadata = metadata;
            #endregion

            return spriteSheet;
        }

        private static IEnumerable<T> convertVector<T>(string vectorString)
        {
            var trimmed = vectorString.Trim('{', '}');
            var split = trimmed.Split(',');
            var type = typeof(T);

            foreach (var s in split)
                yield return (T) Convert.ChangeType(s.Trim('{', '}'), type);
        }
    }
}