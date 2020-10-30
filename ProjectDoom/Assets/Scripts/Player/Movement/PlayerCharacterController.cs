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

    private GameObject pistolFire;
    private GameObject player;

    private Transform groundCheck;

    public LayerMask whatIsGround;

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundDistance = 0.4f;

    private Animator playerCameraAnimator;

    private float cameraVerticalAngle;

    private int smallHealCount = 0;

    private Camera playerCamera;

    private Vector3 characterVelocityMomentum;
    private Vector3 velocity;

    private bool isMoving;
    private bool isGrounded;
    public bool canShoot = false;

    private HealthSystem healthSystem;

    public delegate void TriggerSmallHeal();
    public delegate void TriggerShoot();
    public delegate void UpdateSmallHealCount();

    public event TriggerSmallHeal SmallHealed;
    public event TriggerShoot Shooted;
    public event UpdateSmallHealCount SmallHealCountUpdated;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        player = this.gameObject;

        healthSystem = new HealthSystem(200);

        characterController = GetComponent<CharacterController>();

        pistolFire = GameObject.Find("Pistol_Fire").gameObject;
        groundCheck = player.transform.Find("GroundCheck");
        playerCamera = transform.Find("PlayerCamera").GetComponent<Camera>();
        playerCameraAnimator = playerCamera.GetComponent<Animator>();

        isMoving = false;
        pistolFire.SetActive(false);

        OnStartMoving += PlayerCharacterController_OnStartMoving;
        OnStopMoving += PlayerCharacterController_OnStopMoving;
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

    void Update()
    {
        CharacterLook();
        MovementHandling();

        if (Input.GetMouseButtonDown(0) && (!canShoot))
        {
            pistolFire.SetActive(true);
            StartCoroutine(FirePistol());
        }

        //Debug.Log(GetHealthSystem().GetHealth());
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
        //Change shooting to raycast later
        UIItemManager uiItemManager = GameObject.FindObjectOfType(typeof(UIItemManager)) as UIItemManager;
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

        Shooted?.Invoke();
    }

    //Movement Handling
    private void MovementHandling()
    {
        //Check if player is grounded
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 16f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 12f;
        }


        Vector3 newPosition = transform.position;

        if (newPosition != lastPosition)
        {
            if (!isMoving)
            {
                isMoving = true;
                OnStartMoving?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            if (isMoving)
            {
                isMoving = false;
                OnStopMoving?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public int GetSmallHealCount()
    {
        return smallHealCount;
    }

    public int AddValue()
    {
        return smallHealCount + 1;
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
        UIItemManager uiItemManager = GameObject.FindObjectOfType(typeof(UIItemManager)) as UIItemManager;
        HealthPickup healthPickup = collider.GetComponent<HealthPickup>();

        //Change the tag system to something better and cleaner
        if(healthPickup != null && GetHealthSystem().GetHealth() != 200 && healthPickup.tag == "5")
        {
            SmallHealCountUpdated?.Invoke();
            healthPickup.DestroySelf();
        }

        if (healthPickup != null && GetHealthSystem().GetHealth() != 200 && healthPickup.tag == "10")
        {
            Heal(10);
            healthPickup.DestroySelf();
        }

        if (healthPickup != null && GetHealthSystem().GetHealth() != 200 && healthPickup.tag == "50")
        {
            Heal(50);
            healthPickup.DestroySelf();
        }
    }
}
