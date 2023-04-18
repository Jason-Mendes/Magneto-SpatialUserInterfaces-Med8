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
    private Camera cam;
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
    
    // Input variables
    private float rightLeftInput;
    private float forwardBackInput;
    private float magInput;
    private Vector2 mouseInput;
    public bool pauseInput = false;
    void Start() // Initializes components
    {
        magnet = field.GetComponent<Magnet>();
        Cursor.lockState = CursorLockMode.Locked;
        cam = this.GetComponentInChildren<Camera>();
        rb = this.GetComponent<Rigidbody>();
        rb.mass = CalculateMass(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        fieldRenderer = field.GetComponent<MeshRenderer>();
    }
    void Update() // Resets certain variables every frame
    {
        if (inputDevices.Count < 2)
            InitializeInputReader();

        GetMoveInput();
        polarity = magInput;
    }
    void FixedUpdate() // Resets the magnetic field effect and clamps Rigidbody velocity
    {
        fieldRenderer.enabled = false;
        Move();
        ClampVelocity(rb, maxVelocity);
    }
    void GetMoveInput() // Gets the values for player's inputs
    {
        rightLeftInput = Input.GetAxis("Horizontal");
        forwardBackInput = Input.GetAxis("BackNForth");
        magInput = Input.GetAxis("Magnet");
        mouseInput.x += Input.GetAxis("Mouse X");
        mouseInput.y += Input.GetAxis("Mouse Y");
        mouseInput.y = Mathf.Clamp(mouseInput.y,-5,10);
        if(Input.GetKeyDown(KeyCode.Escape)) // Checks for the pause input and flips the pause boolean
        {
            pauseInput = !pauseInput;
        }
    }
    void Move() // Enacts the player's inputs (movement, magnets, carmera controls)
    {
        // Directional movement
        if(rightLeftInput == 1)
        {
            transform.Translate(Vector3.right * Time.fixedDeltaTime * speed);
        }
        if(rightLeftInput == -1)
        {
            transform.Translate(Vector3.left * Time.fixedDeltaTime * speed);
        }
        if(forwardBackInput == 1)
        {
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);
        }
        if(forwardBackInput == -1)
        {
            transform.Translate(Vector3.back * Time.fixedDeltaTime * speed);
        }
        // Magnetic field control 
        if(magInput == 1)
        {
            polarity = magInput;
            fieldRenderer.enabled = true;
            // fieldRenderer.material = posFieldMat;
             fieldRenderer.material = posFieldMatBox;
            field.enabled = true;
        }        
        else if(magInput == -1)
        {
            polarity = magInput;
            fieldRenderer.enabled = true;
            // fieldRenderer.material = negFieldMat;
             fieldRenderer.material = negFieldMatBox;
            field.enabled = true;

        }
        else if(magnet.storeObjTrans.transform.childCount > 0)
        {
            magnet.storeObjTrans.transform.DetachChildren();
            field.enabled = false;
            magnet.DropItems();
            
        }
        // Mouse controlled camera movement
        cam.transform.localRotation = Quaternion.Euler(mouseInput.y,0,0);
        transform.localRotation = Quaternion.Euler(0,mouseInput.x,0); 
    }

    List<InputDevice> inputDevices = new List<InputDevice>();

    void InitializeInputReader()
    {
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller, inputDevices);

        foreach(var inputDevice in inputDevices)
        {
            inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);

            Debug.Log(inputDevice.name + " " + triggerValue);
        }
    }
}