using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//ゲーム主要な制御を担当するクラスです
public class GameDirector : MonoBehaviour
{
    private Image hpGauge;//HpゲージのUIコンポーネント
    private float initialHpAmount;//HPゲージの初期値
    private bool isGameOver = false;//ゲームオーバー状態かどうか
    private Image playerImage;//プレイヤーのイメージコンポーネント
    private float immunityDuration = 5.0f;//無敵の持続時間
    private float immunityTimer = 0.0f;//無敵のタイマー
    private bool isPlayerImmune = false;//プレイヤーが無敵状態かどうか
    public TMP_Text immunityTimerText;//無敵タイマーを表示するテキスト
    public GameObject shopPanel;//ショップパネルのゲームオブジェクト
    public bool isShopPanelOpen = true;//ショップパネルが開いてるかどうか
    public ScoreManager scoreManager;//スクアマネージャーの参照
    public bool hasPurchasedItem = false;//アイテムを購入したかどうか
    public bool hasEnteredImmunity = false;//無敵に入ったかどうか

    //ゲームオーバー状態を取得するプロパティ
    public static bool IsGameOver
    {
        get { return instance.isGameOver; }
    }
    //GameDirectorのシングルトンインスタンス
    public static GameDirector instance;
    //オブジェクトが生成されたときに呼ばれるメソッド
    private void Awake()
    {
        //すでにインスタンスが存在する場合は破棄する
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            //シングルトンインスタンスを設定し、シーン遷移で破棄されないようにする
            instance = this;
            DontDestroyOnLoad(gameObject);
            //無敵タイマーをリセットする
            ResetImmunityTimer();
            //無敵タイマーのテキストを取得する。
            immunityTimerText = GameObject.Find("Immunity")?.GetComponent<TMP_Text>();
        }

    }

    //オブジェクトが有効になったときに呼ばれるメソッド
    private void OnEnable()
    {
        //シーンがロードされた時のイベントにメソッドを登録解除する
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //オブジェクトが無効になったときに呼ばれるメソッド
    private void OnDisable()
    {
        //シーンがロードされた時のイベントからメソッドを登録解除する。
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //最初のフレームの更新前に呼ばれるメソッド。
    void Start()
    {
        //ショップパネルが開いてる場合、それをアクティブにし、ゲームの時間を停止する。
        if (isShopPanelOpen)
        {
            shopPanel.SetActive(true);
            Time.timeScale = 0f;
            //HPゲージを初期化する
            InitializeHpGauge();
            //プレイヤーのイメージを取得する。
            playerImage = GameObject.Find("player")?.GetComponent<Image>();
            //その他のジェネレーターを無効にする
            arrowgenerator.instance.ToggleArrowGeneration(false);
            Meatgenerator.instance.ToggleMeatGenerator(false);
            Hpgenerator.instance.ToggleHpGenerator(false);
            fishgenerator.instance.ToggleFishGenerator(false);
        }
    }
    //ショップパネルを閉じるメソッド
    public void closeShopPanel()
    {
       isShopPanelOpen = false;
       shopPanel.SetActive(false);
       Time.timeScale = 1f;
        //その他のジェネレーターを有効にする。
       arrowgenerator.instance.ToggleArrowGeneration(true);
       Meatgenerator.instance.ToggleMeatGenerator(true);
       Hpgenerator.instance.ToggleHpGenerator(true);
       fishgenerator.instance.ToggleFishGenerator(true);
    }
    //アイテム購入を記録すろメソッド
    public void RecordPurchase()
    {
        hasPurchasedItem = true;
    }
    //HPゲージを初期化するメソッド
    private void InitializeHpGauge()
    {
        hpGauge = GameObject.Find("hpGauge")?.GetComponent<Image>();

        if (hpGauge != null)
        {
            initialHpAmount = hpGauge.fillAmount;
            SetHpGauge(initialHpAmount);
        }


    }

    //フレームごとに呼ばれるメソッド
    public void DecreaseHp()
    {
        Debug.Log("isShieldActive: " + scoreManager.isShieldActive);
        if (!IsGameOver && hpGauge != null)
        {
            if (!IsPlayerImmune())
            {
                if (!scoreManager.isShieldActive)
                {
                    hpGauge.GetComponent<Image>().fillAmount -= 0.1f;
                }
            }
 
            if (hpGauge.GetComponent<Image>().fillAmount <= 0f)
            {
                isGameOver = true;

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    //シーンがロードされたときに呼ばれるメソッド
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        shopPanel = GameObject.Find("ShopPanel");
        isGameOver = false;
        InitializeHpGauge();
        ResetImmunityTimer();
        immunityTimerText = GameObject.Find("Immunity")?.GetComponent<TMP_Text>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    //HPゲージの値を設定するメソッド
    private void SetHpGauge(float value, Image image = null)
    {
        if (image == null)
        {
            image = hpGauge;
        }

        if (image != null)
        {
            image.fillAmount = value;
        }
    }
    //無敵状態をアクテイブにするメソッド
    public void ActiveImmunity(float duration)
    {
        isPlayerImmune = true;
        StartCoroutine(ImmunityTimer(duration));
        hasEnteredImmunity = true;
    }
    //無敵タイマーを管理するコルーチン
    private IEnumerator ImmunityTimer(float duration)
    {

        if (playerImage != null)
        {
            playerImage.enabled = false;
        }

        immunityTimer = duration;
        while (immunityTimer > 0.0f)
        {
            UpdateImmunityTimerText();
            yield return new WaitForSeconds(duration);
            immunityTimer -= 0.1f;
        }

        immunityTimer = 0.0f;

        if (playerImage != null)
        {
            playerImage.enabled = true;
        }
        isPlayerImmune = false;
        UpdateImmunityTimerText();
    }
    //プレイヤーが無敵状態かどうかを返すメソッド
    public bool IsPlayerImmune()
    {
        return immunityTimer > 0.0f;
    }
    //無敵タイマーのテキストを更新するメソッド
    private void UpdateImmunityTimerText()
    {
        if (immunityTimerText != null && IsPlayerImmune())
        {
            immunityTimerText.text = "Immunity: " + immunityTimer.ToString("F1") + "s";
        }
        else if (immunityTimerText != null)
        {
            immunityTimerText.text = " ";
        }
    }
    //無敵タイマーをレセットするメソッド
    private void ResetImmunityTimer()
    {
        isPlayerImmune = false;
        immunityTimer = 0.0f;
        if (playerImage != null)
        {
            playerImage.enabled = true;
        }
        //無敵タイマーのテキストを更新する
        UpdateImmunityTimerText();
    }
    //HPを回復するメソッド
    public void RecoverHp(float amount)
    {
        //HPゲージの値が初期値を超えないようにする
        hpGauge.GetComponent<Image>().fillAmount += amount;

        hpGauge.GetComponent<Image>().fillAmount = Mathf.Min(hpGauge.GetComponent<Image>().fillAmount, initialHpAmount);
    }
    //毎フレーム呼ばれるメソッド
    private void Update()
    {
        //Bキーが押されたらショップパネルを閉じる。
        if (Input.GetKeyDown(KeyCode.B))
        {
            closeShopPanel();
        }
    }
}
