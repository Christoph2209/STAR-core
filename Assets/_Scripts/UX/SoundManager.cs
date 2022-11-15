using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;


    public SaveObject so;
    [SerializeField]
    public AudioSource _MusicSource, _SFXSource;

    private void Awake()
    {
        so = SaveManager.Load();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _MusicSource.volume = so.volume_mu;
            _SFXSource.volume = so.volume_sfx;
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
