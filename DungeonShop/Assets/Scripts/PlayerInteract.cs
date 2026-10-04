using UnityEngine;
using UnityEngine.InputSystem; // Importante: requerimos el nuevo sistema

public class PlayerInteract : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float interactRange = 3f; 
    public Transform raycastOrigin; 

    [Header("Controles")]
    // Permite asignar la tecla directamente desde el Inspector de Unity
    public InputAction interactAction; 

    // En el nuevo Input System, las acciones deben habilitarse y deshabilitarse
    private void OnEnable()
    {
        interactAction.Enable();
    }

    private void OnDisable()
    {
        interactAction.Disable();
    }

    void Update()
    {
        Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * interactRange, Color.green);

        // Verificamos si la acción fue activada en este frame
        if (interactAction.WasPressedThisFrame()) 
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Estante"))
            {
                // Buscamos el script Estante en el objeto que hemos golpeado con el rayo
                Estante estanteImpactado = hit.collider.GetComponent<Estante>();
                
                // Si lo encontramos, llamamos a su método
                if (estanteImpactado != null)
                {
                    estanteImpactado.InteractuarConEstante();
                }
            }
        }
    }
}