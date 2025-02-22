using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceSurvival
{
    public class Projectile
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public Vector2 Scale {  get; set; }

        public Projectile(Texture2D texture, Vector2 position, Vector2 velocity,Vector2 scale)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Scale = scale;
        }

        public void Update(float deltaTime)
        {
            Position += Velocity * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch,Vector2 scale)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

}
