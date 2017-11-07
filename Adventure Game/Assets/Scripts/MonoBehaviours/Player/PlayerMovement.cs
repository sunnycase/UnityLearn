using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public float inputHoldDelay = 0.5f;
    public float turnSpeedThreshold = 0.5f;
    public float speedDampTime = 0.1f;
    public float slowingSpeed = 0.175f;
    public float turnSmoothing = 15f;

    public const string startingPositionKey = "startingPosition";

    private WaitForSeconds _inputHoldWait;
    private Vector3 _destinationPosition;
    private Interactable _currentInteractable;
    private bool _handleInput = true;

    private const float StopDistanceProportion = 0.1f;
    private const float NavMeshSampleDistance = 4f;
    private readonly int HashSpeedParameter = Animator.StringToHash("Speed");
    private readonly int HashLocomotionTag = Animator.StringToHash("Locomotion");

    private void Start()
    {
        agent.updateRotation = false;
        _inputHoldWait = new WaitForSeconds(inputHoldDelay);
        _destinationPosition = transform.position;
    }

    private void OnAnimatorMove()
    {
        agent.velocity = animator.deltaPosition / Time.deltaTime;
    }

    private void Update()
    {
        if (agent.pathPending) return;

        var speed = agent.desiredVelocity.magnitude;
        if (agent.remainingDistance <= agent.stoppingDistance * StopDistanceProportion)
            Stopping(out speed);
        else if (agent.remainingDistance <= agent.stoppingDistance)
            Slowing(out speed, agent.remainingDistance);
        else if (speed > turnSpeedThreshold)
            Moving();

        animator.SetFloat(HashSpeedParameter, speed, speedDampTime, Time.deltaTime);
    }

    private void Stopping(out float speed)
    {
        agent.isStopped = true;
        transform.position = agent.destination;
        speed = 0;

        if(_currentInteractable)
        {
            transform.rotation = _currentInteractable.interactionLocation.rotation;
            _currentInteractable.Interact();
            _currentInteractable = null;
            StartCoroutine(WaitForInteraction());
        }
    }

    private void Slowing(out float speed, float distanceToDestination)
    {
        agent.isStopped = true;
        transform.position = Vector3.MoveTowards(transform.position, _destinationPosition, slowingSpeed * Time.deltaTime);

        var proportionDistance = 1.0f - distanceToDestination / agent.stoppingDistance;
        speed = Mathf.Lerp(slowingSpeed, 0, proportionDistance);

        var targetRotation = _currentInteractable ? _currentInteractable.interactionLocation.rotation : transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, proportionDistance);
    }

    private void Moving()
    {
        var targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
    }

    public void OnGroundClick(BaseEventData data)
    {
        if (!_handleInput) return;
        _currentInteractable = null;

        var pointerData = (PointerEventData)data;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pointerData.pointerCurrentRaycast.worldPosition, out hit, NavMeshSampleDistance, NavMesh.AllAreas))
        {
            _destinationPosition = hit.position;
        }
        else
        {
            _destinationPosition = pointerData.pointerCurrentRaycast.worldPosition;
        }

        agent.SetDestination(_destinationPosition);
        agent.isStopped = false;
    }

    public void OnInteractableClick(Interactable interactable)
    {
        if (!_handleInput) return;
        _currentInteractable = interactable;
        _destinationPosition = interactable.interactionLocation.position;

        agent.SetDestination(_destinationPosition);
        agent.isStopped = false;
    }

    private IEnumerator WaitForInteraction()
    {
        _handleInput = false;

        yield return _inputHoldWait;

        while (animator.GetCurrentAnimatorStateInfo(0).tagHash != HashLocomotionTag)
            yield return null;

        _handleInput = true;
    }
}
