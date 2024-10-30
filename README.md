# ghostCodes by darmuh

## Description

There's a ghost in the terminal and it's sending random broadcast codes to mess with the facility!

This mod aims to add a fun little random element to the game. A "ghost" is in the terminal and it is causing all kinds of mayhem.


**[DISCLAIMER]**
- If you have SEVERE EPILEPSY you should disable the following configuration option by setting it to FALSE - [ggRapidLights]
- It is highly recommended to delete your old config and let the mod generate a new one.

The ghost sends codes through the terminal that can do the following:
- Open Blast Doors
- Close Blast Doors
- Cause Blast Doors to go haywire and close/shut multiple times
- Disable Turrets
- Cause Turrets to malfunction and go Berserk
- Disable Mines
- Cause Mines to suddenly combust
- Short Circuit the breaker and cut out the facility's lights
- Broadcast random signal translator messages

There are 3 Modes for this Mod:
- Normal Mode (codes will be sent at random or set intervals depending on configuration)
- Insanity Mode (codes will be sent at random or set intervals that change depending on the group insanity level)
- Ghost Girl Enhanced Mode (BETA) (This mode will only send codes when a ghost girl is present and acts as an extension of her Enemy AI)

During Insanity Mode, when group Insanity levels are at their highest the ghostCodes will initiate rapidFire mode. Which sends burstfire codes through the terminal into the facility.
- This rapidFire mode also creates elecrtrical fluctuations in the facility due to the burstfire codes and causes lights to flicker. 
- For solo players, there is a "courage buff" (SoloAssist) which keeps your sanity levels lower while you brave the facility alone.
- Be warned though, this courage buff dwindles as the day turns to night.

During Ghost Girl Enhanced Mode when she skips at you she causes a rapidFire mode due to her excitement.
- As she runs towards you the facility cannot handle the electrical fluctuations and the lights continue to flicker until she disappears.


The ghost doesn't really care if it's helping or not it's just sending codes to be noticed. The more you notice it the more it likes to say hello :)

## Change Log

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

 [Planned Features (not currently in this version)]
 - System for combatting/delaying ghostCodes for the terminal operator.
 - Above system will have various tasks that need to be completed around the ship.
 - Custom sounds to play over the terminal to replace current sounds.
 - More area effects for the facility during rapidFire mode.
 - More custom events for ghostCodes to cause havoc.
 - Have tell sounds from the terminal also play over the walkie

****

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

