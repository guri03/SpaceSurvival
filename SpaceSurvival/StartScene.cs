using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;

namespace SpaceSurvival
{
    public class StartScene
    {
        private Game1 _game;
        private SpriteFont _font;
        private Texture2D _buttonTexture;

        private Button _playButton;
        private Button _helpButton;
        private Button _aboutButton;
        private Button _highScoreButton;
        private bool _isVisible;

        public string _playerName = string.Empty;
        private StringBuilder _inputBuilder;
        private Rectangle _nameTextBoxBounds;
        private bool _isTyping = false;
        private Texture2D _textBoxTexture;

        private string _placeholderText = "Enter Name";
        private KeyboardState _previousKeyboardState;

        public StartScene(Game1 game)
        {
            _game = game;
            _isVisible = false;
            _inputBuilder = new StringBuilder();
            _nameTextBoxBounds = new Rectangle(10, 10, 200, 40);
            _textBoxTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
            _textBoxTexture.SetData(new Color[] { Color.Gray });
        }

        public void Show()
        {
            if (!_isVisible)
            {
                _font = _game.Content.Load<SpriteFont>("Font");
                _buttonTexture = _game.Content.Load<Texture2D>("ButtonTexture");

                int buttonWidth = _buttonTexture.Width;
                int buttonHeight = _buttonTexture.Height;
                int spaceBetweenButtons = 20;

                int totalHeight = buttonHeight * 3 + spaceBetweenButtons * 2;
                int startY = (_game.GraphicsDevice.Viewport.Height - totalHeight) / 2;

                _playButton = new Button(_buttonTexture, _font, "Play", new Vector2(300, startY));
                _helpButton = new Button(_buttonTexture, _font, "Help", new Vector2(300, startY + buttonHeight + spaceBetweenButtons));
                _aboutButton = new Button(_buttonTexture, _font, "About", new Vector2(300, startY + 2 * (buttonHeight + spaceBetweenButtons)));
                _highScoreButton = new Button(_buttonTexture, _font, "HighScore", new Vector2(300, startY + 3 * (buttonHeight + spaceBetweenButtons)));

                _textBoxTexture = new Texture2D(_game.GraphicsDevice, 1, 1);
                _textBoxTexture.SetData(new Color[] { Color.Gray });

                _isVisible = true;
            }
        }

        public void Hide()
        {
            _isVisible = false;
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Enter) && _inputBuilder.Length > 0)
            {
                _playerName = _inputBuilder.ToString();
                _game.ChangeScene(GameState.Play,_playerName);  //change to Play scene if name is entered
            }
            if (keyboardState.IsKeyDown(Keys.Back) && _inputBuilder.Length > 0)
            {
                _inputBuilder.Remove(_inputBuilder.Length - 1, 1);  // Remove last character on backspace
            }

            foreach (var key in keyboardState.GetPressedKeys())
            {
                if (key >= Keys.A && key <= Keys.Z)  // Add letters only
                {
                    // Only add a key if it wasn't just pressed in the previous state to avoid repeating characters
                    if (!_previousKeyboardState.IsKeyDown(key))
                    {
                        _inputBuilder.Append(key.ToString());
                    }
                }
            }

            _previousKeyboardState = keyboardState; // Update the previous keyboard state for the next frame

            // Handle button clicks for scene change
            if (!_isVisible) return;

            var mouseState = Mouse.GetState();
            var mousePosition = new Point(mouseState.X, mouseState.Y);
            if (_playButton.IsClicked(mouseState) && !string.IsNullOrEmpty(_playerName))
            {
                _game.ChangeScene(GameState.Play,_playerName);
            }
            if (_helpButton.IsClicked(mouseState)) _game.ChangeScene(GameState.Help);
            if (_aboutButton.IsClicked(mouseState)) _game.ChangeScene(GameState.About);
            if(_highScoreButton.IsClicked(mouseState)) _game.ChangeScene(GameState.HighScore); // Handle High Score button click
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!_isVisible) return;

            spriteBatch.Begin();

            if (_inputBuilder.Length == 0)
            {
                spriteBatch.DrawString(_font, _placeholderText, new Vector2(_nameTextBoxBounds.X + 10, _nameTextBoxBounds.Y + 10), Color.Gray);
            }
            else
            {
                spriteBatch.DrawString(_font, _inputBuilder.ToString(), new Vector2(_nameTextBoxBounds.X + 10, _nameTextBoxBounds.Y + 10), Color.White);
            }
            _playButton.Draw(spriteBatch);
            _helpButton.Draw(spriteBatch);
            _aboutButton.Draw(spriteBatch);
            _highScoreButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        public string GetPlayerName()
        {
            return _playerName;
        }
    }
}