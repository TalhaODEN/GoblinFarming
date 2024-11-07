using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentAlphaController : MonoBehaviour
{
    [Range(0f, 1f)]
    public float alphaValue = 1f;

    private List<SpriteRenderer> childSpriteRenderers;

    private void Awake()
    {
        childSpriteRenderers = new List<SpriteRenderer>();

        SpriteRenderer parentSpriteRenderer = GetComponent<SpriteRenderer>();
        if (parentSpriteRenderer != null)
        {
            childSpriteRenderers.Add(parentSpriteRenderer);
        }

        foreach (Transform child in transform)
        {
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                childSpriteRenderers.Add(spriteRenderer);
            }
        }
    }

    public void SetAlpha(float alpha)
    {
        foreach (SpriteRenderer spriteRenderer in childSpriteRenderers)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
