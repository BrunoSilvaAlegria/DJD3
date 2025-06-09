using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseMenuUI;                 // Pause menu GameObject
    public GameObject firstSelectedButton;         // Button to auto-highlight

    private bool isPaused = false;

    // Cache found scripts
    private MonoBehaviour[] overTheShoulderCameras;
    private MonoBehaviour[] freeLookCameras;
    private MonoBehaviour[] playerMovements;
    private MonoBehaviour[] basicMovements;

    void Start()
    {
        
        // Ensure EventSystem exists
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGameNow();
        }
    }

    private void PauseGameNow()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        AudioListener.pause = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;

        // Find all MonoBehaviours in the scene (including inactive)
        var allScripts = FindObjectsOfType<MonoBehaviour>(true);

        int disabledCount = 0;

        // Filter and disable specific script types by name
        overTheShoulderCameras = System.Array.FindAll(allScripts, s => s.GetType().Name == "OverTheShoulderCamera");
        freeLookCameras = System.Array.FindAll(allScripts, s => s.GetType().Name == "FreeLookCamera");
        playerMovements = System.Array.FindAll(allScripts, s => s.GetType().Name == "PlayerMovement");
        basicMovements = System.Array.FindAll(allScripts, s => s.GetType().Name == "BasicMovement");

        foreach (var script in overTheShoulderCameras) { script.enabled = false; disabledCount++; }
        foreach (var script in freeLookCameras) { script.enabled = false; disabledCount++; }
        foreach (var script in playerMovements) { script.enabled = false; disabledCount++; }
        foreach (var script in basicMovements) { script.enabled = false; disabledCount++; }

        if (disabledCount == 0)
        {
            Debug.LogWarning("PauseGame: No OverTheShoulderCamera, FreeLookCamera, PlayerMovement, or BasicMovement scripts found in the scene.");
        }

        // Highlight first selected button
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;

        // Re-enable scripts
        ReenableScripts(overTheShoulderCameras, "OverTheShoulderCamera");
        ReenableScripts(freeLookCameras, "FreeLookCamera");
        ReenableScripts(playerMovements, "PlayerMovement");
        ReenableScripts(basicMovements, "BasicMovement");

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ReenableScripts(MonoBehaviour[] scripts, string typeName)
    {
        if (scripts == null) return;

        foreach (var script in scripts)
        {
            if (script != null && script.GetType().Name == typeName)
                script.enabled = true;
        }
    }

    public void BackToTheLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
