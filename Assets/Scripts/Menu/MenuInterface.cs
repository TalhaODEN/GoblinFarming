using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInterface : MonoBehaviour
{
    private void OnEnable()
    {
        MenuEvents.OnPlayButtonClicked += StartGame;
        MenuEvents.OnControlsButtonClicked += ShowControls;
        MenuEvents.OnMusicButtonClicked += ToggleMusic;
        MenuEvents.OnAudioButtonClicked += OpenAudioSettings;
        MenuEvents.OnQuestionButtonClicked += ShowFAQ;
        MenuEvents.OnInfoButtonClicked += ShowInfo;
        MenuEvents.OnExitButtonClicked += ExitGame;
    }

    private void OnDisable()
    {
        MenuEvents.OnPlayButtonClicked -= StartGame;
        MenuEvents.OnControlsButtonClicked -= ShowControls;
        MenuEvents.OnMusicButtonClicked -= ToggleMusic;
        MenuEvents.OnAudioButtonClicked -= OpenAudioSettings;
        MenuEvents.OnQuestionButtonClicked -= ShowFAQ;
        MenuEvents.OnInfoButtonClicked -= ShowInfo;
        MenuEvents.OnExitButtonClicked -= ExitGame;
    }
    public void OnPlayButton() => MenuEvents.PlayButtonClicked();
    public void OnControlsButton() => MenuEvents.ControlsButtonClicked();
    public void OnMusicButton() => MenuEvents.MusicButtonClicked();
    public void OnAudioButton() => MenuEvents.AudioButtonClicked();
    public void OnQuestionButton() => MenuEvents.QuestionButtonClicked();
    public void OnInfoButton() => MenuEvents.InfoButtonClicked();
    public void OnExitButton() => MenuEvents.ExitButtonClicked();
    private void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }
    private void ShowControls()
    {
        Debug.Log("Showing controls menu...");
    }
    private void ToggleMusic()
    {
        Debug.Log("Toggling music...");
    }
    private void OpenAudioSettings()
    {
        Debug.Log("Opening audio settings...");
    }
    private void ShowFAQ()
    {
        Debug.Log("Showing FAQ...");
    }
    private void ShowInfo()
    {
        Debug.Log("Showing game info...");
    }
    private void ExitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
        #else
            Application.Quit(); // Derlenmiş uygulamada oyunu kapatır
        #endif
    }









}
