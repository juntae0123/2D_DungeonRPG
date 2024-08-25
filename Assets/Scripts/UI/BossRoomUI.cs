using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossRoomUI : MonoBehaviour
{
    public GameObject bossRoomConfirmationPanel; // 보스룸 입장 확인 패널
    public Button yesButton;
    public Button noButton;

    private bool allDungeonsCleared = false;

    void Start()
    {
        bossRoomConfirmationPanel.SetActive(false); // 패널을 처음에 비활성화
        yesButton.onClick.AddListener(() => EnterBossRoom());
        noButton.onClick.AddListener(() => MovePlayerAway());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && allDungeonsCleared)
        {
            bossRoomConfirmationPanel.SetActive(true); // 모든 던전을 클리어한 경우에만 패널 활성화
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossRoomConfirmationPanel.SetActive(false); // 플레이어가 멀어지면 패널 비활성화
        }
    }

    void EnterBossRoom()
    {
        SceneManager.LoadScene("BossRoom"); // 보스룸 씬 로드
    }

    void MovePlayerAway()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position += new Vector3(-1f, 0, 0); // 플레이어를 뒤로 이동
        bossRoomConfirmationPanel.SetActive(false); // 패널 비활성화
    }

    public void SetAllDungeonsCleared(bool cleared)
    {
        allDungeonsCleared = cleared; // 외부에서 던전 완료 여부를 설정할 수 있도록 메서드 추가
    }
}