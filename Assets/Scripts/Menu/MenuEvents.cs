using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEvents : MonoBehaviour
{
    #region Button Actions

        public delegate void ButtonAction();

        #region Play
            public static event ButtonAction OnPlayButtonClicked; 
        #endregion

        #region Controls
            public static event ButtonAction OnControlsButtonClicked;
            public static event ButtonAction OnCloseControlsPanelClicked;
        #endregion

        #region Voices
        #region Music
            public static event ButtonAction OnMusicButtonClicked;
        #endregion
        #region Audio
            public static event ButtonAction OnAudioButtonClicked;
        #endregion
        #endregion

        #region FAQ
            public static event ButtonAction OnQuestionButtonClicked;
            public static event ButtonAction OnCloseFAQPanelClicked;
        #endregion

        #region Info
    public static event ButtonAction OnInfoButtonClicked;
        #endregion

        #region Exit
            public static event ButtonAction OnExitButtonClicked;
        #endregion

    #endregion

    #region Button Click Methods

        #region Play Methods
            public static void PlayButtonClicked() => OnPlayButtonClicked?.Invoke(); 
        #endregion

        #region Control Panel Methods
            public static void ControlsButtonClicked() => OnControlsButtonClicked?.Invoke();
            public static void CloseControlsPanelClicked() => OnCloseControlsPanelClicked?.Invoke();
        #endregion

        #region Voice Methods
            #region Music Methods
                public static void MusicButtonClicked() => OnMusicButtonClicked?.Invoke();
            #endregion

            #region Audio Methods
                public static void AudioButtonClicked() => OnAudioButtonClicked?.Invoke();
    #endregion
    #endregion

        #region FAQ Methods
            public static void QuestionButtonClicked() => OnQuestionButtonClicked?.Invoke();
            public static void CloseFAQPanelClicked() => OnCloseFAQPanelClicked?.Invoke();

        #endregion

        #region Info Methods
            public static void InfoButtonClicked() => OnInfoButtonClicked?.Invoke();

        #endregion

        #region Exit Methods
            public static void ExitButtonClicked() => OnExitButtonClicked?.Invoke(); 

        #endregion

    #endregion
}
