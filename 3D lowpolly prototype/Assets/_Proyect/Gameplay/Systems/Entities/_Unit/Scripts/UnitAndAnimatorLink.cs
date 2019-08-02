using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Animator),typeof(Unit))]
public class UnitAndAnimatorLink : MonoBehaviour
{
    public Animator Animator;
    public Unit Unit;

    private void OnEnable()
    {
        if (Animator == null)
        {
            Animator = GetComponent<Animator>();
        }
        if (Unit == null)
        {
            Unit = GetComponent<Unit>();
        }

        SceneLinkedSMB<UnitAndAnimatorLink>.Initialise(Animator, this);
    }
}
