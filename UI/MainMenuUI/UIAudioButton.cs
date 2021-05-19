using UnityEngine;
using UnityEngine.UI;

public class UIAudioButton : MonoBehaviour
{
    public Image enabledInd;
    private bool _soundsEnabled;

    private void Awake()
    {
        if (!PersistentData.AudioSettigns.HasKey())
            PersistentData.AudioSettigns.SetState(true);

        _soundsEnabled = PersistentData.AudioSettigns.IsEnabled();
        enabledInd.enabled = !_soundsEnabled;
    }

    public void ButtonClick()
    {
        _soundsEnabled = !_soundsEnabled;

        PersistentData.AudioSettigns.SetState(_soundsEnabled);
        PersistentData.Save();

        enabledInd.enabled = !_soundsEnabled;
    }
}
