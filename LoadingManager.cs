using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// シーンの非同期ロードを管理するクラスです。
public class LoadingManager : MonoBehaviour
{
    public GameObject loadingScreen; // ローディング画面のUI。
    public Slider loadingSlider; // ローディングの進捗を表示するスライダー。
    private bool isLoading = false; // ローディング中かどうかのフラグ。

    // シーンのロードを開始するメソッド。
    public void StartLoading(string sceneToLoad)
    {
        // 既にローディング中でなければ、ローディングを開始します。
        if (!isLoading)
        {
            isLoading = true;
            Time.timeScale = 1; // ゲームの時間を通常通り進行させます。
            loadingScreen.SetActive(true); // ローディング画面を表示します。
            StartCoroutine(LoadSceneAsync(sceneToLoad)); // 非同期でシーンをロードします。
        }
        // GameDirectorが存在する場合、購入状態と無敵状態をリセットします。
        if (GameDirector.instance != null)
        {
            GameDirector.instance.hasPurchasedItem = false;
            GameDirector.instance.hasEnteredImmunity = false;
        }
    }

    // シーンを非同期でロードするコルーチン。
    IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        // ロードが完了するまでループします。
        while (!operation.isDone)
        {
            // ロードの進捗をスライダーに反映させます。
            float progress = Mathf.Clamp01(operation.progress / 1.0f);
            loadingSlider.value = progress;
            yield return null;
        }
        yield return new WaitForEndOfFrame(); // フレームの終わりまで待機します。

        // ShopManagerが存在する場合、購入状態をリセットします。
        if (ShopManager.instance != null)
        {
            ShopManager.instance.ResetPurchaseStatus();
        }

        loadingScreen.SetActive(false); // ローディング画面を非表示にします。
    }
}
