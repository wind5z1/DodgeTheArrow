using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// タイマーの管理を行うクラスです。
public class TimerManager : MonoBehaviour
{
    private float currentTime = 0f; // 現在の経過時間。
    private bool isTimeRunning = false; // タイマーが動作中かどうかのフラグ。
    public static TimerManager instance; // TimerManagerのシングルトンインスタンス。

    // オブジェクトが生成されたときに呼ばれるメソッド。
    private void Awake()
    {
        // シングルトンパターンを実装します。
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 既存のインスタンスがある場合は破棄します。
        }
    }

    // フレームごとに呼ばれるメソッド。
    void Update()
    {
        // タイマーが動作中の場合、経過時間を更新します。
        if (isTimeRunning)
        {
            currentTime += Time.deltaTime;
        }
    }

    // タイマーを開始するメソッド。
    public void StartTimer()
    {
        isTimeRunning = true;
    }

    // タイマーを停止するメソッド。
    public void StopTimer()
    {
        isTimeRunning = false;
    }

    // 経過時間を取得するメソッド。
    public float GetElapsedTime()
    {
        return currentTime;
    }
}
