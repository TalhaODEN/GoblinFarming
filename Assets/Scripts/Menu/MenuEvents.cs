using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    public delegate void ButtonAction();
    public static event ButtonAction OnPlayButtonClicked;
    public static event ButtonAction OnControlsButtonClicked;
    public static event ButtonAction OnMusicButtonClicked;
    public static event ButtonAction OnAudioButtonClicked;
    public static event ButtonAction OnQuestionButtonClicked;
    public static event ButtonAction OnInfoButtonClicked;
    public static event ButtonAction OnExitButtonClicked;

    public static void PlayButtonClicked() => OnPlayButtonClicked?.Invoke();
    public static void ControlsButtonClicked() => OnControlsButtonClicked?.Invoke();
    public static void MusicButtonClicked() => OnMusicButtonClicked?.Invoke();
    public static void AudioButtonClicked() => OnAudioButtonClicked?.Invoke();
    public static void QuestionButtonClicked() => OnQuestionButtonClicked?.Invoke();
    public static void InfoButtonClicked() => OnInfoButtonClicked?.Invoke();
    public static void ExitButtonClicked() => OnExitButtonClicked?.Invoke();


}
