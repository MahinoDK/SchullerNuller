using UnityEngine;
using System;
using System.Collections; //IEnumerator + courotines

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; //singleton

    //public bool caveSoundPlaying = false; //to avoid overlapping cave sounds

    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f; //range for a slider
        public bool loop = false;

        [HideInInspector] public AudioSource source;
    }

    public Sound[] sounds; //database of sounds

    private void Awake()
    {
        if (instance != null) //if another audio manager already exists delete this one
        {
            Destroy(gameObject);
            return;
        }
        instance = this; //otherwise set instance to this
        DontDestroyOnLoad(gameObject); //survive scene changes

        //create the audio sources
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, x => x.name == soundName); //find sound by name
        if (s == null)
        {
            Debug.LogWarning("Sound not found!" + soundName);
            return;
        }
        s.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, x => x.name == soundName);
        if (s != null) s.source.Stop();
    }

    public void PlayAtPosition(string soundName, Vector3 position) //find sound in 3d world position! I GUESS WE DONT NEED THIS FOR 2D
    {
        Sound s = Array.Find(sounds, x => x.name == soundName);
        if (s == null)
        {
            Debug.LogWarning("Sound not found: " + soundName);
            return;
        }

        AudioSource.PlayClipAtPoint(s.clip, position, s.volume); //create temporary audio source at position when played
    }

  /* public void PlayRandomCave(params string[] names)
    {
        if (caveSoundPlaying) return;   // Already playing ? skip

        StartCoroutine(PlayCaveRoutine(names));
    }

    private IEnumerator PlayCaveRoutine(string[] names)
    {
        caveSoundPlaying = true;

        // pick random
        int i = UnityEngine.Random.Range(0, names.Length);
        Play(names[i]);

        // find sound gain and wait for the clip to finish
        Sound s = System.Array.Find(sounds, sound => sound.name == names[i]);
        if (s != null)
            yield return new WaitForSeconds(s.clip.length);

        caveSoundPlaying = false;
    }*/

}