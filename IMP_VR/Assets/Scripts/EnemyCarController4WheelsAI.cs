using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCarController4WheelsAI : MonoBehaviour
{
    public WheelCollider[] wheels = new WheelCollider[4];
    GameObject[] wheelMesh = new GameObject[4];

    public float power = 100f; // The power rotating wheels
    public float rot = 45f; // The Angle of wheels's rotatiton
    Rigidbody rb;

    public AudioSource carAudio;

    Transform target;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer[] children = transform.GetComponentsInChildren<MeshRenderer>();
        wheelMesh = (from c in children
                    where c.CompareTag("WheelMesh")
                    select c.gameObject).ToArray();

        for(int i = 0; i < wheelMesh.Length; i++)
        {
            wheels[i].transform.position = wheelMesh[i].transform.position;
        }

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1, 0); // Lower the center of gravity down the y-axis.
        carAudio = GetComponent<AudioSource>();

        carAudio.Play();
    }

    float GetPlayerAngle()
    {
        Vector3 dir = (target.position - transform.position).normalized; // Normal vector towards the player
        float targetDir = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg; // The angle of the normal vector toward the player

        // Adjust the angle calculation method to calculate 360 degrees.
        float curDir = 90 - transform.rotation.eulerAngles.y;
        if (targetDir < 0)
            targetDir += 360;
        if (curDir < 0)
            curDir += 360;
        if (curDir > targetDir)
            targetDir += 360;


        // Launches a Raycast to Detect Obstacles. Launch the raycast by tilting 45 degrees in the direction of progress.
        RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up, new Vector3(Mathf.Cos((curDir + 45) * Mathf.Deg2Rad), 0, Mathf.Sin((curDir + 45) * Mathf.Deg2Rad)), 10f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == transform) continue;
            if (hits[i].transform == target) continue;

            return 1f;
        }

        hits = Physics.RaycastAll(transform.position + Vector3.up, new Vector3(Mathf.Cos((curDir - 45) * Mathf.Deg2Rad), 0, Mathf.Sin((curDir - 45) * Mathf.Deg2Rad)), 10f);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == transform) continue;
            if (hits[i].transform == target) continue;

            return -1f;
        }

        // Returns the opposite direction of the direction in which the raycast detected the collision.
        if (targetDir - curDir < 10)
        {
            return 0;
        }
        if(targetDir - curDir > 180)
        {
            return 1f;
        }
        else
        {
            return -1f;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WheelPosAndAni();

        // If isBack is true, emeyies reverse.
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].motorTorque = (isBack ? -power : power);
        }

        // Check where the player is, and rotate the front wheel toward the player.
        for (int i = 0; i < 2; i++)
        {
            wheels[i].steerAngle = (isBack ? 0 : GetPlayerAngle() * rot);
        }
    }

    void WheelPosAndAni()
    {
        // Wheel Positioning
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);
            wheelMesh[i].transform.position = wheelPosition;
            wheelMesh[i].transform.rotation = wheelRotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float dot = Vector3.Dot(collision.contacts[0].normal, transform.forward.normalized);
        // Run BackCoroutine when enemies hit an obstacle
        if (dot < -0.7f)
        {
            StartCoroutine(BackCoroutine());
        }
    }

    bool isBack = false;
    IEnumerator BackCoroutine()
    {
        isBack = true;
        yield return new WaitForSeconds(1f);
        isBack = false;
    }
}
