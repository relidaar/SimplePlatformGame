using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimplePlatformGame.GameObjects;

namespace SimplePlatformGame
{
    public class PlatformGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        
        private List<Level> _levels;
        private List<Level>.Enumerator _currentLevel;
        private readonly string _levelsDirectory;
        private bool _gameOver;
        private int _coins = 0;
        
        private KeyboardState _state;
        private float _timeDelta;

        public PlatformGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _levelsDirectory = "Levels";
            
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _state = Keyboard.GetState();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Font");

            _levels = new List<Level>();
            foreach (string filePath in Directory.GetFiles( $"{Content.RootDirectory}/{_levelsDirectory}/"))
            {
                _levels.Add(new Level(filePath, _graphics.GraphicsDevice));
            }
            
            _currentLevel = _levels.GetEnumerator();
            _currentLevel.MoveNext();
            _currentLevel.Current?.Load();
        }

        protected override void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();
            _timeDelta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_state.IsKeyDown(Keys.Escape))
                Exit();

            if (_currentLevel.Current == null)
            {
                _gameOver = true;
                base.Update(gameTime);
                return;
            }

            if (_currentLevel.Current.Passed)
            {
                _coins += _currentLevel.Current.CollectedCoins;
                _currentLevel.Current.Unload();
                _currentLevel.MoveNext();
                _currentLevel.Current?.Load();
            }

            var movement = new[]
            {
                (Keys.A, Direction.Left),
                (Keys.D, Direction.Right),
                (Keys.Left, Direction.Left),
                (Keys.Right, Direction.Right),
            };
            var jump = new[] {Keys.Space, Keys.W, Keys.Up};

            foreach (var (key, direction) in movement)
            {
                if (currentState.IsKeyDown(key) && _state.IsKeyUp(key))
                    _currentLevel.Current?.Player.Move(direction);
                if (currentState.IsKeyUp(key) && _state.IsKeyDown(key))
                    _currentLevel.Current?.Player.Stop(direction);
            }

            foreach (var key in jump)
            {
                if (currentState.IsKeyDown(key))
                    _currentLevel.Current?.Player.Jump();
            }

            _state = currentState;
            
            _currentLevel.Current?.Update(_timeDelta);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            _spriteBatch.Begin();
            if (_currentLevel.Current != null)
            {
                _currentLevel.Current?.Draw(_spriteBatch, _timeDelta);
                _spriteBatch.DrawString(_font, $"Coins: {_currentLevel.Current.CollectedCoins}", 
                    new Vector2(10, 10), Color.White);
            }
            
            if (_gameOver)
            {
                _spriteBatch.DrawString(_font, "Game Over", 
                    new Vector2(320, 220), Color.White);
                _spriteBatch.DrawString(_font, $"You collected {_coins} " + (_coins > 1 ? "coins" : "coin"), 
                    new Vector2(260, 260), Color.White);
            }
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}