using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class WeaponManager : MonoBehaviour
{

    // _weaponType will be Grenade or Banana
    //  Based on the tag value, the weapon effect will be changed
    private string _weaponType;    

    private MeshRenderer _grenadeMesh;
    private float _explosionDelay = 2f;
    private float _destoryDelay = 0.5f;
    private int _explosionRad = 4;
    private int _explosionForce = 400;
    [SerializeField] private GameObject _explosionEffect;
    
    void Start()
    {
        // Get the type of weapon from the tag
        _weaponType = this.gameObject.tag;


        // Get grenade mesh
        if (_weaponType == "Grenade")
        {
            _grenadeMesh = this.gameObject.GetComponent<MeshRenderer>();
        }
        
    }

    // Grenade effect
    // If it is Grenade, it will explode after a few seconds of throwing and the explosion will affect to the enemy
    public void TriggerBoom()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait() 
    {
        yield return new WaitForSeconds(_explosionDelay);
        Explode();
        _grenadeMesh.enabled = false;
        yield return new WaitForSeconds(_destoryDelay);
        Destroy(this.gameObject);
    }

    private void Explode()
    {
        //sound plays

        Instantiate(_explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRad);
        foreach (Collider nearByObjects in colliders)
        {
            Rigidbody rb = nearByObjects.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(_explosionForce, transform.position, _explosionRad);
            }

            
        }
    }




    // Banana effect
    // If it is Banana, it will be just thrown on the ground, and when the enemy collides with it, it will make the enemy slip
    public void OnCollisionEnter(Collision other)
    {    
        // The banana effect will be conducted only if the collided object is enemy

        // Check the type of the collided object from the tag
        if (other.gameObject.tag == "Enemy")
        {
            // If the collided object is enemy,
            // then it will access to the WeaponInteract script
            WeaponInteract wi = other.gameObject.GetComponent<WeaponInteract>();

            // Based on the _weaponType, the matching weapon effect function will be called
            // If the _weapon type is banana, the banana will be destroyed after the collision with enemy
            // and the enemy will slip
            if (_weaponType == "Banana")
            {
                Destroy(this.gameObject);
                wi.BananaSlip();
            }
        }
    }







}
