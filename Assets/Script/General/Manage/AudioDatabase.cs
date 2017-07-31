using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDatabase : MonoBehaviour {

    private List<AudioClip> backGroundAudioClipList;
    private List<AudioClip> EffectSoundAudioClipList;

	// Use this for initialization
	private void Awake () {
		backGroundAudioClipList = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/BackGround"));
        EffectSoundAudioClipList = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio/Effect"));
    }

    public AudioClip GetBackGroundAudioByName(string clipName)
    {
        foreach(AudioClip clip in backGroundAudioClipList)
        {
            if (clip.name.Equals(clipName))
            {
                return clip;
            }
        }
        return null;
    }

    public AudioClip GetEffectSoundAudioByName(string clipName)
    {
        foreach (AudioClip clip in EffectSoundAudioClipList)
        {
            if (clip.name.Equals(clipName))
            {
                return clip;
            }
        }
        return null;
    }
	
}
