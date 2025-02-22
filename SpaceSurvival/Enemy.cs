using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceSurvival
{
    public class Enemy
    {
        private Texture2D _texture;
        public Vector2 Position { get; private set; }
        private Vector2 _velocity;

        public Enemy(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            _texture = texture;
            Position = position;
            _velocity = velocity;
        }
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height);
            }
        }

        public void Update(float deltaTime)
        {
            // Move the enemy
            Position += _velocity * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch, float scale = 0.5f)
        {
            Vector2 scaledSize = new Vector2(_texture.Width, _texture.Height) * scale;

            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
    }
}
