# ghostCodes Change Log

## [2.5.1]
 - Fixed SignalTranslator messages only being sent for specific interactions instead of being sent as it's own interaction.
 - Adjusted TerminalAccessibleObject filtering logic to address rare null ref errors
	- Per recommendation from Zaggy, the list this mod generates is now adjusted via game patches to TerminalAccessibleObject Start and OnDestroy
 - Adjusted MineBoom & TurretBerserk logic to generate a random number from all turrets or mines rather than all terminalaccessibleobjects.
	- This may make the turretberserk/mineboom chances feel higher than they were before.
	- On the flip side, this may make the blast door chances feel like they dropped (as previously if the turret or mine action was called on a non turret/mine object it would just call the code on the object which was usually a door)
 - Fixed/Adjusted ToilHead Compatibility 

## [2.5.0]
 - Mod completely reworked and built to be used with OpenLib.
 - Compiled with new custom sounds for the terminal, ghost girl, etc.
 - New interactions added for the cruiser, haunted doors, ship, etc.
 - New mode system to handle having a secondary mode for specific moons
	- Secondary interactions config added for secondary mode when user requests this config.
 - Config has been split into multiple configs, each one is added in-game via LethalConfig.
 - While this mod is in an alpha state, setting extensive logging config item to true by default.
	- Feel free to disable this but understand that it will be more difficult for me to troubleshoot any issues you encounter.
 - For a more in-depth changelog, please see the diffs on github.

 <details>
 <summary>Historical Patch Notes</summary>

## [2.0.6]
 - Recompiled for use with V55 Game Update.
 - Updated project to utilize plugin version number wherever possible, including compiling the assembly with the version number.
	- Now when I update the version number in one spot, it'll update it everywhere (except the changelog, keeping that manual)

## [2.0.5]
- Hotfix for issue discovered with reboot command causing base-game player teleport function not to work.
	- Small tweak was made for how the terminal is locked out from other players.

## [2.0.4]
- Removed [rapidFireCooldown] config option and adjusted [rapidFireMaxHours] to be configured to include the entire day if no cooldown is wanted.
- Added check to make sure commands are only created for actual players for the deathNote feature.
- Fixed various issues where the config options were not actually being implemented in the mod's code, thank you @Mi6k on discord for taking a deep look at the config.
	- Completed a complete config cleanup to make sure no further items are being missed.
	- Changed how configuration items are created with helper methods to make things easier going forward (for me).
- Added cooldown check to ToilHead interactions, thanks Zehs for adding the public variables
- Switched insanity mode buffs/debuffs to be percentage based configuration options instead of direct numbers.
	- This is to make creating the config a bit more intuitive, as not everyone knows the max possible sanity values and how it all works.
- Adjusted logic detemrining if codes should/can continue to be a bit more straight forward following reports that somehow codes were running in orbit.
- Added new config item [modifiedConfigItemsList] to hold any config items that were modified by gcGhostGirlOnly.
	- DO NOT MODIFY [modifiedConfigItemsList], this config item will be populated on it's own and emptied if no items need to be returned to their initial state.
- Added new section for walkie talkie interactions [Walkies]
	- new interaction [ggBreatheOnWalkies] will play the ghost girl breathing sound effect over all walkie talkies
	- [ggBreatheOnWalkies] is based on chance value [ggBreatheOnWalkiesChance]
- Added new section for Item interactions [Item]
	- currently all config options in this section are related to any item with a battery
	- Drain all players, a random one, or the haunted one's items of battery by a configured percentage [gcBatteryDrainPercentage]
- Added config options to enable/disable entire sections: Items, Walkies, Ship Stuff, and Regular Doors.
- Added error handling for rare scenario where everything in the config is disabled and no possible actions can be chosen to run as a ghost code.
- Updated README with new stuff.

