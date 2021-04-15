using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matress : MonoBehaviour
{
    private bool pickedUp = false, canPickUp = true;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float lerpStrength, distanceToCorrect = 2.0f;
    [SerializeField] private BeforeBossLevel3 logic;
		
    void OnMouseDown()
    {
        pickedUp = !pickedUp;
        GetComponent<Collider>().isTrigger = pickedUp;
        MasterManager.Instance.player.AttachObjectToPlayer(gameObject, pickedUp);
    }

    void Update()
    {
        if (pickedUp || !canPickUp) return;
        if (Vector3.Distance(transform.position, targetTransform.position) > distanceToCorrect) return;
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, Time.deltaTime * lerpStrength);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, Time.deltaTime * lerpStrength);
        if (Vector3.Distance(transform.position, targetTransform.position) > 0.5f) return;
        canPickUp = false;
        logic.PlayJumping();
        
        // The matress is placed correctly, so let the boy jump and trigger the chase scene
    }
}
