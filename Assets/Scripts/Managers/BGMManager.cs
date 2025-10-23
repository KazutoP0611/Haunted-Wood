using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [Space]
    [SerializeField] private AudioClip titleSong;
    [SerializeField] private AudioClip gameSceneSong;
    [SerializeField] private AudioClip bossSong;
    [SerializeField] private AudioClip gameOverSong;
    [SerializeField] private AudioClip gameClearSong;

    public static BGMManager instance;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeSongTo(BGMSong song, bool loop = true)
    {
        audioSource.Stop();
        switch(song)
        {
            case BGMSong.titleSong:
                audioSource.clip = titleSong;
                break;
            case BGMSong.gameSceneSong:
                audioSource.clip = gameSceneSong;
                break;
            case BGMSong.bossSong:
                audioSource.clip = bossSong;
                break;
            case BGMSong.gameOverSong:
                audioSource.clip = gameOverSong;
                break;
            case BGMSong.gameClearSong:
                audioSource.clip = gameClearSong;
                break;
        }
        audioSource.loop = loop;
        audioSource.Play(0);
    }
}
