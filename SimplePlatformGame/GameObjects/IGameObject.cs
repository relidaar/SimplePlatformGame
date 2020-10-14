using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimplePlatformGame.GameObjects
{
    public interface IGameObject
    {
        Vector2 Position { get; }
        int Width { get; }
        int Height { get; }

        void Update();
        void Draw(SpriteBatch target);
    }
}