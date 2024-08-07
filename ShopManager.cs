using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// ショップでのアイテム購入を管理するクラスです。
public class ShopManager : MonoBehaviour
{
    public static ShopManager instance; // ShopManagerのシングルトンインスタンス。
    public TaskSystem taskSystem; // タスクシステムへの参照。
    public CoinManager coinManager; // コインマネージャーへの参照。
    public ScoreManager scoreManager; // スコアマネージャーへの参照。
    public GameDirector gameDirector; // ゲームディレクターへの参照。
    private bool hasPurchasedFirstItem = false; // 最初のアイテムを購入したかどうか。
    private bool hasPurchasedSecondItem = false; // 二番目のアイテムを購入したかどうか。
    public int itemCost = 3000; // 最初のアイテムのコスト。
    public int seconditemCost = 10000; // 二番目のアイテムのコスト。

    private void Awake()
    {
        // シングルトンパターンを実装します。
        if (instance == null)
        {
            instance = this;
            coinManager = FindObjectOfType<CoinManager>();
            scoreManager = FindObjectOfType<ScoreManager>();
            taskSystem = FindObjectOfType<TaskSystem>();
            gameDirector = FindObjectOfType<GameDirector>();
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 既存のインスタンスがある場合は破棄します。
        }
    }

    // 最初のアイテムを購入するメソッド。
    public void PurchaseItem()
    {
        if (Purchase(itemCost))
        {
            gameDirector.RecordPurchase(); // 購入記録を行います。
        }
    }

    // 二番目のアイテムを購入するメソッド。
    public void PurchaseSecondItem()
    {
        if (Purchase(seconditemCost))
        {
            gameDirector.RecordPurchase(); // 購入記録を行います。
        }
    }

    // アイテムを購入するための共通処理を行うメソッド。
    private bool Purchase(int cost)
    {
        Debug.Log("Current coins: " + coinManager.coins);
        if (coinManager.coins >= cost)
        {
            // まだ購入していないアイテムの場合、購入処理を行います。
            if ((cost == itemCost && !hasPurchasedFirstItem) || (cost == seconditemCost && !hasPurchasedSecondItem))
            {
                coinManager.coins -= cost; // コインを減らします。
                coinManager.updatecoinText(); // コインテキストを更新します。
                PlayerPrefs.SetInt("Coins", coinManager.coins); // コイン数を保存します。
                PlayerPrefs.Save();
                Debug.Log("Money after buying" + coinManager.coins);

                // 購入したアイテムに応じて処理を行います。
                if (cost == itemCost)
                {
                    scoreManager.ActiveShield(); // シールドをアクティブにします。
                    taskSystem.CompleteTask("task_3"); // タスクを完了します。
                    hasPurchasedFirstItem = true;
                }
                else if (cost == seconditemCost)
                {
                    scoreManager.score = scoreManager.scoreAfterBroughtItem; // スコアを更新します。
                    scoreManager.UpdateScoreText(); // スコアテキストを更新します。
                    taskSystem.CompleteTask("task_3"); // タスクを完了します。
                    hasPurchasedSecondItem = true;
                }
            }
            else
            {
                Debug.Log("You already brought this item!"); // 既に購入済みの場合は警告します。
            }
            return true;
        }
        else
        {
            Debug.Log("Insufficient Coin."); // コインが不足している場合は警告します。
            return false;
        }
    }

    // 購入状態をリセットするメソッド。
    public void ResetPurchaseStatus()
    {
        hasPurchasedFirstItem = false;
        hasPurchasedSecondItem = false;
    }
}
