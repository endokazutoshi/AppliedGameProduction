using UnityEngine;

public class ImageWobble : MonoBehaviour
{
    public float wobbleAmount = 0.1f; // �h��̗�
    public float wobbleSpeed = 5.0f; // �h��̑��x

    private Vector3 originalPosition;
    private float time;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime * wobbleSpeed;

        float xOffset = Mathf.PerlinNoise(time, 0) * 2 - 1; // Perlin Noise���g���Ċ��炩�ȗh��𐶐�
        float yOffset = Mathf.PerlinNoise(0, time) * 2 - 1;

        Vector3 wobblePosition = new Vector3(xOffset, yOffset, 0) * wobbleAmount;
        transform.position = originalPosition + wobblePosition;
    }
}
