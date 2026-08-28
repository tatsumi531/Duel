using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

/// <summary>
/// タイトル画面プレハブ (Assets/Prefabs/TitleScreen.prefab) のコントローラ。
/// UI 階層はプレハブ側に持ち、このスクリプトはボタンの挙動だけを担当する。
/// GameStart は未実装。Exit のみアプリ終了として機能する。
/// </summary>
public class TitleScreen : MonoBehaviour
{
    [SerializeField] Text titleLabel;
    [SerializeField] Button gameStartButton;
    [SerializeField] Button exitButton;

    void Awake()
    {
        EnsureEventSystem();

        if (titleLabel != null)
        {
            titleLabel.text = Application.productName;
        }

        if (gameStartButton != null)
        {
            gameStartButton.onClick.AddListener(OnGameStart);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(OnExit);
        }
    }

    static void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
        {
            return;
        }

        var go = new GameObject("EventSystem");
        go.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
        go.AddComponent<InputSystemUIInputModule>();
#else
        go.AddComponent<StandaloneInputModule>();
#endif
    }

    void OnGameStart()
    {
        // TODO: ゲーム本編へ遷移する処理を実装する。
        Debug.Log("GameStart is not implemented yet.");
    }

    void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
