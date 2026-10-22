using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Configuración de Interacción")]
    public float interactRange = 3f; 
    public Transform raycastOrigin; 

    [Header("Controles")]
    public InputAction interactAction; 

    [Header("Referencias")]
    public HUDManager hudManager; // Arrastra tu objeto con el UI Document aquí
    private PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

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

        // 1. Comprobamos constantemente qué estamos mirando para actualizar la UI
        ComprobarVision();

        // 2. Comprobamos si pulsamos el botón para hacer la acción física
        if (interactAction.WasPressedThisFrame()) 
        {
            TryInteract();
        }
    }

    void ComprobarVision()
    {
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Producto"))
            {
                Producto producto = hit.collider.GetComponent<Producto>();
                if (producto != null)
                {
                    // El error estaba aquí: cambiamos precio por precioCobre
                    hudManager.MostrarInfoProducto(producto.nombreProducto, producto.precioCobre);
                    return; 
                }
            }
        }
        
        // Si el rayo no choca con un Producto, ocultamos el panel
        hudManager.OcultarInfo();
    }

    void TryInteract()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, interactRange))
        {
            if (hit.collider.CompareTag("Estante"))
            {
                Estante estanteImpactado = hit.collider.GetComponentInParent<Estante>();
                if (estanteImpactado != null)
                {
                    estanteImpactado.InteractuarConEstante(inventory);
                }
            }
            else if (hit.collider.CompareTag("Caja"))
            {
                CajaRegistradora cajaImpactada = hit.collider.GetComponentInParent<CajaRegistradora>();
                if (cajaImpactada != null)
                {
                    cajaImpactada.InteractuarConCaja(inventory);
                }
            }

            //----------------------------------------------------

            else if (hit.collider.CompareTag("Producto"))
            {
                if (!inventory.TieneObjetoEnMano())
                {
                    Estante estantePadre = hit.collider.GetComponentInParent<Estante>();
                    if (estantePadre != null)
                    {
                        estantePadre.InteractuarConEstante(inventory);
                    }
                    else
                    {
                        inventory.Recoger(hit.collider.gameObject);
                        Debug.Log("Has recogido un producto del suelo.");
                    }
                }
                else
                {
                    Debug.Log("Ya tienes las manos llenas.");
                }
                
            }
            else
            {
                if (inventory.TieneObjetoEnMano())
                {
                    inventory.SoltarAlSuelo();
                }
            }
        }
        else
        {
            if (inventory.TieneObjetoEnMano())
            {
                inventory.SoltarAlSuelo();
            }
        }
    }
}