using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    public string stringId;
#if UNITY_EDITOR
    public Languages editorLang;
#endif
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (Application.isPlaying)
        {
            OnChangeLanguage(Variables.currentLanguage);
        }
        else
        {
#if UNITY_EDITOR
            OnChangeLanguage(editorLang);
#endif
        }
    }

    public void OnChangeLanguage(Languages lang)
    {
        var stringTableId = DataTableIds.String[(int)lang];
        var stringTable = DataTableManager.Get<StringTable>(stringTableId);
        text.text = stringTable.Get(stringId).Replace("\\n", "\n");
    }
}