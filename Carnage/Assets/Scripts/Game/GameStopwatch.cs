using System.Diagnostics;

public class GameStopwatch
{
    private Stopwatch stopwatch;

    public long TimeElapsed { get { return stopwatch.ElapsedMilliseconds; } }

    public GameStopwatch()
    {
        GameManager.Instance.OnGamePause.AddListener(Pause);
        GameManager.Instance.OnGameResume.AddListener(Resume);
    }

    public void Start()
    {
        stopwatch = Stopwatch.StartNew();
    }

    private void Pause()
    {
        stopwatch.Stop();
    }

    private void Resume()
    {
        stopwatch.Start();
    }
}
