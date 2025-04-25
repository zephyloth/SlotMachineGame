using UnityEngine;

public class CoinBurstController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem CoinParticleSystem;

    //Diminish particle amount in order to seem realistic
    [SerializeField]
    private int ParticleAmountDivision;

    [SerializeField]
    private int MaxParticleAmount;
 
    //Burst coins according to symbols coin amount
    public void BurstCoins(SymbolData[] SymbolDataArray)
    {
        int CoinAmount = CalculateCoinAmount(SymbolDataArray);
        var Emission = CoinParticleSystem.emission;
        var Bursts = new ParticleSystem.Burst[Emission.burstCount];
        Emission.GetBursts(Bursts);

        if (Bursts[0].count.constantMin != CoinAmount)
        {
            Bursts[0].count = new ParticleSystem.MinMaxCurve(CoinAmount, CoinAmount);
            Emission.SetBursts(Bursts);
        }
 
        CoinParticleSystem.time = 0;
        CoinParticleSystem.Play();
    }

    private int CalculateCoinAmount(SymbolData[] SymbolDataArray)
    {
        int Amount = 0;
        foreach (var SymbolData in SymbolDataArray)
            Amount += SymbolData.CoinValue / ParticleAmountDivision;

        if (Amount > MaxParticleAmount) Amount = MaxParticleAmount;
        return Amount;
    }
}
