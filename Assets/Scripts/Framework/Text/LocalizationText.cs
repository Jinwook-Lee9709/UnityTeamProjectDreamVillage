using TMPro;
using UnityEngine;

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
    }

    private void OnEnable()
    {
        if (Application.isPlaying)
        {
            OnChangeLanguage(Variables.currentLanguage);    
        }
        else
        {
            OnChangeLanguage(editorLang);
        }
    }

    public void OnChangeLanguage(Languages lang)
    {
        var stringTableId = DataTableIds.String[(int)lang];
        var stringTable = DataTableManager.Get<StringTable>(stringTableId);
        text.text = stringTable.Get(stringId);
    }
}
