using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Meatオブジェクトの挙動を管理するクラスです。
public class Meat : MonoBehaviour
{
    GameObject player; // プレイヤーオブジェクトへの参照。

    public GameObject particleSystemPrefab; // パーティクルシステムのプレハブ。
    private GameObject particleSystemInstance; // インスタンス化されたパーティクルシステム。
    private float remainingImmunityDuration = 1.0f; // 残りの無敵時間。
    public AudioClip pickUpSound; // ピックアップ時のサウンドクリップ。

    // 最初のフレームの更新前に呼ばれるメソッド。
    void Start()
    {
        // プレイヤーオブジェクトを検索して参照を取得します。
        this.player = GameObject.Find("player");
    }

    // フレームごとに呼ばれるメソッド。
    void Update()
    {
        // アイテムを下に移動させます。
        transform.Translate(0, -0.1f, 0);

        // アイテムが画面外に出たら破棄します。
        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }

        // プレイヤーオブジェクトが存在しない場合は処理を行いません。
        if (player == null)
            return;

        // プレイヤーとの距離を計算します。
        Vector2 p1 = transform.position;
        Vector2 p2 = this.player.transform.position;
        Vector2 dir = p1 - p2;
        float d = dir.magnitude;
        float r1 = 0.5f; // アイテムの半径。
        float r2 = 1.0f; // プレイヤーの半径。

        // プレイヤーとの衝突を検出します。
        if (d < r1 + r2)
        {
            // タスクシステムに肉を集めたことを通知します。
            TaskSystem.instance.CollectMeat();
            // ゲームディレクターから無敵状態をアクティブにするメソッドを呼び出します。
            GameDirector gameDirector = GameObject.Find("GameDirector")?.GetComponent<GameDirector>();
            if (gameDirector != null)
            {
                // プレイヤーが無敵状態でない場合、無敵時間を設定します。
                if (!gameDirector.IsPlayerImmune())
                {
                    remainingImmunityDuration = 1.0f;
                    gameDirector.ActiveImmunity(remainingImmunityDuration);

                    // パーティクルシステムが設定されている場合は、それをインスタンス化して再生します。
                    if (particleSystemPrefab != null)
                    {
                        Vector3 particlePosition = player.transform.position - Vector3.up * 1.0f;
                        particleSystemInstance = Instantiate(particleSystemPrefab, particlePosition, Quaternion.identity);

                        particleSystemInstance.transform.parent = player.transform;
                        ParticleSystem ps = particleSystemInstance.GetComponent<ParticleSystem>();
                        if (ps != null)
                        {
                            ps.Play();
                            var mainModule = ps.main;
                            mainModule.loop = false;
                        }
                    }
                    // ピックアップサウンドが設定されている場合は、それを再生します。
                    if (pickUpSound != null && player.GetComponent<AudioSource>() != null)
                    {
                        player.GetComponent<AudioSource>().PlayOneShot(pickUpSound);
                    }
                }
                // 衝突後はアイテムオブジェクトを破棄します。
                Destroy(gameObject);
            }
        }
    }
}
