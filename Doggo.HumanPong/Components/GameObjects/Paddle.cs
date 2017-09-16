using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Doggo.HumanPong.Components.GameObjects
{
    public class Paddle : CollisionObject
    {
        #region Field Region
        protected Pong GameRef;
        #endregion

        #region Property Region

        #endregion

        #region Constructor Region
        public Paddle(Game game, Texture2D texture, Vector2 position) : base(texture, position)
        {
            GameRef = game as Pong;
            Velocity = new Vector2(0, 100);
        }
        #endregion

        #region Method Region
        #endregion
    }
}
