using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Allows to edit dynamic settings in runtime
public class UISettingsPopup : UIPopup
{
    [SerializeField]
    private TMP_InputField MinSpinDistanceInput;

    [SerializeField]
    private TMP_InputField MaxSpinDistanceInput;

    [SerializeField]
    private TMP_InputField ReelActivationDelayInput;

    [SerializeField]
    private TMP_InputField SpinDurationInput;

    private SaveManager SaveManager;

    public event Action SettingsApplied;

    protected override void Awake()
    {
        base.Awake();
        SaveManager = SaveManager.Instance as SaveManager;

        var GameSettings = GameManager.GameSettings;
    }

    public override void Show()
    {
        base.Show();
        ReadSettings();
    }

    private void ReadSettings()
    {
        var SaveData = SaveManager.SaveData;

        MinSpinDistanceInput.text = SaveData.DynamicSettings.MinSpinDistance.ToString();
        MaxSpinDistanceInput.text = SaveData.DynamicSettings.MaxSpinDistance.ToString();
        ReelActivationDelayInput.text = SaveData.DynamicSettings.ReelActivationDelay.ToString(CultureInfo.InvariantCulture.NumberFormat);
        SpinDurationInput.text = SaveData.DynamicSettings.SpinDuration.ToString(CultureInfo.InvariantCulture.NumberFormat);
    }

    public void ApplySettings()
    {
        var SaveData = SaveManager.SaveData;

        if (SaveData.DynamicSettings == null)
            SaveData.DynamicSettings = new DynamicSettings();

        var DynamicSettings = SaveData.DynamicSettings;
        DynamicSettings.MinSpinDistance = int.Parse(MinSpinDistanceInput.text);
        DynamicSettings.MaxSpinDistance = int.Parse(MaxSpinDistanceInput.text);
        DynamicSettings.ReelActivationDelay = float.Parse(ReelActivationDelayInput.text, CultureInfo.InvariantCulture.NumberFormat);
        DynamicSettings.SpinDuration = float.Parse(SpinDurationInput.text, CultureInfo.InvariantCulture.NumberFormat);
 
        SaveManager.Save();
        Hide();

        SettingsApplied?.Invoke();
    }
}
