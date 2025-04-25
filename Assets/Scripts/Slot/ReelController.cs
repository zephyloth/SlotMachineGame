using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ReelController : MonoBehaviour
{
    //Pool counter for the reels
    public static int ReelsSpinningCount = 0;

    //Setting spinning flag also increases pool count
    private bool isSpinning;
    private bool IsSpinning
    {
        get => isSpinning;
        set
        {
            isSpinning = value;
            if (value)
                ReelsSpinningCount++;
            else
                ReelsSpinningCount--;
        }
    }

    [SerializeField]
    private SpriteRenderer SpriteRenderer;

    [SerializeField]
    private SlotController SlotController;

    private GameManager GameManager;
    private ProbabilityManager ProbabilityManager;
    private SaveManager SaveManager;
    private AudioManager AudioManager;
    private GameSettings GameSettings;

    //Periodic scroll value for the reel shader
    private float SymbolPeriod;

    //BlurAmount increases with the scrolling of the reel
    private float BlurAmount;

    //Declare indices
    private int CurrentSpinIndex;
    private int ReelIndex;
 
    //Symbol world size
    public const float SymbolSize = 1;

    //Used to independently change properties of reel shader
    private MaterialPropertyBlock MaterialPropertyBlock;
 
    void Start()
    {
        GameManager = GameManager.Instance as GameManager;
        ProbabilityManager = ProbabilityManager.Instance as ProbabilityManager;
        SaveManager = SaveManager.Instance as SaveManager;
        AudioManager = AudioManager.Instance as AudioManager;
        GameSettings = GameManager.GameSettings;

        SymbolPeriod = 0;

        CurrentSpinIndex = SaveManager.SaveData.SpinIndex;
        ReelIndex = transform.GetSiblingIndex();

        MaterialPropertyBlock = new MaterialPropertyBlock();

        //Setup symbols
        PlaceSymbols();

        //Update shader properties
        UpdateMaterialBlock();
    }
 
    //Executes take off and slow down effects
    public IEnumerator Spin(float SlowDuration) //Slow down duration is given in the settings
    {
        IsSpinning = true;

        var DynamicSettings = SaveManager.SaveData.DynamicSettings;

        int SymbolCount = GameSettings.ProbabilityTable.MaxTripletCount;
        float TakeOffDuration = DynamicSettings.SpinDuration;
 
        int TotalDist = SlotController.SpinDistance;
        //Total dist in respect to world size
        float TotalWorldDist = TotalDist * SymbolSize;
 
        //Remaining time used for take off effect
        int TakeOffDist = (int)(TotalDist * TakeOffDuration / (TakeOffDuration + SlowDuration));
        float TakeOffWorldDist = TakeOffDist * SymbolSize;
 
        //Execute the effect sequences
        float CurrentDist = 0.0f;
        yield return CalculateSymbolPeriod(CurrentDist, TakeOffWorldDist, TakeOffDuration, Ease.InQuad);

        CurrentDist = TakeOffWorldDist;
        yield return CalculateSymbolPeriod(CurrentDist, TotalWorldDist, SlowDuration, Ease.OutQuad);

        //Play sound
        AudioManager.PlayReelStoppingSound();
        IsSpinning = false;
    }

    //Used Dotween setter callback to create periodic moves
    IEnumerator CalculateSymbolPeriod(float CurrentDist, float TargetDist, float TargetDuration, Ease Ease)
    {
        yield return DOTween.To(() =>
        {
            return CurrentDist;
        }
        ,
        (NewDist) =>
        {
            float Delta = NewDist - CurrentDist;
            CurrentDist = NewDist;
 
            BlurAmount = Mathf.Clamp01(Delta * GameSettings.BlurMultiplier);

            //Increase symbol period according to reel speed
            SymbolPeriod += Delta;
 
            //If the reel completely passes a symbol
            while (SymbolPeriod > SymbolSize / 2.0f)
            {
                //Reset period and increase the spin index
                SymbolPeriod -= SymbolSize;
                OnPeriodComplete();
            }

        }, TargetDist, TargetDuration).SetEase(Ease).WaitForCompletion();
    }

    //Update material properties
    private void UpdateMaterialBlock()
    {
        MaterialPropertyBlock.SetFloat("_Scroll", SymbolPeriod);
        MaterialPropertyBlock.SetFloat("_Blur", BlurAmount);
        SpriteRenderer.SetPropertyBlock(MaterialPropertyBlock);
    }

    //Increases the spin index and wraps, then rolling sound gets played
    private void OnPeriodComplete()
    {
        CurrentSpinIndex = ProbabilityManager.WrapIndex(++CurrentSpinIndex);

        //Replace symbols
        PlaceSymbols();

        if (CurrentSpinIndex % 3 == 1)
        AudioManager.PlayReelRollingSound();
    }

    private void PlaceSymbols()
    {
        //Collect SymbolData
        SymbolData SymbolUp = GameManager.GetSymbolData(ProbabilityManager.GetSpinResult(CurrentSpinIndex-1)[ReelIndex]);
        SymbolData SymbolMiddle = GameManager.GetSymbolData(ProbabilityManager.GetSpinResult(CurrentSpinIndex)[ReelIndex]);
        SymbolData SymbolDown = GameManager.GetSymbolData(ProbabilityManager.GetSpinResult(CurrentSpinIndex+1)[ReelIndex]);
 
        //Update references for each texture
        MaterialPropertyBlock.SetTexture("_Tex1", SymbolUp.Image.texture);
        MaterialPropertyBlock.SetTexture("_Tex1Blurred", SymbolUp.BlurredImage.texture);
        MaterialPropertyBlock.SetTexture("_Tex2", SymbolMiddle.Image.texture);
        MaterialPropertyBlock.SetTexture("_Tex2Blurred", SymbolMiddle.BlurredImage.texture);
        MaterialPropertyBlock.SetTexture("_Tex3", SymbolDown.Image.texture);
        MaterialPropertyBlock.SetTexture("_Tex3Blurred", SymbolDown.BlurredImage.texture);
    }

    private void Update()
    {
        if (!IsSpinning)
        {
            BlurAmount = 0;
        }
        UpdateMaterialBlock();
    }
}
