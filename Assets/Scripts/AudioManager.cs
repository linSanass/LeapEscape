using UnityEngine;

public class AudioManager : SingLetonAutoMono<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("BGM")]
    [SerializeField] private AudioClip music;

    [Header("SFX")]
    [SerializeField] private AudioClip dead;
    [SerializeField] private AudioClip fever;
    [SerializeField] private AudioClip levelUp;
    [SerializeField] private AudioClip perfect;
    [SerializeField] private AudioClip ski;

    // ===================== 播放BGM =====================
    public void StopBGM() => bgmSource.Stop();
    public void PauseBGM() => bgmSource.Pause();
    public void ResumeBGM() => bgmSource.UnPause();

    // ===================== 播放SFX =====================
    public void PlayDead() => PlaySFX(dead);
    public void PlayFever() => PlaySFX(fever);
    public void PlayLevelUp() => PlaySFX(levelUp);
    public void PlayPerfect() => PlaySFX(perfect);
    public void PlaySki() => PlaySFX(ski);
    
    private void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }
}