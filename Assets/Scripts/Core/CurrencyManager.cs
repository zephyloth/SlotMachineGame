using System.Collections;
using UnityEngine;

public class CurrencyManager : SingletonBehaviour<CurrencyManager>
{
    [SerializeField]
    private float CoinEffectDuration;

    [SerializeField]
    private int CoinEffectStep;

    public int FinalCoinAmount{ get; private set; }

    private int currentCoinAmount;
    public int CurrentCoinAmount
    {
        get => currentCoinAmount;
        set 
        {
            currentCoinAmount = value;
            UIManager.UpdateCurrencyInfo();
        }
    }

    private UIManager UIManager;
    private SaveManager SaveManager;

    protected override bool Init()
    {
        if (base.Init()) return true;

        UIManager = UIManager.Instance as UIManager;
        SaveManager = SaveManager.Instance as SaveManager;

        FinalCoinAmount = SaveManager.SaveData.CoinCurrency;
        CurrentCoinAmount = FinalCoinAmount;

        return true;
    }

    public IEnumerator CollectCoins(SymbolData[] SymbolDataArray)
    {
        FinalCoinAmount += SymbolDataArray[0].CoinValue + SymbolDataArray[1].CoinValue + SymbolDataArray[2].CoinValue;
        yield return ParticleManager.CollectEffect(FinalCoinAmount, CurrentCoinAmount, CoinEffectDuration, CoinEffectStep, OnCoinCollectedTick);
    }

    private void OnCoinCollectedTick(int Amount)
    {
        CurrentCoinAmount = Amount;      
    }
}
