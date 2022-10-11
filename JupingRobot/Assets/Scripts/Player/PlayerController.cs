using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public float jumpForce;
    public float gravityScale = 5;

    public float rotateSpeed = 5f;

    private Vector3 moveDirection;

    public CharacterController characterController;
    public Camera playerCamera;
    public GameObject playerModel;

    public Animator animator;
    public int jumpSound;

    public bool isKnocking;
    public float knockBackLength = .5f;
    public float knockBackCounter;
    public Vector2 knockBackPower;

    public GameObject[] playerPices;

    public float bounceForce = 8f;

    public bool stopMove;


    private void Awake() 
    {
        instance = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isKnocking && !stopMove)
        {

        
            float yStore = moveDirection.y;
            // Movimiento
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + transform.right * Input.GetAxisRaw("Horizontal");
            moveDirection.Normalize();
            //moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f , Input.GetAxisRaw("Vertical"));
            //transform.position = transform.position + (moveDirection * Time.deltaTime * moveSpeed);
            moveDirection = moveDirection * moveSpeed;
            moveDirection.y = yStore;

            //Salto
            if(characterController.isGrounded)
            {
                moveDirection.y = 0f;

                if (Input.GetButtonDown("Jump"))
                {
                    moveDirection.y = jumpForce;
                    AudioManager.instance.PlaySfx(jumpSound);  
                }
                
            }

            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            characterController.Move(moveDirection * Time.deltaTime);

            //Rotacion
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);

            }    
        }

        if(isKnocking)
        {
            knockBackCounter -= Time.deltaTime;

            float yStore = moveDirection.y;
            moveDirection = (playerModel.transform.forward * knockBackPower.x);
            moveDirection.y = yStore;

            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

            characterController.Move(moveDirection * Time.deltaTime);

            if(knockBackCounter <= 0)
            {
                isKnocking = false;
            }
        }

        if(stopMove)
        {
            moveDirection = Vector3.zero;
            moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;
            characterController.Move(moveDirection);
        }

        //Animacion
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        animator.SetBool("Grounded", characterController.isGrounded);

    }
        

    public void Knockback()
    {
        isKnocking = true;
        knockBackCounter = knockBackLength;
        Debug.Log("Knoicoked Back");
        moveDirection.y = knockBackPower.y;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    public void Bounce()
    {
        moveDirection.y = bounceForce;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
