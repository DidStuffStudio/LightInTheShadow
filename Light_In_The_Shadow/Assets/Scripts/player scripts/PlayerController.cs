using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    private float _privatePlayerSpeed;

    private float jumpHeight = 1.0f;

    //private Inputmanager inputmanager;
    private Transform cameraTransform;

    public GameObject[]
        menuPanels =
            new GameObject[7]; //0 is main menu, 1 is pause menu, 2 is settings, 3 is inventory, 4 is playPanel(crosshairs), 5 is help menu, 6 is inventory inform

    private bool playerFrozen;
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
    public int mouseSensitivity = 150;
    [SerializeField] private ForwardRendererData _forwardRendererData;
    public int playerHealth = 100;
    private Volume _postProcessing;
    public int healthRegenerationRate = 1;
    [SerializeField] private bool regenerateHealth;
    private int _healthWas;
    [SerializeField] private GameObject healthPanel;
    private List<Image> _healthPanelImages = new List<Image>();


    private void Awake()
    {
        playerControls = new PlayerControls();
    }


    private void Start()
    {
        _forwardRendererData.rendererFeatures[0].SetActive(false);
        MasterManager.Instance.LockCursor(false);
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            isMainMenu = true;
            FreezePlayer(true);
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
        playerControls.Player.PickUp.started += _ => PickupObject();
        playerControls.Player.BreakingIce.performed += _ => breakingIce();
        playerControls.Player.HighlightObject.performed += _ => HighlightObject();
        playerControls.Player.PlayPause.performed += _ => PlayPause();
        playerControls.Player.Torch.performed += _ => EquipTorch(true);
        playerControls.Player.Torch.canceled += _ => EquipTorch(false);

        torch.SetActive(false);
        _postProcessing = MasterManager.Instance.ppVolume;
        foreach (var image in healthPanel.GetComponentsInChildren<Image>())
        {
            _healthPanelImages.Add(image);
        }
        if (regenerateHealth) StartCoroutine(RegenHealth());

    }


    void FixedUpdate()
    {

        if (playerHealth != _healthWas)
        {
            //if (!_postProcessing.profile.TryGet<Vignette>(out var vignette))
              //  throw new NullReferenceException(nameof(vignette));

            var value = Map(playerHealth, 100, 0, 0, 1);

            //vignette.intensity.Override(value);
            foreach (var img in _healthPanelImages)
            {
                img.color = new Color(0,0,0,value);
            }

        }

        if (playerHealth <= 0)
        {
            RespawnPlayer();
        }

        groundedPlayer = controller.isGrounded;

        _privatePlayerSpeed = !controller.isGrounded ? 0.0f : playerSpeed;

        if (controller.isGrounded)
        {
            _privatePlayerSpeed = playerSpeed;
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
        _healthWas = playerHealth;
    }

    public void RespawnPlayer()
    {
        if(MasterManager.Instance.levelIndex == 3) FindObjectOfType<BossFight>().RestartLevel();
        playerHealth = 100;
        transform.position = GetComponent<PlayerController>().respawnLocation;
    }

    public void PlayFromMainMenu()
    {
        FreezePlayer(false);
        ClosePanels();
        menuPanels[4].SetActive(true);
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

    public void breakingIce()
    {
        for (int i = 0; i < inventory.itemsInInventory.Count; i++)
        {
            if (inventory.itemsInInventory[i].GetComponent<item>().id.Contains("420"))
            {
                GameObject.Find("ice to meet you").GetComponent<iceScript>().counterForIce++;
            }
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
            Physics.gravity = new Vector3(0, 0, 0);
        }

        else
        {
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
        menuPanels[4].SetActive(!enable);
        menuPanels[5].SetActive(enable);
        MasterManager.Instance.LockCursor(!enable);
    }

    void OpenInventory()
    {
        if (paused || isMainMenu || !_canOpenInventory) return;



        if (!menuPanels[3].activeSelf) //If inventory is closed open it
        {
            _forwardRendererData.rendererFeatures[0].SetActive(true);
            /*if (torch.activeSelf)
            {
                torch.SetActive(false);
                _wasHoldingTorch = true;
            }
            else _wasHoldingTorch = false;*/
            ClosePanels();
            menuPanels[3].SetActive(true);
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
            menuPanels[4].SetActive(true);
            if (!MasterManager.Instance.isInFocusState) MasterManager.Instance.LockCursor(true);
        }
    }


    void PickupObject()
    {
        if (interactRayCast.inventoryItemHit)
        {
            var temp = Instantiate(interactRayCast.inventoryItem.gameObject, itemHolder.transform, false);
            Destroy(temp.transform.GetChild(0).gameObject);
            inventory.itemsInInventory.Add(temp);
            temp.name = temp.name + "UI";
            inventory.idsInInventory.Add(temp.GetComponent<item>().id);
            temp.GetComponent<item>().inInventory = true;
            temp.GetComponent<Outline>().enabled = false;
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

        if (isMainMenu || menuPanels[3].activeSelf) return;
        ClosePanels();
        if (paused)
        {
            _forwardRendererData.rendererFeatures[0].SetActive(false);
            Time.timeScale = 1.0f;
            FreezePlayer(false);
            menuPanels[4].SetActive(true);
            paused = false;
            if (!MasterManager.Instance.isInFocusState) MasterManager.Instance.LockCursor(true);
        }
        else
        {
            _forwardRendererData.rendererFeatures[0].SetActive(true);
            MasterManager.Instance.LockCursor(false);
            paused = true;
            FreezePlayer(true);
            menuPanels[1].SetActive(true);
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

    public void Settings()
    {
        ClosePanels();
        menuPanels[2].SetActive(true);
    }

    public void Back()
    {
        ClosePanels();
        if (isMainMenu) menuPanels[0].SetActive(true);
        else
        {
            menuPanels[1].SetActive(true);
        }
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
        //if(!freeze)EquipTorch();
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
        inventoryInformText.text = "A " + name + " has been added to your inventory (Press tab to view it)";
        menuPanels[6].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        menuPanels[6].SetActive(false);
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
