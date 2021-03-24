using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions.Must;
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
    public GameObject[] menuPanels = new GameObject[7]; //0 is main menu, 1 is pause menu, 2 is settings, 3 is inventory, 4 is playPanel(crosshairs), 5 is blur plane, 6 is help menu
    private bool playerFrozen;
    public inventorySystem inventory;
    [HideInInspector]
    public bool isRunning;
    public Vector3 respawnLocation;
    public CinemachineVirtualCamera playerCam;
    public PlayerControls playerControls;
    public GameObject itemHolder;
    private bool inventoryOpen, paused, isMainMenu;
    private Interactor interactRayCast;
    public GameObject torch;
    private MasterManager _masterManager;
    public bool hasTorch;
    public Text helpText, inventoryInformText;
    private bool _canEquipTorch = true, _canOpenInventory = true, _wasHoldingTorch = false;
    
    

    private void Awake()
    {
        playerControls = new PlayerControls();
    }


    private void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) {
            isMainMenu = true;
            FreezePlayer(true);
            DOFSwitch(false);
        }
        else
        {
            ClosePanels();
            menuPanels[4].SetActive(true);
        }

        interactRayCast = GetComponent<Interactor>();
        respawnLocation = transform.position;
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        playerControls.Player.OpenInventory.performed += _ => OpenInventory();
        playerControls.Player.PickUp.started += _ => pickupObject();
        playerControls.Player.HighlightObject.performed += _ => highlightObject();
        playerControls.Player.PlayPause.performed += _ => PlayPause();
        playerControls.Player.Torch.performed += _ => EquipTorch();
        _masterManager = GetComponentInParent<MasterManager>();
        torch.SetActive(false);

    }
    

    void Update()
    {
        torch.transform.parent.transform.rotation = cameraTransform.rotation;
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

    public void PlayFromMainMenu()
    {
        FreezePlayer(false);
        DOFSwitch(true);
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
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 50.0f;
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 50.0f;
            gravityValue = -9.81f;
        }
    }

    public void OpenHelpMenu(bool enable)
    {
        var t = 1.0f;
        if (enable) t = 0.0f;
        Time.timeScale = t;
        ClosePanels();
        FreezePlayer(enable);
        menuPanels[5].SetActive(enable);
        menuPanels[6].SetActive(enable);
        menuPanels[4].SetActive(!enable);
        DOFSwitch(true);
    }
    void OpenInventory()
    {
        if (paused || isMainMenu || !_canOpenInventory) return;


        
        if (!menuPanels[3].activeSelf)
        {
            if (torch.activeSelf)
            {
                torch.SetActive(false);
                _wasHoldingTorch = true;
            }
            else _wasHoldingTorch = false;

                menuPanels[3].SetActive(true);
            menuPanels[5].SetActive(true);
            FreezePlayer(true);
            DOFSwitch(false);
            Time.timeScale = 0.0f;


        }
        else
        {
            Time.timeScale = 1.0f;
            menuPanels[3].SetActive(false);
            FreezePlayer(false);
            Destroy(inventory.rotatableObject);
            inventory.rotatableObject = null;
            inventory.highlightedItem = null;
            inventory.descriptionPanel.SetActive(false);
            menuPanels[5].SetActive(false);
            DOFSwitch(true);
            if(hasTorch && _wasHoldingTorch) torch.SetActive(true);
           
        }
    }


    void pickupObject()
    {    
        if (interactRayCast.inventoryItemHit)
        {
            GameObject temp = Instantiate(interactRayCast.inventoryItem, itemHolder.transform, false);
            Destroy(temp.transform.GetChild(0).gameObject);
            inventory.itemsInInventory.Add(temp);
            temp.name = temp.name + "UI";
            temp.GetComponent<item>().inInventory = true;
            temp.transform.localPosition = new Vector3(-250 + 100 * inventory.itemsInInventory.Count, 0, -10) ;
            temp.transform.localScale *= 50;
            temp.transform.gameObject.layer = 5;
            interactRayCast.inventoryItemHit = false;
            StartCoroutine(InventoryAddInform(interactRayCast.inventoryItem.GetComponent<item>().name));
            Destroy(interactRayCast.inventoryItem.gameObject);
            interactRayCast.inventoryItem = null;
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

        if (isMainMenu || menuPanels[3].activeSelf) return;
        ClosePanels();
        if (paused)
        {
            Time.timeScale = 1.0f;
            FreezePlayer(false);
            menuPanels[4].SetActive(true);
            paused = false;
            menuPanels[5].SetActive(false);
            DOFSwitch(true);
        }
        else
        {  
            FreezePlayer(true);
            menuPanels[1].SetActive(true);
            paused = true;
            Time.timeScale = 0.0f;
            menuPanels[5].SetActive(true);
            DOFSwitch(false);
        }
    }


    void ClosePanels()
    {
        foreach (var panel in menuPanels)
        {
            panel.SetActive(false);
        }
    }

    void DOFSwitch(bool enabled)
    {
        foreach (var pp in _masterManager.levelPP)
        {
            pp.components[0].active = enabled;
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

    void EquipTorch()
    {
        if (!hasTorch || !_canEquipTorch) return;
        torch.SetActive(!torch.activeSelf);
    }

    public void FreezePlayerForCutScene(bool freeze)
    {
        FreezePlayer(freeze);
        torch.SetActive(!freeze);
        _canEquipTorch = !freeze;
        _canOpenInventory = !freeze;

    }

    IEnumerator InventoryAddInform(string name)
    {
        inventoryInformText.text = "A " + name + " has been added to your inventory (Press tab to view it)";
        menuPanels[7].SetActive(true);
        yield return new WaitForSeconds(5.0f);
        menuPanels[7].SetActive(false);
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
