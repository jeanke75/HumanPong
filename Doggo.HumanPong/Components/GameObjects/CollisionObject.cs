using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Doggo.HumanPong.Components.GameObjects
{
    public class CollisionObject
    {
        #region Field region
        protected Texture2D texture;
        public Vector2 Position;
        public Vector2 Velocity;
        #endregion

        #region Property Region
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }
        #endregion

        #region Constructor Region
        public CollisionObject(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.Position = position;
        }

        public CollisionObject(Texture2D texture, Vector2 position, Vector2 velocity) : this(texture, position)
        {
            this.Velocity = velocity;
        }
        #endregion

        #region Method Region
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
        }
        #endregion
    }
}
