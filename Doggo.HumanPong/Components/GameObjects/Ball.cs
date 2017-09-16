using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Doggo.HumanPong.Components.GameObjects
{
    public class Ball : CollisionObject
    {
        #region Field Region
        protected Pong GameRef;
        #endregion

        #region Property Region

        #endregion

        #region Constructor Region
        public Ball(Game game, Texture2D texture, Vector2 position) : base(texture, position)
        {
            GameRef = game as Pong;
            //Velocity = new Vector2();
        }
        #endregion

        #region Method Region
        #endregion
    }
}
