using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

    private string _weaponType;
    private WeaponInteract _wi;
    
    // Start is called before the first frame update
    void Start()
    {
        _weaponType = this.gameObject.tag;
        // It will be Grenade or Banana

    }

    // If it is Grenade, it will explode after a few seconds of throwing and the explosion will affect to the enemy




    // If it is Banana, it will be just thrown on the ground, and when the enemy collides with it, it will make the enemy slip
    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Enemy"){
            _wi = other.gameObject.GetComponent<WeaponInteract>();
            if (_weaponType == "Banana"){
                _wi.BananaSlip();
                Destroy(this.gameObject);
            }
        }
    }







}
