using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{

    public AudioClip[] CombatSongs;
    public AudioClip VictorySong;
    public AudioClip DefeatSong;

    bool gameOver = false;

    int song;

    void Start()
    {
        song = Random.Range(0, CombatSongs.Length);
    }

    public void SetVictory()
    {
        if (gameOver) return;
        AudioSource thisSource = GetComponent<AudioSource>();
        thisSource.clip = VictorySong;
        thisSource.Play();
        gameOver = true;
    }

    public void SetDefeat()
    {
        if (gameOver) return;
        AudioSource thisSource = GetComponent<AudioSource>();
        thisSource.clip = DefeatSong;
        thisSource.Play();
        gameOver = true;
    }

    void Update()
    {
        AudioSource thisSource = GetComponent<AudioSource>();
        if (!gameOver && thisSource.isPlaying == false)
        {
            thisSource.clip = CombatSongs[song];
            thisSource.Play();
        }
    }
}
