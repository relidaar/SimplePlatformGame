using Microsoft.Xna.Framework;

namespace SimplePlatformGame.Components
{
    public class Collider : ICollidable
    {
        public Rectangle HitBox { get; set; }
        public Vector2 Dimensions { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 OldPosition { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsStatic { get; }
        
        public bool OnContactBegin(ICollidable collidable, bool fromLeft, bool fromTop)
        {
            throw new System.NotImplementedException();
        }

        public Collider(Rectangle hitBox,
            Vector2 dimensions,
            Vector2 position,
            Vector2 oldPosition,
            Vector2 velocity,
            bool isStatic)
        {
            HitBox = hitBox;
            Dimensions = dimensions;
            Position = position;
            OldPosition = oldPosition;
            Velocity = velocity;
            IsStatic = isStatic;
        }
    }
}