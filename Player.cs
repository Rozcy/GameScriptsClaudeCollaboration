using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fastMoveSpeed = 10f; // Speed when shift is pressed
    public float edgeBoundary = 10f;
    public float zoomSpeed = 1f;
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public Vector2 screenMoveLimitsX = new Vector2(-10f, 10f);
    public Vector2 screenMoveLimitsY = new Vector2(1f, 10f);

    private GameObject targetToFollow;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandleZoom();
        //HandleEdgeMovement();
        HandleKeyboardMovement();

        if (targetToFollow != null)
        {
            FocusOnTarget();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cam.orthographicSize -= scroll * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
    }

    private void HandleEdgeMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.mousePosition.x >= Screen.width - edgeBoundary)
        {
            moveDirection.x += 10;
        }
        if (Input.mousePosition.x <= edgeBoundary)
        {
            moveDirection.x -= 10;
        }
        if (Input.mousePosition.y >= Screen.height - edgeBoundary)
        {
            //moveDirection.y += 10;
        }
        if (Input.mousePosition.y <= edgeBoundary)
        {
           //moveDirection.y -= 10;
        }

        transform.Translate(moveDirection * Time.deltaTime * moveSpeed, Space.World);
    }

    private void FocusOnTarget()
    {
        // Assuming you want to keep the camera's current Y (height) position
        Vector3 targetPosition = new Vector3(targetToFollow.transform.position.x, cam.transform.position.y, targetToFollow.transform.position.z);
        cam.transform.position = targetPosition;
    }

    private void HandleKeyboardMovement()
    {
        Vector3 newPosition = transform.position;

        // Horizontal input (A and D keys) moves along the X-axis
        newPosition.x += Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        // Vertical input (W and S keys) moves along the Y-axis
        newPosition.y += Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // Clamp the position to the defined limits
        newPosition.x = Mathf.Clamp(newPosition.x, screenMoveLimitsX.x, screenMoveLimitsX.y);
        newPosition.y = Mathf.Clamp(newPosition.y, screenMoveLimitsY.x, screenMoveLimitsY.y);

        // Update the camera's position
        transform.position = newPosition;
    }

}
