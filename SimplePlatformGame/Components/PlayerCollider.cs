using Microsoft.Xna.Framework;

namespace SimplePlatformGame.Components
{
    public class PlayerCollider : Collider
    {
        public bool Grounded { get; set; }

        public PlayerCollider(Rectangle hitBox, Vector2 position, bool isStatic = false) 
            : base(hitBox, position, isStatic)
        {
        }

        protected override bool OnContactBegin(Collider other, bool fromLeft, bool fromTop)
        {
            Grounded = true;
            
            return base.OnContactBegin(other, fromLeft, fromTop);
        }
    }
}