
using ghostCodes.Configs;
using ghostCodes.Interactions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ghostCodes.Bools;

namespace ghostCodes
{
    internal class SoundSystem
    {
        internal static List<AudioClip> allSounds = [];
        internal static List<AudioClip> allGiggles = [];

        internal static void LoadCustomSounds()
        {
            var shockAsset = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("ghostCodes.Assets.zap"));
            var chargeAsset = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("ghostCodes.Assets.charge"));
            var girl = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("ghostCodes.Assets.girl"));
            var terminal = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("ghostCodes.Assets.terminal"));
            allGiggles = [.. girl.LoadAllAssets<AudioClip>()];
            TerminalAdditions.Shock = (AudioClip)shockAsset.LoadAsset("negative.ogg");
            TerminalAdditions.Success = (AudioClip)terminal.LoadAsset("complete.ogg");
            TerminalAdditions.Reboot = (AudioClip)terminal.LoadAsset("process.ogg");
            Items.Adjuster = (AudioClip)chargeAsset.LoadAsset("positive.ogg");


        }

        internal static void InsanityAmbient()
        {
            int rand = NumberStuff.GetInt(0, 100);
            if (SetupConfig.AddInsanitySounds.Value <= rand)
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
            if (SetupConfig.TerminalSoundChance.Value < NumberStuff.GetInt(0, 100))
                return;

            InitSounds();

            int num = NumberStuff.GetInt(0, allSounds.Count);
            NetHandler.Instance.GGTermAudioServerRpc(num);
            Plugin.MoreLogs("networked sounds playing");
        }

        private static void NetworkingDisabledSounds(Terminal term)
        {
            if (term == null)
                return;

            if (SetupConfig.TerminalSoundChance.Value < NumberStuff.GetInt(0, 100))
                return;

            term.PlayTerminalAudioServerRpc(3);
            Plugin.MoreLogs("alarm sound played");

        }

        private static void BroadcastEffectLocal(Terminal term)
        {
            if (term == null || !SetupConfig.BroadcastEffect.Value)
                return;

            term.PlayBroadcastCodeEffect();
            Plugin.MoreLogs("effect broadcast");
        }

        private static void BroadcastEffectNet()
        {
            if (!SetupConfig.BroadcastEffect.Value)
                return;

            NetHandler.Instance.TermBroadcastFXServerRpc();
            Plugin.MoreLogs("playing effect on all terminals");
        }

        private static void BaseGirlSounds()
        {
            if (Plugin.instance.DressGirl == null)
                return;

            List<AudioClip> ghostAudios = [Plugin.instance.DressGirl.breathingSFX, Plugin.instance.DressGirl.heartbeatMusic.clip];
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

        internal static void InitSounds()
        {
            allSounds.Clear();
            if (!SetupConfig.GhostCodesSettings.HauntingsMode)
                BaseTerminalSound(); //hopefully can add custom terminal sounds eventually
            else
            {
                CustomGirlSounds();
            }

        }

        internal static void CustomGirlSounds()
        {
            AddToSounds(allGiggles);
        }

        internal static void PlayTerminalSound(AudioClip clip)
        {
            if (clip == null)
                return;
            Plugin.instance.Terminal.terminalAudio.PlayOneShot(clip, 0.8f);
        }

    }
}
