using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject objectiveSelectorGameObject;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject audioPanel;

    [Header("Settings UI")]
    [SerializeField] private Toggle fullScreen;
    [SerializeField] private Toggle mute;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        SettingsManager.ApplySettings(fullScreen, mute, volumeSlider, audioSource);
        optionPanel.SetActive(false);
        PlayerPrefs.SetInt("Objective", 0);
    }

    public void PlayGame()
    {
        mainMenu.SetActive(false);
        objectiveSelectorGameObject.SetActive(true);
    }

    public void OpenOptions()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionPanel.SetActive(false);
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
    }

    public void OpenVideo()
    {
        videoPanel.SetActive(true);
        audioPanel.SetActive(false);
    }

    public void OpenAudio()
    {
        audioPanel.SetActive(true);
        videoPanel.SetActive(false);
    }

    public void SetFullScreen(bool value) => SettingsManager.SetFullScreen(value);
    public void SetMute(bool value) => SettingsManager.SetMute(audioSource, value);
    public void SetVolume(float value) => SettingsManager.SetVolume(audioSource, value);

    public void QuitGame() => Application.Quit();

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        objectiveSelectorGameObject.SetActive(false);
    }

    public void ChoosePurifyObjective()
    {
        PlayerPrefs.SetInt("Objective", 1);
        SceneManager.LoadScene("Game");
    }

    public void ChooseCraftObjective()
    {
        PlayerPrefs.SetInt("Objective", 2);
        SceneManager.LoadScene("Game");
    }
}
