using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [Header("Movimiento")]
    public float speed = 6.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Vista en Primera Persona")]
    public Transform cameraTransform; // Arrastra la Main Camera aquí en el Inspector
    public float mouseSensitivity = 15.0f;
    private float xRotation = 0f;

    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Bloquea el cursor en el centro de la pantalla y lo oculta para que no moleste al jugar
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- 1. LÓGICA DE MIRADA (RATÓN) ---
        if (Mouse.current != null && cameraTransform != null)
        {
            // Obtener el movimiento del ratón
            float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
            float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

            // Calcular rotación vertical (mirar arriba/abajo) y limitarla a 90 grados
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            // Aplicar rotaciones
            cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // La cámara rota arriba/abajo
            transform.Rotate(Vector3.up * mouseX); // El cuerpo del jugador rota a los lados
        }

        // --- 2. GRAVEDAD Y SUELO ---
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // --- 3. MOVIMIENTO HORIZONTAL (TECLADO) ---
        Vector2 inputMovement = Vector2.zero;
        if (Keyboard.current != null)
        {
            float moveX = 0;
            float moveZ = 0;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveZ = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveZ = -1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX = 1;

            inputMovement = new Vector2(moveX, moveZ);
        }

        Vector3 move = transform.right * inputMovement.x + transform.forward * inputMovement.y;
        controller.Move(move * speed * Time.deltaTime);

        // --- 4. SALTO ---
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Aplicar gravedad final
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}