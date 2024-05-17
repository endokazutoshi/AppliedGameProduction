using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed; // プレイヤーの移動スピード
    private Vector2 targetPos; // プレイヤーの目標位置

    [SerializeField] private Vector2 initialPosition = new Vector2(0, 0); // プレイヤーの初期位置
    [SerializeField] private Vector2 minPosition = new Vector2(0, 0); // 最小の移動可能な範囲
    [SerializeField] private Vector2 maxPosition = new Vector2(0, 0); // 最大の移動可能な範囲

    private KeyCode currentKeyCode = KeyCode.None; // 現在押されているキー
    private bool isKeyDown = false; // キーが押されているかどうかのフラグ
    private float keyDownTime = 0f; // キーが押された時間

    private void Start()
    {
        transform.position = initialPosition;
        targetPos = initialPosition;
    }

    void Update()
    {
        Vector2 move = Vector2.zero;

        // キーが押されているかどうかを判定
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (!isKeyDown)
            {
                // キーが初めて押されたときの処理
                isKeyDown = true;
                keyDownTime = Time.time;
            }

            // キーが押されている間に経過した時間を計算
            float elapsedTime = Time.time - keyDownTime;
            if (elapsedTime > 0.1f) // 0.1秒以上経過していたらキーが長押しとみなす
            {
                // キーが長押しされている場合の処理
                if (Input.GetKey(KeyCode.W) && targetPos.y < maxPosition.y && currentKeyCode == KeyCode.None)
                {
                    move.y = 1;
                    currentKeyCode = KeyCode.W;
                }
                else if (Input.GetKey(KeyCode.S) && targetPos.y > minPosition.y && currentKeyCode == KeyCode.None)
                {
                    move.y = -1;
                    currentKeyCode = KeyCode.S;
                }

                if (Input.GetKey(KeyCode.A) && targetPos.x > minPosition.x && currentKeyCode == KeyCode.None)
                {
                    move.x = -1;
                    currentKeyCode = KeyCode.A;
                }
                else if (Input.GetKey(KeyCode.D) && targetPos.x < maxPosition.x && currentKeyCode == KeyCode.None)
                {
                    move.x = 1;
                    currentKeyCode = KeyCode.D;
                }
            }
        }
        else
        {
            // キーが離されたときの処理
            isKeyDown = false;
            currentKeyCode = KeyCode.None;
        }

        if (move != Vector2.zero)
        {
            targetPos += move;
        }

        Move(targetPos);

        // 現在のプレイヤーの位置をデバッグログで表示
        Debug.Log("Player's position: " + targetPos);
    }

    private void Move(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards((Vector2)transform.position, targetPosition,
            _speed * Time.deltaTime);
    }
}
