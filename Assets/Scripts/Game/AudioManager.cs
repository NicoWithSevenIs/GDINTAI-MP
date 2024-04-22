using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioClip[] BGM;

    private Dictionary<string, AudioSource> SFXLibrary;

    [Range(0f, 1f)]
    [SerializeField] private float Volume = 1f;

    public void addSFX(AudioClip sound, GameObject parent, float maxRange, bool willLoop)
    {

        string name = getName(sound, parent); 

        if (SFXLibrary == null || SFXLibrary.ContainsKey(name))
            return;

        AudioSource source = parent.AddComponent<AudioSource>();
        source.loop = willLoop;
        source.clip = sound;
        source.volume = Volume;
        source.spatialBlend = 1;
        source.rolloffMode = AudioRolloffMode.Linear; 
        source.maxDistance= maxRange;


        SFXLibrary.Add(name, source);
     
    }


    public void PlaySFX(string soundName)
    {
        AudioSource source;

        if (SFXLibrary.TryGetValue(soundName, out source))
            source.Play();
        else print("Audio Not Found " + soundName);
    }


    //Work on this
    public IEnumerator PlayMultiple(List<string> playlist)
    {
        foreach (string name in playlist)
        {
            AudioSource source;

            if (SFXLibrary.TryGetValue(name, out source))
            {
                source.Play();
                yield return new WaitForSeconds(source.clip.length);
            }
            else yield return null;
                
        }
    }

    public static string getName(AudioClip clip, GameObject parent)
    {
        return clip.name + parent.GetInstanceID().ToString();
    }



    #region singleton
    public static AudioManager instance { get; private set; }
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else Destroy(gameObject);

        SFXLibrary = new Dictionary<string, AudioSource>();

        foreach (var sound in BGM)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.loop = true;
            source.clip = sound;
            source.volume = Volume;
            source.spatialBlend = 0;
        }
    }
    #endregion


}
