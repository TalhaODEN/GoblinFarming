using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public Transform player;
    public BoxCollider2D cameraLimits;
    private Camera cam;
    private float limXmax;
    private float limYmax;
    private float limXmin;
    private float limYmin;
    private void Start()
    {
        cam = Camera.main;
        limXmax = cameraLimits.bounds.max.x;
        limYmax = cameraLimits.bounds.max.y;
        limXmin = cameraLimits.bounds.min.x;
        limYmin = cameraLimits.bounds.min.y;
    }

    private void LateUpdate()
    {
        CamFollowing();
    }

    private void CamFollowing()
    {
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        float clampedX = Mathf.Clamp(player.position.x, limXmin + halfWidth, limXmax - halfWidth);
        float clampedY = Mathf.Clamp(player.position.y, limYmin + halfHeight, limYmax - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z); 
    }


}
