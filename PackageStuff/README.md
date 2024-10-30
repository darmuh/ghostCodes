# ghostCodes by darmuh

## Description

There's a ghost in the terminal and it's sending random broadcast codes to mess with the facility!

This mod aims to add a fun little random element to the game. A "ghost" is in the terminal and it is causing all kinds of mayhem.

With Ghost Girl Enhanced Mode, this mod gives the ghost girl some interesting powers!

### **[DISCLAIMER]**
- If you have SEVERE EPILEPSY you should do one of the following: 
	- disable the following configuration option by setting it to FALSE - [rfRapidLights]
	- Raise both the following configuration options so that the lights flash slow enough not to affect your condition. - [rfRLmin] & [rfRLmax]
- This mod will now delete old & obsolete configuration options, please make sure to adjust your config following any updates.
- With the ModNetworking configuration option set to [TRUE], EVERYONE will need the mod. With the ModNetworking configuration option set to [FALSE], only the host needs this mod.
	- If ModNetworking is set to [FALSE], any config settings that rely on networking will automatically be set to [FALSE]

### The Different Modes
This Mod comes highly configurable with a variety of different modes available.

**GhostGirlEnhanced (GGE)**
- This is the main mode of this mod and requires [ModNetworking] enabled to work.
- ghost codes will be sent during active hauntings by the ghost girl.
- Various interactions can stop BOTH ghost codes and active hauntings.
- ghost codes will rapidly fire during chase sequences.
	- use [ggIgnoreCodeCount] so that codes are not counted in this mode.
- GhostGirlEnhanced Bypass List [GGEbypassList]
	- Any moon that is listed here will disable this mode in favor of the other modes.
	- Good for moons that have 0% or low odds of the ghost girl spawning.

**Insanity Mode**
- This mode is a secondary mode which will send codes in varying frquencies depending on the calculated group Insanity Level.
- Varying buffs and bonuses will be applied to the base-game insanity values as they are calculated for the group.
- As the group insanity level rises, codes will be sent at more frequent intervals.
- When group insanity levels hit maximum, codes will be sent in a rapid fire fashion (can be disabled in config).
	- The codes will continue to send in a rapidFire fashion until group sanity levels lower from the set maximum.
	- If [rapidFireCooldown] is enabled, codes will stop on a short cooldown when codes have been running for [rapidFireMaxHours].

**Base ghostCodes Mode**
- With the two modes above disabled, this is the third and final mode.
- ghost codes will be sent in intervals throughout the day.
	- Can be set to either random or set intervals based on [useRandomIntervals]
	- The intervals can be further configured in the [Set Interval Configurations] and [Random Interval Configurations] configuration sections.

### Types of ghostCodes
- The ghost sends codes through the terminal that can do the following: (all can be enabled/disabled in config file)
	- Open or Close Blast Doors
	- Cause Blast Doors to go haywire and close/shut multiple times
	- Disable Turrets
	- Cause Turrets to malfunction and go Berserk
	- Disable Mines
	- Cause Mines to suddenly combust
	- Short Circuit the breaker and cut out the facility's lights
	- Broadcast random signal translator messages
	- Open/Close/Lock/Unlock Regular Doors within the facility
	- Mess with the doors on the ship
	- Mess with the lights on the ship
	- Activate either of the teleporters
	- Disable Turrets on Toilheads (Toilhead mod required)
	- Make Turrets on Toilheads go berserk (Toilhead mod required)
	- Drain all players, a random player, or the currently haunted player's items' batteries (NETWORKING REQUIRED)
	- Mess with the monitors on the ship (NETWORKING REQUIRED)
	- Shock terminal users out of the terminal (NETWORKING REQUIRED)
	- Ghost girl breathe on walkies (NETWORKING REQUIRED)
	- Garble all walkie talkies (NETWORKING REQUIRED)
- With [ModNetworking] disabled, any Networking required components in the mod will be disabled and their config options updated.
- With [gcGhostGirlOnly] enabled, any config options listed in [gcGhostGirlOnlyList] will be set to disabled when GhostGirlEnhanced Mode is not active.
	- Updates every time the ship lands on a moon.
	- You may want to double check your config file at the end of a session to set it back to your preferred settings.
- ghost codes will leave it's mark on the terminal via visuals and sounds. These are configurable:
	- [enableBroadcastEffect]: when enabled, will display a code broadcastes symbol on the terminal when a code is sent.
	- [gcEnableTerminalSound]: when enabled, the terminal can play a sound when a ghost code is ran.
		- [gcTerminalSoundChance]: The chances a sound will be displayed over the terminal. 100 = every time a code is ran.
		- [gcUseGirlSounds]: Whether or not the terminal will play ghost girl sounds on the terminal when a ghost girl is present.
		- [gcUseTerminalAlarmSound]: Whether or not to use the Terminal Alarm Sound (With networking disabled, this is the only possible sound to play.)
		- *Custom sounds are a planned feature, not currently implemented.*

### Fight against the hauntings
You can combat against ghost codes by doing the following:
 - Reboot the Terminal (takes anywhere between 30 and 60 seconds)
	- Done by typing "reboot" in the terminal
	- Available in all modes.
 - Emoting
	- In Ghost Girl Enhanced mode, this will get the ghost girl to stop chasing you.
		- This is a group activity and amount of players required is based on [ggEmoteStopChasePlayers]
	- In Insanity Mode, each player emoting will lower group sanity levels by value determined in [emoteBuffNum]
 - Take a shower
	- Only effective during Ghost Girl Enhanced.
	- Wash the ghost girl mark off to stop her from chasing you.
	- Will not stop her from continuing to haunt you after your shower.

### Other things this mod does
 - Transfer Hauntings to another player. (Death Note)
	- If you are currently being haunted, you can type another player's name in the terminal to tell the ghost girl to haunt them instead.
	- For each failed attempt an entry in your death note is removed, and you only have so many entries [ggDeathNoteMaxStrikes]
	- Also every failed attempt will have the ghost girl start chasing you.
 - When a ghost girl starts a chase, she can trigger flipping the breaker.
	- This looks to have been an idea that was scrapped from the main game that I am patching back in.
	- Use [fixGhostGirlBreakers] to enable and [ggVanillaBreakerChance] for the odds it'll happen.
 - When [canSendMessages] is enabled, the signal translator can send random messages from the list [signalMessages]
 - When [monitorsOnShipEvent] interaction is enabled, the ship monitors will display random messages from the list [monitorMessages]

Remember: The ghost doesn't really care if it's helping or not it's just sending codes to be noticed. The more you notice it the more it likes to say hello. Have fun! :)