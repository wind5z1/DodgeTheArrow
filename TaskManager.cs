using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// タスクの状態を管理し、表示するクラスです。
public class TaskManager : MonoBehaviour
{
    public TMP_Text taskStatusText; // タスクの状態を表示するテキスト。

    // ゲーム開始時に一度だけ呼ばれるメソッド。
    private void Start()
    {
        UpdateTaskStatusDisplay(); // タスクの状態を表示します。
    }

    // タスクの状態を更新して表示するメソッド。
    public void UpdateTaskStatusDisplay()
    {
        // TaskSystemが存在する場合、タスクの状態を更新します。
        if (TaskSystem.instance != null)
        {
            taskStatusText.text = ""; // テキストを初期化します。

            // すべてのタスクについて状態を表示します。
            foreach (var task in TaskSystem.instance.GetTasks())
            {
                string status = task.isComplete ? "Completed" : "In Progress"; // タスクの完了状態に応じてテキストを設定します。
                taskStatusText.text += $"{task.description} - {status}\n"; // タスクの説明と状態をテキストに追加します。
            }
        }
    }
}
