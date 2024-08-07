using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 肉アイテムを生成するクラスです。
public class Meatgenerator : MonoBehaviour
{
    public GameObject meatPrefab; // 肉アイテムのプレハブ。
    public GameDirector gameDirector; // ゲームディレクターへの参照。
    float span = 5.0f; // 肉アイテムを生成する間隔。
    float delta = 0; // 経過時間を追跡する変数。
    private bool generateMeat = false; // 肉アイテムの生成を制御するフラグ。
    private TimerManager timerManager; // タイマーマネージャーへの参照。
    public static Meatgenerator instance; // Meatgeneratorのシングルトンインスタンス。

    // 最初のフレームの更新前に呼ばれるメソッド。
    private void Start()
    {
        timerManager = TimerManager.instance; // タイマーマネージャーのインスタンスを取得します。
        // シングルトンパターンを実装します。
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移時に破棄されないようにします。
        }
        else
        {
            Destroy(gameObject); // 既存のインスタンスがある場合は破棄します。
        }

        // シーンのアンロードとロードのイベントにメソッドを登録します。
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 肉アイテムの生成を切り替えるメソッド。
    public void ToggleMeatGenerator(bool enable)
    {
        generateMeat = enable;
    }

    // シーンがアンロードされたときに呼ばれるメソッド。
    public void OnSceneUnloaded(Scene scene)
    {
        generateMeat = false; // 肉アイテムの生成を停止します。
    }

    // シーンがロードされたときに呼ばれるメソッド。
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 特定のシーンでショップパネルが開いていない場合に肉アイテムの生成を開始します。
        if (scene.name == "SampleScene")
        {
            gameDirector = FindObjectOfType<GameDirector>();
            if (!gameDirector.isShopPanelOpen)
            {
                ToggleMeatGenerator(true);
            }
        }
    }

    // フレームごとに呼ばれるメソッド。
    void Update()
    {
        // 肉アイテムの生成が有効でない、またはプレイヤーが無敵状態、またはシールドがアクティブな場合は何もしません。
        if (!generateMeat || gameDirector.IsPlayerImmune() || gameDirector.scoreManager.isShieldActive) return;

        // 経過時間を更新します。
        this.delta += Time.deltaTime;
        // 生成間隔を超えた場合、新しい肉アイテムを生成します。
        if (this.delta > this.span)
        {
            this.delta = 0;
            GameObject go = Instantiate(meatPrefab);
            int px = Random.Range(-7, 8);
            go.transform.position = new Vector3(px, 7, 0);
        }
    }

    // オブジェクトが破棄されるときに呼ばれるメソッド。
    private void OnDestroy()
    {
        // イベントからメソッドの登録を解除します。
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
