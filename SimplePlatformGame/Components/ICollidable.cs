using Microsoft.Xna.Framework;

namespace SimplePlatformGame.Components
{
    public interface ICollidable
    {
        Rectangle HitBox { get; set; }
        Point Dimensions { get; set; }
        Point Position { get; set; }
        Point OldPosition { get; set; }
        Vector2 Velocity { get; set; }
        bool IsStatic { get; }
        bool OnContactBegin(ICollidable other, bool fromLeft, bool fromTop);
        bool Intersects(ICollidable other);
        void ResolveCollision(ICollidable other);
    }
}