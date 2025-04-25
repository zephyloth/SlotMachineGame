using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class UICurrencyPopup : MonoBehaviour
{
    [SerializeField]
    private float AnimDuration;
 
    [SerializeField]
    private TextMeshProUGUI TextMesh;

    [SerializeField]
    private Ease ZoomInEase;

    [SerializeField]
    private Ease ZoomOutEase;

    private WaitForSeconds WaitForPopupDelay = new WaitForSeconds(0.15f);

    private void Awake()
    {
        transform.localScale = Vector3.forward;
    }

    public IEnumerator Show(string Text)
    {
        TextMesh.text = Text;
        yield return transform.DOScale(Vector3.one, AnimDuration).SetEase(ZoomInEase).WaitForCompletion();
        yield return WaitForPopupDelay;
    }

    public IEnumerator Hide()
    {
        yield return transform.DOScale(Vector3.forward, AnimDuration/2.0f).SetEase(ZoomOutEase).WaitForCompletion();
    }
}
