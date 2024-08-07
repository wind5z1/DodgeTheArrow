using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの動きを制御するクラスです。
public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3.0f; // プレイヤーの移動速度。
    private TimerManager timerManager; // タイマーマネージャーへの参照。

    // 最初のフレームの更新前に呼ばれるメソッド。
    void Start()
    {
        Application.targetFrameRate = 60; // フレームレートを60に設定します。
        timerManager = TimerManager.instance; // タイマーマネージャーのインスタンスを取得します。
        timerManager.StartTimer(); // タイマーを開始します。
    }

    // フレームごとに呼ばれるメソッド。
    void Update()
    {
        Vector3 currentPosition = transform.position; // 現在の位置を取得します。

        // 左矢印キーが押された場合、左に移動します。
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentPosition.x -= movementSpeed;
        }

        // 右矢印キーが押された場合、右に移動します。
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentPosition.x += movementSpeed;
        }

        // プレイヤーが画面外に出ないように位置を制限します。
        Camera mainCamera = Camera.main;
        float halfPlayerWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        float clampedX = Mathf.Clamp(currentPosition.x, mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + halfPlayerWidth, mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - halfPlayerWidth);

        // 位置を更新します。
        transform.position = new Vector3(clampedX, currentPosition.y, currentPosition.z);
    }
}
