using System.Drawing;

namespace Texture.Splitter.SpriteSheets
{
    /// <summary>
    /// Texture size, format, texture file name.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// The file format version: 3
        /// </summary>
        public int Format { get; set; }

        /// <summary>
        /// Format used for the pixels.
        /// </summary>
        public string PixelFormat { get; set; }

        /// <summary>
        /// Whether to premultiply the Alpha channel.
        /// </summary>
        public bool PremultiplyAlpha { get; set; }

        /// <summary>
        /// The file name of the texture.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Size of the texture.
        /// </summary>
        public Size Size { get; set; }

        public override string ToString() => FileName;
    }
}