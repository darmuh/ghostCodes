# ghostCodes v2.5.0

## WARNING: This mod has been completely reworked as of version 2.5.0 and is still under development! Please report any issues on github or via the modding discord. 

## **For client-side only use, disable ModNetworking in config (found in the "__NETWORKING" section)**

## Description

There's a ghost in the terminal and it's sending random broadcast codes to mess with the facility!

This mod aims to add a fun little random element to the game. A "ghost" is in the terminal and it is causing all kinds of mayhem.

### **[DISCLAIMER]**
- If you have SEVERE EPILEPSY you should do one of the following: 
	- disable the following configuration option by setting it to FALSE - [RapidLights]
		- This setting can be found in the ghostCodes.Setup config under the section "Effects/Sound"	
	- Raise both the following configuration options so that the lights flash slow enough not to affect your condition. - [RapidLightsMin] & [RapidLightsMax]
- This mod will now delete old & obsolete configuration options, please make sure to adjust your config following any updates.
- With the ModNetworking configuration option set to [TRUE], EVERYONE will need the mod. With the ModNetworking configuration option set to [FALSE], only the host needs this mod.
	- If ModNetworking is set to [FALSE], any config settings that rely on networking will automatically be set to [FALSE]

### The Different Modes
This Mod comes highly configurable with a variety of different modes available.

- With the 2.5.0 rework came a simplification of the possible ghostcode configurations. There are now three clear modes to choose from:
	- Intervals: GhostCodes will be sent to the terminal at intervals. These intervals can be set or random depending on your RandomIntervals setting.
	- Insanity: GhostCodes will be sent to the terminal at intervals, with group insanity levels adjusting the frequency of these intervals.
	- Hauntings: GhostCodes will be sent during hauntings from the ghost girl. When the ghost girl begins chasing a player the codes will fire rapidly.
 - Version 2.5.0 also replaced GGEBypass with a SecondaryMode option. Now if you'd like to have a set list of moons to run a different mode you will set the mode here.
	- For different interaction chances/configurations enable the [UseSecondaryInteractionsConfig] config item.
 - In all modes except for hauntings, pulling the apparatus will now completely stop ghostcodes from continuing.

### ghostCode Interactions
- GhostCodes has a plethora of different possible interactions. Please see the ghostCodes.Interactions config to set these to your liking.
- Some Examples of ghostcode interactions are below:
	- Open or Close Blast Doors
	- Cause Blast Doors to go haywire and close/shut multiple times
	- Disable Turrets
	- Cause Turrets to malfunction and go Berserk
	- Disable Mines
	- Cause Mines to suddenly combust
	- Short Circuit the breaker and cut out the facility's lights
	- Broadcast random signal translator messages
	- Open/Close/Lock/Unlock Regular Doors within the facility
	- Haunted Doors
	- Mess with the doors on the ship
	- Mess with the lights on the ship
	- Activate either of the teleporters
	- Disable Turrets on Toilheads (Toilhead mod required)
	- Make Turrets on Toilheads go berserk (Toilhead mod required)
	- Adjust all players, a random player, or the currently haunted player's items' batteries **(NETWORKING REQUIRED)**
	- Mess with the monitors on the ship **(NETWORKING REQUIRED)**
	- Shock terminal users out of the terminal **(NETWORKING REQUIRED)**
	- Ghost girl breathe on walkies **(NETWORKING REQUIRED)**
	- Garble all walkie talkies **(NETWORKING REQUIRED)**
	- Cruiser Interactions added in v66

### Fight against the hauntings (CounterPlay)
 - You can combat against ghost codes by doing the following:
	 - Reboot the Terminal (takes anywhere between 30 and 90 seconds)
		- Done by typing "reboot" in the terminal, this will make the terminal unusable for the duration of the reboot.
		- Following a successful reboot, the ghostcodes will go away for a little while.
		- Available in all modes. **(NETWORKING REQUIRED)**
	 - Transfer Hauntings to another player. (Death Note)
		- Only available in the Hauntings mode.
		- If you are currently being haunted, you can type another player's name in the terminal to tell the ghost girl to haunt them instead.
		- For each failed attempt an entry in your death note is removed, and you only have so many entries [DeathNoteMaxStrikes]
		- Also every failed attempt will have the ghost girl start chasing you.
	 - Emoting
		- In the Hauntings mode, this will get the ghost girl to stop chasing you.
			- This is a group activity and amount of players required is based on [EmoteStopChaseRequiredPlayers]
		- In Insanity Mode, each player emoting will lower group sanity levels by value determined in [EmoteBuff]
	 - Take a shower
		- Only effective in the Hauntings Mode.
		- Wash the ghost girl mark off to stop her from chasing you.
		- Will not stop her from continuing to haunt you after your shower.

### Other things this mod does (Misc)

 - When a ghost girl starts a chase, she can trigger flipping the breaker.
	- This looks to have been an idea that was scrapped from the main game that I am patching back in.
	- Use [FixGhostGirlBreakers] to enable and [VanillaBreakersChance] for the odds it'll happen.

### Config Helpers (NEW in 2.5.0)
 - Utilizing OpenLib's config to webpage feature, you can now generate a web page for your config item to modify it to your liking.
	- In the future I will host an example page if you'd rather use this than run a local webpage on your computer.
 - For steps on how to utilize this feature, please see my guide created for LethalConstellations [here](https://thunderstore.io/c/lethal-company/p/darmuh/LethalConstellations/wiki/2563-how-to-use-webconfig/)

Remember: The ghost doesn't really care if it's helping or not it's just sending codes to be noticed. The more you notice it the more it likes to say hello. Have fun! :)

And one final reminder that this rework is still in progress with more features planned. Please treat 2.5.0 as an alpha version for your testing enjoyment!