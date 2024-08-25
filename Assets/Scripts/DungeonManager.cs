using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    private int totalMonsters; // 던전 내 총 몬스터 수
    private int defeatedMonsters; // 죽은 몬스터 수 추적

    void Start()
    {
        // 현재 씬에서 존재하는 몬스터의 수를 세기 위해 적절히 설정합니다.
        totalMonsters = FindObjectsOfType<EnemyBase>().Length;
        defeatedMonsters = 0;
    }

    // 몬스터가 죽을 때 호출되는 함수
    public void OnMonsterDefeated()
    {
        defeatedMonsters++;

        // 모든 몬스터가 죽었을 때 타운으로 복귀
        if (defeatedMonsters >= totalMonsters)
        {
            ReturnToTown();
        }
    }

    void ReturnToTown()
    {
        // 타운 씬으로 전환
        SceneManager.LoadScene("Town");
    }
}