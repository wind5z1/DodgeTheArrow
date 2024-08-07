using UnityEngine;
using UnityEngine.UI;

// ボリュームパネルスクリプト
public class VolumePanel : MonoBehaviour
{
    // メニューオブジェクトへの参照
    public GameObject pauseMenu;
    public GameObject volumeMenu;
    public GameObject returnButton;
    // ボリュームメニューが開いているかどうかのフラグ
    private bool isVolumeMenuOpen = false;
    // ポーズボタンへの参照
    public Button pauseButton;

    // 初期化メソッド
    void Start()
    {
        // ボリュームメニューと戻るボタンを非表示に設定
        volumeMenu.SetActive(false);
        returnButton.SetActive(false);
    }

    // 更新メソッド
    void Update()
    {
        // ボリュームメニューが開いている場合
        if (isVolumeMenuOpen)
        {
            // Escapeキーが押された場合
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ゲームに戻る
                ReturnToGame();
            }
        }
    }

    // ボリュームメニューを切り替えるメソッド
    public void ToggleVolumeMenu()
    {
        // ボリュームメニューの開閉状態を切り替え
        isVolumeMenuOpen = !isVolumeMenuOpen;
        // ボリュームメニューの表示を切り替え
        volumeMenu.SetActive(isVolumeMenuOpen);
        // ボリュームメニューが開いている場合
        if (isVolumeMenuOpen)
        {
            // ポーズボタンを無効にし、ポーズメニューと戻るボタンを表示
            pauseButton.interactable = false;
            pauseMenu.SetActive(false);
            returnButton.SetActive(true);
        }
        else
        {
            // ポーズボタンを有効にし、ポーズメニューを表示し、戻るボタンを非表示にする
            pauseButton.interactable = true;
            pauseMenu.SetActive(true);
            returnButton.SetActive(false);
        }
    }

    // ポーズメニューに戻るメソッド
    public void ReturnToPauseMenu()
    {
        // ボリュームメニューを非表示にし、ポーズメニューと戻るボタンを表示
        volumeMenu.SetActive(false);
        pauseMenu.SetActive(true);
        returnButton.SetActive(false);
        // ポーズボタンを有効にする
        pauseButton.interactable = true;
        // ボリュームメニューの開閉状態を閉じた状態に設定
        isVolumeMenuOpen = false;
    }

    // ゲームに戻るメソッド
    public void ReturnToGame()
    {
        // ボリュームメニューと戻るボタンを非表示にする
        volumeMenu.SetActive(false);
        returnButton.SetActive(false);
        // ポーズボタンを有効にする
        pauseButton.interactable = true;
    }
}
