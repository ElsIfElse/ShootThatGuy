using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    GameObject parentObject;

    public void PlaySound(AudioClip clip, float volume, bool isLoop, bool isSpatial)
    {
        if (Audio_Pooling.PoolSize() == 0) Audio_Pooling.CreateAudioSource_WithBuffer(parentObject);

        AudioSource source = Audio_Pooling.DequeueAudioSource();
        source.clip = clip;
        source.volume = volume;
        source.loop = isLoop;
        source.spatialBlend = (isSpatial) ? 1 : 0;

        source.Play();

        StartCoroutine(Audio_Pooling.WaitForSoundToFinishAndEnqueue(source));
    }

    void Start()
    {
        Audio_Pooling.CreateAudioSource_AtStart(10, parentObject);
    }






}
