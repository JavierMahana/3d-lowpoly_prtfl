using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBAct : SceneLinkedSMB<UnitAndAnimatorLink>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.Unit.StartActing();
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MonoBehaviour.Unit.TryToAct();
    }
}
