# Rift of the Realms

## About the game
Rift of the Realms is a squad based sci-fi shooter that explores the eternal conflict between light and dark forces.

Players must align themselves with either the celestial Light Realm or the sinister Dark Realm, each with unique powers.

## Game Objective and Controls
**Light Realm players:** Your objective is to switch into the Dark Realm, locate and collect spare machine parts, then switch
back to your Light Realm to locate and fix your machines. If you fix all your machines before the timer
runs out, your team will **WIN!**

**Dark Realm players:** You must locate and defend your machine parts while eliminating the Light Realm players whenever they appear. You can proactively jump into the Light Realm and eliminate enemies there too! If the Light Realm team fails to fix all their machines before the timer runs out, your team will **WIN!**

**Both teams:** Earn *silver coins* during the game for eliminations, assists, picking up machine parts, etc. Spend these coins at the in-game shop to buy ammo and health.

Winning team members earn *gold coins*, which are saved into their login account and can be spent in the game lobby 
on items like new skins and custom weapons.


**Controls:**
* Basic "ASWD" and mouse for movement
* SPACE to jump
* SHIFT to sprint
* V to switch realms
* LEFT MOUSE to fire
* R to reload
* B to open the in-game shop to spend your silver coins on upgrading weapons
* *Light Realm only:* Hold E when at a machine to fix it

***

## Developer Notes

There are two parts to this game, the underlying database/API (read more about it in its own [GitHub project](https://github.com/mollymcgames) and this Unity project.)

To get started, clone this project and load it into a new project area in Unity Hub.

Before running the game, make sure that you configure the API URL in the "FPSScene" -> "RoomManager" object -> "Network Manager" script.

Then, load the "Login" scene and run it.

### Building
The game should have all the required scenes in the build settings. Before building, be sure your API URL is correct (see above) and that the game duration is as long as you need it to be. Change this in the in the 
"FPSScene" -> "RoomManager" object -> "Timer" script.

### Scene Functions
In the "Scenes" folder, there are four scenes used in the game and they do the following:
1. *Login:* This scene is used to login to the game (it access the API/Database). It is also used to register a new player
into the game.
1. *StartMenu:* This scene has the shop where you can buy character skins, custom weapons, etc. It serves as the launch point into the actual game and includes quit and logout buttons.
1. *FPSScene:* This scene is the actual game. Inside here are the two realms, each enclosed in an inverted collider sphere that
has a SkyMap map to differentiate between the realms. The players are prefabs and are spawned at points defined 
in the RoomManager GameObject.
1. *GameOverScene:* Once the timer expires, the game transitions to this scene. It displays the winning team, earned gold, and post-game stats. Players can return to the lobby from here to start a new game.

**IMPORTANT NOTE:** Trying to run the game in any other scene directly, without going through LOGIN first will cause
numerous errors because LOGIN sets up a singleton object containing the logged in player's details which are
extensively used throughout the other scenes. You have been warned! The login can take up to 5 mins to connect to the database if the database is asleep. Please wait after trying to login the first time for up to 5 mins if this happens.

### Game resources
In the "Resources" folder there are various graphical and audio elements required for the game. Of particular 
note are the "CharacterLR" and "CharacterDR" prefabs which are used as the character models for the Light and 
Dark realm players.

There is also an "audio" folder in which the game sounds and music are stored.

