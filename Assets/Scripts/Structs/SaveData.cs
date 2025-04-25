[System.Serializable]
public class SaveData
{
    public DynamicSettings DynamicSettings = null;
    public int CoinCurrency = 0;
    public int SpinIndex = 0;
}

[System.Serializable]
public class DynamicSettings
{
    public int MinSpinDistance;
    public int MaxSpinDistance;
    public float ReelActivationDelay;
    public float SpinDuration;
}