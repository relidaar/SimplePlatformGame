﻿using Microsoft.Xna.Framework;

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
        bool OnContactBegin(ICollidable collidable, bool fromLeft, bool fromTop);
    }
}