using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private Toggle fullScreen;
    [SerializeField] private Toggle mute;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource audioSource;

    private bool isPaused;

    private void Start()
    {
        SettingsManager.ApplySettings(fullScreen, mute, volumeSlider, audioSource);
        optionPanel.SetActive(false);
    }

    public void Pause()
    {
        if (optionPanel.activeSelf) return;

        if (!isPaused)
            PauseGame();
        else
            Resume();
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenSettings()
    {
        optionPanel.SetActive(true);
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
    }

    public void BackToPauseMenu()
    {
        optionPanel.SetActive(false);
        videoPanel.SetActive(false);
        audioPanel.SetActive(false);
    }

    public void OpenVideoSettings()
    {
        videoPanel.SetActive(true);
        audioPanel.SetActive(false);
    }

    public void OpenAudioSettings()
    {
        audioPanel.SetActive(true);
        videoPanel.SetActive(false);
    }

    public void SetFullScreen(bool value) => SettingsManager.SetFullScreen(value);
    public void SetMute(bool value) => SettingsManager.SetMute(audioSource, value);
    public void SetVolume(float value) => SettingsManager.SetVolume(audioSource, value);
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

public static class SettingsManager
{
    public static void ApplySettings(Toggle fullScreen, Toggle mute, Slider volumeSlider, AudioSource audio)
    {
        fullScreen.isOn = PlayerPrefs.GetInt("FullScreen", Screen.fullScreen ? 1 : 0) == 1;
        mute.isOn = PlayerPrefs.GetInt("Mute", audio.mute ? 1 : 0) == 1;
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", audio.volume);

        Screen.fullScreen = fullScreen.isOn;
        audio.mute = mute.isOn;
        audio.volume = volumeSlider.value;
    }

    public static void SetFullScreen(bool isFull)
    {
        Screen.fullScreen = isFull;
        PlayerPrefs.SetInt("FullScreen", isFull ? 1 : 0);
    }

    public static void SetMute(AudioSource audio, bool isMute)
    {
        audio.mute = isMute;
        PlayerPrefs.SetInt("Mute", isMute ? 1 : 0);
    }

    public static void SetVolume(AudioSource audio, float volume)
    {
        audio.volume = volume;
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
