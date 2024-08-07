using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 魚を生成するためのクラスです。
public class fishgenerator : MonoBehaviour
{
    public GameObject fishPrefab; // 魚のプレハブ。
    public GameDirector gameDirector; // ゲームディレクターへの参照。
    float span = 2.0f; // 魚を生成する間隔。
    float delta = 0; // 経過時間を追跡する変数。
    private bool generateFish = false; // 魚の生成を制御するフラグ。
    private TimerManager timerManager; // タイマーマネージャーへの参照。
    public static fishgenerator instance; // fishgeneratorのシングルトンインスタンス。

    // 最初のフレームの更新前に呼ばれるメソッド。
    private void Start()
    {
        timerManager = TimerManager.instance; // タイマーマネージャーのインスタンスを取得します。
        // シングルトンパターンを実装します。
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // 既存のインスタンスがある場合は破棄します。
        }

        // シーンのアンロードとロードのイベントにメソッドを登録します。
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 魚の生成を切り替えるメソッド。
    public void ToggleFishGenerator(bool enable)
    {
        generateFish = enable;
    }

    // シーンがアンロードされたときに呼ばれるメソッド。
    public void OnSceneUnloaded(Scene scene)
    {
        generateFish = false; // 魚の生成を停止します。
    }

    // シーンがロードされたときに呼ばれるメソッド。
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 特定のシーンでショップパネルが開いていない場合に魚の生成を開始します。
        if (scene.name == "SampleScene")
        {
            gameDirector = FindObjectOfType<GameDirector>();
            if (!gameDirector.isShopPanelOpen)
            {
                ToggleFishGenerator(true);
            }
        }
    }

    // フレームごとに呼ばれるメソッド。
    void Update()
    {
        // 魚の生成が有効でない場合は何もしません。
        if (!generateFish) return;
        // 経過時間を更新します。
        this.delta += Time.deltaTime;
        // 生成間隔を超えた場合、新しい魚を生成します。
        if (this.delta > this.span)
        {
            this.delta = 0;
            GameObject go = Instantiate(fishPrefab);
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
