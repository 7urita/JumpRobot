using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public int levelMusicToPlay;

    public AudioSource[] music;
    public AudioSource[] sfx;
    public int gameMusic;

    public AudioMixerGroup musicMixer, sfxMixer;

    private void Awake() 
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayMusic(gameMusic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(int musicToPlay)
    {
        music[musicToPlay].Play();

    }

    public void PlaySfx(int sfxToPlay)
    {
        sfx[sfxToPlay].Play();
    }

    public void SetMusicLevel()
    {
        musicMixer.audioMixer.SetFloat("MusicVol", UIManager.instance.musicVolSlider.value);
    }

    public void SetSfxLevel()
    {
        sfxMixer.audioMixer.SetFloat("SfxVol", UIManager.instance.sfxVolSlider.value);
    }



}
