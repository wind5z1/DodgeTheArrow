using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// メニューのサウンド管理を行うクラスです。
public class MenuSoundManager : MonoBehaviour
{
    public AudioSource audioSource; // オーディオソースへの参照。
    public AudioClip menuOpenSound; // メニュー開く時のサウンドクリップ。
    public AudioClip menuCloseSound; // メニュー閉じる時のサウンドクリップ。

    private bool isMenuOpen = false; // メニューが開いているかどうかのフラグ。

    // メニュー開くサウンドを再生するメソッド。
    public void PlayMenuSound()
    {
        if (menuOpenSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(menuOpenSound);
        }
    }

    // メニューサウンドを停止するメソッド。
    public void StopMenuSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    // メニュー閉じるサウンドを再生するメソッド。
    public void PlayMenuCloseSound()
    {
        if (menuCloseSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(menuCloseSound);
        }
    }

    // フレームごとに呼ばれるメソッド。
    void Update()
    {
        // Escapeキーが押されたとき、メニューの状態を切り替え、対応するサウンドを再生します。
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpen)
            {
                isMenuOpen = false;
                StopMenuSound();
                PlayMenuCloseSound();
            }
            else
            {
                PlayMenuSound();
                isMenuOpen = true;
            }
        }
    }
}
