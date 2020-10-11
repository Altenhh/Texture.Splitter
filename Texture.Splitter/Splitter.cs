using System.Drawing;
using System.Drawing.Imaging;
using Texture.Splitter.SpriteSheets;

namespace Texture.Splitter
{
    public static class Splitter
    {
        public static Bitmap GetSprite(Frame frame, Bitmap spriteSheet)
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

            var cropped = spriteSheet.Clone(new Rectangle(frame.TextureRect.X, frame.TextureRect.Y, width, height), PixelFormat.DontCare);

            if (frame.Rotated)
                cropped.RotateFlip(RotateFlipType.Rotate270FlipNone);

            return cropped;
        }
    }
}