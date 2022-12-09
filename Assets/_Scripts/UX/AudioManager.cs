using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    void Awake()
    {
        Instance = this;
    }

    public AudioSource Src;

    public AudioClip DestroySFX;
    public AudioClip TargetSFX;
    public AudioClip FireSFX;
    public AudioClip MoveTargetSFX;
    public AudioClip SelectSFX;
    public AudioClip CraftSFX;
    public AudioClip TransferSFX;
    public AudioClip ErrorSFX;
    public AudioClip MoveActionSFX;


    public void PlayDestroySFX()
    {
        Src.PlayOneShot(DestroySFX);
    }

    public void PlayTargetSFX()
    {
        Src.PlayOneShot(TargetSFX);
    }

    public void PlayFireSFX()
    {
        Src.PlayOneShot(FireSFX);
    }

    public void PlayMoveTargetSFX()
    {
        Src.PlayOneShot(MoveTargetSFX);
    }

    public void PlayMoveActionSFX()
    {
        Src.PlayOneShot(MoveActionSFX);
    }

    public void PlaySelectSFX()
    {
        Src.PlayOneShot(SelectSFX);
    }

    public void PlayCraftSFX()
    {
        Src.PlayOneShot(CraftSFX);
    }

    public void PlayTransferSFX()
    {
        Src.PlayOneShot(TransferSFX);
    }

    public void PlayErrorSFX()
    {
        Src.PlayOneShot(ErrorSFX);
    }
}
