using System.Collections;
using UnityEngine;
using static UnityEngine.Object;

namespace ghostCodes.Interactions
{
    internal class WalkieStuff
    {
        internal static bool activeBreathingWalkies = false;
        internal static bool activeGarble = false;

        internal static void BreatheOnWalkiesFunc()
        {
            if (WalkieTalkie.allWalkieTalkies.Count == 0 || Plugin.instance.DressGirl == null)
                return;

            if (activeBreathingWalkies)
                return;
            float waitTime = NumberStuff.GetFloat(1f, 6f);
            Plugin.MoreLogs("Transmitting breathing noise");

            PlayOnAllWalkies(Plugin.instance.DressGirl.breathingSFX, 0.6f);
            activeBreathingWalkies = true;
            GarbleWalkies(true);
            StartOfRound.Instance.StartCoroutine(DelayedReturnFromGarble(waitTime));
            StartOfRound.Instance.StartCoroutine(DelayedReturnFromBreathing());
            
        }

        internal static void GarbleAllWalkiesFunc()
        {
            if (WalkieTalkie.allWalkieTalkies.Count == 0)
                return;

            if (activeGarble)
                return;

            float waitTime = NumberStuff.GetFloat(15f, 45f);

            GarbleWalkies(true);
            activeGarble = true;
            StartOfRound.Instance.StartCoroutine(DelayedReturnFromGarble(waitTime));
        }

        private static void PlayOnAllWalkies(AudioClip clip, float vol)
        {
            foreach (WalkieTalkie walkie in WalkieTalkie.allWalkieTalkies)
            {
                walkie.target.PlayOneShot(clip, vol);
            }
        }

        private static void GarbleWalkies(bool state)
        {
            if (WalkieTalkie.allWalkieTalkies.Count == 0)
                return;

            foreach (WalkieTalkie walkie in WalkieTalkie.allWalkieTalkies)
            {
                walkie.playingGarbledVoice = state;
            }
        }

        internal static IEnumerator DelayedReturnFromGarble(float waitTime)
        {
            
            yield return new WaitForSeconds(waitTime);
            GarbleWalkies(false);
            activeGarble = false;
            Plugin.MoreLogs("walkies returned to normal");
        }

        internal static IEnumerator DelayedReturnFromBreathing()
        {
            yield return new WaitForSeconds(5f);
            activeBreathingWalkies = false;
        }
    }
}
