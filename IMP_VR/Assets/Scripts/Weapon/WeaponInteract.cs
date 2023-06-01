using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInteract : MonoBehaviour
{
    private Rigidbody _enemyRb;

    // test
    private GameObject banana;


    // Start is called before the first frame update
    void Start()
    {
        _enemyRb = this.GetComponent<Rigidbody>();

        


        // test
        banana = GameObject.FindWithTag("Banana");
        TestBanana();
    }

    // Update is called once per frame
    void Update()
    {
        TestBanana();
        if (_b){
            print("--------------------------------");
            print(this.transform.position);
            print(this.transform.rotation);
        }
    }
    private bool _b =false;
    public void BananaSlip(){
        print("--------------------------------");
        print(this.transform.position);
        print(this.transform.rotation);
        _enemyRb.AddForce(new Vector3(0.5f,0f,0f), ForceMode.Impulse);
        print("--------------------------------");
        print(this.transform.position);
        print(this.transform.rotation);
        _b=true;
    }



    // test
    private void TestBanana(){
        this.transform.LookAt(banana.transform);
        this.transform.position = Vector3.MoveTowards(transform.position, banana.transform.position, 0.3f * Time.deltaTime);
    }

    private bool a = false;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.name=="Plane") {
            if (!a){
            print("ys");
            _enemyRb.AddForce(new Vector3(-5f,8f,0f), ForceMode.Impulse);
            a=true;
            }
        }
    }
    private void OnCollisionStay(Collision other) {
        if(other.gameObject.name=="Plane") {
            print("jfgv");
            _enemyRb.AddTorque(new Vector3(0.2f,0f,0f));
        }
    }
}
