using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonController : MonoBehaviour
{
    public Button startButton, pauseButton, settingsButton, aboutButton, rulesButton;
    public GameObject settingsPanel, aboutPanel, rulesPanel;

    private void Start()
    {
        startButton.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(PauseGame);
        settingsButton.onClick.AddListener(ToggleSettings);
        aboutButton.onClick.AddListener(ToggleAbout);
        rulesButton.onClick.AddListener(ToggleRules);
    }

    void StartGame()
    {
        SceneManager.LoadScene("Menu"); // Change to your actual game scene
    }

    void PauseGame()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }

    void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    void ToggleAbout()
    {
        aboutPanel.SetActive(!aboutPanel.activeSelf);
    }

    void ToggleRules()
    {
        rulesPanel.SetActive(!rulesPanel.activeSelf);
    }
}
