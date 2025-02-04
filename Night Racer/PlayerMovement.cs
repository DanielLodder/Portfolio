using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_accelerationForce;
    public Rigidbody m_rigidbody;

    [SerializeField] private float m_maxForwardSpeed;
    [SerializeField] private float m_maxBackwardsSpeed;

    [SerializeField] private float m_turnAngle;

    private bool m_grounded;

    [Range(0,1)]
    [SerializeField] private float m_traction;

    public bool m_canDrive = false;



    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        GameManager.instance._win.AddListener(Finished);

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!m_grounded)
        {
            return;
        }

        //vector for if the player is moving forward or backward
        Vector3 localVelocity = transform.InverseTransformDirection(m_rigidbody.velocity);

        Vector2 velocity = new Vector2(m_rigidbody.velocity.x, m_rigidbody.velocity.z);

        if (!m_canDrive)
        {
            Traction(velocity, localVelocity);
            return;
        }

        //calculates the steering of the player
        float speedPercentage = velocity.magnitude / m_maxForwardSpeed; 

        Vector3 rotation = Vector3.up * m_turnAngle * Input.GetAxis("Horizontal") * speedPercentage * Time.deltaTime;



        //adds the rotation to the player depending on if moving forward or backward;
        if (localVelocity.z > 0)
        {
            transform.localEulerAngles += rotation;
        }
        else if (localVelocity.z < 0)
        {
            transform.localEulerAngles -= rotation;
        }

        //adds the speed to the car
        m_rigidbody.AddForce(transform.rotation * Vector3.forward * m_accelerationForce * Input.GetAxis("Vertical"));


        if (velocity.magnitude > m_maxForwardSpeed && localVelocity.z > 0)
        {
            velocity = velocity.normalized * m_maxForwardSpeed;
            localVelocity = localVelocity.normalized * m_maxForwardSpeed;
        }
        else if (velocity.magnitude > m_maxBackwardsSpeed && localVelocity.z < 0 && Input.GetAxis("Vertical") < 0)
        {
            velocity = velocity.normalized * m_maxBackwardsSpeed;
            localVelocity = localVelocity.normalized * m_maxBackwardsSpeed;
        }

        Traction(velocity, localVelocity);

    }

    private void Traction(Vector2 velocity, Vector3 localVelocity)
    {

        m_rigidbody.velocity = new Vector3(velocity.x, m_rigidbody.velocity.y, velocity.y);

        Vector3 tractionVelocity = new Vector3(0, m_rigidbody.velocity.y, velocity.magnitude);

        if (localVelocity.z < 0)
        {
            tractionVelocity.z *= -1;
        }

        tractionVelocity = transform.rotation * tractionVelocity;

        m_rigidbody.velocity = Vector3.Lerp(m_rigidbody.velocity, tractionVelocity, m_traction / 10);
    }

    private void Finished()
    {
        print("cant drive no more bitch");
        m_canDrive = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Floor"))
        {

            m_grounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Floor"))
        {

            m_grounded = false;
        }
    }
}
