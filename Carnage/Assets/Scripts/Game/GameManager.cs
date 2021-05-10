using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent OnGamePause;
    public UnityEvent OnGameResume;
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private TimeTrial myTimeTrial;

    private bool gameHasEnded = false;

    private void Awake()
    {
        Assert.IsNull(Instance);
        Time.timeScale = 1f;
        Instance = this;
    }


    private void Start()
    {
        gameHasEnded = false;
        myTimeTrial.OnFinish.AddListener(EndGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameHasEnded)
        {
            if (Time.timeScale == 1)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        OnGamePause.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        OnGameResume.Invoke();
    }

    public void OnDestroy()
    {
        PlayerPrefs.Save();
        Instance = null;
    }

    public void EndGame(long time)
    {
        gameHasEnded = true;
        Time.timeScale = 0f;
    }



}
