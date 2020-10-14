using Microsoft.Xna.Framework;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public class Obstacle : GameObject
    {
        public Obstacle(Vector2 position, ISprite sprite)
            : base(position, sprite, new Collider(sprite.Bounds, position, true))
        {
        }

        public override void Update(Vector2 gravity)
        {
        }
    }
}