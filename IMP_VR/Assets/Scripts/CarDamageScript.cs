using UnityEngine;

public class CarDamageScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HPBar.carCurrentHealth -= 1;
            //Debug.Log("collided");
        }
    }
}
