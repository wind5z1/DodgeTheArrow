using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// コインの管理を行うクラスです。
public class CoinManager : MonoBehaviour
{
    public static CoinManager instance; // CoinManagerのシングルトンインスタンス。
    public TaskSystem taskSystem; // タスクシステムへの参照。
    public int coins = 0; // 現在のコイン数。
    public int sessionCoins = 0; // セッション中に獲得したコイン数。
    public Text coinText; // コイン数を表示するテキストUI。
    public Text coinDisplayText; // 別の場所でコイン数を表示するテキストUI。

    // オブジェクトが生成されたときに呼ばれるメソッド。
    private void Awake()
    {
        // シングルトンパターンを実装します。
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移時に破棄されないようにします。
            coins = LoadCoins(); // コインをロードします。
            Debug.Log("Awake - coins loaded: " + coins);
            updatecoinText(); // コインテキストを更新します。
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 既存のインスタンスがある場合は破棄します。
        }
    }

    // コインをロードするメソッド。
    private int LoadCoins()
    {
        int loadedCoins = PlayerPrefs.GetInt("Coins", 0);
        Debug.Log("Coins loaded from playerpref: " + loadedCoins);
        return loadedCoins;
    }

    // オブジェクトが有効になったときに呼ばれるメソッド。
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // シーンがロードされたときのイベントにメソッドを登録します。
    }

    // オブジェクトが無効になったときに呼ばれるメソッド。
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // イベントからメソッドの登録を解除します。
    }

    // シーンがロードされたときに呼ばれるメソッド。
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 必要なコンポーネントを取得して初期化します。
        coinText = GameObject.Find("Coin").GetComponent<Text>();
        coinDisplayText = GameObject.Find("CoinDisplay").GetComponent<Text>();
        taskSystem = GameObject.FindObjectOfType<TaskSystem>();
        coins = LoadCoins(); // コインを再ロードします。
        sessionCoins = 0;
        Debug.Log("OnSceneLoaded - Coins reloaded: " + coins);
        updatecoinText(); // コインテキストを更新します。
    }

    // コインを追加するメソッド。
    public void Addcoins(int amount)
    {
        coins += amount;
        sessionCoins += amount;
        // セッション中に一定数のコインを獲得した場合、タスクを完了します。
        if (sessionCoins >= 10000)
        {
            taskSystem.CompleteTask("task_4");
        }
        PlayerPrefs.SetInt("Coins", coins); // コイン数を保存します。
        PlayerPrefs.Save();
        updatecoinText(); // コインテキストを更新します。
    }

    // コインテキストを更新するメソッド。
    public void updatecoinText()
    {
        // コインテキストが存在する場合は、テキストを更新します。
        if (coinText != null && coinDisplayText != null)
        {
            coinText.text = "X" + coins.ToString();
            coinDisplayText.text = "X" + coins.ToString();
        }
    }
}
