using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameOverSceneの音のパネルの表示状態を制御するクラスです。
public class PanelController : MonoBehaviour
{
    public GameObject panel; // 制御するパネルへの参照。

    // ゲーム開始時に一度だけ呼ばれるメソッド。
    void Start()
    {
        panel.SetActive(false); // パネルを非表示に設定します。    
    }

    // フレームごとに呼ばれるメソッド。
    void Update()
    {
        // Escapeキーが押され、パネルが表示されている場合、パネルを閉じます。
        if (Input.GetKeyDown(KeyCode.Escape) && panel.activeSelf)
        {
            ClosePanel();
        }
    }

    // パネルの表示状態を切り替えるメソッド。
    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf); // パネルの表示状態を反転させます。
    }

    // パネルを閉じるメソッド。
    public void ClosePanel()
    {
        panel.SetActive(false); // パネルを非表示に設定します。
    }
}
