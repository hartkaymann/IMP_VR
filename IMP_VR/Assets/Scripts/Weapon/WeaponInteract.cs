using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteract : MonoBehaviour
{
    private Rigidbody _enemyRb;
    private float _slipTimeLimit = 3f;
    private float _slipTime;
    private float _maxSlipSpeed = 3f;
    private bool _isSlip = false;

    
    void Start()
    {
        _enemyRb = this.GetComponent<Rigidbody>();
    }

    // Per every frame, it will check the condition of the fields for each type of weapon effect
    // Only when the fields values matches with condition, then the effect will be started
    void Update()
    {
        // If the effect is slip, it will check the time for keeping the effect
        if (_isSlip == true)
        {
            // If the time of the effect is still less than the limit time, it will keep showing the effect
            if (_slipTime < _slipTimeLimit)
            {
                // Conducting basic effect
                // It will rotate the enemy in round based on its middle&vertical axis
                this.transform.RotateAround(this.transform.position, this.transform.up, SetSlipSpeed(_slipTime));
                _slipTime += Time.deltaTime;
            }
            
            // If the effect is still slip, but when the time is over,
            // it will initialize the values of the fields and the effect will be finished
            else 
            {
                _slipTime = 0f;
                _isSlip = false;
            }
        }
    }

    // When the enemy is collided with banana,
    // this BananaSlip() function will be called from the script WeaponManager attached to the weapons
    // This function will set the values of the fields that are needed for slippery effect
    public void BananaSlip(){
        _isSlip = true;
        _slipTime = 0f;
    }

    // The slip speed will be fast at first and it will slow at last
    // As time goes by, the speed will be slower
    private float SetSlipSpeed(float currentTime) {
        return _maxSlipSpeed * (1 - currentTime / _slipTimeLimit);
    }

}
