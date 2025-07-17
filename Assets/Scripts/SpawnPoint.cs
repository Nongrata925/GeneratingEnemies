using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private Vector3 _movementDirection = Vector3.forward;

    public Vector3 SetMovementDirection()
    {
        return _movementDirection;
    }
}