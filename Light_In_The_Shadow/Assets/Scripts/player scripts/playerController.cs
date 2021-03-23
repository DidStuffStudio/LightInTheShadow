using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject[] menuPanels = new GameObject[6]; //0 is main menu, 1 is pause menu, 2 is settings, 3 is inventory, 4 is playPanel(crosshairs), 5 is blur plane
    public bool paused = false, isMainMenu = false;
    private bool playerFrozen;
    public inventorySystem inventory;
    [HideInInspector]
    public bool isRunning;
    public Vector3 respawnLocation;
    public CinemachineVirtualCamera playerCam;


    private PlayerControls playerControls;
    

    public GameObject itemHolder,dropBtn,useBtn;
    public bool inventoryOpen;
    
    public string tagName;
    public float distance;// min distance to object via raycast before being able to pick up the object

    private void Awake()
    {
        playerControls = new PlayerControls();
    }


    private void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) {
            isMainMenu = true;
            FreezePlayer(true);
        }
        else menuPanels[0].SetActive(false);
        respawnLocation = transform.position;
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        playerControls.Player.OpenInventory.performed += _ => OpenInventory();
        playerControls.Player.PickUp.started += _ => pickupObject();
        playerControls.Player.HighlightObject.performed += _ => highlightObject();
        playerControls.Player.PlayPause.performed += _ => PlayPause();

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
        if (paused || isMainMenu) return;
        
        if (!menuPanels[3].activeSelf)
        {
            menuPanels[3].SetActive(true);
            FreezePlayer(true);
        }
        else
        {
            menuPanels[3].SetActive(false);
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
    

    public void PlayPause()
    {

        if (isMainMenu) return;
        ClosePanels();
        if (paused)
        {
            Time.timeScale = 1.0f;
            FreezePlayer(false);
            menuPanels[4].SetActive(true);
            paused = false;
            menuPanels[5].SetActive(false);
        }
        else
        {  
            FreezePlayer(true);
            menuPanels[1].SetActive(true);
            paused = true;
            Time.timeScale = 0.0f;
            menuPanels[5].SetActive(true);
        }
    }


    void ClosePanels()
    {
        foreach (var panel in menuPanels)
        {
            panel.SetActive(false);
        }
    }
    
    public void Settings() {
        ClosePanels();
        menuPanels[2].SetActive(true);
    }

    public void Back() {
        ClosePanels();
        if (isMainMenu)menuPanels[0].SetActive(true);
        else menuPanels[1].SetActive(true);
    }


    public void Quality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void Exit() {
        if (isMainMenu) Application.Quit();
        else SceneManager.LoadScene(0);
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
