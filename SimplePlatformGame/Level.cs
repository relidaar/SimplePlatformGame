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
        private readonly IList<Collider> _colliders;
        private readonly IList<GameObject> _gameObjects;

        private readonly Vector2 _gravity;

        private readonly string _path;
        private readonly GraphicsDevice _graphics;

        public Level(string path, GraphicsDevice graphicsDevice)
        {
            _gameObjects = new List<GameObject>();
            _colliders = new List<Collider>();
            _path = path;
            _graphics = graphicsDevice;
            _gravity = new Vector2(0, 10);
        }

        public void Update(float timeDelta)
        {
            Player.Update(_gravity * timeDelta);
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update(_gravity * timeDelta);
            }
 
            foreach (var collider in _colliders)
            {
                foreach (var other in _colliders.Where(c => !c.Equals(collider)))
                {
                    var dynamicCollidable = collider;
                    var staticCollidable = other;

                    if (collider.IsStatic)
                    {
                        dynamicCollidable = other;
                        staticCollidable = collider;
                    }
                    
                    if (dynamicCollidable.Intersects(staticCollidable))
                        dynamicCollidable.ResolveCollision(staticCollidable);
                }
            }
        }

        public void Draw(SpriteBatch target, float timeDelta)
        {
            Player.Draw(target, timeDelta);
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Draw(target, timeDelta);
            }
        }

        public void Load()
        {
            var valuesStrings = System.IO.File.ReadAllLines(_path)
                .Select(x => x)
                .Where(x => !string.IsNullOrWhiteSpace(x));
            foreach (var values in valuesStrings.Select(x => x.Split(" ")))
            {
                var (x, y) = (float.Parse(values[1]), float.Parse(values[2]));
                var (width, height) = (int.Parse(values[3]), int.Parse(values[4]));
                switch (values[0])
                {
                    case "player":
                        Player = new Player(new Vector2(x, y), 5, 5, 
                            new SolidColorSprite(width, height, Color.DimGray, _graphics));
                        _colliders.Add(Player.Collider);
                        break;
                    case "obstacle":
                        var obstacle = new Obstacle(new Vector2(x, y),
                            new SolidColorSprite(width, height, Color.White, _graphics));
                        _gameObjects.Add(obstacle);
                        _colliders.Add(obstacle.Collider);
                        break;
                }
            }
        }

        public void Dispose()
        {
            Player = null;
            _gameObjects.Clear();
            _colliders.Clear();
        }
    }
}