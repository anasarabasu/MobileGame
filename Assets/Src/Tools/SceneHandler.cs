using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour, ISaveable {
    public enum Scene {TitleMenu}
    public static void LoadScene(string sceneName) {
        SceneManager.LoadSceneAsync(sceneName);

        Debug.Log("Loaded " +sceneName);
    }

    public static void LoadScene(Scene sceneKey) {
        SceneManager.LoadSceneAsync((int)sceneKey);

        Debug.Log("Loaded " +sceneKey);
    }

    public static void ReloadScene(InputAction.CallbackContext context) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Scene reloaded");
    }

    public void _QuitGame() => Application.Quit(0);

    public void Save(DataRoot data) {
        if( SceneManager.GetActiveScene().name != "MainMenu")
            data.gameData.lastScene = SceneManager.GetActiveScene().name;
    }

    public void Load(DataRoot data) {
    }
}