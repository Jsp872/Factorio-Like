using UnityEngine;

public class Convoy : UtilityBuildings
{
    [SerializeField] private Vector2 directionToMove;
    [SerializeField] private float moveSpeed;

    public override void Start()
    {
        base.Start();
        SetDirection();
    }

    private void SetDirection()
    {
        float angleY = transform.eulerAngles.y;
        directionToMove = angleY switch
        {
            0 => Vector2.right,
            90 => Vector2.down,
            180 => Vector2.left,
            270 => Vector2.up,
            _ => directionToMove
        };
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = directionToMove * moveSpeed;
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = directionToMove * 0.1f;
    }
}