The game includes a background music that can be muted in the menu.
The audio is splited to 2 groups: Background (for background music) and Sfx (for sound-effects).

There are 3 different levels, each level is defined in a ScriptableObject and composed of:
* Background asset which holds the background image for this level.
* Level duration which defines the max time the player has.
* BallInfo objects array which stores the information about the balls that will be spawned in the game.

The background asset, has an array of Sprites where every sprite is the same image but in different aspect-ratio. When the level is loaded, the image that best fits the screen's aspect-ratio is selected.
This way, we minimize stretching artifacts.

The BallInfo asset holds the following:
* A reference to the prefab that will be intantiated.
* The level of the ball - which defines how many time it can be split (the ball script also uses this variable to determine the ball's size and speed).
* How much time (in seconds) after the level was loaded the ball will appear.

The game uses simple visuals with few icons here and there.