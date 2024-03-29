using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scenes
    {
        MainMenu,
        SampleScene,
        GameOver
    }
    public static void Load(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

}
