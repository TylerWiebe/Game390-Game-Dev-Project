using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class TimeTrial : MonoBehaviour
{
    public LongEvent OnFinish = new LongEvent();

    private ParkingDetector parkingSpot;

    [SerializeField]
    private bool StartRaceOnStart = false;

    private GameStopwatch stopwatch;
    private bool running = false;

    void Start()
    {
        parkingSpot = GameObject.Find("Empty Parking Spot").GetComponent<ParkingDetector>();

        stopwatch = new GameStopwatch();

        if (StartRaceOnStart)
            StartRace();
    }

    public void StartRace()
    {
        string key = PlayerPrefsKeys.GetTimeTrialHighscoreKey(SceneManager.GetActiveScene());
        parkingSpot.OnPlayerParkingEnter.AddListener(FinishRace);
        stopwatch.Start();
        running = true;
    }

    public void FinishRace()
    {
        Assert.IsTrue(running);
        running = false;
        long time = stopwatch.TimeElapsed;
        Debug.Log("Finished with a time of " + time + "ms.");
        string key = PlayerPrefsKeys.GetTimeTrialHighscoreKey(SceneManager.GetActiveScene());
        if (PlayerPrefs.GetString(key).Length == 0)
        {
            PlayerPrefs.SetString(key, time.ToString());
        }
        else if (time < long.Parse(PlayerPrefs.GetString(key)))
        {
            PlayerPrefs.SetString(key, time.ToString());
        }
        OnFinish.Invoke(time);
    }
}
