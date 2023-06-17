using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ObstacleController : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Player")
        {
            // Remove rigidbody contraints as soon as car collides with an obstacle
            if(TryGetComponent<Rigidbody>(out var rb))
            {
                rb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}
