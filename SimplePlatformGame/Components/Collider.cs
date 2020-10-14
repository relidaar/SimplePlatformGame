using Microsoft.Xna.Framework;

namespace SimplePlatformGame.Components
{
    public class Collider : ICollidable
    {
        public Rectangle HitBox { get; set; }
        public Point Dimensions { get; set; }
        public Point Position { get; set; }
        public Point OldPosition { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsStatic { get; }
        
        public bool OnContactBegin(ICollidable collidable, bool fromLeft, bool fromTop)
        {
            throw new System.NotImplementedException();
        }

        public Collider(Rectangle hitBox,
            Point dimensions,
            Point position,
            Point oldPosition,
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