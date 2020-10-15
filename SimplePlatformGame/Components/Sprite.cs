using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame.Components
{
    public class Sprite
    {
        public Texture2D Texture { get; }
        public Rectangle Bounds => Texture.Bounds;
        public int Width => Texture.Width;
        public int Height => Texture.Height;

        public Sprite(Texture2D texture)
        {
            Texture = texture;
        }

        public Sprite(int width, int height, Color color, GraphicsDevice graphics)
        {
            Texture = new Texture2D(graphics, width, height);
            var data = new Color[width * height];
            for (int i = 0; i < data.Length; i++) data[i] = color;
            Texture.SetData(data);
        }
    }
}