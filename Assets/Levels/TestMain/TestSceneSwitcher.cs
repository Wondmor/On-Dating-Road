using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestSceneSwitcher : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);  // 添加监听器
    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene(transform.Find("Text").GetComponent<TextMeshProUGUI>().text);
    }
}