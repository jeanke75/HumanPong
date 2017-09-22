using Doggo.HumanPong.Components.GameObjects;

namespace Doggo.HumanPong.Components.Controller
{
    public enum PlayerState {
        UP,
        DOWN,
        IDLE
    }

    public interface IPlayer
    {
        PlayerState GetState(GameObject ball, GameObject paddle);
    }
}
