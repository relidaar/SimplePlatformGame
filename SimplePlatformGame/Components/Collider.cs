using System;
using Microsoft.Xna.Framework;

namespace SimplePlatformGame.Components
{
    public class Collider
    {
        public Rectangle HitBox { get; }
        public Vector2 Position { get; set; }
        public Vector2 OldPosition { get; set; }
        public Vector2 Velocity { get; set; }
        public bool IsStatic { get; }

        public Collider(Rectangle hitBox, Vector2 position, bool isStatic = false)
        {
            HitBox = hitBox;
            Position = position;
            IsStatic = isStatic;
        }

        protected virtual bool OnContactBegin(Collider other, bool fromLeft, bool fromTop)
        {
            return true;
        }

        public bool Intersects(Collider other)
        {
            var (x, y) = Position + new Vector2(HitBox.Left, HitBox.Top);
            var rect = new Rectangle(new Point((int) x, (int) y), HitBox.Size);
            
            var (otherX, otherY) = other.Position + new Vector2(other.HitBox.Left, other.HitBox.Top);
            var otherRect = new Rectangle(new Point((int) otherX, (int) otherY), other.HitBox.Size);
            
            return rect.Intersects(otherRect);
        }

        public void ResolveCollision(Collider other)
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
                    new Vector2(Position.X - 5, (int) (Position.Y - overlapY)) :
                    new Vector2(Position.X, (int) (Position.Y - overlapY));
            }
            else
            {
                Position = new Vector2(Position.X, (int) (Position.Y + overlapY));
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
                new Vector2((int) (Position.X - overlapX), Position.Y) : 
                new Vector2((int) (Position.X + overlapX), Position.Y);
        }
    }
}