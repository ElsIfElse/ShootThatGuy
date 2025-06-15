using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Audio_Pooling
{
    static public List<AudioSource> sources;
    public static int PoolSize()
    {
        return sources.Count;
    }

    static public void CreateAudioSource_AtStart(int amount, GameObject parent)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newSource = new("AudioSource");
            newSource.transform.parent = parent.transform;
            sources.Add(newSource.AddComponent<AudioSource>());
        }
    }
    static public void CreateAudioSource_WithBuffer(GameObject parent)
    {
        for (int i = 0; i < PoolSize() * 2; i++)
        {
            GameObject newSource = new("AudioSource");
            newSource.transform.parent = parent.transform;
            sources.Add(newSource.AddComponent<AudioSource>());
        }
    }
    static public void EnqueueAudioSource(AudioSource source)
    {
        sources.Add(source);
    }
    static public AudioSource DequeueAudioSource()
    {
        AudioSource source = sources[0];
        sources.RemoveAt(0);
        return source;
    }
    public static IEnumerator WaitForSoundToFinishAndEnqueue(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        EnqueueAudioSource(source);
    }
}
