using UnityEngine;
using UnityEngine.AI;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    
    public bool IsMoving { get; private set; }

    private void Update()
    {
        UpdateMovementState();
    }

    public void MoveTo(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
        UpdateMovementState();
    }

    private void UpdateMovementState()
    {
        if (_navMeshAgent.pathPending)
        {
            IsMoving = true;
            return;
        }

        if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance + 0.1f)
        {
            IsMoving = true;
        }
        else if (_navMeshAgent.velocity.sqrMagnitude < 0.01f)
        {
            IsMoving = false;
        }
    }
}