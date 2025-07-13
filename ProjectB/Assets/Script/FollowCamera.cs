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
        posOffset = new Vector3(0, 0, -10f); // Z 고정
    }

    private void Start()
    {
        // 예시 맵 크기: 중심점 (0,0,0), 너비 32, 높이 16
        _mapSize = new MapData.MapSizeIsometric(Vector3.zero, 32f, 16f, 1f, 1f);

        // 카메라 뷰 사이즈 계산
        Camera cam = Camera.main;
        camHeight = 2 * cam.orthographicSize;
        camWidth = cam.orthographicSize * cam.aspect;
    }

    private void Update()
    {
        Vector3 desiredPos = target.position + posOffset;
        Vector3 currentPos = transform.position;

        // X/Y 제한 범위: 일정 영역(예: 화면 중심 기준 X: ±1.5, Y: ±0.8)
        bool isInsideX = target.position.x > 1.9f || target.position.x < -1.9f;
        bool isInsideY = target.position.y > 0.9f || target.position.y < -0.9f;

        float newX = currentPos.x;
        float newY = currentPos.y;
        float fixedZ = -10f;

        // 범위 안에 있으면 따라감
        if(!isInsideX)
        {
            newX = Mathf.Clamp(desiredPos.x, _mapSize.MinX + camWidth, _mapSize.MaxX - camWidth);
        }

        if(!isInsideY)
        {
            newY = Mathf.Clamp(desiredPos.y, _mapSize.MinY + camHeight / 2f, _mapSize.MaxY - camHeight / 2f);
        }

        // 최종 위치 설정
        Vector3 targetCameraPos = new Vector3(newX, newY, fixedZ);

        // 부드럽게 이동
        transform.position = Vector3.SmoothDamp(currentPos, targetCameraPos, ref velocity, smooth);
    }
}