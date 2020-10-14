using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum ControlType
    {
        WASD,
        Arrows
    }
    
    [Header("Player Control")]
    public ControlType PlayerControlType;
    private string xAxisName;
    private string yAxisName;
    
    [SerializeField] private float movementSpeed = 10f;
    
    private float hInput = 0f;
    private float vInput = 0f;

    [Header("Camera Control")] 
    public bool IsCameraLocked = false;
    [SerializeField] private Camera mainCamera = default;
    [SerializeField] private Camera gunCamera = default;
    [SerializeField] private float cameraSensitivity = 150;
    [SerializeField] private float maxVerticalAngle = 60f;
    [SerializeField] private float minVerticalAngle = -35;
    
    private float mouseX = 0f;
    private float mouseY = 0f;
    
    [Header("Zooming")]
    [SerializeField] private float zoomSmoothing = 10f;
    [SerializeField] private float defaultFOV = 60f;
    [SerializeField] private float zoomFOV = 30f;
    private float targetFOV;
    
    private bool IsZoomEnabled
    {
        set
        {
            targetFOV = value ? zoomFOV : defaultFOV;
        }
    }

    private void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        // Sets correct control type
        xAxisName = PlayerControlType == ControlType.WASD ? "Horizontal" : "Horizontal_Arrows";
        yAxisName = PlayerControlType == ControlType.WASD ? "Vertical" : "Vertical_Arrows";
        
        // Disables zoom by default
        IsZoomEnabled = false;

        // Hides & locks cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
        
        if(!IsCameraLocked)
            HandleZooming();
    }
    
    private void LateUpdate()
    {
        if(!IsCameraLocked)
            CameraControl();
    }

    private void HandleMovement()
    {
        hInput = Input.GetAxis(xAxisName);
        vInput = Input.GetAxis(yAxisName);
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
        transform.Translate(new Vector3(hInput, 0f, vInput) * (movementSpeed * Time.deltaTime));
    }

    private void CameraControl()
    {
        mouseX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
        mouseY -= Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
        
        // Limits camera vertical rotation
        mouseY = Mathf.Clamp(mouseY, minVerticalAngle, maxVerticalAngle);
        
        mainCamera.transform.rotation = Quaternion.Euler(mouseY, mouseX, 0f);
    }

    private void HandleZooming()
    {
        // RMB hold
        IsZoomEnabled = Input.GetMouseButton(1);
        
        // Handles FOV change
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSmoothing);
        gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSmoothing);
    }
}
