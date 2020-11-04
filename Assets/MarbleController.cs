using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleController : MonoBehaviour
{
    public GameObject WinText;
    public GameObject LossText;
    public GameObject pressStartText;
    public AudioSource lossSound;
    public AudioSource winSound;
    public AudioSource bounceSound;
    public AudioSource loopSound;

    private Vector3 initMarblePosition;
    
    private GameObject sphere;
    private Rigidbody sphereRb;

    void Start()
    {
        sphere = GameObject.Find("Sphere");
        sphereRb = sphere.GetComponent<Rigidbody>();
        initMarblePosition = sphere.transform.position;
    }

    void Update()
    {
        float vMagnitude = sphereRb.velocity.magnitude;
        float pitch = (1 + vMagnitude) / 2;
        loopSound.pitch = pitch;
    }

    void ResetPosition() {
        sphere.transform.position = initMarblePosition;
        sphereRb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)

    {
        if (collision.transform.tag == "bounce")
        {
            sphereRb.AddForce(new Vector3(-10, 0), ForceMode.Impulse);
            bounceSound.Play();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "WinCube")
        {
            WinText.SetActive(true);
            pressStartText.SetActive(true);
            winSound.Play();
            ResetPosition();
           
        }

        if (other.name == "GameOverCollider")
        {
            LossText.SetActive(true);
            pressStartText.SetActive(true);
            lossSound.Play();
            ResetPosition();
        }

        if (other.name == "BouncyWall")
        {
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            //rb.AddForce(rb.velocity * -1);
            rb.AddForce(new Vector3(10,10,10));
        }
    }
}
