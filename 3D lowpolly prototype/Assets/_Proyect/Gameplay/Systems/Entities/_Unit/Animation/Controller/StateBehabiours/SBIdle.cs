using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBIdle : SceneLinkedSMB<UnitAndAnimatorLink>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.Unit.StartIdle();
    }
    //public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // acá podria ver el tiempo y regresar a su pocision cuando se cumpla.
    //}
}
