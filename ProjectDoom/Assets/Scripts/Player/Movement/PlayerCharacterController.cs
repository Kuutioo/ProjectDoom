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

    public GameObject pistolFire;

    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private Animator playerCameraAnimator;
    [SerializeField] private Animator pistolAnimator;

    private Vector3 characterVelocityMomentum;

    private bool isMoving;
    private bool canShoot = false;

    private HealthSystem healthSystem;

    private void Awake()
    {
        pistolFire.SetActive(false);

        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

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

        transform.Rotate(new Vector3(0f, mouseX * mouseSensitivity, 0f), Space.Self);
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
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 lastPosition = transform.position;

        Vector3 characterVelocity = (transform.right * x * moveSpeed + transform.forward * z * moveSpeed);

        characterVelocity += characterVelocityMomentum;

        characterController.Move(characterVelocity * Time.deltaTime);

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

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
