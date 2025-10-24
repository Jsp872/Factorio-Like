using UnityEngine;
using UnityEngine.UI;

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