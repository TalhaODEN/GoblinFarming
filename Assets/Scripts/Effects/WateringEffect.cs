using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WateringEffect : MonoBehaviour
{
    public Tilemap tilemap; 
    public ParticleSystem waterParticleEffectPrefab;
    public static WateringEffect wateringEffect;

    private void Awake()
    {
        if (wateringEffect == null)
        {
            wateringEffect = this;
        }
    }

    public void StartWateringEffect(Vector3Int tilePosition)
    {
        Vector3 worldPosition = tilemap.CellToWorld(tilePosition);
        worldPosition.y += 0.5f;
        worldPosition.x += 0.5f;
        Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);
        ParticleSystem newParticleEffect = Instantiate(waterParticleEffectPrefab, worldPosition, rotation);
        newParticleEffect.Play();
        Destroy(newParticleEffect.gameObject, newParticleEffect.main.duration);
    }
}
