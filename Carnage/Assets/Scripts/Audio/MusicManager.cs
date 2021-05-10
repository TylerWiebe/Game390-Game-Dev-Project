using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    
    private bool isGameScene;
    private bool startedGameMusicLoop;

    public int MainMenuSceneIndex;
    public int[] MainGameSceneIndex;
    
    public AudioSource musicPlayer;

    public AudioClip mainTheme;
    public AudioClip MainGameInitial;
    public AudioClip MainGameLoop;
    public AudioClip Results;

    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.sceneUnloaded += onSceneUnloaded;
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    private void Start()
    {
        int numMusicPlayers = FindObjectsOfType<MusicManager>().Length;
        if (numMusicPlayers > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void onSceneUnloaded(Scene scene)
    {
        if (MainGameSceneIndex.Contains(scene.buildIndex))
        {
            playMenuMusic();
        }
    }
    
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == MainMenuSceneIndex)
        {
            playMenuMusic();
        }

        if (MainGameSceneIndex.Contains(scene.buildIndex))
        {
            playInGameMusic();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isGameScene && !startedGameMusicLoop)
        {
            if (musicPlayer.isPlaying == false)
            {
                startedGameMusicLoop = true;
                musicPlayer.loop = true;
                musicPlayer.clip = MainGameLoop;
                musicPlayer.Play();
            }
        }
    }

    public void playInGameMusic()
    {
        isGameScene = true;
        musicPlayer.loop = false;
        musicPlayer.clip = MainGameInitial;
        musicPlayer.Play();
    }

    public void playMenuMusic()
    {
        isGameScene = false;
        startedGameMusicLoop = false;
        musicPlayer.loop = true;
        musicPlayer.clip = mainTheme;
        musicPlayer.Play();
    }
    public void PlayResult()
    {
        startedGameMusicLoop = false;
        musicPlayer.loop = false;
        musicPlayer.clip = Results;
        musicPlayer.Play();
    }
}
