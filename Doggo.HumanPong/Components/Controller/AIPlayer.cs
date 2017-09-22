using Doggo.HumanPong.Components.GameObjects;

namespace Doggo.HumanPong.Components.Controller
{
    public class AIPlayer : IPlayer
    {
        #region Constructor Region
        public AIPlayer() { }
        #endregion

        #region Method Region
        public PlayerState GetState(GameObject ball, GameObject paddle)
        {
            // only move if the ball is coming towards the player
            if ((paddle.Position.X < ball.Position.X && ball.Velocity.X < 0) || (paddle.Position.X > ball.Position.X && ball.Velocity.X > 0))
            {
                if (paddle.BoundingBox.Center.Y > ball.BoundingBox.Center.Y)
                {
                    return PlayerState.UP;
                }
                else if (paddle.BoundingBox.Center.Y < ball.BoundingBox.Center.Y)
                {
                    return PlayerState.DOWN;
                }
            }

            return PlayerState.IDLE;
        }
        #endregion
    }
}
