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
        private List<Coin> _coins;
        private List<Enemy> _enemies;
        private List<GameObject> _gameObjects;

        private readonly Vector2 _gravity;

        private readonly string _path;
        private readonly GraphicsDevice _graphics;

        public Level(string path, GraphicsDevice graphicsDevice)
        {
            _obstacles = new List<Obstacle>();
            _coins = new List<Coin>();
            _enemies = new List<Enemy>();
            _gameObjects = new List<GameObject>();
            
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

            _enemies.ForEach(x => x.Update(gravity));
            foreach (var enemy in _enemies)
            {
                foreach (var obstacle in _obstacles.Where(obstacle => enemy.Collider.Intersects(obstacle.Collider)))
                {
                    enemy.Collider.ResolveCollision(obstacle.Collider);
                }
            }

            _coins = _coins.Where(coin => !Player.Collider.Intersects(coin.Collider)).ToList();
        }

        public void Draw(SpriteBatch target, float timeDelta) => 
            _gameObjects.ForEach(x => x.Draw(target, timeDelta));

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
            _gameObjects.Add(Player);

            foreach (var token in json["obstacles"])
            {
                var jsonObject = JsonConvert.DeserializeObject<JsonGameObject>(token.ToString());
                var gameObject = new Obstacle(
                    new Vector2(jsonObject.X, jsonObject.Y),
                    new SolidColorSprite(jsonObject.Width, jsonObject.Height,
                        JsonGameObject.Colors[(string) token["Color"]], _graphics)
                );
                _obstacles.Add(gameObject);
                _gameObjects.Add(gameObject);
            }

            foreach (var token in json["coins"])
            {
                var jsonObject = JsonConvert.DeserializeObject<JsonGameObject>(token.ToString());
                var gameObject = new Coin(
                    new Vector2(jsonObject.X, jsonObject.Y),
                    new SolidColorSprite(jsonObject.Width, jsonObject.Height,
                        JsonGameObject.Colors[(string) token["Color"]], _graphics)
                );
                _coins.Add(gameObject);
                _gameObjects.Add(gameObject);
            }

            foreach (var token in json["enemies"])
            {
                var jsonObject = JsonConvert.DeserializeObject<JsonEnemy>(token.ToString());
                var gameObject = new Enemy(
                    new Vector2(jsonObject.X, jsonObject.Y), jsonObject.Speed, jsonObject.Direction,
                    new SolidColorSprite(jsonObject.Width, jsonObject.Height, 
                        JsonGameObject.Colors[(string)token["Color"]], _graphics)
                );
                _enemies.Add(gameObject);
                _gameObjects.Add(gameObject);
            }
        }

        public void Dispose()
        {
            Player = null;
            _obstacles.Clear();
            _coins.Clear();
            _enemies.Clear();
        }
    }
}