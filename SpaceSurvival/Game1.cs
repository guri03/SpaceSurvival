/*Name - Gursimar Kaur
 *Purpose- Final Project
 *Date submitted- 8 Dec 2024*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml.Linq;
using System.Drawing;
using System.IO;
using System.Reflection;
using System;
using System.Runtime.InteropServices;

namespace SpaceSurvival
{
    public enum GameState { Start, Play, Help, About,GameOver,Pause,HighScore }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState _currentState;
        private StartScene _startScene;
        private PlayScene _playScene;
        private HelpScene _helpScene;
        private AboutScene _aboutScene;
        private PauseMenu _pauseMenu;
        private GameOverScene _gameOverScene;
        private HighscoreScene _highscoreScene;
        public ScoreManager ScoreManager { get; private set; }

        public string _playerName;
        public int _playerScore;
        private bool _isPaused = false;
        private string _iconFilePath = "Content/SpaceIcon.ico";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
       
        protected override void Initialize()
        {
            base.Initialize();
            _currentState = GameState.Start;
            ScoreManager = new ScoreManager();
            _pauseMenu = new PauseMenu(this);
        }
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Song backgroundMusic = this.Content.Load<Song>("BackgroundMusic");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            // Initialize scenes
            _startScene = new StartScene(this);
            _playScene = new PlayScene(this);
            _helpScene = new HelpScene(this);
            _aboutScene = new AboutScene(this);
            _gameOverScene = new GameOverScene(this,_playerScore);
            _pauseMenu = new PauseMenu(this);
            _pauseMenu.LoadContent(Content);
            _highscoreScene = new HighscoreScene(this);
            _highscoreScene.LoadContent(Content);
            // Show the Start scene
            _startScene.Show();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_currentState == GameState.Play && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ChangeScene(GameState.Pause);
            }
            // Handle scene updates based on the current game state
            switch (_currentState)
            {
                case GameState.Start:
                    _startScene.Update(gameTime);
                    break;

                case GameState.Play:
                    _playScene.Update(gameTime);
                    break;

                case GameState.Help:
                    _helpScene.Update();
                    break;

                case GameState.About:
                    _aboutScene.Update();
                    break;
                case GameState.GameOver:
                    _gameOverScene?.Update(gameTime);
                    break;
                case GameState.Pause:
                    _pauseMenu.Update(gameTime);
                    break;
                case GameState.HighScore:
                    _highscoreScene.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            if (_isPaused)
            {
                _pauseMenu.Draw(_spriteBatch); // If paused, draw the PauseMenu
                return;
            }
            // Draw the current scene based on the game state
            switch (_currentState)
            {
                case GameState.Start:
                    _startScene.Draw(_spriteBatch);
                    break;

                case GameState.Play:
                    _playScene.Draw(_spriteBatch);
                    break;

                case GameState.Help:
                    _helpScene.Draw(_spriteBatch);
                    break;

                case GameState.About:
                    _aboutScene.Draw(_spriteBatch);
                    break;
                case GameState.GameOver:
                    _gameOverScene?.Draw(_spriteBatch);
                    break;
                case GameState.Pause:
                    _pauseMenu.Draw(_spriteBatch);
                    break;
                case GameState.HighScore:
                    _highscoreScene.Draw(_spriteBatch);
                    break;
            }

            base.Draw(gameTime);
        }
        public void ResumeGame()
        {
            _isPaused = false; // Set the game as not paused
            _currentState = GameState.Play; // Optionally change state back to Play
        }

        /// <summary>
        /// Changes the current game scene and ensures proper scene transitions.
        /// </summary>
        /// <param name="newState">The new game state to transition to.</param>
        public void ChangeScene(GameState newState, string playerName = null, int playerScore = 0)
        {
            // Hide all scenes
            _startScene.Hide();
            _playScene.Hide();
            _helpScene.Hide();
            _aboutScene.Hide();

            _currentState = newState;
            switch (_currentState)
            {
                case GameState.Start:
                    _startScene.Show();
                    break;
                case GameState.Play:
                    if (!string.IsNullOrEmpty(playerName))
                    {
                        _playerName = playerName;
                    }
                    _playScene.SetPlayerName(_playerName);
                    _playScene.Show();
                    break;
                case GameState.Help:
                    _helpScene.Show();
                    break;
                case GameState.About:
                    _aboutScene.Show();
                    break;
                case GameState.GameOver:
                    _gameOverScene = new GameOverScene(this, _playerScore);
                    _gameOverScene.LoadContent(Content);
                    break;
                case GameState.Pause:
                    // Show pause menu
                   _pauseMenu.LoadContent(Content);
                    break;
            }
        }
    }
}
