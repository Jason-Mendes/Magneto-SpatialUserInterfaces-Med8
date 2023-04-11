using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Magnet2 : MonoBehaviour
{
    // This script handles the behavior of the Magnet GameObjects
    // The main variables and components of the Magnet 

    /*[SerializeField] PlayerController player;
    [Header("True = Positive, False = Negative")]
    [SerializeField] bool polarity;

    private Rigidbody rb;
    private SphereCollider field;
    public float maxVelocity;*/
    
    [SerializeField] PlayerController player;
    [SerializeField] public bool polarity;
    private Rigidbody rb;
    private SphereCollider field;
    public float maxVelocity;
    private Utilities utilities = new Utilities();
    private Vector3 initialPosition;

    // Remove unnecessary reference to Utilities class
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = CalculateMass(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        field = GetComponent<SphereCollider>();
        initialPosition = transform.position;


        // Float the object at the start
        rb.useGravity = false;
    }

    // Add code to reset position when space key is press
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetPosition();
        }
    }

    void FixedUpdate()
    {
        ClampVelocity(rb, maxVelocity);
    }

    void OnTriggerStay(Collider other)
    {
        if(other == player.field)
        {
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

    void Attract()
    {
        if(player.rb.mass > rb.mass)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.2f);
        }
        else if(player.rb.mass <= rb.mass)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, 0.2f);
        } 
    }
    void Repel() // Repulsion logic
{
    Vector3 direction = transform.position - player.transform.position;
    float distance = Mathf.Clamp(direction.magnitude, 5f, 10f);
    transform.position += direction.normalized / distance;
    player.transform.position -= direction.normalized / distance;
}


   /* void Repel()
    {
        if(player.transform.localScale.x > transform.localScale.x)
        { 
            Vector3 vector = transform.position - player.transform.position;
            float distance = Mathf.Clamp(Vector3.Magnitude(vector), 5f, 10f);
            vector.Normalize();
            vector *= 1/distance;
            transform.position += vector;  
        }
        else if(player.transform.localScale.x <= transform.localScale.x)
        {
            Vector3 vector = player.transform.position - transform.position;
            float distance = Mathf.Clamp(Vector3.Magnitude(vector), 5f, 10f);
            vector.Normalize();
            vector *= 1/distance;
            player.transform.position += vector;   
        } 
    }*/

    // Add method to reset position to starting position
   
    public void ResetPosition(float duration = 1f)
    {
        Vector3 startPos = initialPosition;
        Vector3 endPos = transform.position;

        StartCoroutine(SmoothResetPosition(startPos, endPos, duration));
    }

    private IEnumerator SmoothResetPosition(Vector3 startPos, Vector3 endPos, float duration)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(endPos, startPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;
    }

   /*void ResetPosition(float duration)
{
    rb.useGravity = false;
    Vector3 originalPosition = transform.position;
    transform.position = Vector3.zero;
    rb.velocity = Vector3.zero;
    StartCoroutine(MoveObject(originalPosition, duration));
}

IEnumerator MoveObject(Vector3 targetPosition, float duration)
{
    Vector3 startPosition = transform.position;
    float startTime = Time.time;
    float endTime = startTime + duration;

    while (Time.time < endTime)
    {
        float progress = (Time.time - startTime) / duration;
        transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
        yield return null;
    }

    transform.position = targetPosition;
    rb.useGravity = true;
}*/

   /* void ResetPosition()
    {
        rb.useGravity = false;
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
    }
*/
}
