using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInterface : MonoBehaviour
{
    #region Audio Settings
    [Header("Audio")]
    [SerializeField] private Slider audioSlider;
    [SerializeField] private Button audioButton;
    #endregion

    #region Info Panel
    [Header("Info")]
    [SerializeField] private GameObject infoPanel;
    private float timeToAutoClose = 5f;
    private float currentTime = 0f;
    #endregion

    #region Music
    [Header("Music")]
    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private Button musicButton;
    #endregion

    #region Controls Panel
    [Header("Controls Panel")]
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private Scrollbar controlScroll;
    #endregion

    #region FAQ
    [Header("FAQ")]
    [SerializeField] private GameObject FAQpanel;
    [SerializeField] private Scrollbar faqScroll;
    #endregion


    #region Unity Methods
    private void OnEnable()
    {
        MenuEvents.OnPlayButtonClicked += StartGame;
        MenuEvents.OnControlsButtonClicked += ShowControls;
        MenuEvents.OnCloseControlsPanelClicked += CloseControlsPanel;
        MenuEvents.OnMusicButtonClicked += ToggleMusic;
        MenuEvents.OnAudioButtonClicked += OpenAudioSettings;
        MenuEvents.OnQuestionButtonClicked += ShowFAQ;
        MenuEvents.OnCloseFAQPanelClicked += CloseFAQ;
        MenuEvents.OnInfoButtonClicked += ShowInfo;
        MenuEvents.OnExitButtonClicked += ExitGame;

        audioSlider.onValueChanged.AddListener(UpdateAudioSituation);
    }

    private void OnDisable()
    {
        MenuEvents.OnPlayButtonClicked -= StartGame;
        MenuEvents.OnControlsButtonClicked -= ShowControls;
        MenuEvents.OnCloseControlsPanelClicked -= CloseControlsPanel;
        MenuEvents.OnMusicButtonClicked -= ToggleMusic;
        MenuEvents.OnAudioButtonClicked -= OpenAudioSettings;
        MenuEvents.OnQuestionButtonClicked -= ShowFAQ;
        MenuEvents.OnCloseFAQPanelClicked -= CloseFAQ;
        MenuEvents.OnInfoButtonClicked -= ShowInfo;
        MenuEvents.OnExitButtonClicked -= ExitGame;

        audioSlider.onValueChanged.RemoveListener(UpdateAudioSituation);
    }
    private void Update()
    {
        if (infoPanel.activeSelf && CheckInfoPanelTime())
        {
            ShowInfo();
        }
        if (controlsPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseControlsPanel();
        }
        if (FAQpanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseFAQ();
        }
    }

    #endregion

    #region Button Methods
    public void OnPlayButton() => MenuEvents.PlayButtonClicked();
    public void OnControlsButton() => MenuEvents.ControlsButtonClicked();
    public void OnCloseControlsButton() => MenuEvents.CloseControlsPanelClicked();
    public void OnMusicButton() => MenuEvents.MusicButtonClicked();
    public void OnAudioButton() => MenuEvents.AudioButtonClicked();
    public void OnQuestionButton() => MenuEvents.QuestionButtonClicked();
    public void OnCloseFAQButton() => MenuEvents.CloseFAQPanelClicked();
    public void OnInfoButton() => MenuEvents.InfoButtonClicked();
    public void OnExitButton() => MenuEvents.ExitButtonClicked();
    #endregion

    #region Menu Actions

    #region Start Game Functions
    private void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }
    #endregion

    #region Control Functions
    private void ShowControls()
    {
        controlsPanel.SetActive(true);
    }
    private void CloseControlsPanel()
    {
        controlScroll.value = 1f;
        controlsPanel.SetActive(false);
    }
    #endregion

    #region Voice Functions
    private void ToggleMusic()
    {
        if (musicButton.image.color == new Color(1, 1, 1, 1))
        {
            menuMusic.volume = 0;
            musicButton.image.color = new Color(0, 0, 0, 1);
        }
        else
        {
            menuMusic.volume = 1;
            musicButton.image.color = new Color(1, 1, 1, 1);
        }
    }
    private void OpenAudioSettings()
    {
        if (audioSlider.gameObject.activeSelf)
        {
            audioSlider.gameObject.SetActive(false);
        }
        else
        {
            audioSlider.gameObject.SetActive(true);
        }
    }

    #endregion

    #region FAQ Functions
    private void ShowFAQ()
    {
        switch (FAQpanel.activeSelf)
        {
            case true:
                faqScroll.value = 1f;
                FAQpanel.SetActive(false);
                break;

            case false:
                FAQpanel.SetActive(true);
                break;
        }
    }
    private void CloseFAQ()
    {
        faqScroll.value = 1f;
        FAQpanel.SetActive(false);
    }

    #endregion

    #region Info Functions
    private void ShowInfo()
    {
        if (infoPanel.activeSelf)
        {
            infoPanel.SetActive(false);
        }
        else
        {
            infoPanel.SetActive(true);
            currentTime = 0f;
        }
    }
    #endregion

    #region Exit Functions
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit(); 
#endif
    }

    #endregion

    #endregion

    #region Helper Methods

    #region Audio Methods
    private void UpdateAudioSituation(float value)
    {
        value *= 0.01f;
        AudioListener.volume = value;

        if (value == 0)
        {
            audioButton.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        }
        else
        {
            audioButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
    #endregion

    #region Info Methods
    private bool CheckInfoPanelTime()
    {
        currentTime += Time.deltaTime;
        return currentTime >= timeToAutoClose;
    }
    #endregion

    #endregion
}
