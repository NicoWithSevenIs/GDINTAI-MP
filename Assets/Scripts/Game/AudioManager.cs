using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;


public class Sound
{

    public AudioClip clip { get; private set; }
    public bool willLoop { get; private set; }
    public GameObject origin { get; private set; }
    public float maxRange { get; private set; }

    public Sound(AudioClip clip, GameObject origin, float maxRange, bool willLoop)
    {
        this.clip = clip;
        this.origin = origin;
        this.maxRange = maxRange;
        this.willLoop = willLoop;
    }

}

[System.Serializable]
public class Music
{
    public string name;
    public AudioClip music;

    [Range(0f,1f)]
    public float volumeMultiplier;

}

public class AudioManager : MonoBehaviour
{

    [SerializeField] private Music[] BGM;
    [SerializeField] private int poolSize;

    private Dictionary<string, Sound> SFXLibrary;

    [Range(0f, 1f)]
    [SerializeField] private float Volume = 1f;


    private List<GameObject> soundPlayerPool;

    public void addSFX(AudioClip sound, GameObject parent, float maxRange, bool willLoop)
    {

        string name = getName(sound, parent); 

        if (SFXLibrary == null || SFXLibrary.ContainsKey(name))
            return;

        SFXLibrary.Add(name, new Sound(sound, parent, maxRange, willLoop));
  

    }

    private IEnumerator DisableOnElapse(GameObject audioObject, float duration)
    {
        yield return new WaitForSeconds(duration);
        audioObject.SetActive(false);
    }

    public void PlaySFX(string soundName)
    {
        Sound toPlay;

        if(SFXLibrary.TryGetValue(soundName, out toPlay))
        {
            GameObject poolable = getFromPool();

            AudioSource source = poolable.GetComponent<AudioSource>();
            source.loop = toPlay.willLoop;
            source.clip = toPlay.clip;
            source.maxDistance = toPlay.maxRange;
            source.volume = Volume;
       
            poolable.transform.position = toPlay.origin.transform.position;
            poolable.SetActive(true);


            source.Play();
            StartCoroutine(DisableOnElapse(poolable, toPlay.clip.length));
        }

    }


    public void PlaySFXSequential(List<string> playlist, float durationOffset)
    {
        StartCoroutine(playSequential(playlist, durationOffset));
    }

    private IEnumerator playSequential(List<string> playlist, float durationOffset)
    {
        foreach(var name in playlist)
        {
            Sound toPlay;
            if (SFXLibrary.TryGetValue(name, out toPlay))
            {
                PlaySFX(name);
                yield return new WaitForSeconds(toPlay.clip.length + durationOffset);
            }
            else yield return null;
        }
    }

    public static string getName(AudioClip clip, GameObject parent)
    {
        return clip.name + parent.GetInstanceID().ToString();
    }


    #region ObjectPooling

    private GameObject insertToPool()
    {
        GameObject emptyObject = new GameObject();
        emptyObject.SetActive(false);
        AudioSource source = emptyObject.AddComponent<AudioSource>();

        source.spatialBlend = 1;
        source.rolloffMode = AudioRolloffMode.Linear;

        emptyObject.transform.parent = transform;

        soundPlayerPool.Add(emptyObject);
        return emptyObject;
    }

    private void initializePool()
    {
        soundPlayerPool = new List<GameObject>();
        for(int i =0; i < poolSize; i++)
        {
            insertToPool();
        }
    }

    private GameObject getFromPool()
    {
        foreach (var pool in soundPlayerPool)
        {
            if (!pool.activeInHierarchy)
            {
                return pool;
            }
        }

        return insertToPool();
    }

    #endregion

    #region singleton
    public static AudioManager instance { get; private set; }
    private void Awake()
    {

        
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
       

        DontDestroyOnLoad(gameObject);

        SFXLibrary = new Dictionary<string, Sound>();


        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            foreach(var Music in BGM)
            {
                if(Music.name == scene.name)
                {
                    AudioSource source = GetComponent<AudioSource>();
                    source.clip = Music.music;
                    source.volume = Volume * Music.volumeMultiplier;
                    source.Play();
                }

            }
        };

        SceneManager.activeSceneChanged += (Scene prev, Scene current) =>
        {
            GetComponent<AudioSource>().Stop();
            foreach(var poolable in soundPlayerPool)
            {
                poolable.GetComponent<AudioSource>().Stop();   
            }
            SFXLibrary.Clear();
        };

        initializePool();
    }
    #endregion


}
