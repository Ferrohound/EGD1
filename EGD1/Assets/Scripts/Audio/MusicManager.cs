using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public Stem[] stems;
    public AudioSource[] sources;

	// Use this for initialization
	void Start () {
        sources = new AudioSource[stems.Length];
		for (int i = 0; i < stems.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].clip = stems[i].clip;
        }
	}

    public void play(int stem)
    {
        bool firstStem = true;
        int playingStem = -1;
        for(int i = 0; i < sources.Length; i++)
        {
            if (sources[i].isPlaying)
            {
                firstStem = false;
                playingStem = i;
                break;
            }
        }
        if (firstStem)
        {
            sources[stem].volume = 1f;
            sources[stem].Play();
        } else
        {
            sources[stem].timeSamples = sources[playingStem].timeSamples;
            sources[stem].volume = 1f;
            sources[stem].Play();
        }
    }

    public void stop(int stem)
    {
        sources[stem].Stop();
        sources[stem].timeSamples = 0;
    }

    public void fadeIn(int stem, float fadeLength)
    {
        StartCoroutine(Fade(sources[stem], 1f, fadeLength));
    }

    public void fadeOut(int stem, float fadeLength)
    {
        StartCoroutine(Fade(sources[stem], 0f, fadeLength));
    }
    static IEnumerator Fade(AudioSource source, float targetVolume, float fadeLength)
    {
        float volDiff = Mathf.Abs(source.volume - targetVolume);
        if(targetVolume > source.volume)
        {
            while(source.volume < targetVolume)
            {
                source.volume += volDiff * (Time.deltaTime / fadeLength);
                yield return null;
            }
        } else
        {
            while (source.volume > targetVolume)
            {
                source.volume -= volDiff * (Time.deltaTime / fadeLength);
                yield return null;
            }
        }
    }

}

[System.Serializable]
public class Stem
{
    public AudioClip clip;
    public int length;
}