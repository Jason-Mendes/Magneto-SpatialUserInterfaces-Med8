using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Magnet : MonoBehaviour
{
    // This script handles the behavior of the Magnet GameObjects
    // The main variables and components of the Magnet 
    [SerializeField] PlayerController player;
    [Header("True = Positive, False = Negative")]
    [SerializeField] public bool polarity;
    private Rigidbody rb;
    private SphereCollider field;
    public float maxVelocity;
    private Utilities utilities = new Utilities();
    void Start() // Initializes components
    {
        rb = this.GetComponent<Rigidbody>();
        rb.mass = CalculateMass(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        field = this.GetComponent<SphereCollider>();
    }
    void FixedUpdate() // Clamps Rigidbody velocity
    {
        ClampVelocity(rb, maxVelocity);
    }

    void OnTriggerStay(Collider other) // Determines if the player is within the magnetic field of this magnet
    {
        if(other == player.field)
        {
            // These check the polarity of the magnet and the player and call the appropriate method accordingly
            if(player.polarity == 1) 
            {
                if(polarity)
                {
                    Attract();
                }
                else
                {
                    Repel();
                }
            }
            else if(player.polarity == -1)
            {
                if(!polarity)
                {
                    Attract();
                }
                else
                {
                    Repel();
                }
            }
        }
    }

    void Attract() // Attraction logic
    {
        
        if(player.rb.mass > rb.mass) // Moves smaller magnet towards player
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.2f);
            //Vector3 vector = transform.position - player.transform.position;
            //float distance = Mathf.Clamp(Vector3.Magnitude(vector), 5f, 10f);
            //vector.Normalize();
            //vector *= 1 / distance;
            //transform.position -= vector;

            // ^ Alternate attraction method, not being used ^
        }
        else if(player.rb.mass <= rb.mass) // Moves player towards larger magnet
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, 0.2f);
            //Vector3 vector = player.transform.position - transform.position;
            //float distance = Mathf.Clamp(Vector3.Magnitude(vector), 5f, 10f);
            //vector.Normalize();
            //vector *= 1 / distance;
            //player.transform.position -= vector;

            // ^ Alternate attraction method, not being used ^
        } 
    }
    void Repel() // Repulsion logic
    {
        if(player.transform.localScale.x > transform.localScale.x) // Moves smaller magnet away from player
        { 
            Vector3 vector = transform.position - player.transform.position;
            float distance = Mathf.Clamp(Vector3.Magnitude(vector), 5f, 10f);
            vector.Normalize();
            vector *= 1/distance;
            transform.position += vector;  
        }
        else if(player.transform.localScale.x <= transform.localScale.x) // Moves player away from larger magnet
        {
            Vector3 vector = player.transform.position - transform.position;
            float distance = Mathf.Clamp(Vector3.Magnitude(vector), 5f, 10f);
            vector.Normalize();
            vector *= 1/distance;
            player.transform.position += vector;   
        } 
    }
}