## [2.0.3]
- Reworked ghost girl enhanced handling patches to address some null reference errors that were breaking the mod in certain profiles (thank you Lunxara)
	- Removed DressGirl AI Update patch and ended ghostgirlenhanced coroutine when the girl is not staring in a haunt.
	- Removed BeginChasing prefix, it looked to have been interfering with other mods that adjust the AI.
		- Vanilla breaker fix is now done via a postfix. This means the lights will always flicker briefly before they completely go out (depending on the odds if they do of course).
	- Coroutines will now run when the ghostgirl calls different interactions
		- The regular ghost girl enhanced coroutine is called when the girl is staring in haunt.
		- The rapidFire codes will be called when the girl is actively chasing (this is unchanged).
	- ghost codes will now be initialized directly after the blast door codes have been generated,
		- This is much sooner than before, so please adjust your interval configs accordingly.

## [2.0.2]
 - Fixed incorrect configuration information related to gcGhostGirlOnly & gcGhostGirlOnlyList
	- Also fixed some issues discovered with handling leading spaces.
	- Updated default configuration option to remove spaces after commas anyway.
	- Reworded some other config options for, hopefully, some better clarity.
 - Added new config section for any compatible mods that have added functionality to ghostCodes.
	- Called "External Mod Stuff", this will be where you can find config options relating to various compatible mods.
	- The first mod for this section is ToilHeads, which slaps a turret on top of SOME of the coilheads.
	- Like a regular turret, ghostCodes can either disable the turret on their head or make the turret go berserk.
	- Each action has configurable odds and can be completely disabled.
	- Currently both ToilHeads and Facility Meltdown have direct compatibility written into this mod.
 - Added the ability for ghostCodes to mess with the teleporters.
	- Find the config options for these interactions in "Ship Stuff"
	- Came with one new patch to grab teleporter instances when they spawn.
 - Updated handling from previous hotfix so that ghostly responses will change on loading a new node again.
	- Added some better error-handling cases so that this wont error out like in 2.0.0
	- Also added a check when terminal first spawns to grab the default text colors, rather than assuming it is green.
		- Want to add support for mods that change the text color at some point, as i'm not sure they will have change it this early on.

## [2.0.1]
 - Hotfix to address compatibility issues with Terminal patch and other mods.
	- Thanks to SourceShard of the modding discord for the quick report!
	- Ghostly responses on the terminal will now always return to regular green terminal text color after 3 seconds.
		- May change this in the future with an alternative solution.

## [2.0.0]
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

## [1.5.1]

 - Added a configuration option to bypass GGE if a moon does not have the possibility for a ghost girl spawning.
 - Added configuration option to modify the moons list for when GGE will be bypassed for the other modes.
 - Changed signal translator messages sent by ghostGirl and added a common handling method for this action.
 - Added configuration option for custom messages to be sent by the ghost girl over the signal translator.
 - Added a extra null reference handling for the nethandler to deal with an error that occurs on lobby restart.

## [1.5.0]

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



## [1.1.1]
 - Fixed cases where there were no interactable objects throwing error in the console.
 - Fixed rare cases where the codes still kept going after the ship had left.
 - Fixed the number of codes not being reset going into the next round.
 - Added minimum codes to send per round configuration option
 - Added filter option for each interactable object (door, landmine, & turret).
 - Added Insanity Mode which decreases time between ghostCodes as the group's total insanity levels goes up.
 - Added Turret Berserk Mode as another possible interaction when the ghostCode targets a turret object.
 - Added configuration options for the chances of turrets going berserk in both Normal ghostCodes & Max Insanity mode.
 - Added configuration options for Insanity Mode.

## [1.0.1]
 - Fixed ghostCodes not disabling if the ship leaves before the codes hit max.

## [1.0.0]
*Initial release version*

Configurable Values:
- set interval wait times
- random interval wait times
- max codes to be sent in a round
- terminal sound for when a code is sent (enable/disable)
- use random intervals or set intervals (enable/disable random)

</details>