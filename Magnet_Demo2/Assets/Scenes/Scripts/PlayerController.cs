using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static Utilities;

public class PlayerController : MonoBehaviour
{
    // This script handles gathering player inputs and enacting player controls 
    // The main variables and components of the Player
    [SerializeField] float speed;
    [SerializeField] float maxVelocity;
    [Header ("1 = Positive, -1 = Negative")]
    public float polarity;
    public Rigidbody rb;
    public Collider field;
    private MeshRenderer fieldRenderer;
    private Magnet magnet;

    public Material posFieldMatBox;
    public Material negFieldMatBox;
    [Header("The materials for the magnetic fields")]
    // [SerializeField] Material posFieldMat;
    // [SerializeField] Material negFieldMat;
    [SerializeField] Material fieldMat;

    public AudioSource posAudioSource;
    public AudioSource negAudioSource;

    
    // Input variables
    private float magInput;
    void Start() // Initializes components
    {
        magnet = field.GetComponent<Magnet>();
        fieldRenderer = field.GetComponent<MeshRenderer>();
    }
    void FixedUpdate() // Resets the magnetic field effect and clamps Rigidbody velocity
    {

    }
    void GetMoveInput() // Gets the values for player's inputs
    {
        magInput = Input.GetAxis("Magnet");
    }
    void Update() // Enacts the player's inputs (movement, magnets, carmera controls)
    { 

        if (rightInputDevices.Count < 1)
            InitializeInputReader();

        magInput = GetInput();

        // Magnetic field control 
        if (magInput == 1)
        {
            polarity = magInput;
            fieldRenderer.enabled = true;
            // fieldRenderer.material = posFieldMat;
             fieldRenderer.material = posFieldMatBox;
            //field.enabled = true;
            field.gameObject.SetActive(true);
            if (!posAudioSource.isPlaying)
                posAudioSource.Play();
            if (negAudioSource.isPlaying)
                negAudioSource.Stop();
        }
        else if(magInput == -1)
        {
            polarity = magInput;
            fieldRenderer.enabled = true;
            // fieldRenderer.material = negFieldMat;
             fieldRenderer.material = negFieldMatBox;
            //field.enabled = true;
            field.gameObject.SetActive(true);
            if (posAudioSource.isPlaying)
                posAudioSource.Stop();
            if (!negAudioSource.isPlaying)
                negAudioSource.Play();


        }
        else if(field.gameObject.activeSelf)
        {
            fieldRenderer.material = null;
            magnet.storeObjTrans.transform.DetachChildren();
            field.gameObject.SetActive(false);
            //field.enabled = false;
            magnet.DropItems();
            if (posAudioSource.isPlaying)
                posAudioSource.Stop();
            if (negAudioSource.isPlaying)
                negAudioSource.Stop();
        }
    }

    List<InputDevice> rightInputDevices = new List<InputDevice>();
    int inputLastFrame = 0;
    int GetInput()
    {
        int input = 0;
        foreach (var inputDevice in rightInputDevices)
        {
            inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue);

            Debug.Log(triggerValue + " , " + gripValue);
            if (triggerValue < 0.1f && gripValue < 0.1f)
                input = 0;
               else if (triggerValue > gripValue)
                input = -1;
            else if (triggerValue < gripValue)
                input = 1;
            else if (triggerValue > 0.1f && gripValue > 0.1f)
                input = inputLastFrame;

        }
        inputLastFrame = input;
        return input;
    }

    void InitializeInputReader()
    {
        //InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, inputDevices);
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, rightInputDevices);
    }
}