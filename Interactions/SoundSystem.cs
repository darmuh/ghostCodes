﻿
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ghostCodes.Bools;

namespace ghostCodes
{
    internal class SoundSystem
    {
        internal static List<AudioClip> allSounds = new List<AudioClip> {};

        internal static void InsanityAmbient()
        {
            int rand = NumberStuff.GetInt(0, 100);
            if (ModConfig.gcAddInsanitySounds.Value <= rand)
                return;

            SoundManager.Instance.PlayAmbientSound(syncedForAllPlayers: true, SoundManager.Instance.playingInsanitySoundClipOnServer);
        }

        internal static void HandleCodeEffects()
        {
            if (!AreAnyPlayersInShip())
                return;

            Terminal term = Plugin.instance.Terminal;

            if (!ModConfig.ModNetworking.Value)
            {
                NetworkingDisabledSounds(term);
                BroadcastEffectLocal(term);
            }
            else
            {
                NetworkingEnabledSounds();
                BroadcastEffectNet();
            }
        }

        private static void NetworkingEnabledSounds()
        {
            if (!ModConfig.gcEnableTerminalSound.Value)
                return;

            if (ModConfig.gcTerminalSoundChance.Value < NumberStuff.GetInt(0, 100))
                return;

            InitSounds();

            int num = NumberStuff.GetInt(0, allSounds.Count - 1);
            NetHandler.Instance.GGTermAudioServerRpc(num);
            Plugin.MoreLogs("networked sounds playing");
        }

        private static void NetworkingDisabledSounds(Terminal term)
        {
            if (term == null || !ModConfig.gcEnableTerminalSound.Value || !ModConfig.gcUseTerminalAlarmSound.Value)
                return;

            if (ModConfig.gcTerminalSoundChance.Value < NumberStuff.GetInt(0, 100))
                return;

            term.PlayTerminalAudioServerRpc(3);
            Plugin.MoreLogs("alarm sound played");

        }

        private static void BroadcastEffectLocal(Terminal term)
        {
            if (term == null || !ModConfig.enableBroadcastEffect.Value)
                return;

            term.PlayBroadcastCodeEffect();
            Plugin.MoreLogs("effect broadcast");
        }

        private static void BroadcastEffectNet()
        {
            if (!ModConfig.enableBroadcastEffect.Value)
                return;

            NetHandler.Instance.TermBroadcastFXServerRpc();
            Plugin.MoreLogs("playing effect on all terminals");
        }

        private static void BaseGirlSounds()
        {
            if (Plugin.instance.DressGirl == null || !ModConfig.gcUseGirlSounds.Value)
                return;

            List<AudioClip> ghostAudios = new List<AudioClip> { Plugin.instance.DressGirl.breathingSFX, Plugin.instance.DressGirl.heartbeatMusic.clip };
            Plugin.MoreLogs("ghost detected, adding dressgirl audios");

            AddToSounds(ghostAudios);
            AddToSounds(Plugin.instance.DressGirl.appearStaringSFX.ToList());
        }

        private static void AddToSounds(List<AudioClip> sounds)
        {
            foreach (AudioClip sound in sounds)
            {
                allSounds.Add(sound);
                Plugin.MoreLogs($"Added sound: [{sound.name}]");
            }
        }

        private static void BaseTerminalSound()
        {
            allSounds.Add(Plugin.instance.Terminal.codeBroadcastSFX);
            Plugin.MoreLogs("adding terminal broadcast code sound");
        }

        private static void AddBaseAlarmSound()
        {
            if (!ModConfig.gcUseTerminalAlarmSound.Value && ModConfig.ModNetworking.Value)
                return;

            allSounds.Add(Plugin.instance.Terminal.syncedAudios[3]);
            Plugin.MoreLogs("Added base terminal alarm sound");
        }

        internal static void InitSounds()
        {
            allSounds.Clear();
            BaseTerminalSound();
            BaseGirlSounds();
        }

        internal static void PlayTerminalSound(AudioClip clip)
        {
            Plugin.instance.Terminal.terminalAudio.PlayOneShot(clip);
        }

    }
}
