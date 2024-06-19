using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = 0.5f;
    private float time;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.color = GetTextColorAlpha(text.color);
    }

    Color GetTextColorAlpha(Color color)
    {
        time += Time.deltaTime * speed * 4.0f;
        color.a = Mathf.Sin(time);

        return color;
    }
}
