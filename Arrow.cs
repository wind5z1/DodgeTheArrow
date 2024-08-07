using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// このスクリプトは、プレイヤーと矢印との衝突を検出し、プレイヤーと矢印のパーティクルシステムをトリガーする役割を持っています。
public class NewBehaviourScript : MonoBehaviour
{
    GameObject player; // プレイヤーのゲームオブジェクト。
    public GameObject particleSystemPrefab; // 通常のパーティクルシステムのプレハブ。
    public GameObject shieldParticleSystemPrefab; // シールドがアクティブなときに使用するパーティクルシステムのプレハブ。
    private GameObject particleSystemInstance; // インスタンス化されたパーティクルシステム。
    public AudioClip hitSound; // 衝突時に再生する音声クリップ。
    private GameDirector gameDirector; // ゲームディレクターへの参照。

    // 最初のフレームの更新前に呼ばれるメソッド。
    void Start()
    {
        // プレイヤーとゲームディレクターの参照を取得します。
        this.player = GameObject.Find("player");
        this.gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    // フレームごとに呼ばれるメソッド。
    void Update()
    {
        // オブジェクトを下に移動させます。
        transform.Translate(0, -0.1f, 0);

        // オブジェクトが画面外に出たら破棄します。
        if (transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }

        // プレイヤーが存在しない場合は処理を行いません。
        if (player == null)
            return;

        // プレイヤーとの距離を計算します。
        Vector2 p1 = transform.position;
        Vector2 p2 = this.player.transform.position;
        Vector2 dir = p1 - p2;
        float d = dir.magnitude;
        float r1 = 0.5f; // このオブジェクトの半径。
        float r2 = 1.0f; // プレイヤーの半径。

        // 衝突判定。
        if (d < r1 + r2)
        {
            // シールドがアクティブな場合は、シールドのパーティクルシステムをトリガーします。
            if (gameDirector.scoreManager.isShieldActive)
            {
                TriggerParticleSystem(shieldParticleSystemPrefab);
            }
            else
            {
                // シールドがアクティブでない場合は、通常のパーティクルシステムをトリガーし、HPを減少させます。
                TriggerParticleSystem(particleSystemPrefab);
                gameDirector.DecreaseHp();
            }

            // 衝突後はオブジェクトを破棄します。
            Destroy(gameObject);
        }

        // ゲームオーバーの場合は、ゲームオーバーシーンに移行します。
        if (GameDirector.IsGameOver)
        {
            SceneManager.LoadScene("GG");
        }
    }

    // パーティクルシステムをトリガーするメソッド。
    private void TriggerParticleSystem(GameObject prefab)
    {
        // パーティクルシステムのプレハブが設定されている場合にのみ処理を行います。
        if (prefab != null)
        {
            // パーティクルシステムをプレイヤーの上にインスタンス化します。
            Vector3 particlePosition = player.transform.position + Vector3.up * 1.0f;
            particleSystemInstance = Instantiate(prefab, particlePosition, Quaternion.identity);
            particleSystemInstance.transform.parent = player.transform;
            ParticleSystem ps = particleSystemInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                // パーティクルシステムを再生します。
                ps.Play();
                var mainModule = ps.main;
                mainModule.loop = false;
            }
            // 衝突音が設定されている場合は、音を再生します。
            if (hitSound != null && player.GetComponent<AudioSource>() != null)
            {
                player.GetComponent<AudioSource>().PlayOneShot(hitSound);
            }
        }
    }
}
