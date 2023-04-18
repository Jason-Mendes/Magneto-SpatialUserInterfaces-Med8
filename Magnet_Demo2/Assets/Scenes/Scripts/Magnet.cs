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
    private BoxCollider field;
    public float maxVelocity;
    private Utilities utilities = new Utilities();

    public Transform storeObjTrans;
    public float speed = .1f;

    List<GameObject> pickedUpObjects = new List<GameObject>();

    public Transform dir;

    public Transform stopTrans;

    void Start() // Initializes components
    {

    }

    void OnTriggerEnter(Collider other) // Determines if the player is within the magnetic field of this magnet
    {
        Item collindingItem = other.GetComponent<Item>();
        if (collindingItem)
        {
            //other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            other.transform.SetParent(storeObjTrans, true);
            pickedUpObjects.Add(other.gameObject);
            other.GetComponent<Item>().PickUp(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Item collindingItem = other.GetComponent<Item>();
        if (collindingItem)
        {
            other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            //other.GetComponent<Rigidbody>().useGravity = true;
            other.transform.SetParent(null);
            other.GetComponent<Item>().PickUp(false);
            pickedUpObjects.Remove(other.gameObject);
        }
    }

    public void RemoveItem(GameObject removedObj)
    {
        removedObj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        removedObj.GetComponent<Item>().PickUp(false);
        pickedUpObjects.Remove(removedObj);
    }

    public void DropItems()
    {
        foreach (GameObject obj in pickedUpObjects)
        {
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            obj.GetComponent<Item>().PickUp(true);

        }
        //obj.GetComponent<Rigidbody>().useGravity = true;
        pickedUpObjects.Clear();
    }

    private void FixedUpdate()
    {
        if (player.polarity == 1)
        {
            Attract();
        }
        else if (player.polarity == -1)
            Repel();
    }

    void Attract() // Attraction logic
    {
        foreach (GameObject obj in pickedUpObjects)
        {
            Vector3 norm = Vector3.Normalize(dir.position - storeObjTrans.position);

            obj.GetComponent<Item>().Attract(norm, speed);
            //obj.GetComponent<Rigidbody>().MovePosition(obj.transform.position - norm * speed * Time.deltaTime);
            //obj.transform.position -= norm * speed;
        }
        //if(player.rb.mass > rb.mass) // Moves smaller magnet towards player
        {
            //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 0.2f);
            //Vector3 vector = transform.position - player.transform.position;
            //float distance = Mathf.Clamp(Vector3.Magnitude(vector), 5f, 10f);
            //vector.Normalize();
            //vector *= 1 / distance;
            //transform.position -= vector;

            // ^ Alternate attraction method, not being used ^
        }
        //else if(player.rb.mass <= rb.mass) // Moves player towards larger magnet
        {
            //player.transform.position = Vector3.MoveTowards(player.transform.position, transform.position, 0.2f);
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
        foreach (GameObject obj in pickedUpObjects)
        {
            Vector3 norm = Vector3.Normalize(dir.position - storeObjTrans.position);

            obj.GetComponent<Item>().Repel(norm, speed);

            //obj.transform.position += norm * speed;
            //obj.GetComponent<Rigidbody>().MovePosition(obj.transform.position + norm * speed * Time.deltaTime);
        }
        /*if(player.transform.localScale.x > transform.localScale.x) // Moves smaller magnet away from player
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
        } */
    }
}