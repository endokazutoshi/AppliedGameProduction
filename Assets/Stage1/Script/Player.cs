using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed; // �v���C���[�̈ړ��X�s�[�h
    private Vector2 targetPos; // �v���C���[�̖ڕW�ʒu

    [SerializeField] private Vector2 initialPosition = new Vector2(0, 0); // �v���C���[�̏����ʒu
    [SerializeField] private Vector2 minPosition = new Vector2(0, 0); // �ŏ��̈ړ��\�Ȕ͈�
    [SerializeField] private Vector2 maxPosition = new Vector2(0, 0); // �ő�̈ړ��\�Ȕ͈�

    private KeyCode currentKeyCode = KeyCode.None; // ���݉�����Ă���L�[
    private bool isKeyDown = false; // �L�[��������Ă��邩�ǂ����̃t���O
    private float keyDownTime = 0f; // �L�[�������ꂽ����

    private void Start()
    {
        transform.position = initialPosition;
        targetPos = initialPosition;
    }

    void Update()
    {
        Vector2 move = Vector2.zero;

        // �L�[��������Ă��邩�ǂ����𔻒�
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (!isKeyDown)
            {
                // �L�[�����߂ĉ����ꂽ�Ƃ��̏���
                isKeyDown = true;
                keyDownTime = Time.time;
            }

            // �L�[��������Ă���ԂɌo�߂������Ԃ��v�Z
            float elapsedTime = Time.time - keyDownTime;
            if (elapsedTime > 0.1f) // 0.1�b�ȏ�o�߂��Ă�����L�[���������Ƃ݂Ȃ�
            {
                // �L�[������������Ă���ꍇ�̏���
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
            // �L�[�������ꂽ�Ƃ��̏���
            isKeyDown = false;
            currentKeyCode = KeyCode.None;
        }

        if (move != Vector2.zero)
        {
            targetPos += move;
        }

        Move(targetPos);

        // ���݂̃v���C���[�̈ʒu���f�o�b�O���O�ŕ\��
        Debug.Log("Player's position: " + targetPos);
    }

    private void Move(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards((Vector2)transform.position, targetPosition,
            _speed * Time.deltaTime);
    }
}
