using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame
{
    public interface IGameObject
    {
        Vector2 Position { get; }
        int Width { get; }
        int Height { get; }
        bool IsStatic { get; }

        bool OnCollisionEnter(IGameObject other)
        {
            return Position.X < other.Position.X + other.Width &&
                   Position.X + Width > other.Position.X &&
                   Position.Y < other.Position.Y + other.Height &&
                   Position.Y + Height > other.Position.Y;
        }

        void Draw(SpriteBatch target);
    }
}