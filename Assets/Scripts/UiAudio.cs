using UnityEngine;

namespace RPG
{
    public static class UiAudio
    {
        private static AudioSource source;
        private static AudioClip confirm;
        private static AudioClip investigate;
        private static AudioClip pickup;
        private static AudioClip doorOpen;
        private static AudioClip doorClose;
        private static AudioClip switchPressed;
        private static AudioClip ironGateOpen;
        private static AudioClip landing;

        public static void PlayConfirm()
        {
            if (confirm == null) confirm = Resources.Load<AudioClip>("Audio/ui_confirm");
            if (confirm == null) return;
            if (source == null)
            {
                GameObject host = new GameObject("UI Audio");
                Object.DontDestroyOnLoad(host);
                source = host.AddComponent<AudioSource>();
                source.playOnAwake = false;
            }
            source.PlayOneShot(confirm, .55f);
        }

        public static void PlayInvestigate()
        {
            if (investigate == null) investigate = Resources.Load<AudioClip>("Audio/investigate");
            if (investigate == null) return;
            EnsureSource();
            source.PlayOneShot(investigate, .5f);
        }

        public static void PlayPickup()
        {
            if (pickup == null) pickup = Resources.Load<AudioClip>("Audio/pickup");
            if (pickup == null) return;
            EnsureSource();
            source.PlayOneShot(pickup, .55f);
        }

        public static void PlayDoor(bool opening)
        {
            AudioClip clip = opening ? doorOpen : doorClose;
            if (clip == null)
            {
                clip = Resources.Load<AudioClip>(opening ? "Audio/door_open" : "Audio/door_close");
                if (opening) doorOpen = clip; else doorClose = clip;
            }
            if (clip == null) return;
            EnsureSource();
            source.PlayOneShot(clip, .5f);
        }

        public static void PlaySwitchPressed()
        {
            if (switchPressed == null) switchPressed = Resources.Load<AudioClip>("Audio/switch_pressed");
            if (switchPressed == null) return;
            EnsureSource();
            source.PlayOneShot(switchPressed, .6f);
        }

        public static void PlayIronGateOpen()
        {
            if (ironGateOpen == null) ironGateOpen = Resources.Load<AudioClip>("Audio/iron_gate_open");
            if (ironGateOpen == null) return;
            EnsureSource();
            source.PlayOneShot(ironGateOpen, .78f);
        }

        public static void PlayLanding()
        {
            if (landing == null) landing = Resources.Load<AudioClip>("Audio/landing_mud");
            if (landing == null) return;
            EnsureSource();
            source.PlayOneShot(landing, .72f);
        }

        private static void EnsureSource()
        {
            if (source != null) return;
            GameObject host = new GameObject("UI Audio");
            Object.DontDestroyOnLoad(host);
            source = host.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }
    }
}
