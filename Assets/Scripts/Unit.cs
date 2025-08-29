using UnityEngine;

public class Unit : MonoBehaviour
{
    private const string UNIT_ISWALKING = "IsWalking";

    [SerializeField] private Animator unitAnimator;

    private Vector3 targetPosition;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        float stoppingDistance = 0.1f;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > stoppingDistance)
        {
            unitAnimator.SetBool(UNIT_ISWALKING, true);

            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            transform.position += moveSpeed * Time.deltaTime * moveDirection;
        }
        else
        {
            unitAnimator.SetBool(UNIT_ISWALKING, false);
        }
    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
