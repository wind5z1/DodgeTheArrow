using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 矢を生成するためのクラスです。
public class arrowgenerator : MonoBehaviour
{
    public GameObject arrowPrefab; // 矢のプレハブ。
    public GameDirector gameDirector; // ゲームディレクターへの参照。
    float span = 1.0f; // 矢を生成する間隔。
    float delta = 0; // 時間の経過を追跡するための変数。
    float maxSpeed = 0.2f; // 矢の最大速度。
    float minSpan = 0.6f; // 矢を生成する最小間隔。
    private TimerManager timerManager; // タイマーマネージャーへの参照。
    private float totalElapsedTime = 0f; // 経過した総時間。
    private bool generateArrows = false; // 矢を生成するかどうかのフラグ。

    public static arrowgenerator instance; // arrowgeneratorのシングルトンインスタンス。

    // 最初のフレームの更新前に呼ばれるメソッド。
    private void Start()
    {
        // タイマーマネージャーのインスタンスを取得します。
        timerManager = TimerManager.instance;
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

    // 矢の生成を切り替えるメソッド。
    public void ToggleArrowGeneration(bool enable)
    {
        generateArrows = enable;
    }

    // シーンがアンロードされたときに呼ばれるメソッド。
    private void OnSceneUnloaded(Scene scene)
    {
        generateArrows = false; // 矢の生成を停止します。
    }

    // シーンがロードされたときに呼ばれるメソッド。
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 特定のシーンでショップパネルが開いていない場合に矢の生成を開始します。
        if (scene.name == "SampleScene" && !gameDirector.isShopPanelOpen)
        {
            ToggleArrowGeneration(true);
        }
    }

    // フレームごとに呼ばれるメソッド。
    void Update()
    {
        // 矢の生成が有効でない場合は何もしません。
        if (!generateArrows) return;

        // 経過時間を更新します。
        this.delta += Time.deltaTime;
        // 経過時間を正規化して、矢の生成間隔を計算します。
        float normalizedTime = Mathf.Clamp01(this.delta / maxSpeed);
        this.span = Mathf.Lerp(minSpan, maxSpeed, normalizedTime);

        // 生成間隔を超えた場合、新しい矢を生成します。
        if (this.delta > this.span)
        {
            this.delta = 0;

            // 矢を生成する位置をランダムに決定します。
            float px = Random.Range(-6f, 7f);
            float py = 7f;

            // 矢のプレハブから新しいオブジェクトを生成し、位置を設定します。
            GameObject go = Instantiate(arrowPrefab);
            go.transform.position = new Vector3(px, py, 0);
        }

        // 総経過時間を更新します。
        totalElapsedTime += Time.deltaTime;

        // 一定時間が経過したら、矢の速度と生成間隔を変更します。
        if (totalElapsedTime >= 1200f)
        {
            maxSpeed = 0.1f;
            minSpan = 0.3f;
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
