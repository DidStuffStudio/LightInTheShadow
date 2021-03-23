using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class playerController : MonoBehaviour
{
    [HideInInspector]
    public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    //private Inputmanager inputmanager;
    private Transform cameraTransform;

    private bool playerFrozen;
    public inventorySystem inventory;
    [HideInInspector]
    public bool isRunning;

    public Vector3 respawnLocation;
    public CinemachineVirtualCamera playerCam;
    public GameObject blur;
    

    private PlayerControls playerControls;
    

    public GameObject inventoryHolder,itemHolder,dropBtn,useBtn;
    public bool inventoryOpen;
    
    public string tagName;
    public float distance;// min distance to object via raycast before being able to pick up the object

    private void Awake()
    {
        playerControls = new PlayerControls();
    }


    private void Start()
    {
        playerCam = GetComponentInChildren<CinemachineVirtualCamera>();
        respawnLocation = transform.position;
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        playerControls.Player.OpenInventory.performed += _ => OpenInventory();
        playerControls.Player.PickUp.started += _ => pickupObject();
        playerControls.Player.HighlightObject.performed += _ => highlightObject();

    }
    

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (!playerFrozen)
        {
        Vector2 movement = playerControls.Player.Movement.ReadValue<Vector2>();

        Vector3 move = new Vector3(movement.x,0f,movement.y);
        move = cameraTransform.forward * move.z+cameraTransform.right*move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);        
        }
    }

    public void NewRespawnPoint()
    {
        respawnLocation = transform.position;
    }

    public void FreezePlayer(bool freeze)
    {
        if (freeze)
        {
            playerFrozen = true;
            playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.0f;
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0.0f;
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0.0f;
            gravityValue = 0.0f;
        }

        else
        {
            playerFrozen = false;
            playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1.0f;
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 300.0f;
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 300.0f;
            gravityValue = -9.81f;
        }
    }

    void OpenInventory()
    {
        if (!inventoryHolder.activeSelf)
        {
            inventoryHolder.SetActive(true);
            FreezePlayer(true);
            blur.SetActive(true);
        }
        else
        {
            blur.SetActive(false);
            inventoryHolder.SetActive(false);
            FreezePlayer(false);
            Destroy(inventory.rotatableObject);
            inventory.rotatableObject = null;
            inventory.highlightedItem = null;
            inventory.descriptionPanel.SetActive(false);
        }
    }


    void pickupObject()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit && hitInfo.distance < distance)
        { 
            if (hitInfo.transform.gameObject.tag == tagName)
            {
                GameObject temp = Instantiate(hitInfo.transform.gameObject, itemHolder.transform, false);
                
                inventory.itemsInInventory.Add(hitInfo.transform.gameObject);
                temp.name = "UI";
                temp.transform.localPosition = new Vector3(100 + 100 * inventory.itemsInInventory.Count, 0, -10) ;
                temp.transform.localScale *= 25;
                temp.transform.gameObject.layer = 5;
                hitInfo.transform.gameObject.SetActive(false);
            }
        }
    }
    void highlightObject()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit && hitInfo.transform.gameObject.layer == 5)
        {
            inventory.showHightlightedItem(hitInfo.transform.gameObject);
        }
        
            
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();

    }
}
