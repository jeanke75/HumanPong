# HumanPong
Pong totally made by Humans™


### UserSettings

The first time you run the game a file "UserSettings.cfg" will appear in the root of the game folder. Below are all the available settings, what they do and their possible values. For now this is the only way to change these settings, but there will be an ingame options menu down the road ;)

| Key           | Description   | Possible values | Default |
|---------------|---------------|-----------------|---------|
| resolution    | The resolution to run the game at | See resolutions below | R1920x1080 |
| fullscreen    | Run in fullscreen mode | 0,1 | 0 |
| fpsCounter    | Default fps counter state | 0,1 | 0 |
| toggleFps     | Toggle the fps counter on/off | https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx | F1 |
| player1Up     | Press/hold this key to move player1 up | https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx | Z |
| player1Down   | Press/hold this key to move player1 down | https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx | S |
| player2Up     | Press/hold this key to move player2 up | https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx | Up |
| player2Down   | Press/hold this key to move player2 down | https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx | Down |
| startRound    | Press this key when the ball is static in the middle to start the round | https://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx | Space |
| gameMode      | Sets the mode the game will run (temporarily) | 0 (AI vs AI), 1 (Player vs AI), 2 (Player vs Player) |
| gameScoreMax  | Sets the score to reach to win the game (not used ingame yet) | value between 1 and 256 |


#### Resolutions

| Aspect Ratio   | Resolution code |
|:--------------:|-----------------|
| 4:3            | R640x480, R800x600, R1024x768, R1152x864, R1280x960, R1400x1050, R1600x1200, R1920x1440, R2048x1536 |
| 16:9           | R640x360, R852x480, R1024x576, R1280x720, R1365x768, R1600x900, R1920x1080, R2560x1440 |
