using UnityEngine;
using System.Collections;

public class ShowButtonSymbol : MonoBehaviour
{
    public GameObject buttonSymbol;
    public float blinkDuration;
    public Vector3 offset;
    private Coroutine blinkCoroutine;
    private RectTransform buttonSymbolRectTransform;
    private bool isBlinking = false;

    private UIManager uiManager;
    private bool wasBlinkingBeforePanelOpen = false;

    void Start()
    {
        buttonSymbolRectTransform = buttonSymbol.GetComponent<RectTransform>();
        buttonSymbol.SetActive(false); 

        uiManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        if (uiManager != null && uiManager.IsAnyPanelOpen())
        {
            if (isBlinking)
            {
                wasBlinkingBeforePanelOpen = true; 
                StopBlinking();
            }
            return; 
        }

        if (wasBlinkingBeforePanelOpen && !uiManager.IsAnyPanelOpen())
        {
            StartBlinking();
            wasBlinkingBeforePanelOpen = false;
        }

        if (isBlinking) 
        {
            UpdateButtonSymbolPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isBlinking && !uiManager.IsAnyPanelOpen()) 
        {
            StartBlinking();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isBlinking)
        {
            StopBlinking();
        }
    }

    private void StartBlinking()
    {
        isBlinking = true;
        buttonSymbol.SetActive(true); 
        UpdateButtonSymbolPosition(); 
        blinkCoroutine = StartCoroutine(BlinkSymbol());
    }

    private void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        if (buttonSymbol != null && buttonSymbol.activeSelf)
        {
            buttonSymbol.SetActive(false); 
        }
        isBlinking = false;
    }


    IEnumerator BlinkSymbol()
    {
        while (isBlinking)
        {
            buttonSymbol.SetActive(true);
            yield return new WaitForSeconds(blinkDuration);
            buttonSymbol.SetActive(false);
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    private void UpdateButtonSymbolPosition()
    {
        Vector3 worldPosition = transform.position + offset;
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
        buttonSymbolRectTransform.position = screenPosition; 
    }
}
