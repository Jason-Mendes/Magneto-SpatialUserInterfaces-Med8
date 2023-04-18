using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    bool touchesSurface = false;
    bool pickedUp = false;

    private void Update()
    {
        if (pickedUp)
            if (touchesSurface)
                if (transform.position.y < 0)
                    GameObject.FindObjectOfType<Magnet>().RemoveItem(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
            touchesSurface = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
            touchesSurface = false;
    }

    public void Attract(Vector3 dir, float speed)
    {
        if (!touchesSurface)
        transform.position -= dir * speed;
    }
    public void Repel(Vector3 dir, float speed)
    {
        if (!touchesSurface)
           transform.position += dir * speed;
    }

    public void PickUp(bool pickup)
    {
        pickedUp = pickup;
    }
}
