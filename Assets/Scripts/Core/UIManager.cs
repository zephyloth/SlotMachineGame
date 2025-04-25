using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    //Reference UI side currency popups
    [SerializeField]
    private UICurrencyPopup[] UICurrencyPopupArray;

    //Reference coin text upon the slot
    [SerializeField]
    private TextMeshProUGUI UICurrencyInfoText;

    private CurrencyManager CurrencyManager;
    private AudioManager AudioManager;

    protected override bool Init()
    {
        if (base.Init()) return true;

        CurrencyManager = CurrencyManager.Instance as CurrencyManager;
        AudioManager = AudioManager.Instance as AudioManager;

        return true;
    }
 
    public IEnumerator ShowCurrencyPopup(int Index, int Value)
    {
        AudioManager.PlayCurrencyPopupSound();
        yield return UICurrencyPopupArray[Index].Show(Value.ToString());
    }

    public IEnumerator HideCurrencyPopup(int Index)
    {
        yield return UICurrencyPopupArray[Index].Hide();
    }

    public void UpdateCurrencyInfo()
    {
        UICurrencyInfoText.text = CurrencyManager.CurrentCoinAmount.ToString();
    }
}
