using UnityEngine;

namespace Logic
{
    public static class SoundHelper
    {
        public static void PostSoundEvent(string eventName,GameObject gameObject)
        {
            AkSoundEngine.PostEvent(eventName,gameObject);
        }
    }
}