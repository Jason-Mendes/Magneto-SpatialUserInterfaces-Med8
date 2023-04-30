using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject pastTutorial;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            tutorial.SetActive(false);
            pastTutorial.SetActive(true);
        }
    }
}
