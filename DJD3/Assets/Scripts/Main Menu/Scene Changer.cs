using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneChanger : MonoBehaviour
{
    public TMP_InputField seedInputField;

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Hub");
    }

    public void StartProcedural()
    {
        if (SeedManager.Instance != null)
        {
            string input = seedInputField != null ? seedInputField.text : "";
            SeedManager.Instance.SetSeed(input);
        }

        SceneManager.LoadSceneAsync("Procedural Test");
    }

    public void QuitGame()
    {
        Debug.Log("[SceneChanger] QuitGame called.");
        Application.Quit();
    }
}
