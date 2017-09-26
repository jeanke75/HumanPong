using System;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace Doggo.HumanPong.Components.Utility
{
    public enum Resolution
    {
        //4:3
        R640x480,
        R800x600,
        R1024x768,
        R1152x864,
        R1280x960,
        R1400x1050,
        R1600x1200,
        R1920x1440,
        R2048x1536,

        //16:9
        R640x360,
        R852x480,
        R1024x576,
        R1280x720,
        R1365x768,
        R1600x900,
        R1920x1080,
        R2560x1440
    }

    public enum GameMode
    {
        AI,
        Singleplayer,
        Multiplayer
    }

    public class UserSettings
    {
        #region Field Region
        private const string path = "UserSettings.cfg";

        // display settings
        public Resolution resolution;
        public bool fullscreen;
        public bool fpsCounter;

        // general controls
        public Keys toggleFps;

        // game controls
        public Keys player1Up;
        public Keys player1Down;
        public Keys player2Up;
        public Keys player2Down;
        public Keys startRound;

        // game settings
        public GameMode gameMode; // will be removed when game menu
        public byte gameScoreMax;
        #endregion

        #region Property Region
        public int width
        {
            get
            {
                switch (resolution)
                {
                    case Resolution.R640x360:
                    case Resolution.R640x480:
                        return 640;
                    case Resolution.R800x600:
                        return 800;
                    case Resolution.R852x480:
                        return 852;
                    case Resolution.R1024x576:
                    case Resolution.R1024x768:
                        return 1024;
                    case Resolution.R1152x864:
                        return 1152;
                    case Resolution.R1280x720:
                    case Resolution.R1280x960:
                        return 1280;
                    case Resolution.R1365x768:
                        return 1365;
                    case Resolution.R1400x1050:
                        return 1400;
                    case Resolution.R1600x900:
                    case Resolution.R1600x1200:
                        return 1600;
                    case Resolution.R1920x1080:
                    case Resolution.R1920x1440:
                        return 1920;
                    case Resolution.R2048x1536:
                        return 2048;
                    case Resolution.R2560x1440:
                        return 2560;
                    default:
                        return Pong.TargetWidth;
                }
            }
        }

        public int height
        {
            get
            {
                switch (resolution)
                {
                    case Resolution.R640x360:
                        return 360;
                    case Resolution.R640x480:
                    case Resolution.R852x480:
                        return 480;
                    case Resolution.R1024x576:
                        return 576;
                    case Resolution.R800x600:
                        return 600;
                    case Resolution.R1280x720:
                        return 720;
                    case Resolution.R1024x768:
                    case Resolution.R1365x768:
                        return 768;
                    case Resolution.R1152x864:
                        return 864;
                    case Resolution.R1600x900:
                        return 900;
                    case Resolution.R1280x960:
                        return 960;
                    case Resolution.R1400x1050:
                        return 1050;
                    case Resolution.R1920x1080:
                        return 1080;
                    case Resolution.R1600x1200:
                        return 1200;
                    case Resolution.R2048x1536:
                        return 1536;
                    case Resolution.R1920x1440:
                    case Resolution.R2560x1440:
                        return 1440;
                    default:
                        return Pong.TargetWidth;
                }
            }
        }
        #endregion

        #region Constructor Region
        private UserSettings() { }
        #endregion

        #region Method Region
        public static UserSettings LoadSettings()
        {
            var settings = new UserSettings();

            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] param = line.Split(' ');
                    switch (param[0])
                    {
                        case "resolution":
                            settings.resolution = (Resolution)Enum.Parse(typeof(Resolution), param[1]);
                            break;
                        case "fullscreen":
                            settings.fullscreen = Convert.ToBoolean(int.Parse(param[1]));
                            break;
                        case "fpsCounter":
                            settings.fpsCounter = Convert.ToBoolean(int.Parse(param[1]));
                            break;
                        
                        case "toggleFps":
                            settings.toggleFps = (Keys)Enum.Parse(typeof(Keys), param[1]);
                            break;
                        
                        case "player1Up":
                            settings.player1Up = (Keys)Enum.Parse(typeof(Keys), param[1]);
                            break;
                        case "player1Down":
                            settings.player1Down = (Keys)Enum.Parse(typeof(Keys), param[1]);
                            break;
                        case "player2Up":
                            settings.player2Up = (Keys)Enum.Parse(typeof(Keys), param[1]);
                            break;
                        case "player2Down":
                            settings.player2Down = (Keys)Enum.Parse(typeof(Keys), param[1]);
                            break;
                        case "startRound":
                            settings.startRound = (Keys)Enum.Parse(typeof(Keys), param[1]);
                            break;
                        
                        case "gameMode":
                            settings.gameMode = (GameMode)int.Parse(param[1]);
                            break;
                        case "gameScoreMax":
                            settings.gameScoreMax = Convert.ToByte(param[1]);
                            break;
                        
                        default:
                            break;
                    }
                }
            }
            else
            {
                settings.SetDefault();
            }

            return settings;
        }

        public void SaveSettings()
        {
            using (StreamWriter file = new StreamWriter(path))
            {
                file.WriteLine("resolution {0}", resolution.ToString());
                file.WriteLine("fullscreen {0}", (fullscreen ? 1 : 0));
                file.WriteLine("fpsCounter {0}", (fpsCounter ? 1 : 0));
                file.WriteLine("toggleFps {0}", toggleFps.ToString());
                file.WriteLine("player1Up {0}", player1Up.ToString());
                file.WriteLine("player1Down {0}", player1Down.ToString());
                file.WriteLine("player2Up {0}", player2Up.ToString());
                file.WriteLine("player2Down {0}", player2Down.ToString());
                file.WriteLine("startRound {0}", startRound.ToString());
                file.WriteLine("gameMode {0}", (int)gameMode);
                file.WriteLine("gameScoreMax {0}", gameScoreMax.ToString());
            }
        }

        public void SetDefault()
        {
            resolution = Resolution.R1920x1080;
            fullscreen = false;
            fpsCounter = false;

            toggleFps = Keys.F1;

            player1Up = Keys.Z;
            player1Down = Keys.S;
            player2Up = Keys.Up;
            player2Down = Keys.Down;
            startRound = Keys.Space;

            gameMode = GameMode.Singleplayer;
            gameScoreMax = 5;

            SaveSettings();
        }
        #endregion
    }
}
