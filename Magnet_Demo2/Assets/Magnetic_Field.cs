using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic_Field : MonoBehaviour
{
    public Transform target;  // The object to be attracted to the magnetic field
    public float strength = 10f;  // The strength of the magnetic field

    private bool isAttracting = false;  // Flag to indicate whether the object is currently being attracted
    private Vector3 originalPosition;  // The original position of the object before it was attracted

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            isAttracting = true;
            originalPosition = target.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            isAttracting = false;
            target.position = originalPosition;  // Return the object to its original position
        }
    }

    void FixedUpdate()
    {
        if (isAttracting)
        {
            // Calculate the direction from the target to the magnetic field
            Vector3 direction = transform.position - target.position;

            // Apply a force to the target in the direction of the magnetic field
            target.GetComponent<Rigidbody>().AddForce(direction.normalized * strength, ForceMode.Force);
        }
    }
}
