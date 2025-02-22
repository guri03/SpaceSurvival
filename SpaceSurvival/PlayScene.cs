using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace SpaceSurvival
{
    public class PlayScene
    {
        private Game1 _game;
        private SpriteFont _font;

        private string _playerName;
        public int lives;

        private Texture2D _playerTexture;
        private Vector2 _playerPosition;
        private float _playerSpeed = 200f;

        private Texture2D _enemyTexture;
        private List<Enemy> _enemies;
        private Texture2D _coinTexture;
        private List<Coin> _coins;
        private Texture2D _attackTexture;
        private Vector2 _attackPosition; 
        private bool _isAttacking;
        private float _attackDuration = 0.2f; 
        private float _attackTimer = 0f;
        private Random _random;

        private float _spawnTimer = 0f;
        private float _coinSpawnTimer = 0f;
        private int _score;
        private int _health;

        // Sound effects and music
        private SoundEffect _coinSound;
        private SoundEffect _enemyHitSound;
        private Song _backgroundMusic;
        private SoundEffect _gameOverMusic;

        private List<Projectile> _projectiles;


        public PlayScene(Game1 game)
        {
            _game = game;
            _random = new Random();
            _enemies = new List<Enemy>();
            _coins = new List<Coin>();
            _projectiles = new List<Projectile>();
        }

        public void Show()
        {
            // Load resources
            _font = _game.Content.Load<SpriteFont>("Font");
            _playerTexture = _game.Content.Load<Texture2D>("DurrrSpaceShip");
            _enemyTexture = _game.Content.Load<Texture2D>("EnemyTexture");
            _coinSound = _game.Content.Load<SoundEffect>("CoinSound");
            _attackTexture = _game.Content.Load<Texture2D>("AttackTexture");
            _coinTexture = _game.Content.Load<Texture2D>("CoinTexture");  // Load the coin texture
            _enemyHitSound = _game.Content.Load<SoundEffect>("crash");
            _backgroundMusic = _game.Content.Load<Song>("BackgroundMusic");
            _gameOverMusic = _game.Content.Load<SoundEffect>("GameOver");

            // Play background music
            MediaPlayer.Play(_backgroundMusic);
            MediaPlayer.IsRepeating = true;

            // Initialize variables
            _playerPosition = new Vector2(400, 300);
            _score = 0;
            _health = 100;
            lives = 5;

            // Clear any existing enemies or coins
            _enemies.Clear();
            _coins.Clear();
        }
        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdatePlayer(deltaTime);

            UpdateEnemies(deltaTime);

            UpdateCoins(deltaTime);

            HandleAttack(gameTime);

            foreach (var projectile in _projectiles)
            {
                projectile.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            // Check collisions between projectiles and enemies
            // Create a copy of the collections to iterate over
            var projectilesCopy = new List<Projectile>(_projectiles);
            var enemiesCopy = new List<Enemy>(_enemies);

            foreach (var projectile in projectilesCopy)
            {
                foreach (var enemy in enemiesCopy)
                {
                    if (projectile.Bounds.Intersects(enemy.Bounds))
                    {
                        if (_health < 100)
                        {
                            _health += 15;
                        }// Increase score
                        _enemies.Remove(enemy); // Remove the enemy
                        _projectiles.Remove(projectile); // Remove the projectile
                        break;
                    }
                }
                if (IsGameOver())
                {
                    _game.ScoreManager.SaveScore(_score);
                    _game.ChangeScene(GameState.GameOver, _playerName, _score);
                }
            }

            _projectiles.RemoveAll(p => p.Position.X > _game.GraphicsDevice.Viewport.Width);


            // Increment score
            _score++;

            // End game logic if health <= 0 or lives finished
            if (_health <= 0 || lives ==0)
            {
                MediaPlayer.Stop(); 
                _gameOverMusic.Play();

                _game.ScoreManager.SaveScore(_score);

                _game.ChangeScene(GameState.GameOver);
            }
        }
        private void UpdatePlayer(float deltaTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.W)) _playerPosition.Y -= _playerSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.S)) _playerPosition.Y += _playerSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.A)) _playerPosition.X -= _playerSpeed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.D)) _playerPosition.X += _playerSpeed * deltaTime;

            // Keep the player within the screen bounds
            _playerPosition.X = MathHelper.Clamp(_playerPosition.X, 0, _game.GraphicsDevice.Viewport.Width - _playerTexture.Width);
            _playerPosition.Y = MathHelper.Clamp(_playerPosition.Y, 0, _game.GraphicsDevice.Viewport.Height - _playerTexture.Height);
        }

        private void UpdateEnemies(float deltaTime)
        {
            // Spawn enemies every 2 seconds
            _spawnTimer += deltaTime;
            if (_spawnTimer >= 0.5f)
            {
                _spawnTimer = 0f;
                Vector2 position = new Vector2(_random.Next(0, _game.GraphicsDevice.Viewport.Width - _enemyTexture.Width), -_enemyTexture.Height);
                Vector2 velocity = new Vector2(0, _random.Next(50, 150)); // Move downwards
                _enemies.Add(new Enemy(_enemyTexture, position, velocity));
            }

            foreach (var enemy in _enemies)
            {
                enemy.Update(deltaTime);

                if (_isAttacking)
                {
                    Rectangle attackBounds = new Rectangle(
                        (int)_attackPosition.X,
                        (int)_attackPosition.Y,
                        _attackTexture.Width,
                        _attackTexture.Height
                    );

                    if (attackBounds.Intersects(enemy.Bounds))
                    {
                        if (_health < 100)
                        {
                            _health += 15;
                        }// Increase score by 20
                        _enemies.Remove(enemy); // Remove the enemy
                        break; // Prevent hitting multiple enemies with a single attack
                    }
                }

                if (IsColliding(_playerPosition, _playerTexture.Bounds.Size.ToVector2(), enemy.Position, _enemyTexture.Bounds.Size.ToVector2()))
                {
                    _health -= 10; // Reduce health on collision
                    lives -= 1;
                    _enemyHitSound.Play(); // Play enemy hit sound
                    _enemies.Remove(enemy);
                    break;
                }
            }


            // Remove enemies that leave the screen
            _enemies.RemoveAll(e => e.Position.Y > _game.GraphicsDevice.Viewport.Height);
        }

        private void UpdateCoins(float deltaTime)
        { 
            _coinSpawnTimer += deltaTime;
            if (_coinSpawnTimer >= 1.5f) // Spawn coins every 3 seconds
            {
                _coinSpawnTimer = 0f; // Reset the timer
                Vector2 position = new Vector2(_random.Next(0, _game.GraphicsDevice.Viewport.Width - _coinTexture.Width), -_coinTexture.Height);
                Vector2 velocity = new Vector2(0, _random.Next(50, 150)); //
                _coins.Add(new Coin(_coinTexture, position, _coinSound,velocity));
            }

            foreach (var coin in _coins)
            {
                coin.Update(deltaTime);

                if (IsColliding(_playerPosition, _playerTexture.Bounds.Size.ToVector2(), coin.Position, _coinTexture.Bounds.Size.ToVector2()))
                {
                    _coins.Remove(coin);
                    if (_health < 100)
                    {
                        _health += 5;
                    }
                    _coinSound.Play();
                    break; // Prevent multiple coins from being collected at once
                }
            }

            // Remove coins that leave the screen
            _coins.RemoveAll(c => c.Position.Y > _game.GraphicsDevice.Viewport.Height);
        }

        private bool IsColliding(Vector2 pos1, Vector2 size1, Vector2 pos2, Vector2 size2)
        {
            return !(pos1.X + size1.X < pos2.X || pos1.X > pos2.X + size2.X ||
                     pos1.Y + size1.Y < pos2.Y || pos1.Y > pos2.Y + size2.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (!string.IsNullOrEmpty(_playerName))
            {
                var font = _game.Content.Load<SpriteFont>("Font");
                spriteBatch.DrawString(font, $"Player: {_playerName}", new Vector2(10, 10), Color.White);
            }
            spriteBatch.Draw(_playerTexture, _playerPosition, Color.White);

            if (_isAttacking)
            {
                Vector2 attackScale = new Vector2(0.1f, 0.1f);
                spriteBatch.Draw(_attackTexture, _attackPosition, null, Color.White, 0f, Vector2.Zero, attackScale, SpriteEffects.None, 0f);
            }
            foreach (var enemy in _enemies)
            {
                enemy.Draw(spriteBatch, scale: 1f);
            }

            foreach (var coin in _coins)
            {
                coin.Draw(spriteBatch);
            }

            foreach (var projectile in _projectiles)
            {
                Vector2 projectileScale = new Vector2(0.3f, 0.3f); // Shrink projectiles too
                projectile.Draw(spriteBatch, projectileScale);
            }


            // Draw UI (score, health)
            spriteBatch.DrawString(_font, $"Score: {_score}", new Vector2(10, 40), Color.White);
            spriteBatch.DrawString(_font, $"Health: {_health}", new Vector2(10, 70), Color.White);
            spriteBatch.DrawString(_font, $"Lives: {lives}", new Vector2(10, 100), Color.White);

            spriteBatch.End();
        }
        public void Hide()
        {
        }

        public void SetPlayerName(string playerName)
        {
            _playerName = playerName;
        }

        private void HandleAttack(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                // Fire a projectile
                Vector2 projectilePosition = new Vector2(
                    _playerPosition.X + (_playerTexture.Width / 2) - (_attackTexture.Width / 2),
                    _playerPosition.Y + (_playerTexture.Height / 2) - (_attackTexture.Height / 2)
                );

                Vector2 projectileVelocity = new Vector2(0,-300f); // Move right at a constant speed
                Vector2 projectileScale = new Vector2(0.3f, 0.3f);
                _projectiles.Add(new Projectile(_attackTexture, projectilePosition, projectileVelocity,projectileScale));
            }
        }
        private bool IsGameOver()
        {
            return false; 
        }
    }
}
