using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    private Transform gunHolder;
    public Transform gun;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Camera topDownCamera;
    [SerializeField] private Vector3 cameraOffset;
    private Player player;
    
    private Vector3 velocity;

    private void Start(){
        gunHolder = animator.GetBoneTransform(HumanBodyBones.RightHand);
        gun.SetParent(gunHolder);
        gun.SetLocalPositionAndRotation(new Vector3(0.07f, 0f, 0.015f), Quaternion.Euler(-9.2f, 64.4f, -90f));
        gun.localScale = Vector3.one;

        player = GetComponent<Player>();
    }
    private void Update(){
        if (!player.isAlive) {
            audioSource.Pause();
            return;
        }

        // get input from player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        // move player
        Vector3 left = topDownCamera.transform.right.normalized;
        Vector3 forward = (topDownCamera.transform.forward.normalized - Vector3.Project(topDownCamera.transform.forward.normalized, Vector3.up)).normalized;
        Vector3 move = left * x + forward * z;
        move = Vector3.ClampMagnitude(move, 1f);
        controller.Move(speed * Time.deltaTime * move);
        
        animator.SetFloat("Speed", move.magnitude);
        if (move.magnitude > 0.1f){
            audioSource.UnPause();
        }
        else{
            audioSource.Pause();
        }


        // apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // move and rotate camera
        topDownCamera.transform.position = transform.position + cameraOffset;
        topDownCamera.transform.LookAt(transform);

        // rotate player
        Ray ray = topDownCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f)) {
            Vector3 lookAt = hit.point;
            lookAt.y = transform.position.y;
            transform.LookAt(lookAt);

            float angle = Vector3.Angle(lookAt - transform.position, Vector3.ProjectOnPlane(gun.forward, Vector3.up));
            transform.RotateAround(transform.position, Vector3.up, angle);
        }

    }

    public void MoveToStart(){
        controller.enabled = false;
        transform.position = new Vector3(2.47f, 1.77f, -0.92f);
        controller.enabled = true;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(topDownCamera.ScreenPointToRay(Input.mousePosition).GetPoint(10f), 0.1f);
        Gizmos.DrawRay(new Ray(transform.position, transform.forward));
        Gizmos.DrawRay(new Ray(gun.position, gun.forward));
    }
}
