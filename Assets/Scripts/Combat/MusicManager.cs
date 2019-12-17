using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static AudioSource[] aud = new AudioSource[2];

    public AudioClip[] Songs;
    public AudioClip VictorySong;
    public AudioClip DefeatSong;

    bool gameOver = false;

    int song;

    void Start()
    {
        aud[0] = gameObject.AddComponent<AudioSource>();
        aud[1] = gameObject.AddComponent<AudioSource>();
        song = Random.Range(0, Songs.Length);
    }

    public void SetVictory()
    {
        if (gameOver) return;
        StartCoroutine(CrossFade(1.0f, VictorySong));
        gameOver = true;
    }

    public void SetDefeat()
    {
        if (gameOver) return;
        StartCoroutine(CrossFade(1.0f, DefeatSong));
        gameOver = true;
    }

    IEnumerator CrossFade(float duration, AudioClip fadeTo)
    {
        aud[1].clip = fadeTo;
        aud[1].volume = 0f;
        aud[1].Play();
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            aud[0].volume = 1f - (elapsed / duration);
            aud[1].volume = elapsed / duration;
            elapsed += Time.deltaTime;
            yield return null;
        }
        aud[1].volume = 1f;
        aud[0].Stop();
        yield break;
    }

    void Update()
    {
        if (!gameOver && aud[0].isPlaying == false)
        {
            aud[0].clip = Songs[song];
            aud[0].Play();
        }
    }
}
