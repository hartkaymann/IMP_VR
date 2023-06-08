using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCarController4Wheels : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    GameObject[] wheelMesh = new GameObject[4];

    public float power = 100f; // The power rotating wheels
    public float rot = 45f; // The Angle of wheels's rotatiton
    Rigidbody rb;

    public AudioSource carAudio;

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer[] children = transform.GetComponentsInChildren<MeshRenderer>();
        wheelMesh = (from c in children
                     where c.CompareTag("WheelMesh")
                     select c.gameObject).ToArray();


        for (int i = 0; i < wheelMesh.Length; i++)
        {
            wheels[i].transform.position = wheelMesh[i].transform.position;
        }

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1, 0); // Lower the center of gravity down the y-axis.
        carAudio = GetComponent<AudioSource>();

        carAudio.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WheelPosAndAni();

        // Through the 'for', move the entire wheel collider with as much force as power according to the vertical input.
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].motorTorque = Input.GetAxis("Vertical") * power;
            
        }

        // Since only the front wheel should be angled, set the 'for' to be only the front wheels.
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = Input.GetAxisRaw("Horizontal") * rot;
        }
    }

    void WheelPosAndAni()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }
}
