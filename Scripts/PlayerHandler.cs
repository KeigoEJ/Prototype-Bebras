using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private float playerSpeed;
    public bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!canMove) return; // Prevent movement if disabled

        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.MovePosition(rb.position + movement * playerSpeed * Time.fixedDeltaTime);
    }
}

