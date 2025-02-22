using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SpaceSurvival
{
    public class GameOverScene
    {
        private Game1 _game;
        private SpriteFont _font;
        private Texture2D _buttonTexture;

        private Button _retryButton;
        private Button _backButton;
        private Button _highScoreButton;

        private int _playerScore {  get; set; }
        private bool _showHighscores = false;

        private List<int> _highScores;

        public GameOverScene(Game1 game, int playerScore)
        {
            _game = game;
            _playerScore = playerScore;
            _highScores = new List<int>();
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            // Load assets
            _font = content.Load<SpriteFont>("Font");
            _buttonTexture = content.Load<Texture2D>("ButtonTexture");

            // Initialize buttons
            _retryButton = new Button(_buttonTexture, _font, "Retry", new Vector2(300, 400));
            _retryButton.OnClick = () =>
            {
                _game.ChangeScene(GameState.Play);
            };
            _backButton = new Button(_buttonTexture, _font, "Back", new Vector2(300, 200));
            _backButton.OnClick = () =>
            {
                _game.ChangeScene(GameState.Start);
            };
            _highScoreButton = new Button(_buttonTexture, _font, "HighScore", new Vector2(300, 300));
            _highScoreButton.OnClick = () =>
            {
                _game.ChangeScene(GameState.HighScore);
            };


            // Save current score and load high scores
            _game.ScoreManager.SaveScore(_playerScore);
            _highScores = _game.ScoreManager.LoadScores();
        }

        public void Update(GameTime gameTime)
        {
            // button clicks
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_retryButton.IsClicked(mouseState))
                {
                    _retryButton.OnClick.Invoke();
                }
                if (_backButton.IsClicked(mouseState))
                {
                    _backButton.OnClick.Invoke();
                }
                if (_highScoreButton.IsClicked(mouseState))
                {
                   _highScoreButton.OnClick.Invoke();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Game Over", new Vector2(300, 50), Color.White);            

            _backButton.Draw(spriteBatch);
            _retryButton.Draw(spriteBatch);
            _highScoreButton.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
