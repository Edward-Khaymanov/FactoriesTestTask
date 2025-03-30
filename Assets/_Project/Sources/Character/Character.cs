using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private MovementComponent _movementComponent;
    [SerializeField] private Animator _animator;

    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private CharacterState _currentState;
    private int _idleStateHash;
    private int _moveStateHash;
    private int _collectStateHash;

    private void Awake()
    {
        _idleStateHash = Animator.StringToHash("IdleState");
        _moveStateHash = Animator.StringToHash("MoveState");
        _collectStateHash = Animator.StringToHash("CollectState");
    }

    public void MoveTo(Vector3 position)
    {
        CancelCurrentTask();
        var token = _cancellationTokenSource.Token;
        MoveToAsync(position, token);
    }

    public void CollectResources(FactoryObject targetFactory)
    {
        if (_currentState == CharacterState.Collecting)
            return;

        CancelCurrentTask();
        var token = _cancellationTokenSource.Token;
        CollectResourcesAsync(targetFactory, token);
    }

    private async UniTask MoveToAsync(Vector3 position, CancellationToken token)
    {
        _movementComponent.MoveTo(position);
        if (_currentState != CharacterState.Move)
        {
            _animator.Play(_moveStateHash);
            await UniTask.NextFrame(token);
            SetState(CharacterState.Move);
        }

        while (_movementComponent.IsMoving)
        {
            await UniTask.NextFrame(token);
        }

        _animator.Play(_idleStateHash);
        await UniTask.NextFrame(token);
        SetState(CharacterState.Idle);
    }

    private async UniTask CollectResourcesAsync(FactoryObject targetFactory, CancellationToken token)
    {
        await MoveToAsync(targetFactory.CollectPoint.transform.position, token);

        if (targetFactory.CollectPoint.ObjectInPoint(transform))
            return;

        _animator.Play(_collectStateHash);
        await UniTask.NextFrame(token);
        SetState(CharacterState.Collecting);

        while (_animator.GetCurrentAnimatorStateInfo(0) is var stateInfo
            && stateInfo.shortNameHash == _collectStateHash
            && stateInfo.normalizedTime < 1f)
        {
            await UniTask.NextFrame(token);
        }

        var saveLoadService = ServiceLocator.Get<ISaveLoadService>();
        var collectedResources = ServiceLocator.Get<FactoriesManager>().TakeResources(targetFactory.FactoryId);
        var savedResources = saveLoadService.LoadResources();
        savedResources.TryGetValue(collectedResources.Key, out var currentAmount);
        saveLoadService.SaveResource(collectedResources.Key, currentAmount + collectedResources.Value);

        _animator.Play(_idleStateHash);
        await UniTask.NextFrame(token);
        SetState(CharacterState.Idle);
    }

    private void SetState(CharacterState newState)
    {
        Debug.Log($"Transition from {_currentState} to {newState}");
        _currentState = newState;
    }

    private void CancelCurrentTask()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
    }
}