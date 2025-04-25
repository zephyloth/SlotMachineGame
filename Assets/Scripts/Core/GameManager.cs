using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    //Make gamesettings as readonly
    [SerializeField]
    private GameSettings gameSettings;
    public GameSettings GameSettings => gameSettings;

    //Take reference to the slotcontroller
    [SerializeField]
    private SlotController SlotController;

    //Button used to spin and get the result
    [SerializeField]
    private SpinButton SpinButton;

    //Declare singletons
    private CurrencyManager CurrencyManager;
    private ParticleManager ParticleManager;
    private UIManager UIManager;
    private SaveManager SaveManager;

    //Map symbol data with corresponding enum types
    private Dictionary<SymbolType, SymbolData> SymbolData = new Dictionary<SymbolType, SymbolData>();

    public SymbolData GetSymbolData(SymbolType SymbolType)
    {
        SymbolData.TryGetValue(SymbolType, out var symbolData);
        return symbolData;
    }

    protected override bool Init()
    {
        if(base.Init()) return true;

        CurrencyManager = CurrencyManager.Instance as CurrencyManager;
        ParticleManager = ParticleManager.Instance as ParticleManager;
        UIManager = UIManager.Instance as UIManager;
        SaveManager = SaveManager.Instance as SaveManager;

        //Map symbol types to symbol data
        InitializeSymbolData();

        //Start the game loop
        StartCoroutine(GameLoop());   

        return true;
    }

    private void InitializeSymbolData()
    {
        var SymbolSettings = GameSettings.SymbolSettings;
        GameSettings.SymbolSettings.ForEach(s => SymbolData.Add(s.Key, s.Value));
    }

    private IEnumerator GameLoop()
    {
        //Init triplet result buffer
        SymbolData[] SymbolData = new SymbolData[SlotController.ReelsCount];

        while (true)
        {
            //Wait for spin buttons respond
            yield return SpinButton.WaitForPress();

            SymbolTriplet SymbolResult = new SymbolTriplet(); 
            yield return SlotController.Spin((SymbolTripletResult) =>
            {
                SymbolResult = SymbolTripletResult;
            });

            //If the three symbols are same
            if (SymbolResult.AreAllSame())
            {
                //Show coin value for per associated symbol
                for (int i = 0; i < SlotController.ReelsCount; i++)
                {
                    SymbolData[i] = GetSymbolData(SymbolResult[i]);
                    yield return UIManager.ShowCurrencyPopup(i, SymbolData[i].CoinValue);
                }

                for (int i = 0; i < SlotController.ReelsCount; i++)
                    yield return UIManager.HideCurrencyPopup(i);

                //Burst coin particles corresponding to their sum of value
                yield return ParticleManager.BurstCoinParticles(SymbolData);
                StartCoroutine(CurrencyManager.CollectCoins(SymbolData));
            }
        }
    }

    //Save game
    private void OnApplicationQuit()
    {
        var SaveData = SaveManager.SaveData;
        SaveData.CoinCurrency = CurrencyManager.FinalCoinAmount;
        SaveData.SpinIndex = SlotController.SpinIndex;

        SaveManager.Save();
    }
}
