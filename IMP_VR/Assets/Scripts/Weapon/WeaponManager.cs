using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


[RequireComponent(typeof(AudioSource))]
public class WeaponManager : MonoBehaviour
{

    // _weaponType will be Grenade, Banana, Gun or Bullet
    //  Based on the tag value, the weapon effect will be changed
    private string _weaponType;    

    // fields that are needed for grenade explosion effect
    private float _explosionDelay = 3f;
    private float _destoryDelay = 0.5f;
    private int _explosionRad = 4;
    private int _explosionForce = 400;
    [SerializeField] private GameObject _explosionEffect;

    // fields that are needed for gun shooting effect
    private Transform _barrel;
    private float _shootForce = 100f;
    [SerializeField] private GameObject _bullet;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _grenadeExplosionSound;
    [SerializeField] private AudioClip _gunShotSound;

    

    void Start()
    {
        // Get the type of weapon from the tag
        _weaponType = this.gameObject.tag;

        // Get _barrel transform
        if (_weaponType == "Gun")
        {
            _barrel = this.transform.Find("Barrel_Location");
        }
        _audioSource = this.GetComponent<AudioSource>();
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
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        while(Vector3.Magnitude(transform.position - playerTransform.position) < 0.5f)
            yield return new WaitForSeconds(.1f);
            
        Explode();
        yield return new WaitForSeconds(_destoryDelay);
        Destroy(this.gameObject);
    }

    private void Explode()
    {
        Instantiate(_explosionEffect, transform.position, transform.rotation);

        _audioSource.PlayOneShot(_grenadeExplosionSound, 0.5f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRad);
        foreach (Collider nearByObjects in colliders)
        {
            if (nearByObjects.tag == "Enemy")
            {
                Rigidbody rb = nearByObjects.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(_explosionForce, transform.position, _explosionRad);
                    Destroy(nearByObjects.gameObject);
                }
            }
            
        }
    }




    // Banana effect
    // If it is Banana, it will be just thrown on the ground, and when the enemy collides with it, it will make the enemy slip
    // Bullet effect
    // If it is Bullet, it will be shoot by gun, after that when the collision with enemy happens, it will make the enemy dead
    public void OnCollisionEnter(Collision other)
    {    
        // The banana effect and bullet effect will be conducted only if the collided object is enemy

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
            if (_weaponType == "Bullet")
            {
                Destroy(this.gameObject);
                Debug.Log("Bullet collision!");
            }

        }
    }



    // Gun effect
    // If it is Gun, when the trigger is clicked (Activated), it will instantiate bullet to the barrel point and shoot it
    public void Shoot()
    {
        GameObject bullet = Instantiate(_bullet, _barrel.position, _barrel.transform.rotation);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.AddForce(_barrel.forward*_shootForce, ForceMode.Impulse);
        _audioSource.PlayOneShot(_gunShotSound, 0.5f);
    }

    
    // For the weapons that should be in box all the time when they are not used
    // After using it (Select Exit), it will be in box again
    public void PutInBox()
    {
        Vector3 boxPos = GameObject.FindGameObjectWithTag("Box").transform.position + new Vector3(0f, 0.08f, 0f);
        this.transform.position = boxPos;
    }


    public void RemoveKinematic()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public void RemoveParent()
    {
        this.transform.SetParent(null);
    }



}
