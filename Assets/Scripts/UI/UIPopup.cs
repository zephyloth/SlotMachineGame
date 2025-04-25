using System.Text;
using TMPro;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject PopupScreenDarkener;

    [SerializeField]
    protected TextMeshProUGUI ContentTextMesh;

    protected GameManager GameManager;
    protected ProbabilityManager ProbabilityManager;

    protected StringBuilder ContentBuffer = new StringBuilder();

    protected virtual void Awake()
    {
        ProbabilityManager = ProbabilityManager.Instance as ProbabilityManager;
        GameManager = GameManager.Instance as GameManager;
    }
 
    protected virtual void AddTextContent() {}

    public void ShowTextContent()
    {
        PopupScreenDarkener.gameObject.SetActive(true);
        gameObject.SetActive(true);
 
        ContentBuffer.Clear();
        AddTextContent();

        ContentTextMesh.text = ContentBuffer.ToString();
    }

    public virtual void Show()
    {
        PopupScreenDarkener.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        PopupScreenDarkener.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
