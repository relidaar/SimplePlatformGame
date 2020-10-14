using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame.Components
{
    public interface ISprite
    {
        Texture2D Texture { get; }
        Rectangle Bounds { get; }
        int Width { get; }
        int Height { get; }
    }
}