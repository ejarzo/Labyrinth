using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO.Ports;


public class GameController : MonoBehaviour
{
    public GameObject marble;
    public GameObject board;

    public float sensitivity = 1;

    public GameObject titleText;
    public GameObject pressStartText;
    public GameObject lossText;
    public GameObject winText;
    public AudioSource startSound;

    public string portName;

    private SerialPort arduino;
    private Vector3 initMarblePosition;
    private Rigidbody marbleRb;

    private float smooth = 0.2f;

    void Start()
    {
        marbleRb = marble.GetComponentInChildren<Rigidbody>();
        initMarblePosition = marble.transform.position;
        marbleRb.isKinematic = true;

        arduino = new SerialPort(portName, 9600);
        arduino.Open();

        // Initialize handshake
        arduino.Write("Start");
    }

    void Update()
    {
        // Space bar is fallback start button 
        if (Input.GetKeyDown("space") && pressStartText.activeInHierarchy)
        {
            StartGame();
        }

        if (arduino.IsOpen)
        {
            // Parse serial input
            string inputString = arduino.ReadLine();
            string[] words = inputString.Split(',');

            // Convert input range [0, 100] to [-50, 50]
            int xRotation = int.Parse(words[0]) - 50; 
            int zRotation = int.Parse(words[1]) - 50;
            int buttonState = int.Parse(words[2]);

            // Map to tilt values
            float xTilt = xRotation / sensitivity;
            float zTilt = zRotation / sensitivity;

            // Smooth transition to new rotation
            Quaternion target = Quaternion.Euler(xTilt, 0, zTilt);
            board.transform.rotation = Quaternion.Slerp(board.transform.rotation, target, smooth);

            // Read start button press
            if(buttonState == 1 && pressStartText.activeInHierarchy)
            {
                StartGame();
            }
        }
    }

    void StartGame()
    {
        // Hide UI
        titleText.SetActive(false);
        pressStartText.SetActive(false);
        lossText.SetActive(false);
        winText.SetActive(false);

        startSound.Play();

        // Initialize marble position
        marble.transform.position = initMarblePosition;
        marbleRb.isKinematic = false;
        marble.SetActive(true);
    }
}
