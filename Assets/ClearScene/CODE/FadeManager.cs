using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyNamespace
{
    public class FadeManager : MonoBehaviour
    {
        public string nextSceneName; // �J�ڐ�̃V�[������Inspector����ݒ�

        private Image fadeImage;

        void Start()
        {
            // Image�R���|�[�l���g���擾
            fadeImage = GetComponent<Image>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(StartFadeSequence());
            }
        }

        IEnumerator StartFadeSequence()
        {
            // 2�b�҂�
            yield return new WaitForSeconds(3.0f);

            // �t�F�[�h�A�E�g���J�n
            yield return StartCoroutine(Color_FadeOut());

            // �V�[�������[�h
            SceneManager.LoadScene(nextSceneName);

            // �t�F�[�h�C�����J�n
            yield return StartCoroutine(Color_FadeIn());
        }

        IEnumerator Color_FadeOut()
        {
            if (fadeImage == null) yield break;

            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f); // ���F�A�s����

            const float fade_time = 4.0f; // �t�F�[�h�A�E�g�ɂ����鎞��
            float elapsed_time = 0f;

            while (elapsed_time < fade_time)
            {
                elapsed_time += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsed_time / fade_time); // 1����0�܂ł͈̔͂Ōv�Z
                fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                yield return null; // ���̃t���[���܂őҋ@
            }
        }

        IEnumerator Color_FadeIn()
        {
            if (fadeImage == null) yield break;

            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f); // ���F�A����

            const float fade_time = 4.0f; // �t�F�[�h�C���ɂ����鎞��
            float elapsed_time = 0f;

            while (elapsed_time < fade_time)
            {
                elapsed_time += Time.deltaTime;
                float alpha = Mathf.Clamp01(1.0f - (elapsed_time / fade_time)); // 0����1�܂ł͈̔͂Ōv�Z
                fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                yield return null; // ���̃t���[���܂őҋ@
            }
        }
    }

}