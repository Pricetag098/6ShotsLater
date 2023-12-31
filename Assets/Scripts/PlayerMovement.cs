using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
public class PlayerMovement : MonoBehaviour
{
    CapsuleCollider capsuleCollider;
    [SerializeField] Transform cam;
    [SerializeField] Transform head;

    [SerializeField] InputActionProperty move;
    [SerializeField] InputActionProperty rotate;

    [SerializeField] float speed;
    [SerializeField] float rotspeed = 1;

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
        rotate.action.Enable();
    }
    private void OnDisable()
    {
        move.action.Disable();
        rotate.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        head.eulerAngles = new Vector3(0, cam.eulerAngles.y, 0);
        capsuleCollider.height = Vector3.Distance(transform.position,cam.position);
        capsuleCollider.center = new Vector3(cam.localPosition.x, Mathf.Lerp(0,cam.localPosition.y,.5f), cam.localPosition.z);
        inputDir = move.action.ReadValue<Vector2>();
        float a = rotate.action.ReadValue<Vector2>().x;
        transform.RotateAround(cam.position, Vector3.up, a * Time.deltaTime * rotspeed);
    }

    private void FixedUpdate()
    {
        rb.velocity = (head.transform.forward * inputDir.y + head.transform.right * inputDir.x) * speed + new Vector3(0,rb.velocity.y,0);

    }
}
