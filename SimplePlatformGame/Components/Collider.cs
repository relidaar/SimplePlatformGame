using Microsoft.Xna.Framework;

namespace SimplePlatformGame.Components
{
    public abstract class Collider : ICollidable
    {
        public Rectangle HitBox { get; set; }
        public Point Dimensions { get; set; }
        public Point Position { get; set; }
        public Point OldPosition { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsStatic { get; }

        public abstract bool OnContactBegin(ICollidable other, bool fromLeft, bool fromTop);

        public bool Intersects(ICollidable other)
        {
            var point = Position + new Point(HitBox.Left, HitBox.Top);
            var rect = new Rectangle(point, HitBox.Size);
            
            var otherPosition = other.Position + new Point(other.HitBox.Left, other.HitBox.Top);
            var otherRect = new Rectangle(otherPosition, other.HitBox.Size);
            
            return rect.Intersects(otherRect);
        }

        protected Collider(Rectangle hitBox,
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