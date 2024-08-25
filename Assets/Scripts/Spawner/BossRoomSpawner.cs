using UnityEngine;

public class BossRoomSpawner : MonoBehaviour
{
    public GameObject bossPrefab; // 보스 프리팹을 할당할 변수
    public float mapWidth = 48.54f; // 맵의 가로 길이, 필요시 수정
    public float mapHeight = 16.03f; // 맵의 세로 길이, 필요시 수정

    void Start()
    {
        // 보스 프리팹이 할당되었는지 확인
        if (bossPrefab != null)
        {
            // 보스 스폰 위치를 맵의 오른쪽 외곽으로 설정
            Vector3 spawnPosition = new Vector3(mapWidth / 2f, 0, 0);
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("보스가 보스룸의 오른쪽 외곽에 스폰되었습니다.");
        }
        else
        {
            Debug.LogError("보스 프리팹이 할당되지 않았습니다!");
        }
    }
}