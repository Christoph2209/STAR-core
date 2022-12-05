using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Src;

    public AudioClip DestroySFX;
    public AudioClip TargetSFX;
    public AudioClip FireSFX;
    public AudioClip MoveSFX;
    public AudioClip SelectSFX;
    public AudioClip CraftSFX;
    public AudioClip TransferSFX;


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

    public void PlayMoveSFX()
    {
        Src.PlayOneShot(MoveSFX);
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
}
