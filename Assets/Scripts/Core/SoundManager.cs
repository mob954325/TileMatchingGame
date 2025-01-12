using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public List<AudioSource> audioSources;

    private int audioCount = 8;

    public void Awake()
    {
        audioSources = new List<AudioSource>(audioCount);

        GameObject childObj = new GameObject();
        childObj.transform.parent = this.transform;

        for (int i = 0; i < audioCount; i++)
        { 
            AudioSource comp = childObj.AddComponent<AudioSource>();
            audioSources.Add(comp);
        }
    }

    public void PlaySound(SoundType type)
    {
        int sourceNum = FindSource();

        if (sourceNum != -1)
        {
            if (type == SoundType.Pop)
            {
                int rand = UnityEngine.Random.Range(2, 5);
                audioSources[sourceNum].clip = audioClips[rand];
            }
            else
            {
                audioSources[sourceNum].clip = audioClips[(int)type];
            }
            audioSources[sourceNum].playOnAwake = false;
            StartCoroutine(SoundProcess(sourceNum, audioClips[(int)type].length));
        }
    }

    public void PlaySound(int type)
    {
        PlaySound((SoundType)type);
    }

    private int FindSource()
    {
        for(int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].clip == null) return i;
        }

        return -1;
    }

    private IEnumerator SoundProcess(int index, float soundLength)
    {
        float timeElapsed = 0f;

        audioSources[index].Play();
        while(timeElapsed < soundLength)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        audioSources[index].Stop();
        audioSources[index].clip = null;
    }
}