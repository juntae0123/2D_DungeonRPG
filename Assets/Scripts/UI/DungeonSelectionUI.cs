using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;  // TextMeshProUGUI를 사용하기 위해 필요

public class DungeonSelectionUI : MonoBehaviour
{
    public GameObject dungeonSelectionPanel; // 던전 선택 패널
    public Button dungeon1Button;
    public Button dungeon2Button;
    public Button dungeon3Button;
    public Button dungeon4Button;

    void Start()
    {
        dungeonSelectionPanel.SetActive(false); // 처음엔 패널 비활성화

        // 버튼 클릭 시 각 던전 씬 로드
        dungeon1Button.onClick.AddListener(() => LoadDungeon("Dungeon1"));
        dungeon2Button.onClick.AddListener(() => LoadDungeon("Dungeon2"));
        dungeon3Button.onClick.AddListener(() => LoadDungeon("Dungeon3"));
        dungeon4Button.onClick.AddListener(() => LoadDungeon("Dungeon4"));

        // 각 버튼의 텍스트 설정
        dungeon1Button.GetComponentInChildren<TextMeshProUGUI>().text = "Dungeon 1";
        dungeon2Button.GetComponentInChildren<TextMeshProUGUI>().text = "Dungeon 2";
        dungeon3Button.GetComponentInChildren<TextMeshProUGUI>().text = "Dungeon 3";
        dungeon4Button.GetComponentInChildren<TextMeshProUGUI>().text = "Dungeon 4";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dungeonSelectionPanel.SetActive(true); // 플레이어가 포탈에 접근하면 패널 활성화
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == null) return; // 오브젝트가 이미 파괴된 경우, 아무 작업도 하지 않음.

        if (other.gameObject.CompareTag("Player"))
        {
            // 관련된 로직...
        }
    }
    void LoadDungeon(string dungeonSceneName)
    {
        Debug.Log("Loading dungeon: " + dungeonSceneName);
        SceneManager.LoadScene(dungeonSceneName); // 던전 씬 로드
    }
}