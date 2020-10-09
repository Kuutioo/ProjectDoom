using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterController : MonoBehaviour
{
    private CharacterController characterController;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;
    public event EventHandler OnShoot;

    private GameObject pistolFire;
    private GameObject projectDoomGuyPistol;
    private GameObject projectDoomGuy;

    private Transform groundCheck;

    public LayerMask whatIsGround;

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundDistance = 0.4f;

    private Animator playerCameraAnimator;
    private Animator pistolAnimator;

    private float cameraVerticalAngle;

    private Camera playerCamera;

    private Vector3 characterVelocityMomentum;
    private Vector3 velocity;

    private bool isMoving;
    private bool isGrounded;
    private bool canShoot = false;

    private HealthSystem healthSystem;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        healthSystem = new HealthSystem(200);

        pistolFire = GameObject.Find("Pistol_Fire").gameObject;

        pistolFire.SetActive(false);

        projectDoomGuyPistol = GameObject.Find("Weapon").gameObject;
        pistolAnimator = projectDoomGuyPistol.GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();

        playerCamera = transform.Find("PlayerCamera").GetComponent<Camera>();
        playerCameraAnimator = playerCamera.GetComponent<Animator>();

        projectDoomGuy = this.gameObject;

        groundCheck = projectDoomGuy.transform.Find("GroundCheck");
       
        isMoving = false;

        OnStartMoving += PlayerCharacterController_OnStartMoving;
        OnStopMoving += PlayerCharacterController_OnStopMoving;
        OnShoot += PlayerCharacterController_OnShoot;
    }


    //Animator stuff
    private void PlayerCharacterController_OnStopMoving(object sender, EventArgs e)
    {
        playerCameraAnimator.SetBool("IsWalking", isMoving);
    }

    private void PlayerCharacterController_OnStartMoving(object sender, EventArgs e)
    {
        playerCameraAnimator.SetBool("IsWalking", isMoving);
    }

    private void PlayerCharacterController_OnShoot(object sender, EventArgs e)
    {
        pistolAnimator.SetTrigger("Shoot");
    }

    void Update()
    {
        CharacterLook();
        MovementHandling();

        if (Input.GetMouseButtonDown(0) && (!canShoot))
        {
            pistolFire.SetActive(true);
            StartCoroutine(FirePistol());
        }

        Debug.Log(GetHealthSystem().GetHealth());
    }

    private IEnumerator FirePistol()
    {
        canShoot = true;
        ShootHandling();

        yield return new WaitForSeconds(0.075f);

        pistolFire.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        
        canShoot = false;
    }

    private void CharacterLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        transform.Rotate(new Vector3(0f, mouseX * mouseSensitivity, 0f), Space.Self);

        cameraVerticalAngle -= mouseY * mouseSensitivity;
        cameraVerticalAngle = Mathf.Clamp(cameraVerticalAngle, -89f, 89f);

        playerCamera.transform.localEulerAngles = new Vector3(cameraVerticalAngle, 0, 0);
    }

    //Shoot Handling
    private void ShootHandling()
    {
        Vector3 halfBoxSize = new Vector3(0.5f, 0.55f, 20f);
        float playerHeightOffset = 0.5f;
        Collider[] colliderArray = Physics.OverlapBox(transform.position + transform.up * playerHeightOffset + transform.forward * halfBoxSize.z, halfBoxSize, transform.rotation);

        foreach(Collider collider in colliderArray)
        {
            EnemyHealth shootCube = collider.GetComponent<EnemyHealth>();

            if(shootCube != null)
            {
                shootCube.Damage();
                break;
            }
        }

        OnShoot?.Invoke(this, EventArgs.Empty);
        Debug.Log("Shoot");
    }

    //Movement Handling
    private void MovementHandling()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 lastPosition = transform.position;

        Vector3 characterVelocity = (transform.right * x * moveSpeed + transform.forward * z * moveSpeed);

        characterVelocity += characterVelocityMomentum;

        characterController.Move(characterVelocity * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        if (characterVelocityMomentum.magnitude > 0f)
        {
            float momentumDrag = 3f;
            characterVelocityMomentum -= characterVelocityMomentum * momentumDrag * Time.deltaTime;

            if (characterVelocityMomentum.magnitude < .0f)
            {
                characterVelocityMomentum = Vector3.zero;
            }
        }

        Vector3 newPosition = transform.position;

        if (newPosition != lastPosition)
        {
            if (!isMoving)
            {
                isMoving = true;
                OnStartMoving?.Invoke(this, EventArgs.Empty);
                Debug.Log("Is moving");
            }
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                Debug.Log("Not moving");
            }
        }
    }

    //Health System stuff
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }

    public void Heal(int healAmount)
    {
        healthSystem.Heal(healAmount);
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    private void OnTriggerEnter(Collider collider)
    {
        HealthPickup healthPickup = collider.GetComponent<HealthPickup>();

        if(healthPickup != null && GetHealthSystem().GetHealth() != 200)
        {
            Heal(50);
            healthPickup.DestroySelf();
        }
    }
}
