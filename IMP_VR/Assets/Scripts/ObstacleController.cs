using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ObstacleController : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Player")
        {
            if(TryGetComponent<Rigidbody>(out var rb))
            {
                rb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}
