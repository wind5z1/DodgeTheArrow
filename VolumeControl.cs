using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ボリュームコントロールスクリプト
public class VolumeControl : MonoBehaviour
{
    // ボリュームスクロールバーへの参照
    public Scrollbar volumeScrollbar;
    // オーディオソースへの参照
    public AudioSource audioSource;

    // 初期化メソッド
    // 最初のフレームの更新前に呼び出されます
    void Start()
    {
        // オーディオソースのボリュームをスクロールバーの値に設定
        volumeScrollbar.value = audioSource.volume;
    }

    // ボリュームを調整するメソッド
    // このメソッドは毎フレーム呼び出されます
    public void AdjustVolume()
    {
        // スクロールバーの値に基づいてオーディオソースのボリュームを調整
        audioSource.volume = volumeScrollbar.value;
    }
}
