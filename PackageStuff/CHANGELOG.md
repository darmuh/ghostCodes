## Change Log

### [2.0.1] **CURRENT VERSION**
 - Hotfix to address compatibility issues with Terminal patch and other mods.
	- Thanks to SourceShard of the modding discord for the quick report!
	- Ghostly responses on the terminal will now always return to regular green terminal text color after 3 seconds.
		- May change this in the future with an alternative solution.

### [2.0.0]
 - Mod has been completely reworked for better performance and more readability between modes.
 - Readme updated for more clarity on different modes and configuration options.
 - Updated configuration file sections for clarity.
 - Added RemoveOrphanedEntries method by Kittenji to remove old unused configuration options.
 - Added new methods to fight back against ghost codes
	- Terminal reboot
	- Group Emotes
 - Added new insanity handling methods including buffs/bonuses
	- Buffs (insanity value decreases) include: soloAssist and emoteBuff
	- Bonuses (insanity value increases) include: deathBonus and ggBonus
 - Added ways to fight back against ghost girl hauntings (and their associated ghost codes)
	- ggShowerCheck: Take a shower to stop getting chased and delay codes/hauntings.
		- ggShowerStopChasingChance: Percentage chance 
	- ggDeathNote: Transfer your haunting to another player by typing their name in the terminal.
		- ggDeathNoteChance: Percentage chance typing another player's name in the terminal will get the ghost girl to haunt them.
		- ggDeathNoteMaxStrikes: Maxmium amount of times you can type another player's name in the terminal.
	- ggEmoteCheck: When [ggEmoteStopChasePlayers] amount of players are emoting, stop a ghost girl from chasing you.
		- ggEmoteStopChasingChance: Percentage chance having the right amount of players emoting will actually stop her from chasing you.
 - Added new max rapidfire duration configuration items.
	- rapidFireCooldown: enable or disable max rapidfire duration.
	- rapidFireMaxHours: hours until cooldown from rapidfire, minium is 1 hour.
- Added compatibility with facilityMeldtown and it's meltdown event.
	- rapidFire lights will be disabled during the meltdown event.
- Added new configuration option for changing the color of the lights during rapidFire
	- rfRLcolorChange: enable or disable changing the color of the lights during rapidFire.
	- rfRLcolorValue: The color to change the lights to during rapid fire event.
 - Added the ability for ghost codes to mess with regular doors inside the facility, including locking them.
	- See "Regular Doors" section of the config.
 - Added the ability for ghost codes to mess with stuff inside the ship including lights, doors, monitors, and the terminal itself.
	- See "Ship Stuff" section of the config.
 - Added the ability for ghost codes not to use any of the typical terminal object codes that affect blast doors, mines, and turrets.
	- This should allow the plugin to function if none of these objects exist or you have set the config to ignore them all.
	- This also fixes a minor bug from the previous version that would ALWAYS pick one of the 3 objects to run during a ghost code event.
 - Updated code handling to handle up to 3 actions at once.
	- I may make this configurable in the future.
	- This does not mean 3 actions will always happen each code, it is dependendent on the set configurations.
	- If all interactions are set to high chances, 3 actions will happen more often than not.
	- if all interactions are set to low chances, 3 actions happening at once will be more rare.
 - Added extensiveLogging configuration to move most debugging log info to a hidden log source that will only be enabled if you want it.
 - Many more changes in the code itself, hard to remember every single thing as this rework took some time.

 [Planned Features (**not currently in this version**)]
 - Custom sounds to play over the terminal to replace current sounds.
 - Have tell sounds from the terminal also play over the walkie
 

 <details>
 <summary>Historical Patch Notes</summary>

### [1.5.1]

 - Added a configuration option to bypass GGE if a moon does not have the possibility for a ghost girl spawning.
 - Added configuration option to modify the moons list for when GGE will be bypassed for the other modes.
 - Changed signal translator messages sent by ghostGirl and added a common handling method for this action.
 - Added configuration option for custom messages to be sent by the ghost girl over the signal translator.
 - Added a extra null reference handling for the nethandler to deal with an error that occurs on lobby restart.

### [1.5.0]

- Reworked code for mod to be more modular to add new mode focused entirely on ghostGirl interactions.
- Fixed cases where the ship landed at places without interactable objects like the company building. (you've likely seen/heard this).
- Above is fixed by checking for levels that are marked as safe or the level name is specifically "CompanyBuildingLevel"
- Fixed other rare cases where codes tried to continue running after the ship had left.
- Added Solo Assist for solo players who play with Insanity Mode.
- BETA: Ghost Girl Enhanced Mode (enabled by default) will send codes ONLY when a ghost girl has spawned on the level and is actively hunting someone.
- Ghost Girl Enhanced Mode is IN BETA, please report any bugs on discord or github.
- Added Optional Networking as part of the above mode and some other various functions that require networking. With networking on, EVERYONE will need the mod. Without, only the host needs this mod.
- Added spooky lights flickering (networking required) during rapidFire codes event.
- Added the ability for mines to blow up when called by a ghostCode.
- Added the ability for doors to get hungry and start chomping when called by a ghostCode (hungrydoors).
- Added the ability for a ghostCode to flip the breaker and cut all facility lights.
- All of the above new functions from ghostCodes have configurable percentage chances. The higher the chance of these, the less likely a normal code is sent.
- There are also configuration options for the chances these actions happen during rapidFire, if they can be called.
- Added configuration for rapidFire lights flickering to disable them for those of you who have epilepsy or find flashing lights annoying more than anything else.
- Added configuration for customizing how rapid the lights flicker during rapidFire mode.
- Added configuration options for SoloAssist mode to adjust how much insanity this buff removes at different periods of the day. (this only affects insanity for this mod)
- If Solo Assist buff removes more insanity than you presently have, it will set the insanity level to 0. (this only affects insanity for this mod)
- Added a fix for what I believe was a typo from Zeekers and the ghost girl now has the chance of setting the entire facility's lights off.
- The above fix can be configured on/off depending on your preference, i've also modified it to be less likely than Zeekers had it in their code by default.
- Chances of ghost girl setting the facility's lights off is configurable as well.
- Added code broadcast effect to terminal (networking required) when ghostCodes are used.
- Terminal will now play different sounds whenever a ghostCode is sent. (only plays the alarm sound without networking)
- When a ghost girl enemy exists, terminal will play random sounds of her's when a ghostCode is sent.
- If you have a Signal Translator, certain ghost codes will send a signal translator message with their action.



### [1.1.1]
 - Fixed cases where there were no interactable objects throwing error in the console.
 - Fixed rare cases where the codes still kept going after the ship had left.
 - Fixed the number of codes not being reset going into the next round.
 - Added minimum codes to send per round configuration option
 - Added filter option for each interactable object (door, landmine, & turret).
 - Added Insanity Mode which decreases time between ghostCodes as the group's total insanity levels goes up.
 - Added Turret Berserk Mode as another possible interaction when the ghostCode targets a turret object.
 - Added configuration options for the chances of turrets going berserk in both Normal ghostCodes & Max Insanity mode.
 - Added configuration options for Insanity Mode.

### [1.0.1]
 - Fixed ghostCodes not disabling if the ship leaves before the codes hit max.

### [1.0.0]
*Initial release version*

Configurable Values:
- set interval wait times
- random interval wait times
- max codes to be sent in a round
- terminal sound for when a code is sent (enable/disable)
- use random intervals or set intervals (enable/disable random)

</details>