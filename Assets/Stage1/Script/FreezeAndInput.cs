using UnityEngine;
using UnityEngine.UI;

public class FreezeAndInput : MonoBehaviour
{
    public GameObject inputPanel; // ���̓p�l��
    public InputField inputField; // ���̓t�B�[���h
    public Text feedbackText;     // �t�B�[�h�o�b�N�p�̃e�L�X�g
    [SerializeField] private string answerPanel; // �����̃p�X���[�h

    private bool isFrozen = false;

    void Start()
    {
        if (inputPanel == null)
        {
            Debug.LogError("inputPanel is not assigned in the inspector");
            return;
        }

        if (inputField == null)
        {
            Debug.LogError("inputField is not assigned in the inspector");
            return;
        }

        if (feedbackText == null)
        {
            Debug.LogError("feedbackText is not assigned in the inspector");
            return;
        }

        inputPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);

        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    void Update()
    {
        if (IsSpaceKeyPressedNearTileType3())
        {
            if (isFrozen)
            {
                Unfreeze();
            }
            else
            {
                Freeze();
            }
        }

        if (inputPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            CheckInput();
        }
    }

    public void Freeze()
    {
        Debug.Log("Freeze ���\�b�h���Ăяo����܂����B");

        GameObject[] players1 = GameObject.FindGameObjectsWithTag("Player1");
        foreach (var player in players1)
        {
            var component = player.GetComponent<Player>();
            if (component != null)
            {
                component.SetFrozen(true);
                Debug.Log($"Player '{player.name}' ���t���[�Y����܂����B");
            }
            else
            {
                Debug.LogWarning($"GameObject '{player.name}' with tag 'Player1' does not have a Player component.");
            }
        }

        GameObject[] players2 = GameObject.FindGameObjectsWithTag("Player2");
        foreach (var player in players2)
        {
            var component = player.GetComponent<Player>();
            if (component != null)
            {
                component.SetFrozen(true);
                Debug.Log($"Player '{player.name}' ���t���[�Y����܂����B");
            }
            else
            {
                Debug.LogWarning($"GameObject '{player.name}' with tag 'Player2' does not have a Player component.");
            }
        }

        inputPanel.SetActive(true);
        inputField.text = ""; // ���̓t�B�[���h���N���A
        feedbackText.gameObject.SetActive(false); // �t�B�[�h�o�b�N�e�L�X�g���\���ɂ���
        inputField.ActivateInputField(); // ���̓t�B�[���h���A�N�e�B�u�ɂ���
        isFrozen = true;
    }

    public void Unfreeze()
    {
        Debug.Log("Unfreeze ���\�b�h���Ăяo����܂����B");

        GameObject[] players1 = GameObject.FindGameObjectsWithTag("Player1");
        foreach (var player in players1)
        {
            var component = player.GetComponent<Player>();
            if (component != null)
            {
                component.SetFrozen(false); // �v���C���[�̃t���[�Y����������
                Debug.Log($"Player '{player.name}' ���A���t���[�Y����܂����B");
            }
            else
            {
                Debug.LogWarning($"GameObject '{player.name}' with tag 'Player1' does not have a Player component.");
            }
        }

        GameObject[] players2 = GameObject.FindGameObjectsWithTag("Player2");
        foreach (var player in players2)
        {
            var component = player.GetComponent<Player>();
            if (component != null)
            {
                component.SetFrozen(false); // �v���C���[�̃t���[�Y����������
                Debug.Log($"Player '{player.name}' ���A���t���[�Y����܂����B");
            }
            else
            {
                Debug.LogWarning($"GameObject '{player.name}' with tag 'Player2' does not have a Player component.");
            }
        }

        inputPanel.SetActive(false);
        isFrozen = false;
    }

    public bool IsPasswordCorrect()
    {
        return inputField.text == answerPanel;
    }

    void OnEndEdit(string inputText)
    {
        Debug.Log("OnEndEdit called with input: " + inputText);
    }

    void CheckInput()
    {
        string inputText = inputField.text;
        feedbackText.text = "ESC�Ŗ߂�";
        if (inputText == answerPanel)
        {
            Unfreeze();
        }
        else
        {
            feedbackText.text = "�ԈႢ�ł�";
            Debug.Log("�s����! ������x�����Ă��������B");
        }

        feedbackText.gameObject.SetActive(true);
        inputField.ActivateInputField();
    }

    private bool IsSpaceKeyPressedNearTileType3()
    {
        Vector3 playerPosition = transform.position;

        for (int xOffset = -1; xOffset <= 1; xOffset++)
        {
            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                Vector3 tilePosition = playerPosition + new Vector3(xOffset, yOffset, 0);

                if (IsTileType3(tilePosition))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsTileType3(Vector3 position)
    {
        Ground ground = FindObjectOfType<Ground>();
        if (ground != null)
        {
            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);
            if (x >= 0 && x < ground.Width && y >= 0 && y < ground.Length)
            {
                int tileType = ground.GetTileTypeAtPosition(position);
                return tileType == 3;
            }
        }
        return false;
    }

    public void CloseFreezeScreen()
    {
        inputPanel.SetActive(false);
        feedbackText.gameObject.SetActive(false);
        Unfreeze();
        Debug.Log("�t���[�Y��ʂ���܂����B");
    }
}
