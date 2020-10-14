using Microsoft.Xna.Framework;

namespace SimplePlatformGame.Components
{
    public interface ICollidable
    {
        Rectangle HitBox { get; set; }
        Vector2 Dimensions { get; set; }
        Vector2 Position { get; set; }
        Vector2 OldPosition { get; set; }
        Vector2 Velocity { get; set; }
        bool IsStatic { get; }
        bool OnContactBegin(ICollidable collidable, bool fromLeft, bool fromTop);
    }
}