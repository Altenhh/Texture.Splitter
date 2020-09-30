using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Texture.Splitter.SpriteSheets;

namespace Texture.Splitter
{
    public static class Splitter
    {
        public static Image GetSprite(Frame frame, Image spriteSheet)
        {
            int width;
            int height;

            if (frame.Rotated)
            {
                width = frame.TextureRect.Height;
                height = frame.TextureRect.Width;
            }
            else
            {
                width = frame.TextureRect.Width;
                height = frame.TextureRect.Height;
            }

            return spriteSheet.Clone(i =>
            {
                i.Crop(new Rectangle(frame.TextureRect.X, frame.TextureRect.Y, width, height));

                if (frame.Rotated)
                    i.Rotate(RotateMode.Rotate270);
            });
        }
    }
}