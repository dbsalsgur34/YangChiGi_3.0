using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioDatabase audioDatabase;
    private AudioSource backGroundAudioSource;
    private AudioSource effectAudioSource;
    public static AudioManager Instance;

    // Use this for initialization
    private void Awake()
    {
        audioDatabase = GetComponent<AudioDatabase>();
        backGroundAudioSource = GetComponents<AudioSource>()[0];
        effectAudioSource = GetComponents<AudioSource>()[1];
        if (Instance == null)           //Static 변수를 지정하고 이것이 없을경우 - PlayManage 스크립트를 저장하고 이것이 전 범위적인 싱글톤 오브젝트가 된다.
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);   //싱글톤 오브젝트가 있을경우 다른 오브젝트를 제거.
        }
    }

    public void PlayBackGroundClipByName(string Name, float startTime)
    {
        if (!backGroundAudioSource.isPlaying)
        {
            backGroundAudioSource.clip = audioDatabase.GetBackGroundAudioByName(Name);
            backGroundAudioSource.time = startTime;
        }
    }

    public void PlayOneShotEffectClipByName(string Name)
    {
        effectAudioSource.PlayOneShot(audioDatabase.GetEffectSoundAudioByName(Name));
    }

    public void PlayOneShotEffectClipByName(string Name,float tempVolume)
    {
        effectAudioSource.PlayOneShot(audioDatabase.GetEffectSoundAudioByName(Name),tempVolume);
    }

    public void BackGroundClipAttenuation()
    {
        StartCoroutine(AudioSourceSpatialBlendSmoothChange(this.backGroundAudioSource,0,0.8f));
    }

    public void BackGroundClipRestore()
    {
        StartCoroutine(AudioSourceSpatialBlendSmoothChange(this.backGroundAudioSource, 0.8f, 0));
    }

    private IEnumerator AudioSourceSpatialBlendSmoothChange(AudioSource target, float startValue, float endValue)
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        for (float i = 0; i < 1.1; i += 0.2f)
        {
            target.spatialBlend = Mathf.Lerp(startValue, endValue, i);
            yield return waitTime;
        }
    }

    public void PlayEffectClipByName(string Name,float playTime)
    {
        if (!effectAudioSource.isPlaying)
        {
            effectAudioSource.clip = audioDatabase.GetEffectSoundAudioByName(Name);
            effectAudioSource.PlayScheduled(playTime);
        }
    }

    public void PlayEffectClipByName(string Name, float playTime, float endTime)
    {
        if (!effectAudioSource.isPlaying)
        {
            effectAudioSource.clip = audioDatabase.GetEffectSoundAudioByName(Name);
            effectAudioSource.time = playTime;
            effectAudioSource.Play();
            if (endTime > playTime)
            {
                effectAudioSource.SetScheduledEndTime(AudioSettings.dspTime + (endTime - playTime));
            }
        }
    }

    private void Update()
    {
        if (!backGroundAudioSource.isPlaying && backGroundAudioSource.clip != null)
        {
            backGroundAudioSource.PlayDelayed(1f);
        }

        if (!effectAudioSource.isPlaying && effectAudioSource.clip != null)
        {
            effectAudioSource.PlayDelayed(1f);
        }

        this.backGroundAudioSource.volume = PlayManage.Instance.Sound/100;
        this.effectAudioSource.volume = PlayManage.Instance.EffectSound / 100;
    }

    public void InitEffectAudio()
    {
        this.effectAudioSource.Stop();
        this.effectAudioSource.clip = null;
        this.effectAudioSource.spatialBlend = 0f;
        this.effectAudioSource.pitch = 1;
    }

    public void InitBackGroundAudio()
    {
        this.backGroundAudioSource.Stop();
        this.backGroundAudioSource.clip = null;
        this.backGroundAudioSource.spatialBlend = 0f;
        this.backGroundAudioSource.pitch = 1;
    }

    public void SwitchAudioMute()
    {
        this.backGroundAudioSource.mute = !this.backGroundAudioSource.mute;
        this.effectAudioSource.mute = !this.effectAudioSource.mute;
    }

    public void PlayEffectSoundByIndivisualAudioSource(AudioSource targetSource, float playTime, string audioClipName)
    {
        targetSource.clip = audioDatabase.GetEffectSoundAudioByName(audioClipName);
        targetSource.PlayScheduled(playTime);
    }

    public void PlayEffectSoundByIndivisualAudioSource(AudioSource targetSource, float playTime, float volume, string audioClipName)
    {
        targetSource.clip = audioDatabase.GetEffectSoundAudioByName(audioClipName);
        targetSource.volume = volume;
        targetSource.PlayScheduled(playTime);
    }
}
