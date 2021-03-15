using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

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
<<<<<<< Updated upstream
    private bool playerFrozen;
    [HideInInspector]
    public bool isRunning;

    public Vector3 respawnLocation;
    public CinemachineVirtualCamera playerCam;
    
    
=======
    private PlayerControls playerControls;
    
    [HideInInspector]
    public bool isRunning;

    public GameObject inventoryHolder,itemHolder;
    public bool inventoryOpen;
    private inventorySystem inventory;
    public string tagName;
    public float distance;// min distance to object via raycast before being able to pick up the object

    private void Awake()
    {
        playerControls = new PlayerControls();
        inventory = GameObject.Find("inventory").GetComponent<inventorySystem>();
    }

>>>>>>> Stashed changes
    private void Start()
    {
        playerCam = GetComponentInChildren<CinemachineVirtualCamera>();
        respawnLocation = transform.position;
        controller = GetComponent<CharacterController>();
        //inputmanager = Inputmanager.Instance;
        cameraTransform = Camera.main.transform;

        playerControls.Player.OpenInventory.performed += _ => OpenInventory();
        playerControls.Player.PickUp.started += _ => pickupObject();

    }
    

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
<<<<<<< Updated upstream

        if (!playerFrozen)
        {

        Vector2 movement = inputmanager.getPlayerMovement() ;
=======
        
        Vector2 movement = playerControls.Player.Movement.ReadValue<Vector2>();
>>>>>>> Stashed changes
        Vector3 move = new Vector3(movement.x,0f,movement.y);
        move = cameraTransform.forward * move.z+cameraTransform.right*move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
<<<<<<< Updated upstream
                    
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

=======

       
        
    }
    void OpenInventory()
    {
        if (!inventoryHolder.activeSelf)
        {
            inventoryHolder.SetActive(true);
        }
        else
        {
            inventoryHolder.SetActive(false);

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
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                GameObject temp = Instantiate(hitInfo.transform.gameObject, itemHolder.transform, false);
                temp.transform.localPosition = new Vector3(100 + 100 * inventory.itemsInInventory.Count, 0, -10) ;
                temp.transform.localScale *= 25;
                temp.transform.gameObject.layer = 5;
                inventory.itemsInInventory.Add(temp);
                Destroy(hitInfo.transform.gameObject);

            }

        }
    }



    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
>>>>>>> Stashed changes
    }
}
