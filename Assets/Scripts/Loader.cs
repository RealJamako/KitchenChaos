using System;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static GameScenes targetScene;

    public static void LoadScene(GameScenes targetScene)
    {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Enum.GetName(typeof(GameScenes), GameScenes.LoadingScene));
    }

    public static void LoaderCallBack()
    {
        SceneManager.LoadScene(Enum.GetName(typeof(GameScenes), targetScene));
    }
}