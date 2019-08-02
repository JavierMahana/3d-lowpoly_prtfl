using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.AI;
using AnimatorStateMachineUtil;

[RequireComponent(typeof(Rigidbody), typeof(UnitAndAnimatorLink), typeof(NavMeshAgent))]
public class Unit : Entity
{
    public static Action<Unit> UnitCreated = delegate { };
    public static Action<Unit> UnitDestroyed = delegate { };

    [FoldoutGroup("Unit Attributes")]
    public TeamsToInteract InteractablesTeams;
    [FoldoutGroup("Unit Attributes")]
    public bool InteractOnlyWithUnits;
    [FoldoutGroup("Unit Attributes")]
    public float DistanceMarginToReachDestination = 0.1f;
    [FoldoutGroup("Unit Attributes")]
    public SensorController SensorController;
    [FoldoutGroup("Unit Attributes")]
    public ActionModule ActionBehabiour;
    [FoldoutGroup("Unit Attributes")]
    public Transform ModelRoot;



    //set by the control panel and by himself
    private Vector3 destination;

    public Entity Target { get; set; }

    protected override void OnEnable()
    {
        base.OnEnable();

        destination = transform.position;

        actWaitForSeconds = new WaitForSeconds(ActionBehabiour.DelayBetweenActions);
        path = new NavMeshPath();
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        UnitCreated(this);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        UnitDestroyed(this);
    }


    public void ChangeToStateMovingState(MoveStateType type, Vector3 destination)
    {
        this.destination = destination;

        switch (type)
        {
            case MoveStateType.ATTACK_MOVE:
                animator.Play("Attack Move.Move");
                break;

            case MoveStateType.DIRECT_MOVE:
                animator.Play("Direct Move.Move");
                break;
        }
    }


    public void StartIdle()
    {
        navAgent.ResetPath();
    }
    public void StartPersue()
    {
        
        SetNewPathStoringHisLastPoint(Target.transform.position);
    }
    public void PersueTarget()
    {
        if (IsNewPathNedeed(finalPiontOfCurrentPath, Target.transform.position))
        {
            SetNewPathStoringHisLastPoint(Target.transform.position);
        }
    }
    public void StartMovementTowardDestination()
    {
        SetNewPathStoringHisLastPoint(destination);
    }
    public void MoveTowardDestination()
    {
        CheckDestination();

        if (IsNewPathNedeed(finalPiontOfCurrentPath, destination))
            SetNewPathStoringHisLastPoint(destination);
    }
    public void StartActing()
    {
        navAgent.ResetPath();
        ExecuteAction();
    }
    public void TryToAct()
    {
        if (!isActing)
        {
            ExecuteAction();
        }
    }


    private IEnumerator ActDelay()
    {
        isActing = true;
        yield return actWaitForSeconds;
        isActing = false;
    }
    private void ExecuteAction()
    {
        ActionBehabiour.Execute(Target);
        StartCoroutine(ActDelay());
    }
    private bool IsNewPathNedeed(Vector3 finalPointOfPath, Vector3 objectiveToReach )//ojo con el pocisionamiento luego de estructuras...Arruinaria todas las path actuales.
    {
        if (finalPointOfPath.DistanceXZ(objectiveToReach) > DistanceMarginToReachDestination)
        {
            return true;
        }
        return false;
    }


    private void SetNewPathStoringHisLastPoint(Vector3 goal)
    {
        if (navAgent.CalculatePath(goal, path))
        {
            finalPiontOfCurrentPath = path.corners[path.corners.Length - 1];
            navAgent.SetPath(path);
        }
    }
    private void CheckDestination()
    {
        if (destination.DistanceXZ(transform.position) < DistanceMarginToReachDestination)
        {
            ChangeMacroState(MacroState.AUTOMATIC);
        }
    }



    public void PlayerMoveComand(Vector3 destination, MacroState newMacro)
    {
        this.destination = destination;
        ChangeMacroState(newMacro);
    }
    private void ChangeMacroState(MacroState newMacro)
    {
        switch (newMacro)
        {
            case MacroState.AUTOMATIC:
                animator.SetInteger("Macro State", (int)MacroState.AUTOMATIC);
                break;
            case MacroState.DIRECT_MOVE:
                animator.SetInteger("Macro State", (int)MacroState.DIRECT_MOVE);
                break;
            case MacroState.ATTACK_MOVE:
                animator.SetInteger("Macro State", (int)MacroState.ATTACK_MOVE);
                break;
        }
    }




    private WaitForSeconds actWaitForSeconds;
    private bool isActing = false;
    private Vector3 finalPiontOfCurrentPath;
    private NavMeshPath path;
    private Animator animator;
    private NavMeshAgent navAgent;
    [Flags]
    public enum TeamsToInteract
    {
        SELF = 1,
        OTHER = 2,
    }
    public enum MacroState
    {
        AUTOMATIC,
        DIRECT_MOVE,
        ATTACK_MOVE
    }
    public enum MoveStateType
    {
        ATTACK_MOVE,
        DIRECT_MOVE
    }
    private void OnDrawGizmos()
    {
        if (path != null)
        {
            Vector3[] points = path.corners;

            for (int i = 0; i < points.Length - 1; i++)
            {
                Gizmos.color = Color.blue;

                if (i == points.Length - 2)
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawLine(points[i], points[i + 1]);
            }
        }
    }
}