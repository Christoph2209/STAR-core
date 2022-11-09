using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private AudioSource _MusicSource, _SFXSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }




    public void PlaySound(AudioClip clip)
    {
        if(_SFXSource.isPlaying)
        {
            _SFXSource.Stop();
        }

        _SFXSource.PlayOneShot(clip);
    }
}
