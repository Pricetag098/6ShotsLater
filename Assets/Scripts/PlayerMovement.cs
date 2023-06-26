using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class PlayerMovement : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    [SerializeField] Transform camera;
    [SerializeField] Transform head;

    [SerializeField] InputActionProperty move;

    [SerializeField] float speed;
    
    Vector2 inputDir;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        move.action.Enable();
    }
    private void OnDisable()
    {
        move.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        head.eulerAngles = new Vector3(0, camera.eulerAngles.y, 0);
        capsuleCollider.height = Vector3.Distance(transform.position,camera.position);
        capsuleCollider.center = new Vector3(camera.localPosition.x, Mathf.Lerp(0,camera.localPosition.y,.5f), camera.localPosition.z);
        inputDir = move.action.ReadValue<Vector2>();
        
    }

    private void FixedUpdate()
    {
        rb.velocity = (head.transform.forward * inputDir.y + head.transform.right * inputDir.x) * speed + new Vector3(0,rb.velocity.y,0);

    }
}
