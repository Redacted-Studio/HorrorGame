using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RDCT.Audio;


public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;
    //Stats
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float walkSpeed;
    private float RunSpeed;
    private float jumpPower;
    private float gravity;
    private float lookSpeed;
    private float lookXLimit;
    private float defaultHeight;
    private float crouchHeight;
    private float crouchSpeed;

    //other
    private bool IsGrounded;
    private bool IsOpenInventory = false;
    private bool IsOpenJournal = false;
    private bool ToggleFlashLight = false;
    private bool IsRunning = false;
    private bool IsCrouch = false;
    [SerializeField] GameObject InventoryUI;
    [SerializeField] GameObject JournalUI;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float JarakInteract;
    private Image invUI;
    ItemGrid inventoryGrid;
    Vector3 movedirection = Vector3.zero;
    RaycastHit hit;
    //Camera
    [SerializeField] private Camera _cam;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InventoryUI = FindAnyObjectByType<GridInteract>().gameObject;
        inventoryGrid = InventoryUI.GetComponent<ItemGrid>();
        invUI = InventoryUI.GetComponent<Image>();
        invUI.enabled = false;
        //InventoryUI.SetActive(false);
    }

    private void Update()
    {
        CheckInteract();
    }

    private void Awake()
        {
        //mindahin data SO ke variabel local
        walkSpeed = _stats.walkSpeed;
        RunSpeed = _stats.RunSpeed;
        jumpPower = _stats.jumpPower;
        gravity = _stats.gravity;
        lookSpeed = _stats.lookSpeed;
        lookXLimit = _stats.lookXLimit;
        defaultHeight = _stats.defaultHeight;
        crouchHeight = _stats.crouchHeight;
        crouchSpeed = _stats.crouchSpeed;

        try
        {
            AudioController.Instance.PlayBGM("SewerAmbiance");
        }
        catch
        {
            Debug.LogWarning("Audio controller is missing from scene.");
        }
    }
    
    

    #region Player_Movement & input
    //Player Movement
    public void ProcessMove(Vector2 input)
    {
        //Movement Input
        movedirection.x = input.x;
        movedirection.z = input.y;

        controller.Move(transform.TransformDirection(movedirection) * walkSpeed * Time.deltaTime);
        
        //apply Gravity
        playerVelocity.y -= gravity * Time.deltaTime;

        //Set Fall Speed Limit
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        //Player Move
        controller.Move(playerVelocity * Time.deltaTime);



    }

    //PlayerRun
    public void Running()
    {
        //Buat toggle Lari
        IsRunning = !IsRunning;
        //kalo true pake RunSpeed, kalo false pake Walkspeed
        if (IsRunning)
        {
            walkSpeed = RunSpeed;
        }
        else
        {
            walkSpeed = _stats.walkSpeed;
        }

    }

    //Player Jump
    public void Jump()
    {
        //Kalo Grounded baru bisa lompat
        if (controller.isGrounded)
        {           
            playerVelocity.y = jumpPower;
        }

    }

    //Player Crouch
    public void Crouch()
    {
        //Toggle Crouch
        IsCrouch = !IsCrouch;
        if (IsCrouch)
        {
            controller.height = crouchHeight;
            walkSpeed = crouchSpeed;
        }
        else
        {
            controller.height = defaultHeight;
            walkSpeed = _stats.walkSpeed;
        }


    }
    //Player Interact
    public void Interact()
    {
        //Kalo Raycast gk nemu apa apa, gk ngapa ngapain
        if (hit.collider == null)
        {
            //Debug.Log("gk ada apa apa");
            return;
        }

        //Kalo ada objek, Function dalam Object dijalanin
        InteractObject interactObject = hit.collider.gameObject.GetComponent<InteractObject>();
        interactObject.Interaction();

    }

    //Player Open/Close Inventory
    public void Inventory()
    {
        //toggle Open/Close Inventory
        IsOpenInventory = !IsOpenInventory;
        
        //InventoryUI.SetActive(IsOpenInventory);

        if (IsOpenInventory)
        {
            invUI.enabled = true;
            inventoryGrid.enableAllItem(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            invUI.enabled = false;
            inventoryGrid.enableAllItem(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        

    }

    //Player On/Off FlashLight
    public void FlashLight()
    {
        ToggleFlashLight = !ToggleFlashLight;
        if (ToggleFlashLight)
        {
            Debug.Log("Nyalain senter");
        }
        else
        {
            Debug.Log("matiin senter");
        }
    }
    // Player Open/Close Journal
    public void Journal()
    {
        IsOpenJournal = !IsOpenJournal;

        if (IsOpenJournal)
        {
            Debug.Log("Buka Journal");
        }
        else
        {
            Debug.Log("Tutup Journal");
        }
    }
    //Player
    #endregion

    #region Camera

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        //calculate camera rotation for up and down
        xRotation -= (mouseY * Time.deltaTime) * lookSpeed;
        xRotation = Mathf.Clamp(xRotation, -lookXLimit, lookXLimit);

        _cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * lookSpeed);
    }

        #endregion

    #region Interact 
    
    public void CheckInteract()
    {
        Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, JarakInteract, layerMask);       
    }
    #endregion

}



