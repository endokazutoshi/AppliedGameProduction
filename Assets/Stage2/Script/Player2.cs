using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player2 : MonoBehaviour
{
    [SerializeField] private float _speed; // プレイヤーの移動スピード
    private Vector2 targetPos; // プレイヤーの目標位置
    [SerializeField] private Vector2 initialPosition = new Vector2(1, 1); // プレイヤーの初期位置
    private Vector2 minPosition = new Vector2(0, 0); // 最小の移動可能な範囲
    private Vector2 maxPosition = new Vector2(22, 9); // 最大の移動可能な範囲

    private KeyCode currentKeyCode = KeyCode.None; // 現在押されているキー
    private bool isKeyDown = false; // キーが押されているかどうかのフラグ
    private float keyDownTime = 0f; // キーが押された時間

    private bool goalReached = false; // ゴールに到達したかどうかのフラグ

    private bool isFrozen = false; // プレイヤーが凍結されているかどうかのフラグ

    [SerializeField] Vector2[] passwordPositionChange; // 変更したいマスの位置を指定
    [SerializeField] int[] passwordNewTileType; // 新しいタイルタイプを指定

    [SerializeField] Vector2[] LeverPositionChange;
    [SerializeField] int[] LeverNewTileType;
    [SerializeField] int[] LeverOriginalTileType;

    private bool leverOn = false; // レバーの状態（オンかオフか）

    [SerializeField] private GameObject leverOnPrefab; // レバーがオンのときのPrefab
    [SerializeField] private GameObject leverOffPrefab; // レバーがオフのときのPrefab
    private GameObject currentLeverPrefab; // 現在表示されているレバーのPrefab

    public void SetFrozen(bool frozen)
    {
        isFrozen = frozen;
    }

    private void Start()
    {
        transform.position = initialPosition;
        targetPos = initialPosition;
    }

    void Update()
    {
        if (goalReached || isFrozen) return;

        ProcessMovementInput();//Playerを動かす処理

        if (CheckSpaceKeyPressed())//スペースキーを押したら
        {
            ProcessSpaceKeyAction();
        }
        //ゴールシーン
        CheckGoalReached();
    }

    private bool CheckSpaceKeyPressed()//スペースキーの処理
    {
        if (Input.GetKeyDown(KeyCode.Space))//スペースキーを押したら...
        {
            int currentX = Mathf.RoundToInt(transform.position.x);
            int currentY = Mathf.RoundToInt(transform.position.y);

            Debug.Log("Current position: (" + currentX + ", " + currentY + ")");//Playerの現在地のデバック

            if (HasNearbyGimmickWall(currentX, currentY))
            {
                Debug.Log("Freeze and input triggered or nearby gimmick wall found.");
                FreezeAndInput freezeAndInput = FindObjectOfType<FreezeAndInput>();
                if (freezeAndInput != null)
                {
                    freezeAndInput.Freeze();
                    SetFrozen(true);
                    StartCoroutine(WaitAndUnfreeze(freezeAndInput));
                    return true;
                }
            }
            else if (HasNearbyGimmickLever(currentX, currentY))
            {
                // レバーの近くでSpaceキーが押された場合
                leverOn = !leverOn; // レバーの状態を切り替える
                Debug.Log("Lever toggled. Lever is now: " + (leverOn ? "ON" : "OFF"));

                if (leverOn)
                {
                    // レバーがオンの場合、タイルの変更を行う
                    ChangeTileAfterLeverOn();

                }
                else
                {
                    ChangeTileAfterLeverOff();

                }

                return true;
            }
        }
        return false;
    }


    private void ProcessSpaceKeyAction()//スペースキーのアクション
    {
        int currentX = Mathf.RoundToInt(transform.position.x);
        int currentY = Mathf.RoundToInt(transform.position.y);

        if (Ground2.map[currentY, currentX] == 3)
        {
            FreezeAndInput freezeAndInput = FindObjectOfType<FreezeAndInput>();
            if (freezeAndInput != null)
            {
                freezeAndInput.Freeze();
                SetFrozen(true);
                StartCoroutine(WaitAndUnfreeze(freezeAndInput));
            }
        }
    }

    private void ProcessMovementInput()
    {
        Vector2 move = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (!isKeyDown)
            {
                isKeyDown = true;
                keyDownTime = Time.time;
            }

            float keyElapsedTime = Time.time - keyDownTime;
            if (keyElapsedTime > 0.1f)
            {
                if (Input.GetKey(KeyCode.W) && currentKeyCode == KeyCode.None)
                {
                    move.y = 1;
                    currentKeyCode = KeyCode.W;
                }
                else if (Input.GetKey(KeyCode.S) && currentKeyCode == KeyCode.None)
                {
                    move.y = -1;
                    currentKeyCode = KeyCode.S;
                }
                else if (Input.GetKey(KeyCode.A) && currentKeyCode == KeyCode.None)
                {
                    move.x = -1;
                    currentKeyCode = KeyCode.A;
                }
                else if (Input.GetKey(KeyCode.D) && currentKeyCode == KeyCode.None)
                {
                    move.x = 1;
                    currentKeyCode = KeyCode.D;
                }
            }
        }
        else
        {
            isKeyDown = false;
            currentKeyCode = KeyCode.None;
        }

        if (move != Vector2.zero)
        {
            Vector2 newPos = targetPos + move;
            if (IsMoveValid(newPos))
            {
                targetPos = newPos;
            }
        }

        Move(targetPos);
    }

    private bool IsMoveValid(Vector2 newPos)
    {
        int x = Mathf.RoundToInt(newPos.x);
        int y = Mathf.RoundToInt(newPos.y);

        if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
        {
            return Ground2.map[y, x] == 1 || Ground2.map[y, x] == 2 || Ground2.map[y, x] == 4 || Ground2.map[y, x] == 6;
        }
        return false;
    }

    private void Move(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards((Vector2)transform.position, targetPosition, _speed * Time.deltaTime);
    }

    private void CheckGoalReached()
    {
        int targetX = Mathf.RoundToInt(targetPos.x);
        int targetY = Mathf.RoundToInt(targetPos.y);

        if (Mathf.Approximately(transform.position.x, targetPos.x) && Mathf.Approximately(transform.position.y, targetPos.y))
        {
            if (Ground2.map[targetY, targetX] == 2)
            {
                if (BothPlayersAtGoal())
                {
                    Debug.Log("Both players reached the goal. Loading ClearScene...");
                    SceneManager.LoadScene("ClearScene");
                }
            }

            if (Ground2.map[targetY, targetX] == 4)
            {
                Debug.Log("A player reached the death tile. Loading DeadScene...");
                SceneManager.LoadScene("GameOverScene");
            }
        }
    }

    private bool BothPlayersAtGoal()
    {
        GameObject[] player1Objects = GameObject.FindGameObjectsWithTag("Player1");
        GameObject[] player2Objects = GameObject.FindGameObjectsWithTag("Player2");

        bool player1AtGoal = false;
        bool player2AtGoal = false;

        foreach (GameObject p1 in player1Objects)
        {
            Player p1Script = p1.GetComponent<Player>();
            int playerX = Mathf.RoundToInt(p1Script.GetTargetPos().x);
            int playerY = Mathf.RoundToInt(p1Script.GetTargetPos().y);

            if (Ground2.map[playerY, playerX] == 2)
            {
                player1AtGoal = true;
                break;
            }
        }

        foreach (GameObject p2 in player2Objects)
        {
            Player p2Script = p2.GetComponent<Player>();
            int playerX = Mathf.RoundToInt(p2Script.GetTargetPos().x);
            int playerY = Mathf.RoundToInt(p2Script.GetTargetPos().y);

            if (Ground2.map[playerY, playerX] == 2)
            {
                player2AtGoal = true;
                break;
            }
        }

        return player1AtGoal && player2AtGoal;
    }



    private void CloseFreezeScreen()
    {
        FreezeAndInput freezeAndInput = FindObjectOfType<FreezeAndInput>();
        if (freezeAndInput != null)
        {
            freezeAndInput.Unfreeze();
        }
    }

    private bool HasNearbyGimmickWall(int x, int y)//暗証番号の近くにいるか判定
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < Ground2.map.GetLength(1) && ny >= 0 && ny < Ground2.map.GetLength(0))
                {
                    if (Ground2.map[ny, nx] == 3)
                    {
                        return true;
                    }


                }
            }
        }
        return false;
    }

    private bool HasNearbyGimmickLever(int x, int y)//レバーマスの近くにいるか判定
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;

                if (nx >= 0 && nx < Ground2.map.GetLength(1) && ny >= 0 && ny < Ground2.map.GetLength(0))
                {
                    if (Ground2.map[ny, nx] == 7)
                    {
                        return true;
                    }

                }
            }
        }
        return false;
    }

    public Vector2 GetTargetPos()
    {
        return targetPos;
    }



    private IEnumerator WaitAndUnfreeze(FreezeAndInput freezeAndInput)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("ESCキーが押されたため、操作画面に戻ります。");
                CloseFreezeScreen();
                SetFrozen(false);
                yield break;
            }

            if (freezeAndInput.IsPasswordCorrect())
            {
                freezeAndInput.Unfreeze();
                SetFrozen(false);
                ChangeTileAfterPasswordCorrect();
                Debug.Log("パスワードが正しいため、アンフリーズされました。ここには通りました。");
                break;
            }
            else
            {
                yield return null;
            }
        }
    }
    private void ChangeTileAfterPasswordCorrect()//暗証番号が入力されたら指定したTileTypeを変更する関数
    {
        Ground2 Ground2 = FindObjectOfType<Ground2>();
        if (Ground2 != null && passwordPositionChange.Length == passwordNewTileType.Length)
        {
            for (int i = 0; i < passwordPositionChange.Length; i++)
            {
                int x = (int)passwordPositionChange[i].x;
                int y = (int)passwordPositionChange[i].y;
                int newType = passwordNewTileType[i];
                Debug.Log("これは暗証番号入力がよかったら行われる処理なんだけどなぁ");
                Ground2.UpdateTileType(x, y, newType);
            }
        }
    }

    private void ChangeTileAfterLeverOn()
    {
        Ground2 Ground2 = FindObjectOfType<Ground2>();
        if (Ground2 != null && LeverPositionChange.Length == LeverNewTileType.Length)
        {
            for (int i = 0; i < LeverPositionChange.Length; i++)
            {
                int x = (int)LeverPositionChange[i].x;
                int y = (int)LeverPositionChange[i].y;
                int newType = LeverNewTileType[i];
                Ground2.UpdateTileType(x, y, newType);
            }
        }
        Ground2.DisplayLeverPrefab(true); // レバーがオンのPrefabを表示
    }

    private void ChangeTileAfterLeverOff()
    {
        Ground2 Ground2 = FindObjectOfType<Ground2>();
        if (Ground2 != null && LeverPositionChange.Length == LeverOriginalTileType.Length)
        {
            for (int i = 0; i < LeverPositionChange.Length; i++)
            {
                int x = (int)LeverPositionChange[i].x;
                int y = (int)LeverPositionChange[i].y;
                int originalType = LeverOriginalTileType[i];
                Ground2.UpdateTileType(x, y, originalType);
            }
        }
        Ground2.DisplayLeverPrefab(false); // レバーがオフのPrefabを表示
    }

}