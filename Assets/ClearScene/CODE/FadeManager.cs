using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MyNamespace
{
    public class FadeManager : MonoBehaviour
    {
        public string nextSceneName; // 遷移先のシーン名をInspectorから設定

        private Image fadeImage;

        void Start()
        {
            // Imageコンポーネントを取得
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
            // 2秒待つ
            yield return new WaitForSeconds(3.0f);

            // フェードアウトを開始
            yield return StartCoroutine(Color_FadeOut());

            // シーンをロード
            SceneManager.LoadScene(nextSceneName);

            // フェードインを開始
            yield return StartCoroutine(Color_FadeIn());
        }

        IEnumerator Color_FadeOut()
        {
            if (fadeImage == null) yield break;

            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f); // 黒色、不透明

            const float fade_time = 4.0f; // フェードアウトにかかる時間
            float elapsed_time = 0f;

            while (elapsed_time < fade_time)
            {
                elapsed_time += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsed_time / fade_time); // 1から0までの範囲で計算
                fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                yield return null; // 次のフレームまで待機
            }
        }

        IEnumerator Color_FadeIn()
        {
            if (fadeImage == null) yield break;

            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f); // 黒色、透明

            const float fade_time = 4.0f; // フェードインにかかる時間
            float elapsed_time = 0f;

            while (elapsed_time < fade_time)
            {
                elapsed_time += Time.deltaTime;
                float alpha = Mathf.Clamp01(1.0f - (elapsed_time / fade_time)); // 0から1までの範囲で計算
                fadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
                yield return null; // 次のフレームまで待機
            }
        }
    }

}