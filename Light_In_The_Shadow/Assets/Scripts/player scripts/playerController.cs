using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class playerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private Inputmanager inputmanager;
    private Transform cameraTransform;

   


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        inputmanager = Inputmanager.Instance;
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = inputmanager.getPlayerMovement() ;
        Vector3 move = new Vector3(movement.x,0f,movement.y);
        move = cameraTransform.forward * move.z+cameraTransform.right*move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
