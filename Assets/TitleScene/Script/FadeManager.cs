using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    private Image fade;
    private const float fade_time = 2.0f;
    private const int loop_count = 10;
    private float wait_time;
    private float alpha_interval;

    void Start()
    {
        // Image�R���|�[�l���g���擾
        fade = GetComponent<Image>();

        // �t�F�[�h�C���ɂ����鎞�ԂƐF�̊Ԋu��ݒ�
        wait_time = fade_time / loop_count;
        alpha_interval = 1.0f / loop_count; // 255.0f / loop_count -> 1.0f / loop_count (0-1 range for alpha)
    }

    void Update()
    {
        // Enter�L�[��Space�L�[�������ꂽ��t�F�[�h�C���E�t�F�[�h�A�E�g�J�n
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeInOut());
        }
    }

    IEnumerator FadeInOut()
    {
        yield return StartCoroutine(Color_FadeIn());
        yield return new WaitForSeconds(2.0f); // 2�b�҂�
        yield return StartCoroutine(Color_FadeOut());
        SceneManager.LoadScene("StageSelectScene"); // ���̃V�[���Ɉړ� (�V�[������K�X�ύX)
    }

    IEnumerator Color_FadeIn()
    {
        // �t�F�[�h���̐F��ݒ� (��)
        fade.color = new Color(0, 0, 0, 1);

        // �F�����X�ɕς��郋�[�v
        for (float alpha = 1.0f; alpha >= 0.0f; alpha -= alpha_interval)
        {
            // �҂�����
            yield return new WaitForSeconds(wait_time);

            // Alpha�l�������Â�����
            Color new_color = fade.color;
            new_color.a = alpha;
            fade.color = new_color;
        }
    }

    IEnumerator Color_FadeOut()
    {
        // �t�F�[�h���̐F��ݒ� (����)
        fade.color = new Color(0, 0, 0, 0);

        // �F�����X�ɕς��郋�[�v
        for (float alpha = 0.0f; alpha <= 1.0f; alpha += alpha_interval)
        {
            // �҂�����
            yield return new WaitForSeconds(wait_time);

            // Alpha�l�������Âグ��
            Color new_color = fade.color;
            new_color.a = alpha;
            fade.color = new_color;
        }
    }
}
