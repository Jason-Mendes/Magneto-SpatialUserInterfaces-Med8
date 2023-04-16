using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatStrength = 1f; // The strength of the float force
    public float magnetStrength = 10f; // The strength of the magnet force
    public Transform magnet; // The magnet object to attract or repel the floating object
    public bool isAttracted; // Whether the floating object is currently attracted to the magnet

    private Vector3 originalPosition; // The original position of the floating object
    private Rigidbody rb; // The Rigidbody component of the floating object

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (isAttracted)
        {
            // Calculate the force vector between the magnet and the floating object
            Vector3 forceDirection = magnet.position - transform.position;
            float distance = forceDirection.magnitude;
            forceDirection.Normalize();

            // Apply a force to the floating object in the direction of the magnet
            rb.AddForce(forceDirection * magnetStrength / distance, ForceMode.Force);
        }
        else
        {
            // Apply a constant upward force to keep the object floating
            rb.AddForce(Vector3.up * floatStrength, ForceMode.Force);
        }
    }

    public void SetAttracted(bool attracted)
    {
        isAttracted = attracted;

        // If no longer attracted, return to original position
        if (!isAttracted)
        {
            transform.position = originalPosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float floatStrength = 1f; // The strength of the float force
    public float magnetStrength = 10f; // The strength of the magnet force
    public Transform magnet; // The magnet object to attract or repel the floating object
    public bool isAttracted; // Whether the floating object is currently attracted to the magnet

    private Vector3 originalPosition; // The original position of the floating object
    private Rigidbody rigidbody; // The Rigidbody component of the floating object

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        originalPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (isAttracted)
        {
            // Calculate the force vector between the magnet and the floating object
            Vector3 forceDirection = magnet.position - transform.position;
            float distance = forceDirection.magnitude;
            forceDirection.Normalize();

            // Apply a force to the floating object in the direction of the magnet
            rigidbody.AddForce(forceDirection * magnetStrength / distance, ForceMode.Force);
        }
        else
        {
            // Apply a constant upward force to keep the object floating
            rigidbody.AddForce(Vector3.up * floatStrength, ForceMode.Force);
        }
    }

    public void SetAttracted(bool attracted)
    {
        isAttracted = attracted;

        // If no longer attracted, return to original position
        if (!isAttracted)
        {
            transform.position = originalPosition;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
*/