using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    //public int targetFrameRate = 60;

    PLayerControls playerControls;

    [Header("Movement Input")]
    [SerializeField] float horizontalInput;
    [SerializeField] float verticalInput;
    public float horizontal_Input;
    public float vertical_Input;

    [Header("Player Action Input")]
    //Dash -  to do

    [Header("UI")]
    [SerializeField] bool pause_input = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        // when the scene changes, run this logic
        SceneManager.activeSceneChanged += OnSceneChange;

        instance.enabled = false;
        //instance.enabled = true; // delete this and uncomment above line later when main menu logic is added

        if (playerControls != null)
        {
            playerControls.Disable();
            //playerControls.Enable(); // delete this and uncomment above line later when main menu logic is added
        }

        //Application.targetFrameRate = targetFrameRate;
        //QualitySettings.vSyncCount = 0;
    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        // If we are loading into our world scene, enable our players controls
        if (newScene.buildIndex == 1)
        {
            instance.enabled = true;

            if (playerControls != null)
            {
                playerControls.Enable();
            }
        }
        // otherwise we must be at the main menu, disable our player controls
        else
        {
            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PLayerControls();

            // Movement
            playerControls.PlayerMovement.Horizontal.performed += i => horizontalInput = i.ReadValue<float>();
            playerControls.PlayerMovement.Horizontal.canceled += i => horizontalInput = 0f;

            playerControls.PlayerMovement.Vertical.performed += i => verticalInput = i.ReadValue<float>();
            playerControls.PlayerMovement.Vertical.canceled += i => verticalInput = 0f;

            // Actions

            //UI
            playerControls.UI.Pause.performed += i => pause_input = true;
        }

        playerControls.Enable();
    }

    private void OnDestroy()
    {
        // if we destroy this object, unsubscribe from this event
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    // if we minimize or lower the window, stop adjusting inputs
    private void OnApplicationFocus(bool focus)
    {
        if (enabled)
        {
            if (focus)
            {
                playerControls.Enable();
            }
            else
            {
                playerControls.Disable();
            }
        }
    }

    private void Update()
    {
        HandleAllInputs();
    }

    private void HandleAllInputs()
    {
        HandlePlayerMovementInput();
        HandlePause();
    }

    // Movement
    private void HandlePlayerMovementInput()
    {
        horizontal_Input = horizontalInput;
        vertical_Input = verticalInput;
    }

    // Action

    //UI
    public void HandlePause()
    {
        if (pause_input)
        {
            pause_input = false;

            if (PauseManager.instance.isPauseMenuOpen == false)
            {
                PauseManager.instance.OpenPauseMenu();
            }
            else
            {
                PauseManager.instance.Continue();
            }
        }
    }
}