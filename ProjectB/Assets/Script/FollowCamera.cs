using System;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 posOffset;
    [SerializeField] float smooth = 0.2f;

    Vector3 velocity;

    private MapData.MapSize _mapSize;

    float camWidth;
    float camHeight;

    private void Awake()
    {
        posOffset = new Vector3(0, 0, -10f); // Z√‡ ∞Ì¡§
    }

    private void Start()
    {
        _mapSize = new MapData.MapSizeIsometric(new Vector3(0f, 0f, 0f), 32f, 16f, 1f, 1f);

        Camera cam = Camera.main;
        camHeight = 2 * cam.orthographicSize;
        camWidth = cam.orthographicSize * cam.aspect;
    }

    private void Update()
    {
        Vector3 desiredPos = target.position + posOffset;
        Vector3 currentPos = transform.position;

        bool outOfX = target.position.x < -1.5f || target.position.x > 1.5f;
        bool outOfY = target.position.y < -0.3f || target.position.y > 0.3f;

        float newX = currentPos.x;
        float newY = currentPos.y;
        float fixedZ = -10f;

        if(!outOfX)
        {
            newX = Mathf.Clamp(desiredPos.x, _mapSize.MinX + camWidth, _mapSize.MaxX - camWidth);
        }

        if(!outOfY)
        {
            newY = Mathf.Clamp(desiredPos.y, _mapSize.MinY + camHeight, _mapSize.MaxY - camHeight);
        }

        Vector3 targetCameraPos = new Vector3(newX, newY, fixedZ);
        transform.position = Vector3.SmoothDamp(currentPos, targetCameraPos, ref velocity, smooth);
    }
}
