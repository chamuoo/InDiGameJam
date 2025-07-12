using UnityEngine;
using UnityEngine.Tilemaps;

public enum WallType
{
    Normal,
    StrongWall,
    BombWall,
    AutoWall,
    ChallengeWall
}

// 노말 블럭
public class Wall : MonoBehaviour
{
    [SerializeField] WallType wallType;

    int hp = 10;

    // 블록 이미지들
    [SerializeField] Sprite normal;
    [SerializeField] Sprite strong;
    [SerializeField] Sprite bomb;
    [SerializeField] Sprite auto;
    [SerializeField] Sprite challenge;

    SpriteRenderer spriteRenderer;

    private Tilemap tilemap;


    [SerializeField] LayerMask enemyLayer;

    int bombDamage = 5;
    private bool hasExploded = false; // 중복 방지를 위한 플래그

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        tilemap = FindObjectOfType<Tilemap>();

        switch(wallType)
        {
            case WallType.Normal:    // 일반 벽
                spriteRenderer.sprite = normal;
                hp = 10;
                break;
            case WallType.StrongWall:    // 단단한 벽
                spriteRenderer.sprite = strong;
                hp = 20;
                break;
            case WallType.BombWall:      // 폭탄 벽
                spriteRenderer.sprite = bomb;
                hp = 1; // Bombs usually have low HP to trigger quickly
                break;
            case WallType.AutoWall:      // 오토타겟팅 벽
                spriteRenderer.sprite = auto;
                hp = 10;
                break;
            case WallType.ChallengeWall:     // 도발 벽
                spriteRenderer.sprite = challenge;
                hp = 10;
                break;

            default:     // 아무것도 없음
                print("없는 블록");
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        print($"해당 블럭: {this.gameObject.name}, 남은 HP: {hp}");

        if(hp <= 0)
        {
            if(wallType == WallType.BombWall)
            {
                Bomb();
            }
            Destroy(this.gameObject);
        }
    }

    void Bomb()
    {
        Debug.Log($"--- BombWall 폭발: 주변 3x3 범위에 {bombDamage} 데미지 적용 시작 ---");

        // 해당 플레이어의 위치를 Int로 바꿔서 블럭의 그리드 값과 비슷한 위치를 출력
        Vector3Int bombWallCell = tilemap.WorldToCell(transform.position);

        bool affectedAnyObject = false;

        // 3 x 3 인식
        for(int x = bombWallCell.x - 1; x <= bombWallCell.x + 1; x++)
        {
            for(int y = bombWallCell.y - 1; y <= bombWallCell.y + 1; y++)
            {
                Vector3Int currentGridCell = new Vector3Int(x, y, bombWallCell.z);

                Vector3 currentCellWorldCenter = tilemap.GetCellCenterWorld(currentGridCell);

                Collider2D[] hitObjects = Physics2D.OverlapBoxAll(
                    currentCellWorldCenter,
                    new Vector2(0.9f, 0.9f),
                    0f
                );

                if(hitObjects.Length > 0)
                {
                    Debug.Log($"타일 ({currentGridCell.x}, {currentGridCell.y})에서 객체 발견:");
                    foreach(Collider2D hitCollider in hitObjects)
                    {
                        if(hitCollider.gameObject == this.gameObject)
                        {
                            Debug.Log($"- (자신) {hitCollider.name}는 건너뜝니다.");
                            continue;
                        }

                        // Check tags and apply damage
                        if(hitCollider.CompareTag("Enemy"))
                        {
                            affectedAnyObject = true;
                            Debug.Log($"- Enemy {hitCollider.name}에게 {bombDamage} 데미지 적용.");
                            // 몬스터 타격 함수 호출
                            
                        }
                        else if(hitCollider.CompareTag("Player"))
                        {
                            affectedAnyObject = true;
                            Debug.Log($"- Player {hitCollider.name}에게 {bombDamage} 데미지 적용.");
                            hitCollider.GetComponent<PlayerController>()?.TakeDamage(bombDamage);
                        }
                        else if(hitCollider.CompareTag("Wall"))
                        {
                            affectedAnyObject = true;
                            Debug.Log($"- Wall {hitCollider.name}에게 {bombDamage} 데미지 적용.");
                            hitCollider.GetComponent<Wall>()?.TakeDamage(bombDamage);
                        }
                        else
                        {
                            Debug.Log($"- 인식된 객체: {hitCollider.name} (태그 없음 또는 해당 없음)");
                        }
                    }
                }
            }
        }

        if(!affectedAnyObject)
        {
            Debug.Log("주변 3x3 범위에서 영향을 받은 객체를 찾지 못했습니다.");
        }
        Debug.Log("--- 폭발 효과 종료 ---");
    }
}
