using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Button gameStartButton;
    [SerializeField] Button exitButton;


    void Awake()
    {
        gameStartButton.onClick.AddListener(OnGameStart);
        exitButton.onClick.AddListener(OnExit);
    }

    void OnGameStart()
    {
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
