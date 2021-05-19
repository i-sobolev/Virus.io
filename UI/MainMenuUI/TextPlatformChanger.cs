using UnityEngine;
using UnityEngine.UI;

public class TextPlatformChanger : MonoBehaviour
{
    private Text _textBox;
    [SerializeField] private string _androidText = string.Empty;
    [SerializeField] private string _iosText = string.Empty;

    private void Awake()
    {
        _textBox = GetComponent<Text>();

        _textBox.text = _androidText;

#if UNITY_IOS
        _textBox.text = _iosText;
#endif
    }
}