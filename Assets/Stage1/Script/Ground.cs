using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _tilePrefab;

    const int _nLengthStart = 1;
    const int _nWidthStart = 1;
    const int _nLengthEnd = 22;
    const int _nWidthEnd = 9;
    const int _nCenter = 11;

    [SerializeField] private int _length;
    [SerializeField] private int _width;
    private void Start()
    {
        for (int length = 0; length < _length; length++)
        {
            for (int width = 0; width < _width; width++)
            {
                if (length >= _nLengthStart && length < _nLengthEnd && width >= _nWidthStart && width < _nWidthEnd) // ‰¡‚ÌÀ•W‚ª0‚©‚ç22‚Ì”ÍˆÍ‚Ìê‡
                {
                    Instantiate(_wallPrefab, new Vector3(length, width, 0), Quaternion.identity, transform);
                }
                else // ‚»‚êˆÈŠO‚Ìê‡
                {
                    Instantiate(_tilePrefab, new Vector3(length, width, 0), Quaternion.identity, transform);

                }
                if (length == _nCenter)
                {
                    Instantiate(_tilePrefab, new Vector3(length, width, 0), Quaternion.identity, transform);
                }

            }
        }
    }
}