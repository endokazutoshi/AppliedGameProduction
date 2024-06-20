using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public string NextSceneName;

    private Image fadeImage;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FadeOutAndLoadScene());
        }
    }

    IEnumerator FadeOutAndLoadScene()
    {
        //2秒待つ
        yield return new WaitForSeconds(2.0f);

        //フェードイン開始
        yield return StartCoroutine(Color_FadeIn());

        //フェードアウト開始
        yield return StartCoroutine(Color_FadeOut());

        //シーンをロード
        SceneManager.LoadScene(NextSceneName);
    }

    IEnumerator Color_FadeIn()
    {
        fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);

        const float fade_time = 2.0f;
        const int loop_count = 50;
        float wait_time = fade_time / loop_count;
        float alpha_interval = 1.0f / loop_count;

        for(float alpha = 0.0f; alpha >= 1.0f; alpha -= alpha_interval)
        {
            yield return new WaitForSeconds(wait_time);
            Color new_color = fadeImage.color;
            new_color.a = alpha;
            fadeImage.color = new_color;
        }

    }

    IEnumerator Color_FadeOut()
    {
        fadeImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

        const float fade_time = 2.0f;
        const int loop_count = 50;
        float wait_time = fade_time / loop_count;
        float alpha_interval = 1.0f / loop_count;

        for(float alpha = 1.0f; alpha <= 0.0f; alpha += alpha_interval)
        {
            yield return new WaitForSeconds(wait_time);
            Color new_color = fadeImage.color;
            new_color.a = alpha;
            fadeImage.color = new_color;
        }
    }
}
