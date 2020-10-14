using Microsoft.Xna.Framework;
using SimplePlatformGame.Components;

namespace SimplePlatformGame.GameObjects
{
    public class Obstacle : GameObject
    {
        public ICollidable Collider { get; }
        
        public Obstacle(Vector2 position, ISprite sprite) : base(position, sprite)
        {
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}