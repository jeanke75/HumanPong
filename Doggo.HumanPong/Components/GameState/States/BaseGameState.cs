using System;
using Microsoft.Xna.Framework;

namespace Doggo.HumanPong.Components.GameState.States
{
    public class BaseGameState : GameState
    {
        #region Field Region
        protected static Random random = new Random();
        protected Pong GameRef;
        #endregion

        #region Constructor Region
        public BaseGameState(Game game) : base(game)
        {
            GameRef = (Pong)game;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion
    }
}
