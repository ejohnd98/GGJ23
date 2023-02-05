using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// TODO:
// Make music switching more robust/easier to understand


public class SoundSystem : MonoBehaviour
{
    [System.Serializable]
    public class VariantSound{
        public string name;
        public AudioClip[] sounds;
        private int lastSoundIndex = -1;

        public AudioClip GetSound(){
            int rand = Random.Range(0, sounds.Length);
            if(rand == lastSoundIndex){
                rand = (rand + 1) % sounds.Length;
            } 
            lastSoundIndex = rand;
            return sounds[rand];
        }
    }

    public static SoundSystem instance;

    AudioSource musicPlayer;
    public GameObject soundEffectPrefab, globalSoundEffectPrefab;
    public float overallVolume = 1.0f;
    public float musicVolume = 0.5f;
    public float sfxVolume = 0.8f;
    public bool playPlaceholder = false;
    public bool musicStarted = false;

    public AudioClip[] music;
    public AudioClip[] sfxList;
    public VariantSound[] varientSfxList;

    Dictionary<string, AudioClip> sfx;
    Dictionary<string, VariantSound> varientSfx;

    AudioClip firstMusic;
    AudioClip nextMusic;

    bool fadingOut = false;
    public float fadeOutTime = 2.0f;
    float fadeOutCounter = 0.0f;

    float currentMusicVol = 1.0f;

    private void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
            return;
        }else{
            instance = this;
        }

        if(sfx == null){
            sfx = new Dictionary<string, AudioClip>();
            varientSfx = new Dictionary<string, VariantSound>();

            foreach(AudioClip clip in sfxList){
                sfx.Add(clip.name, clip);
            }
            foreach(VariantSound variant in varientSfxList){
                varientSfx.Add(variant.name, variant);
            }
        }

        musicPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update(){
        if(fadingOut && currentMusicVol > 0.0){
            currentMusicVol = 1.0f * Easings.EaseInOutCubic(1.0f - fadeOutCounter/fadeOutTime);
            fadeOutCounter += Time.deltaTime;
        }
        if(!fadingOut && currentMusicVol < 1.0f){
            currentMusicVol = 1.0f * Easings.EaseInOutCubic(1.0f - fadeOutCounter/fadeOutTime);
            fadeOutCounter -= Time.deltaTime;
        }
        musicPlayer.volume = currentMusicVol * musicVolume* overallVolume;
    }

    public void FadeOut(float time){
        fadeOutTime = time;
        fadingOut = true;
    }

    public static void PlaySoundStatic(string sndName, Transform location = null, bool persist = false, bool dontParent = true){
        instance.PlaySound(sndName, location, persist);
    }

    public void PlaySound(string sndName, Transform location = null, bool persist = false, bool dontParent = true){
        AudioClip clip;
        if (sfx.ContainsKey(sndName)){
            clip = sfx[sndName];
        }else if (varientSfx.ContainsKey(sndName)){
            clip = varientSfx[sndName].GetSound();
        }else{
            if(playPlaceholder){
                clip = sfx["placeholder"];
            }else{
                return;
            }
        }

        bool localSound = location != null;

        GameObject sndObj = Instantiate(localSound? globalSoundEffectPrefab : soundEffectPrefab, localSound ? location : transform);
        AudioSource newSrc = sndObj.GetComponent<AudioSource>();
        soundEffectPrefab.GetComponent<DestroyAfterDone>().SetPersist(persist);
        if(persist && localSound){
            sndObj.transform.SetParent(transform, true);
        }
        if(localSound && dontParent){
            sndObj.transform.SetParent(null, true);
        }
        newSrc.volume = sfxVolume * overallVolume;
        newSrc.clip = clip;
    }

    public void ChangeMusic(AudioClip newMusic){
        if(!musicStarted){
            firstMusic = newMusic;
            return;
        }
        nextMusic = newMusic;
        if(musicPlayer.isPlaying && fadingOut && nextMusic == musicPlayer.clip){
            fadingOut = false;
        }else{
            StartCoroutine(TransitionMusic());
        }
        
    }

    public void AllowMusic(){
        musicStarted = true;
        ChangeMusic(firstMusic);
    }

    public void PlayGameOver(){
        fadeOutTime = 0.5f;
        ChangeMusic(sfx["game over"]);
        musicPlayer.loop = false;
    }

    public void StopMusic(){
        ChangeMusic((AudioClip)null);
    }

    IEnumerator TransitionMusic(){
        fadingOut = true;
        yield return new WaitUntil(() => (currentMusicVol <= 0.001f || !musicPlayer.isPlaying));
        fadingOut = false;
        fadeOutCounter = 0.0f;
        musicPlayer.Stop();
        if(nextMusic != null){
            musicPlayer.clip = nextMusic;
            currentMusicVol = 1.0f;
            musicPlayer.Play();
        }
    }
}
