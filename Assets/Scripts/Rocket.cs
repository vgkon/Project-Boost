using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    bool collisionsDisabled = false;
    int sceneCount=0;

    [SerializeField] Light thrustLight;

    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float mainThrust = 250f;
    [SerializeField] float levelLoadDelay=2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip win;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] GameSession gameSession;


    [SerializeField] enum State { Alive, Dying, Transcending }
    State state = State.Alive;


    // Start is called before the first frame update
    void Start()
    {
        sceneCount = SceneManager.sceneCountInBuildSettings;
        print(sceneCount);
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        print("paused " + gameSession.GetPaused());
        //somewhere stop sound on death
        if (state == State.Alive && !gameSession.GetPaused())
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebug();
        }
    }

    private void RespondToDebug()
    {
        if (Input.GetKeyDown("l"))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown("g"))
        {
            collisionsDisabled = !collisionsDisabled;
        }
        if (Input.GetKeyDown("c"))
        {
            collisionsDisabled = !collisionsDisabled;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive || collisionsDisabled)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Fuel":
                print("Fuel");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(win);
        winParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == sceneCount - 1)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation

        float rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationSpeed);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))  //can thrust while rotating
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            if(thrustLight.intensity>0) thrustLight.intensity -= 1;
            mainEngineParticles.Stop();
        }
    }

    void ApplyThrust()
    {  
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
        if(thrustLight.intensity <11) thrustLight.intensity += 1;
    }
}
