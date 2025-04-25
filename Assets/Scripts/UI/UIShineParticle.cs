using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShineParticle : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;

    private int ShineAnim;

    private void Start()
    {
        ShineAnim = Animator.StringToHash("Shine");
    }

    public void Play()
    {
        Animator.Play(ShineAnim);
    }
}
