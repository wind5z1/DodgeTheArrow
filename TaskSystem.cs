using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// タスクシステムスクリプト
public class TaskSystem : MonoBehaviour
{
    // シングルトンインスタンス
    public static TaskSystem instance;
    // スコアマネージャーへの参照
    public ScoreManager scoreManager;
    // タスクリストを表示するテキスト
    public TMP_Text taskListText;
    // タスクリスト
    private List<Task> tasks = new List<Task>();
    // 肉のカウント
    public int meatCount = 0;
    // タスク5と6が完了したかどうかのフラグ
    public static bool task5Completed = false;
    public static bool task6Completed = false;
    // タスク完了テキストが表示されたかどうかのフラグ
    public bool hasShownTaskCompleteText = false;

    // シングルトンパターンの実装
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // シーンがロードされたときのイベントハンドラを登録
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // シーンがアンロードされたときのイベントハンドラを解除
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンがロードされたときに実行されるメソッド
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // タスクリストテキストの参照を取得
        taskListText = GameObject.Find("TaskText").GetComponent<TMP_Text>();
        // スコアマネージャーの参照を取得
        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        if (scoreManagerObj != null)
        {
            scoreManager = scoreManagerObj.GetComponent<ScoreManager>();
        }
    }

    // タスククラス
    public class Task
    {
        public string id; // タスクID
        public string description; // タスクの説明
        public bool isComplete; // タスクが完了したかどうか
        public bool hasBeenShown; // タスク完了テキストが表示されたかどうか

        // コンストラクタ
        public Task(string id, string description)
        {
            this.id = id;
            this.description = description;
            isComplete = false;
            hasBeenShown = false;
        }
    }

    // 初期化メソッド
    void Start()
    {
        // タスクを追加
        AddTask("task_1", "Collect 10 meat.");
        AddTask("task_2", "Reach 3000 point.");
        AddTask("task_3", "Buy an item in the shop.");
        AddTask("task_4", "Collect 10000 coin in a game.");
        AddTask("task_5", "Win the game without buying any item.");
        AddTask("task_6", "Win the game without enter immune.");
    }

    // タスクを追加するメソッド
    public void AddTask(string id, string description)
    {
        // 新しいタスクを作成し、リストに追加
        Task newTask = new Task(id, description);
        tasks.Add(newTask);
    }

    // タスクを完了するメソッド
    public void CompleteTask(string id)
    {
        // タスクリストをループして、指定されたIDのタスクを探す
        foreach (Task task in tasks)
        {
            // タスクが見つかり、まだ完了していない場合
            if (task.id == id && !task.isComplete)
            {
                // タスクを完了に設定
                task.isComplete = true;

                // タスク完了テキストがまだ表示されていない場合
                if (!task.hasBeenShown)
                {
                    // タスクリストテキストを更新
                    UpdateTaskListText(task);
                    // タスク完了テキストを表示済みに設定
                    task.hasBeenShown = true;
                }
                // タスク5または6が完了した場合、対応するフラグを設定
                if (id == "task_5")
                {
                    task5Completed = true;
                }
                if (id == "task_6")
                {
                    task6Completed = true;
                }
                break;
            }
        }
    }

    // タスクリストテキストを更新するメソッド
    void UpdateTaskListText(Task taskToShow)
    {
        // タスクが完了し、まだ表示されていない場合
        if (taskToShow.isComplete && !taskToShow.hasBeenShown)
        {
            // タスクの説明と" - Completed"を追加
            taskListText.text += $"{taskToShow.description} - Completed\n";
            // タスクリストテキストを表示
            taskListText.gameObject.SetActive(true);
            // 3秒後にタスクリストテキストを非表示にするコルーチンを開始
            StartCoroutine(HideTaskListTextAfterDelay(3));
            // タスク完了テキストを表示済みに設定
            taskToShow.hasBeenShown = true;
        }

    }

    // 一定時間後にタスクリストテキストを非表示にするコルーチン
    private IEnumerator HideTaskListTextAfterDelay(float delay)
    {
        // 指定された遅延時間を待つ
        yield return new WaitForSeconds(delay);
        // タスクリストテキストを非表示にする
        taskListText.gameObject.SetActive(false);
    }

    // 肉を収集するメソッド
    public void CollectMeat()
    {
        // 肉のカウントを増やす
        meatCount++;
        // 肉のタスクをチェック
        CheckMeatTask();
    }

    // 肉のタスクをチェックするメソッド
    private void CheckMeatTask()
    {
        // 肉のタスクを探す
        Task meatTask = tasks.Find(t => t.id == "task_1");
        // 肉のカウントが10以上で、タスクが見つかり、まだ完了していない場合
        if (meatCount >= 10 && meatTask != null && !meatTask.isComplete)
        {
            // タスクを完了に設定
            meatTask.isComplete = true;
            // タスク完了テキストがまだ表示されていない場合
            if (!meatTask.hasBeenShown)
            {
                // タスクリストテキストを表示
                taskListText.gameObject.SetActive(true);
                // タスクリストテキストを更新
                UpdateTaskListText(meatTask);
                // タスク完了テキストを表示済みに設定
                meatTask.hasBeenShown = true;
            }
        }
    }

    // スコアのタスクをチェックするメソッド
    public void CheckScoreTask(int score)
    {
        // スコアが3000以上の場合
        if (score >= 3000)
        {
            // タスクリストテキストを表示
            taskListText.gameObject.SetActive(true);
            // タスク2を完了
            CompleteTask("task_2");
        }
    }

    // 完了したタスクのテキストを取得するメソッド
    public string GetCompletedTasksText()
    {
        // 完了したタスクのテキストを返す
        return taskListText.text;
    }

    // タスクリストを取得するメソッド
    public List<Task> GetTasks()
    {
        // タスクリストを返す
        return tasks;
    }

    // 特定のタスクをリセットするメソッド
    public void ResetSpecificTasks()
    {
        // タスク5と6を探す
        Task task5 = tasks.Find(t => t.id == "task_5");
        Task task6 = tasks.Find(t => t.id == "task_6");

        // タスク5が見つかり、まだ完了していない場合
        if (task5 != null && !task5.isComplete)
        {
            // タスク完了テキストを表示済みに設定
            task5.hasBeenShown = true;
        }
        // タスク6が見つかり、まだ完了していない場合
        if (task6 != null && !task6.isComplete)
        {
            // タスク完了テキストを表示済みに設定
            task6.hasBeenShown = true;
        }
    }
}
