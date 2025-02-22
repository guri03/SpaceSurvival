using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceSurvival
{
    public class Button
    {
        private Texture2D _texture;
        private SpriteFont _font;
        private string _text;
        private Vector2 _position;
        private Rectangle _bounds;
        internal Action OnClick;

        public Button(Texture2D texture, SpriteFont font, string text, Vector2 position)
        {
            _texture = texture;
            _font = font;
            _text = text;
            _position = position;
            _bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public bool IsClicked(MouseState mouseState)
        {
            return mouseState.LeftButton == ButtonState.Pressed &&
                   _bounds.Contains(mouseState.Position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Scale the button texture when drawing it
            float scale = 0.5f;
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            // Recalculate text position based on the scaled size of the button
            var textSize = _font.MeasureString(_text);
            Vector2 textPosition = new Vector2(
                _position.X + (_texture.Width * scale - textSize.X) / 2, // Center text horizontally
                _position.Y + (_texture.Height * scale - textSize.Y) / 2 // Center text vertically
            );

            spriteBatch.DrawString(_font, _text, textPosition, Color.White);
        }

    }
}

