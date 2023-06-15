using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInteract : MonoBehaviour
{
    private Rigidbody _enemyRb;

    // for banana slip
    private float _slipTimeLimit = 5f;
    private float _slipTime;
    private float _maxSlipSpeed = 8f;
    private bool _isSlip = false;

    // for bullet shot
    private Transform _leftNum2KillNum;
    private TextMeshProUGUI _leftNum2KillNumText;
    private int _leftNumLife = 5;
    
    void Start()
    {
        _enemyRb = this.GetComponent<Rigidbody>();
        ShowLeftNumLife();
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
        if (currentTime<_slipTimeLimit/3)
        {
            return _maxSlipSpeed;
        }
        else if (currentTime<_slipTimeLimit*2/3)
        {
            return _maxSlipSpeed*2/3;
        }
        else
        {
            return _maxSlipSpeed/3;
        }

        // return _maxSlipSpeed * (1 - currentTime / _slipTimeLimit);
    }

    // interaction because of collision with bullet
    // whenever it collides with bullet, it will lose it's left number of life and show it to the UI
    // and when the left number of life is equal or under 0, it will be destroyed
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Bullet")
        {
            _leftNumLife -= 1;
            ShowLeftNumLife();
            if (_leftNumLife <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void ShowLeftNumLife()
    {
        _leftNum2KillNum = this.gameObject.transform.Find("LeftNum2Kill").Find("Canvas").Find("Num");
        _leftNum2KillNumText = _leftNum2KillNum.GetComponent<TextMeshProUGUI>();
        _leftNum2KillNumText.text = _leftNumLife.ToString();
    }

}
