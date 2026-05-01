using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource loopSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource pauseBgmSource;

    [Header("Clips")]
    [SerializeField] private AudioClip countdownClip;
    [SerializeField] private AudioClip goClip;
    [SerializeField] private AudioClip runLoopClip;
    [SerializeField] private AudioClip playerShootClip;
    [SerializeField] private AudioClip playerJumpClip;
    [SerializeField] private AudioClip playerHitClip;
    [SerializeField] private AudioClip enemyShootClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip winClip;

    [Header("BGM")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip pauseBgmClip;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;

        if (pauseBgmSource != null)
            pauseBgmSource.ignoreListenerPause = true;
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayCountdown() => PlaySFX(countdownClip);
    public void PlayGo() => PlaySFX(goClip);
    public void PlayPlayerShoot() => PlaySFX(playerShootClip);
    public void PlayPlayerJump() => PlaySFX(playerJumpClip);
    public void PlayPlayerHit() => PlaySFX(playerHitClip);
    public void PlayEnemyShoot() => PlaySFX(enemyShootClip);
    public void PlayGameOver() => PlaySFX(gameOverClip);
    public void PlayWin() => PlaySFX(winClip);

    public void StartRunLoop()
    {
        if (loopSource == null || runLoopClip == null) return;
        if (loopSource.clip == runLoopClip && loopSource.isPlaying) return;

        loopSource.clip = runLoopClip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopRunLoop()
    {
        if (loopSource == null) return;
        if (loopSource.isPlaying)
            loopSource.Stop();
    }

    public void PlayBGM()
    {
        if (bgmSource == null || bgmClip == null) return;
        if (bgmSource.clip == bgmClip && bgmSource.isPlaying) return;

        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource == null) return;
        if (bgmSource.isPlaying)
            bgmSource.Stop();
    }

    public void PlayPauseBGM()
    {
        if (pauseBgmSource == null || pauseBgmClip == null) return;
        if (pauseBgmSource.clip == pauseBgmClip && pauseBgmSource.isPlaying) return;

        pauseBgmSource.clip = pauseBgmClip;
        pauseBgmSource.loop = true;
        pauseBgmSource.Play();
    }

    public void StopPauseBGM()
    {
        if (pauseBgmSource == null) return;
        if (pauseBgmSource.isPlaying)
            pauseBgmSource.Stop();
    }

    public void PauseAllGameplayAudio()
    {
        AudioListener.pause = true;
        PlayPauseBGM();
    }

    public void ResumeAllGameplayAudio()
    {
        StopPauseBGM();
        AudioListener.pause = false;
    }

    public void FadeOutBGM(float duration = 1f)
    {
        if (bgmSource == null) return;
        StartCoroutine(CoFadeOut(duration));
    }

    private IEnumerator CoFadeOut(float duration)
    {
        float startVolume = bgmSource.volume;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.volume = startVolume;
    }
}