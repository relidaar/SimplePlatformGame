using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimplePlatformGame.Components;
using SimplePlatformGame.GameObjects;
using SimplePlatformGame.JsonObjects;

namespace SimplePlatformGame
{
    public class Level
    {
        public Player Player { get; private set; }
        private readonly List<Obstacle> _obstacles;
        private List<Obstacle> _enemyBounds;
        private List<Coin> _coins;
        private List<Enemy> _enemies;

        private readonly Vector2 _gravity;

        private readonly string _path;
        private readonly GraphicsDevice _graphics;

        public Level(string path, GraphicsDevice graphicsDevice)
        {
            _obstacles = new List<Obstacle>();
            _enemyBounds = new List<Obstacle>();
            _coins = new List<Coin>();
            _enemies = new List<Enemy>();
            
            _path = path;
            _graphics = graphicsDevice;
            _gravity = new Vector2(0, 10);
        }

        public void Update(float timeDelta)
        {
            var gravity = _gravity * timeDelta;
            
            Player.Update(gravity);
            foreach (var obstacle in _obstacles.Where(obstacle => Player.Collider.Intersects(obstacle.Collider)))
            {
                Player.Collider.ResolveCollision(obstacle.Collider);
            }
            if (_enemies.Any(x => Player.Collider.Intersects(x.Collider)))
            {
                Player.Alive = false;
                Unload();
                Load();
            }
            _coins = _coins.Where(coin => !Player.Collider.Intersects(coin.Collider)).ToList();

            _enemies.ForEach(x => x.Update(gravity));
            foreach (var enemy in _enemies)
            {
                foreach (var obstacle in _obstacles.Where(obstacle => enemy.Collider.Intersects(obstacle.Collider)))
                {
                    enemy.Collider.ResolveCollision(obstacle.Collider);
                }
                foreach (var _ in _enemyBounds.Where(x => enemy.Collider.Intersects(x.Collider)))
                {
                    enemy.ChangeDirection();
                }
                foreach (var otherEnemy in _enemies.Where(x => x != enemy && 
                                                               enemy.Collider.Intersects(x.Collider)))
                {
                    otherEnemy.ChangeDirection();
                    enemy.ChangeDirection();
                }
            }
        }

        public void Draw(SpriteBatch target, float timeDelta)
        {
            Player.Draw(target, timeDelta);
            _obstacles.ForEach(x => x.Draw(target, timeDelta));
            _enemyBounds.ForEach(x => x.Draw(target, timeDelta));
            _coins.ForEach(x => x.Draw(target, timeDelta));
            _enemies.ForEach(x => x.Draw(target, timeDelta));
        }

        public void Load()
        {
            var jsonString = File.ReadAllText(_path);
            var json = JObject.Parse(jsonString);

            var jsonToken = json["player"];
            var jsonPlayer = JsonConvert.DeserializeObject<JsonPlayer>(jsonToken.ToString());
            
            Player = new Player(
                new Vector2(jsonPlayer.X, jsonPlayer.Y), 
                jsonPlayer.RunSpeed, jsonPlayer.JumpSpeed, 
                new SolidColorSprite(jsonPlayer.Width, jsonPlayer.Height, 
                    JsonGameObject.Colors[(string)jsonToken["Color"]], _graphics)
            );

            foreach (var token in json["obstacles"])
            {
                var jsonObject = JsonConvert.DeserializeObject<JsonGameObject>(token.ToString());
                _obstacles.Add(new Obstacle(
                    new Vector2(jsonObject.X, jsonObject.Y),
                    new SolidColorSprite(jsonObject.Width, jsonObject.Height,
                        JsonGameObject.Colors[(string) token["Color"]], _graphics)
                ));
            }

            foreach (var token in json["enemy_bounds"])
            {
                var jsonObject = JsonConvert.DeserializeObject<JsonGameObject>(token.ToString());
                _enemyBounds.Add(new Obstacle(
                    new Vector2(jsonObject.X, jsonObject.Y),
                    new SolidColorSprite(jsonObject.Width, jsonObject.Height,
                        JsonGameObject.Colors[(string) token["Color"]], _graphics)
                ));
            }

            foreach (var token in json["coins"])
            {
                var jsonObject = JsonConvert.DeserializeObject<JsonGameObject>(token.ToString());
                _coins.Add(new Coin(
                    new Vector2(jsonObject.X, jsonObject.Y),
                    new SolidColorSprite(jsonObject.Width, jsonObject.Height,
                        JsonGameObject.Colors[(string) token["Color"]], _graphics)
                ));
            }

            foreach (var token in json["enemies"])
            {
                var jsonObject = JsonConvert.DeserializeObject<JsonEnemy>(token.ToString());
                _enemies.Add(new Enemy(
                    new Vector2(jsonObject.X, jsonObject.Y), jsonObject.Speed, jsonObject.Direction,
                    new SolidColorSprite(jsonObject.Width, jsonObject.Height, 
                        JsonGameObject.Colors[(string)token["Color"]], _graphics)
                ));
            }
        }

        public void Unload()
        {
            Player = null;
            _obstacles.Clear();
            _coins.Clear();
            _enemies.Clear();
            _enemyBounds.Clear();
        }
    }
}