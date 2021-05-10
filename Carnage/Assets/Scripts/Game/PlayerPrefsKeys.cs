using UnityEngine.SceneManagement;

public static class PlayerPrefsKeys
{
    public static string GetTimeTrialHighscoreKey(Scene scene)
    {
        return scene.name + "_timetrialhighscore";
    }
}
