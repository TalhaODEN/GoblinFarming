using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MenuEvents;

public class PauseInterface : MonoBehaviour
{
    public delegate void Actions();
    public static event Actions OnClosePanel;
    public static event Actions OnOpenPanel;
    public static event Actions OnMusicButtonClicked;
    public static event Actions OnAudioButtonClicked;
    public static event Actions OnQuitButtonClicked;

    [Header("Panels")]
    [SerializeField] private GameObject EscPanel;
    [SerializeField] private GameObject FAQPanel;
    [SerializeField] private GameObject ControlPanel;

    [Header("Audio")]
    [SerializeField] private Slider audioSlider;
    [SerializeField] private Button audioButton;

    [Header("Music")]
    [SerializeField] private AudioSource Music;
    [SerializeField] private Button musicButton;

    [SerializeField] private Scrollbar controlScroll;
    [SerializeField] private Scrollbar faqScroll;
    public static void ClosePanel() => OnClosePanel?.Invoke();
    public static void OpenPanel() => OnOpenPanel?.Invoke();
    public static void MusicButtonClicked() => OnMusicButtonClicked?.Invoke();
    public static void AudioButtonClicked() => OnAudioButtonClicked?.Invoke();
    public static void QuitButtonClicked() => OnQuitButtonClicked?.Invoke();

    private void OnEnable()
    {
        OnClosePanel += CheckEscMenu;
        OnOpenPanel += OpenPanel;
        OnMusicButtonClicked += ToggleMusic;
        OnAudioButtonClicked += OpenAudioSettings;
        OnQuitButtonClicked += QuitAction;
        audioSlider.onValueChanged.AddListener(UpdateAudioSituation);
    }
    private void OnDisable()
    {
        OnClosePanel -= CheckEscMenu;
        OnOpenPanel -= OpenPanel;
        OnMusicButtonClicked -= ToggleMusic;
        OnAudioButtonClicked -= OpenAudioSettings;
        OnQuitButtonClicked -= QuitAction;
        audioSlider.onValueChanged.RemoveListener(UpdateAudioSituation);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Inventory_UI.inventory_UI.show)
        {
            CheckEscMenu();
        }
    }
    public void CheckEscMenu()
    {
        if (ControlPanel.activeSelf)
        {
            controlScroll.value = 1f;
            ControlPanel.SetActive(false);
            EscPanel.SetActive(true);
            return;
        }
        if (FAQPanel.activeSelf)
        {
            faqScroll.value = 1f;
            FAQPanel.SetActive(false);
            EscPanel.SetActive(true);
            return;
        }
        if (EscPanel.activeSelf)
        {
            EscPanel.SetActive(false);
            TimeManager.timeManager.ResumeTime();
        }
        else if (!EscPanel.activeSelf)
        {
            EscPanel.SetActive(true);
            TimeManager.timeManager.PauseTime();
        }
    }
    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        EscPanel.SetActive(false);
    }
    public void ToggleMusic()
    {
        if (musicButton.image.color == new Color(1, 1, 1, 1))
        {
            Music.volume = 0;
            musicButton.image.color = new Color(0, 0, 0, 1);
        }
        else
        {
            Music.volume = 1;
            musicButton.image.color = new Color(1, 1, 1, 1);
        }
    }
    public void OpenAudioSettings()
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

    public void UpdateAudioSituation(float value)
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
    public void QuitAction()
    {
        ToggleMusic();
        SceneManager.LoadScene(0);
    }
}
