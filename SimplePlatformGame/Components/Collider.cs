using System;
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

        public void ResolveCollision(ICollidable other)
        {
            (float left, float top, float right, float bottom) bounds = (
                Position.X + HitBox.Left,
                Position.Y + HitBox.Top,
                Position.X + HitBox.Right,
                Position.Y + HitBox.Bottom
            );

            (float left, float top, float right, float bottom) otherBound = (
                other.Position.X + other.HitBox.Left,
                other.Position.Y + other.HitBox.Top,
                other.Position.X + other.HitBox.Right,
                other.Position.Y + other.HitBox.Bottom
            );

            (float left, float right, float top, float bottom) overlap = (
                bounds.right - otherBound.left, 
                otherBound.right - bounds.left, 
                bounds.bottom - otherBound.top, 
                otherBound.bottom - bounds.top
            );

            bool fromLeft = Math.Abs(overlap.left) < Math.Abs(overlap.right);
            bool fromTop = Math.Abs(overlap.top) < Math.Abs(overlap.bottom);
            
            (float x, float y) minOverlap = (
                fromLeft ? overlap.left : overlap.right,
                fromTop ? overlap.top : overlap.bottom
            );

            if (!OnContactBegin(other, fromLeft, fromTop) || !other.OnContactBegin(this, fromLeft, fromTop)) 
                return;
            
            if (Math.Abs(minOverlap.x) > Math.Abs(minOverlap.y))
            {
                CollisionY(minOverlap.y, fromTop);
            }
            else if (Math.Abs(minOverlap.x) < Math.Abs(minOverlap.y))
            {
                CollisionX(minOverlap.x, minOverlap.y, fromLeft, fromTop);
            }
        }

        private void CollisionY(float overlapY, bool fromTop, bool stair = false)
        {
            Velocity = new Vector2(Velocity.X, 0);
            if (fromTop)
            {
                Position = stair ? 
                    new Point(Position.X - 5, (int) (Position.Y - overlapY)) :
                    new Point(Position.X, (int) (Position.Y - overlapY));
            }
            else
            {
                Position = new Point(Position.X, (int) (Position.Y + overlapY));
            }
        }

        private void CollisionX(float overlapX, float overlapY, bool fromLeft, bool fromTop)
        {
            if (overlapY < 20 && fromLeft) 
            { 
                CollisionY(overlapY, fromTop, true);
                return;
            }
                
            Velocity = new Vector2(0, Velocity.Y);
            Position = fromLeft ? 
                new Point((int) (Position.X - overlapX), Position.Y) : 
                new Point((int) (Position.X + overlapX), Position.Y);
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