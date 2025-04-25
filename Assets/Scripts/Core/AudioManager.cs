using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonBehaviour<AudioManager>
{
    [SerializeField]
    private AudioSource AudioSource;

    [SerializeField]
    private List<AudioClip> ReelStoppingClips;

    [SerializeField]
    private AudioClip ReelRollingClip;

    [SerializeField]
    private List<AudioClip> CurrencyPopupClips;

    private int CurrencyPopupSoundSequence = 0;

    [SerializeField]
    private List<AudioClip> CurrencyGatherSoundClips;

    [SerializeField]
    private AudioClip ButtonClickSoundClip;

    public void PlayReelStoppingSound() => AudioSource.PlayOneShot(ReelStoppingClips[Random.Range(0, ReelStoppingClips.Count)]);
    public void PlayReelRollingSound() => AudioSource.PlayOneShot(ReelRollingClip);
    public void PlayCurrencyPopupSound()
    {
        AudioSource.PlayOneShot(CurrencyPopupClips[CurrencyPopupSoundSequence]);
        CurrencyPopupSoundSequence = (CurrencyPopupSoundSequence + 1) % CurrencyPopupClips.Count;
    }
    public void PlayCurrencyGatherSound() => AudioSource.PlayOneShot(CurrencyGatherSoundClips[Random.Range(0, CurrencyGatherSoundClips.Count)]);
    public void PlayButtonClickSound() => AudioSource.PlayOneShot(ButtonClickSoundClip);
}
