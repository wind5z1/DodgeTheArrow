using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

//スクア管理を担当するクラスです
public class ScoreManager : MonoBehaviour
{
    public int score;//現在のスクア
    public int scoreAfterBroughtItem = 6000;//アイテムを購入後のスコア
    public Text scoreText;//スクアを表示するテキスト
    public GameObject shieldObject;//シールドがアクテイブかどうか
    public bool isShieldActive = false;//シールドがアクテイブかどうか
    public GameObject loadingUI;//ローディングUI
    public GameDirector gameDirector;//ゲームディレクターへの参照
    public TaskSystem taskSystem;//タスクシステムへの参照
    public TMP_Text loadingTaskText;//ローディング中のタスクテキスト
    public Slider loadingProgressBar;//ローディングブログレスバー
    GetData data;//ハイスコアデータ

    void Start()
    {
        //必要なコンポネントを取得して初期化します
        this.scoreText = GameObject.Find("Text").GetComponent<Text>();
        taskSystem = FindObjectOfType<TaskSystem>();
        this.loadingTaskText = GameObject.Find("TaskText").GetComponent<TMP_Text>();
        gameDirector = FindObjectOfType<GameDirector>();
        UpdateScoreText();
        OnTapGet();
        isShieldActive = false;
    }
    //スコアを追加するメソッド
    public void AddScore(int points)
    {
        score += points;
        if(score >= 3000)
        {
            TaskSystem.instance.CompleteTask("task_2");
        }
        UpdateScoreText();
        //スコアが10000に達したら、新しいシーンをロードします。
        if(score >= 10000)
        {
            loadingUI.SetActive(true);
            StartCoroutine(LoadTargetScene());
        }
        //特定の条件を満たした場合、タスクを完了します
       if(score >= 10000)
        {
            if (!gameDirector.hasPurchasedItem)
            {
                TaskSystem.instance.CompleteTask("task_5");
            }

            if (!gameDirector.hasEnteredImmunity)
            {
                TaskSystem.instance.CompleteTask("task_6");
            }
            loadingUI.SetActive(true);
            StartCoroutine(LoadTargetScene());
        }
    }
    //新しいシーンを非同期でロードするコルーチン
    private IEnumerator LoadTargetScene()
    {
        loadingTaskText.text = taskSystem.GetCompletedTasksText();
        loadingTaskText.gameObject.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync("gameoverscene2");

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 1.0f);
            loadingProgressBar.value = progress;

            yield return null;
        }
        loadingUI.SetActive(false);
    }
    //スクアテキストを更新するメソッド
    public void UpdateScoreText()
    {
        this.scoreText.text = "HighScore: " + score.ToString();
    }

    //サーバーからスコアデータを取得するメソッド
    public void OnTapGet()
    {
        StartCoroutine(SendGetMessage("http://localhost:8088/score"));
    }
    //サーバーからGETリクエストを送信するコルーチン
    IEnumerator SendGetMessage(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if(uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(uwr.error);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);

            data = JsonUtility.FromJson<GetData>(uwr.downloadHandler.text);
        }
    }
    //シールドをアクティブにするメソッド
    public void ActiveShield()
    {
        if(!isShieldActive && score < 2000)
        {
            shieldObject.SetActive(true);
            isShieldActive = true;
            shieldObject.GetComponent<AudioSource>().Play();
        }
    }
    //毎フレーム呼ばれるメソッド
    private void Update()
    {
        //スクアが２０００に達したら、シールドを非アクティブします
        if(isShieldActive && score >= 2000)
        {
            shieldObject.SetActive(false);
            isShieldActive = false;
        }
    }

}
//サーバーから取得したハイスコアデータを格納するクラス
public class GetData
{
    public Data[] highscore;
}
//個々のハイスコアデータを格納するクラス
public class Data
{
    public int score;
    public string user;
}
