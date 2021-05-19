using UnityEngine;

public class SecretSkin : MonoBehaviour
{
    private int _colorCounter = 0;
    private PlayerVirusHead _playerHead;

    public static Color[] Colors =
    {
        new Color(0.2196078f, 0.5801803f, 0.7921569f),
        new Color(0.2196078f, 0.4580126f, 0.7921569f),
        new Color(0.2196078f, 0.3138841f, 0.7921569f),
        new Color(0.3476579f, 0.2196078f, 0.7921569f),
        new Color(0.4486843f, 0.2196078f, 0.7921569f),
        new Color(0.5198268f, 0.2196078f, 0.7921569f),
        new Color(0.6072203f, 0.2196078f, 0.7921569f),
        new Color(0.6621267f, 0.2196078f, 0.7921569f),
        new Color(0.7545513f, 0.2196078f, 0.7921569f),
        new Color(0.7921569f, 0.2196078f, 0.6792645f),
        new Color(0.7921569f, 0.2196078f, 0.5342178f),
        new Color(0.7921569f, 0.2196078f, 0.3667734f),
        new Color(0.7924528f, 0.2613521f, 0.2205411f),
        new Color(0.7921569f, 0.3701333f, 0.2196078f),
        new Color(0.7921569f, 0.5061629f, 0.2196078f),
        new Color(0.7921569f, 0.5867476f, 0.2196078f),
        new Color(0.7921569f, 0.6744767f, 0.2196078f),
        new Color(0.7921569f, 0.7705702f, 0.2196078f),
        new Color(0.6826695f, 0.7921569f, 0.2196078f),
        new Color(0.5660676f, 0.7921569f, 0.2196078f),
        new Color(0.3732419f, 0.7921569f, 0.2196078f),
        new Color(0.2196078f, 0.7921569f, 0.3788739f),
        new Color(0.2196078f, 0.7921569f, 0.6102525f),
        new Color(0.2196078f, 0.7921569f, 0.7529706f),
        new Color(0.2196078f, 0.7041147f, 0.7921569f)
    };

    private void Awake()
    {
        _playerHead = GetComponent<PlayerVirusHead>();
        gameObject.GetComponent<SpriteRenderer>().color = Colors[_colorCounter];
        _playerHead.OnNewTail += ChangeTailColor;
    }

    private void ChangeTailColor()
    {
        if (_colorCounter > Colors.Length - 1)
            _colorCounter = 0;

        _playerHead.LastTail.GetComponent<SpriteRenderer>().color = Colors[_colorCounter++];
    }
}
