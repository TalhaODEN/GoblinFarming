using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;
    [SerializeField] private List<GameObject> shakeButtonsList;
    private Dictionary<GameObject, bool> isShakingStates = new Dictionary<GameObject, bool>();
    public static UIManager uiManager;
    private float shakeDuration;
    private float shakeMagnitude;
    private void Awake()
    {
        if(uiManager == null)
        {
            uiManager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        foreach (GameObject shakebutton in shakeButtonsList)
        {
            isShakingStates.Add(shakebutton,false);
        }
    }
    public bool IsAnyPanelOpen()
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf) 
            {
                return true; 
            }
        }
        return false; 
    }
    public bool IsSpecificPanelsOpen(params GameObject[] panels)
    {
        foreach(var panel in panels)
        {
            if (IsPanelOpen(panel))
            {
                return true;
            }
        }
        return false;
    }
    public bool IsAnyPanelActiveExcept(GameObject panelToIgnore)
    {
        foreach (GameObject panel in panels)
        {
            if (panel != panelToIgnore && panel.activeSelf)
            {
                return true; 
            }
        }
        return false; 
    }

    public bool IsPanelOpen(GameObject panel)
    {
        return panel.activeSelf; 
    }
    public void ShakeButton(GameObject button,float duration = 0.5f,float magnitude = 0.5f)
    {
        if (!isShakingStates.ContainsKey(button))
        {
            isShakingStates[button] = false; 
        }

        if (!isShakingStates[button])
        {
            isShakingStates[button] = true; 
            StopAllCoroutines();
            StartCoroutine(Shake(button.transform,duration,magnitude));
        }
    }

    private IEnumerator Shake(Transform buttonTransform, float duration = 0.5f, float magnitude = 0.5f)
    {
        Vector3 originalPosition = buttonTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = UnityEngine.Random.Range(-magnitude, magnitude);
            buttonTransform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y, originalPosition.z);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        buttonTransform.localPosition = originalPosition;
        isShakingStates[buttonTransform.gameObject] = false; 
    }

}
