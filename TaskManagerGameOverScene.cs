using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//GameOverSceneに遷移する時にあそこのタスクのテキストを更新するためのスクリプト
public class TaskManagerGameOverScene : MonoBehaviour
{
    public TMP_Text taskCompleteText;//タスク５のテキストのオブジェクト
    public TMP_Text taskCompleteText2;//タスク6のテキストのオブジェクト
    public float displayDuration = 3f;//テキストの表示時間

    // Startは最初のフレームの更新前に呼び出されます
    void Start()
    {
        // タスク5が完了し、まだタスク完了テキストが表示されていないかどうかを確認します
        if (TaskSystem.task5Completed && !TaskSystem.instance.hasShownTaskCompleteText)
        {
            taskCompleteText.text = "Win the game without buying any item. - Completed";
            StartCoroutine(DisplayTaskCompleteText());
        }

        // タスク6が完了し、まだタスク完了テキストが表示されていないかどうかを確認します
        if (TaskSystem.task6Completed && !TaskSystem.instance.hasShownTaskCompleteText)
        {
            taskCompleteText2.text = "Win the game without enter immune. - Completed";
            StartCoroutine(DisplayTaskCompleteText());
        }
    }

    // タスク完了テキストを表示するコルーチン
    IEnumerator DisplayTaskCompleteText()
    {
        taskCompleteText.gameObject.SetActive(true);
        taskCompleteText2.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        taskCompleteText.gameObject.SetActive(false);
        taskCompleteText2.gameObject.SetActive(false);
        TaskSystem.instance.hasShownTaskCompleteText = true;//タスクのテキストが1回出たら、次回以降に出ないようにする
    }
}
