using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerWalkSpeed = 2.0f;
    public float playerRunSpeed = 12.0f;
    private float _privatePlayerSpeed;

    private float jumpHeight = 1.0f;

    //private Inputmanager inputmanager;
    private Transform cameraTransform;

    public GameObject[]
        menuPanels =
            new GameObject[7]; //0 is pause menu, 1 is settings, 2 is inventory, 3 is playPanel(crosshairs), 4 is help menu, 5 is inventory inform

    public bool playerFrozen;
    public InventorySystem inventory;
    [HideInInspector] public bool isRunning;
    public Vector3 respawnLocation;
    public CinemachineVirtualCamera playerCam;
    public PlayerControls playerControls;
    public GameObject itemHolder;
    private bool inventoryOpen, paused, isMainMenu;
    private Interactor interactRayCast;
    public GameObject torch;
    public bool hasTorch, holdingTorch;
    public Text helpText, inventoryInformText;
    private bool _canEquipTorch = true, _canOpenInventory = true, _wasHoldingTorch = false;
    public float gravity = -2;
    public float mouseSensitivity = 150;
    [SerializeField] private ForwardRendererData _forwardRendererData;
    public int playerHealth = 100;
    private Volume _postProcessing;
    public int healthRegenerationRate = 1;
    [SerializeField] private bool regenerateHealth;
    [SerializeField] private GameObject healthPanel;
    private List<Image> _healthPanelImages = new List<Image>();
    [SerializeField] private Slider lookSensitivitySlider;
    private bool _running;
    [SerializeField] private Transform attachPoint;
    public bool frozenForCutscene = false;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }


    private void Start()
    {
        _forwardRendererData.rendererFeatures[0].SetActive(false);
        MasterManager.Instance.LockCursor(false);
       

        interactRayCast = GetComponent<Interactor>();
        respawnLocation = transform.position;
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        playerControls.Player.OpenInventory.performed += _ => OpenInventory();
        playerControls.Player.PickUp.started += _ => PickupObject();
        //playerControls.Player.BreakingIce.performed += _ => BreakingIce();
        playerControls.Player.HighlightObject.performed += _ => HighlightObject();
        playerControls.Player.PlayPause.performed += _ => PlayPause();
        
        playerControls.Player.Torch.performed += _ => EquipTorch(true);
        playerControls.Player.Torch.canceled += _ => EquipTorch(false);

        playerControls.Player.Run.performed += _ => Run(true);
        playerControls.Player.Run.canceled += _ => Run(false);

        torch.SetActive(false);
        _postProcessing = MasterManager.Instance.ppVolume;
        foreach (var image in healthPanel.GetComponentsInChildren<Image>())
        {
            _healthPanelImages.Add(image);
        }
        if (regenerateHealth) StartCoroutine(RegenHealth());

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            isMainMenu = true;
            FreezePlayer(true);
        }
        else
        {
            ClosePanels();
            menuPanels[3].SetActive(true);
        }
    }

    public void LookSensitivity()
    {
        mouseSensitivity = lookSensitivitySlider.value;
    }
    void Run(bool run)
    {
        if (run)
        {
            _running = true;
            _privatePlayerSpeed = playerRunSpeed;
            playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 3.0f;
        }
        else
        {
            _running = false;
            _privatePlayerSpeed = playerWalkSpeed;
            playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 1.0f;
        }
    }

    void FixedUpdate()
    {

        
            if (!_postProcessing.profile.TryGet<Vignette>(out var vignette)) throw new NullReferenceException(nameof(vignette));

            var value = Map(playerHealth, 100, 0, 0, 1);

            vignette.intensity.Override(value);
            
            foreach (var img in _healthPanelImages)
            {
                img.color = new Color(0,0,0,value);
            }
            

        if (playerHealth <= 0)
        {
            RespawnPlayer();
        }
        
        if (controller.isGrounded)
        {
            _privatePlayerSpeed = _running ? playerRunSpeed : playerWalkSpeed;
            playerVelocity.y = gravity;
        }
        else if (!playerFrozen)
        {
            _privatePlayerSpeed = 0.0f;
            playerVelocity.y += gravity * Time.deltaTime;
        }

        if (!playerFrozen)
        {
            var movement = playerControls.Player.Movement.ReadValue<Vector2>();
            var move = new Vector3(movement.x, 0f, movement.y);
            move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
            move.y = 0;
            controller.Move(move * Time.deltaTime * _privatePlayerSpeed);
            controller.Move(playerVelocity * Time.deltaTime);
        }

        torch.transform.parent.transform.rotation = cameraTransform.rotation;
    }

    public void RespawnPlayer()
    {
        if(MasterManager.Instance.levelIndex == 3) FindObjectOfType<BossFight>().RestartLevel();
        playerHealth = 100;
        GetComponent<CharacterController>().enabled = false;
        transform.position = GetComponent<PlayerController>().respawnLocation;
        GetComponent<CharacterController>().enabled = true;
    }

    public void PlayFromMainMenu()
    {
        FreezePlayer(false);
        ClosePanels();
        menuPanels[3].SetActive(true);
        isMainMenu = false;
        MasterManager.Instance.portals[0].SetActive(true);
        MasterManager.Instance.portals[1].SetActive(true);
        MasterManager.Instance.soundtrackMaster.MainThemeVolume(0, 5.0f);
        MasterManager.Instance.soundtrackMaster.PlayLevelMusic(1, true);
        MasterManager.Instance.soundtrackMaster.PlayLevelAmbience(1, true);
        MasterManager.Instance.soundtrackMaster.LevelMusicVolume(1, 100, 5.0f);
        MasterManager.Instance.soundtrackMaster.LevelAmbienceVolume(1, 0, 0.1f);
        MasterManager.Instance.LockCursor(true);


    }

  
    
    public void NewRespawnPoint()
    {
        respawnLocation = transform.position;
    }

    public void FreezePlayer(bool freeze)
    {
        if (freeze)
        {
            interactRayCast.enabled = false;
            playerFrozen = true;
            playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.0f;
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = 0.0f;
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = 0.0f;
            Physics.gravity = new Vector3(0, 0, 0);
        }

        else
        {
            interactRayCast.enabled = true;
            playerFrozen = false;
            playerCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1.0f;
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = mouseSensitivity;
            playerCam.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = mouseSensitivity;
            Physics.gravity = new Vector3(0, gravity, 0);
        }
    }

    public void OpenHelpMenu(bool enable)
    {
        _forwardRendererData.rendererFeatures[0].SetActive(enable);
        var t = 1.0f;
        if (enable) t = 0;
        Time.timeScale = t;
        ClosePanels();
        FreezePlayer(enable);
        menuPanels[3].SetActive(!enable);
        menuPanels[4].SetActive(enable);
        MasterManager.Instance.LockCursor(!enable);
    }

    void OpenInventory()
    {
        if (paused || isMainMenu || !_canOpenInventory) return;



        if (!menuPanels[2].activeSelf) //If inventory is closed open it
        {
            _forwardRendererData.rendererFeatures[0].SetActive(true);
            /*if (torch.activeSelf)
            {
                torch.SetActive(false);
                _wasHoldingTorch = true;
            }
            else _wasHoldingTorch = false;*/
            ClosePanels();
            menuPanels[2].SetActive(true);
            FreezePlayer(true);
            Time.timeScale = 0;
            MasterManager.Instance.LockCursor(false);
        }
        else //If inventory is open close it
        {
            _forwardRendererData.rendererFeatures[0].SetActive(false);
            Time.timeScale = 1.0f;
            ClosePanels();
            FreezePlayer(false);
            Destroy(inventory.rotatableObject);
            inventory.rotatableObject = null;
            inventory.descriptionPanel.SetActive(false);
            //if(hasTorch && _wasHoldingTorch) torch.SetActive(true);
            menuPanels[3].SetActive(true);
            if (!MasterManager.Instance.isInFocusState) MasterManager.Instance.LockCursor(true);
        }
    }


    void PickupObject()
    {
        if (interactRayCast.inventoryItemHit)
        {
            interactRayCast.inventoryItem.gameObject.GetComponent<Outline>().enabled = false;
            Destroy(interactRayCast.inventoryItem.gameObject.GetComponent<Outline>());
            var temp = Instantiate(interactRayCast.inventoryItem.gameObject, itemHolder.transform, false);
            Destroy(temp.transform.GetChild(0).gameObject);
            inventory.itemsInInventory.Add(temp);
            temp.name = temp.GetComponent<item>().id + "UI";
            inventory.idsInInventory.Add(temp.GetComponent<item>().id);
            temp.GetComponent<item>().inInventory = true;
            //temp.GetComponent<Outline>().enabled = false;
            temp.transform.localPosition = new Vector3(-250 + 100 * inventory.itemsInInventory.Count, 0, -10);
            temp.transform.localScale *= 50;
            temp.transform.gameObject.layer = 5;
            interactRayCast.inventoryItemHit = false;
            var item = interactRayCast.inventoryItem.gameObject;
            StartCoroutine(InventoryAddInform(item.GetComponent<item>().name));
            interactRayCast.ReleaseInventoryItem(item);
            Destroy(item);
        }
    }

    void HighlightObject()
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        if (hit && hitInfo.transform.gameObject.layer == 5)
        {
            inventory.ShowHighlightedItem(hitInfo.transform.gameObject);
        }
    }


    public void PlayPause()
    {
        if (isMainMenu)
        {
            PlayFromMainMenu();
            return;
        }
        if (menuPanels[2].activeSelf) return;
        ClosePanels();
        if (paused)
        {
            _forwardRendererData.rendererFeatures[0].SetActive(false);
            Time.timeScale = 1.0f;
            FreezePlayer(false);
            menuPanels[3].SetActive(true);
            paused = false;
            if (!MasterManager.Instance.isInFocusState) MasterManager.Instance.LockCursor(true);
        }
        else
        {
            _forwardRendererData.rendererFeatures[0].SetActive(true);
            MasterManager.Instance.LockCursor(false);
            paused = true;
            FreezePlayer(true);
            menuPanels[0].SetActive(true);
            Time.timeScale = 0;
        }
    }


    void ClosePanels()
    {
        foreach (var panel in menuPanels)
        {
            panel.SetActive(false);
        }
    }
    
    

    public void AttachObjectToPlayer(GameObject gameObject, bool pickup)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = pickup;
        gameObject.GetComponent<Rigidbody>().useGravity = !pickup;
        if (pickup)
        {
            gameObject.transform.SetParent(attachPoint);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            //gameObject.GetComponent<SpringJoint>().connectedBody = attachPoint.GetComponent<Rigidbody>();
        }
        else gameObject.transform.SetParent(MasterManager.Instance.transform);
    }
    
    
    public void Settings()
    {
        ClosePanels();
        menuPanels[1].SetActive(true);
        menuPanels[0].transform.parent.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Low"));
    }

    public void Back()
    {
        ClosePanels();
        menuPanels[0].SetActive(true);
        menuPanels[0].transform.parent.GetComponent<EventSystem>().SetSelectedGameObject(GameObject.Find("Play Button"));
    }


    public void Quality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void Exit() => Application.Quit();


    void EquipTorch(bool equip)
    {
        if (!hasTorch || !_canEquipTorch) return;
        holdingTorch = equip;
        torch.SetActive(equip);
    }

    public void FreezePlayerForCutScene(bool freeze)
    {
        FreezePlayer(freeze);
        _canEquipTorch = !freeze;
        _canOpenInventory = !freeze;
        frozenForCutscene = freeze;
    }

    IEnumerator RegenHealth()
    {
        while (true)
        {
            if (playerHealth < 100)
            {
                playerHealth += healthRegenerationRate; 
            }
            else
            {
                playerHealth = 100;
            }
            yield return new WaitForSeconds(1.0f); 
        }
        
    }


IEnumerator InventoryAddInform(string name)
    {
        inventoryInformText.text = name + " has been added to your inventory (Press tab to view it)";
        menuPanels[5].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        menuPanels[5].SetActive(false);
    }

float Map(float s, float a1, float a2, float b1, float b2)
{
    return b1 + (s-a1)*(b2-b1)/(a2-a1);
}
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
        _forwardRendererData.rendererFeatures[0].SetActive(false);
    }
    
    
}
