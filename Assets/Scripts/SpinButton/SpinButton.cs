using System.Collections;
using UnityEngine;

public class SpinButton : MonoBehaviour
{
    [SerializeField]
    private Sprite IdleSprite;

    [SerializeField]
    private Sprite PressSprite;

    [SerializeField]
    private Texture2D HandCursor;

    [SerializeField]
    SpriteRenderer SpriteRenderer;

    private AudioManager AudioManager;

    private bool DoAction;
    private bool IsWaiting;

    private void Start()
    {
        AudioManager = AudioManager.Instance as AudioManager;

        DoAction = false;
        IsWaiting = true;
    }

    public void OnPointerEnter()
    {
        Cursor.SetCursor(HandCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnPress()
    {
        SpriteRenderer.sprite = PressSprite;
        AudioManager.PlayButtonClickSound();
    }

    public void OnRelease()
    {
        SpriteRenderer.sprite = IdleSprite;

        if (!IsWaiting) return;
        DoAction = true;
    }

    public IEnumerator WaitForPress()
    {
        IsWaiting = true;
        while (true)
        {
            if (DoAction)
            {
                DoAction = false;
                IsWaiting = false;
                yield break;
            }
            yield return null;
        }
    }
}
