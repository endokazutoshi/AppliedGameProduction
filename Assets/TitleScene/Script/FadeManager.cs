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
        // Imageコンポーネントを取得
        fade = GetComponent<Image>();

        // フェードインにかかる時間と色の間隔を設定
        wait_time = fade_time / loop_count;
        alpha_interval = 1.0f / loop_count; // 255.0f / loop_count -> 1.0f / loop_count (0-1 range for alpha)
    }

    void Update()
    {
        // EnterキーかSpaceキーが押されたらフェードイン・フェードアウト開始
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeInOut());
        }
    }

    IEnumerator FadeInOut()
    {
        yield return StartCoroutine(Color_FadeIn());
        yield return new WaitForSeconds(2.0f); // 2秒待つ
        yield return StartCoroutine(Color_FadeOut());
        SceneManager.LoadScene("StageSelectScene"); // 次のシーンに移動 (シーン名を適宜変更)
    }

    IEnumerator Color_FadeIn()
    {
        // フェード元の色を設定 (黒)
        fade.color = new Color(0, 0, 0, 1);

        // 色を徐々に変えるループ
        for (float alpha = 1.0f; alpha >= 0.0f; alpha -= alpha_interval)
        {
            // 待ち時間
            yield return new WaitForSeconds(wait_time);

            // Alpha値を少しづつ下げる
            Color new_color = fade.color;
            new_color.a = alpha;
            fade.color = new_color;
        }
    }

    IEnumerator Color_FadeOut()
    {
        // フェード元の色を設定 (透明)
        fade.color = new Color(0, 0, 0, 0);

        // 色を徐々に変えるループ
        for (float alpha = 0.0f; alpha <= 1.0f; alpha += alpha_interval)
        {
            // 待ち時間
            yield return new WaitForSeconds(wait_time);

            // Alpha値を少しづつ上げる
            Color new_color = fade.color;
            new_color.a = alpha;
            fade.color = new_color;
        }
    }
}
