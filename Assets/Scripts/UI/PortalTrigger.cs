using UnityEngine;

public class PortalTrigger : MonoBehaviour
{
    public GameObject dungeonSelectionPanel; // 던전 선택 UI 패널
    public GameObject bossRoomConfirmationPanel; // 보스룸 입장 확인 UI 패널

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (dungeonSelectionPanel != null)
            {
                dungeonSelectionPanel.SetActive(true); // 던전 포탈 접근 시 UI 패널 표시
            }
            if (bossRoomConfirmationPanel != null)
            {
                bossRoomConfirmationPanel.SetActive(true); // 보스 포탈 접근 시 UI 패널 표시
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (dungeonSelectionPanel != null)
            {
                dungeonSelectionPanel.SetActive(false); // 플레이어가 포탈에서 멀어지면 UI 패널 숨김
            }
            if (bossRoomConfirmationPanel != null)
            {
                bossRoomConfirmationPanel.SetActive(false); // 플레이어가 보스 포탈에서 멀어지면 UI 패널 숨김
            }
        }
    }
}