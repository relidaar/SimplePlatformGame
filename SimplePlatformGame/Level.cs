using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimplePlatformGame.Components;
using SimplePlatformGame.GameObjects;

namespace SimplePlatformGame
{
    public class Level
    {
        public Player Player { get; private set; }
        public IList<Obstacle> Obstacles { get; }
        public IList<ICollidable> Collidables { get; }

        private readonly string _path;
        private readonly GraphicsDevice _graphics;
        
        public Level(string path, GraphicsDevice graphicsDevice)
        {
            Obstacles = new List<Obstacle>();
            Collidables = new List<ICollidable>();
            _path = path;
            _graphics = graphicsDevice;
        }

        public void Update()
        {
            Player.Update();
            foreach (var gameObject in Obstacles)
            {
                gameObject.Update();
            }
        }

        public void Draw(SpriteBatch target)
        {
            Player.Draw(target);
            foreach (var gameObject in Obstacles)
            {
                gameObject.Draw(target);
            }
        }

        public void Load()
        {
            var valuesStrings = System.IO.File.ReadAllLines(_path)
                .Select(x => x)
                .Where(x => !string.IsNullOrWhiteSpace(x));
            foreach (var values in valuesStrings.Select(x => x.Split(" ")))
            {
                float posX = float.Parse(values[1]);
                float posY = float.Parse(values[2]);
                int width = int.Parse(values[3]);
                int height = int.Parse(values[4]);
                switch (values[0])
                {
                    case "player":
                        Player = new Player(new Vector2(posX, posY), 5, 
                            new SolidColorSprite(width, height, Color.DimGray, _graphics));
                        Collidables.Add(Player.Collider);
                        break;
                    case "obstacle":
                        var obstacle = new Obstacle(new Vector2(posX, posY),
                            new SolidColorSprite(width, height, Color.White, _graphics));
                        Obstacles.Add(obstacle);
                        Collidables.Add(obstacle.Collider);
                        break;
                }
            }
        }

        public void ResolveCollision(ICollidable a, ICollidable b)
        {
            (float left, float top, float right, float bottom) aBounds = (
                a.Position.X + a.HitBox.Left,
                a.Position.Y + a.HitBox.Top,
                a.Position.X + a.HitBox.Right,
                a.Position.Y + a.HitBox.Bottom
            );

            (float left, float top, float right, float bottom) bBounds = (
                b.Position.X + b.HitBox.Left,
                b.Position.Y + b.HitBox.Top,
                b.Position.X + b.HitBox.Right,
                b.Position.Y + b.HitBox.Bottom
            );

            (float left, float right, float top, float bottom) overlap = (
                aBounds.right - bBounds.left, 
                bBounds.right - aBounds.left, 
                aBounds.bottom - bBounds.top, 
                bBounds.bottom - aBounds.top
            );

            bool fromLeft = Math.Abs(overlap.left) < Math.Abs(overlap.right);
            bool fromTop = Math.Abs(overlap.top) < Math.Abs(overlap.bottom);
            
            (float x, float y) minOverlap = (
                fromLeft ? overlap.left : overlap.right,
                fromTop ? overlap.top : overlap.bottom
            );

            void YCollision(float overlapY, bool stair = false)
            {
                a.Velocity = new Vector2(a.Velocity.X, 0);
                if (fromTop)
                {
                    a.Position = stair ? 
                        new Point(a.Position.X - 5, (int) (a.Position.Y - overlapY)) :
                        new Point(a.Position.X, (int) (a.Position.Y - overlapY));
                }
                else
                {
                    a.Position = new Point(a.Position.X, (int) (a.Position.Y + overlapY));
                }
            }
            
            void XCollision(float overlapX, float overlapY) 
            {
                if (overlapY < 20 && fromLeft) 
                { 
                    YCollision(overlapY, true);
                    return;
                }
                
                a.Velocity = new Vector2(0, a.Velocity.Y);
                a.Position = fromLeft ? 
                    new Point((int) (a.Position.X - overlapX), a.Position.Y) : 
                    new Point((int) (a.Position.X + overlapX), a.Position.Y);
            }

            if (!a.OnContactBegin(b, fromLeft, fromTop) || !b.OnContactBegin(a, fromLeft, fromTop)) return;
            
            if (Math.Abs(minOverlap.x) > Math.Abs(minOverlap.y))
            {
                YCollision(minOverlap.y);
            }
            else if (Math.Abs(minOverlap.x) < Math.Abs(minOverlap.y))
            {
                XCollision(minOverlap.x, minOverlap.y);
            }
        }

        public void Dispose()
        {
            Player = null;
            Obstacles.Clear();
            Collidables.Clear();
        }
    }
}