using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float effectDuration = 5f;

    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;
    public float jumpVelocity = 5f;

    public GameObject bullet;
    public float bulletSpeed = 100f;

    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;

    private float vInput;
    private float hInput;

    private Rigidbody _rb;
    private CapsuleCollider _col;

    public float speedMultiplier = 2f;
    private bool pogoAcquired = false;

    private GameBehavior _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameBehavior>();
    }
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.name)
        {
            case "Enemy":
                {
                    _gameManager.HP -= 1;
                    break;
                }
            case "Health_Pickup":
                {
                    Debug.Log("Health Item collected!");
                    Destroy(collision.gameObject.transform.parent.gameObject);
                    break;
                }
            case "Strength_Pickup":
                {
                    Debug.Log("Strength Item collected!");
                    Destroy(collision.gameObject.transform.parent.gameObject);
                    break;
                }
            case "Speed_Pickup":
                {
                    Debug.Log("Speed Item collected!");
                    SpeedBoost();
                    Destroy(collision.gameObject.transform.parent.gameObject);
                    break;
                }
            case "Pogo_Pickup":
                {
                    Debug.Log("Pogo Item collected!");
                    pogoOn();
                    Destroy(collision.gameObject.transform.parent.gameObject);
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        vInput = Input.GetAxis("Vertical") * moveSpeed;
        hInput = Input.GetAxis("Horizontal") * rotateSpeed;
        /*
        this.transform.Translate(Vector3.forward * vInput * Time.deltaTime);
        this.transform.Rotate(Vector3.up * hInput * Time.deltaTime);
        */
        if ((Input.GetKeyDown(KeyCode.Space) || pogoAcquired) && IsGrounded())
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
        }

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet, this.transform.position + this.transform.right, this.transform.rotation) as GameObject;
            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();
            bulletRB.velocity = this.transform.forward * bulletSpeed;
        }
    }

    void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * hInput;

        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position + this.transform.forward * vInput * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);
    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);

        return grounded;
    }

    void SpeedBoost()
    {
        this.moveSpeed *= speedMultiplier;
        Invoke("SpeedBoostEnd", effectDuration);
    }

    void SpeedBoostEnd()
    {
        this.moveSpeed /= speedMultiplier;
    }

    void pogoOn()
    {
        pogoAcquired = true;
        Invoke("pogoOff", effectDuration);
    }

    void pogoOff()
    {
        pogoAcquired = false;
    }
}
