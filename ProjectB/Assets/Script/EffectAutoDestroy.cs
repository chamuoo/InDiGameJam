using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
    private Animator animator;
    private bool isDestoryed = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !isDestoryed)
        {
            isDestoryed = true;
            Destroy(gameObject);
        }
    }
}
