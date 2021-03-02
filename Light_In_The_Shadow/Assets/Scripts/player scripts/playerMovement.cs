using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    float horizontal, vertical;
    Rigidbody rb;

    public float mouseSpeed,movementSpeed;    
    public Transform viewport;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        horizontal += mouseSpeed * mouseX;
        vertical -= mouseSpeed * mouseY;
        viewport.transform.eulerAngles = new Vector3(vertical,horizontal,0f);
        transform.eulerAngles = new Vector3(0f, horizontal, 0f);

        Vector3 move = new Vector3(moveHorizontal,0f,moveVertical);
        movePlayer(move);
    }
    void movePlayer(Vector3 direction)
    {
        transform.Translate(direction * movementSpeed * Time.deltaTime);
    }
}
