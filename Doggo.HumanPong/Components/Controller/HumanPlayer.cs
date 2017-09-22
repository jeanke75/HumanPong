using Doggo.HumanPong.Components.GameObjects;
using Doggo.HumanPong.Components.Utility;
using Microsoft.Xna.Framework.Input;

namespace Doggo.HumanPong.Components.Controller
{
    public class HumanPlayer : IPlayer
    {
        #region Field Region
        Keys KeyUp;
        Keys KeyDown;
        #endregion

        #region Constructor Region
        public HumanPlayer(Keys up, Keys down)
        {
            KeyUp = up;
            KeyDown = down;
        }
        #endregion

        #region Method Region
        public PlayerState GetState(GameObject ball, GameObject paddle)
        {
            if (Xin.KeyboardState.IsKeyDown(KeyUp))
            {
                return PlayerState.UP;
            }
            else if (Xin.KeyboardState.IsKeyDown(KeyDown))
            {
                return PlayerState.DOWN;
            }
            
            return PlayerState.IDLE;
        }
        #endregion
    }
}
