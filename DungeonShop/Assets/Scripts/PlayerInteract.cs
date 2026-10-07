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
        
        // Si el rayo choca con algo...
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, interactRange))
        {
            // 1. Miramos un Estante
            if (hit.collider.CompareTag("Estante"))
            {
                // Busca el script Estante en el cubo golpeado o en sus padres (EstanteBase)
                Estante estanteImpactado = hit.collider.GetComponentInParent<Estante>();
                if (estanteImpactado != null)
                {
                    estanteImpactado.InteractuarConEstante(inventory);
                }
            }
            // 2. Miramos un Producto suelto
           // 2. Miramos un Producto
            else if (hit.collider.CompareTag("Producto"))
            {
                if (!inventory.TieneObjetoEnMano())
                {
                    // NUEVO: Comprobamos si el producto está emparentado a un estante
                    Estante estantePadre = hit.collider.GetComponentInParent<Estante>();
                    
                    if (estantePadre != null)
                    {
                        // Si está en un estante, le mandamos la interacción al estante para que él gestione el vaciado
                        estantePadre.InteractuarConEstante(inventory);
                    }
                    else
                    {
                        // Si no hay estante, es que está suelto por el suelo
                        inventory.Recoger(hit.collider.gameObject);
                        Debug.Log("Has recogido un producto del suelo.");
                    }
                }
                else
                {
                    Debug.Log("Ya tienes las manos llenas.");
                }
            }
        }
        // Si el rayo no choca con absolutamente nada (mirando al cielo/vacío)
        else
        {
            if (inventory.TieneObjetoEnMano())
            {
                inventory.SoltarAlSuelo();
            }
        }
    }
    private PlayerInventory inventory;

    private void Awake()
    {
        // Obtiene el script de inventario asignado a este mismo GameObject
        inventory = GetComponent<PlayerInventory>();
    }
    
}