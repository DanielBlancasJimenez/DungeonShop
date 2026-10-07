using UnityEngine;

public class Estante : MonoBehaviour
{
    public GameObject productoAlmacenado = null; 
    
    // Crea un Empty GameObject hijo del estante, ponlo en la superficie y arrástralo aquí
    public Transform puntoDeColocacion; 

    public void InteractuarConEstante(PlayerInventory inventario)
    {
        // Si el estante tiene un objeto y el jugador tiene las manos vacías (Recoger del estante)
        if (productoAlmacenado != null && !inventario.TieneObjetoEnMano())
        {
            inventario.Recoger(productoAlmacenado);
            productoAlmacenado = null;
            Debug.Log("Has recogido el producto del estante.");
        }
        // Si el estante está vacío y el jugador tiene un objeto en la mano (Colocar en el estante)
       else if (productoAlmacenado == null && inventario.TieneObjetoEnMano())
        {
            productoAlmacenado = inventario.Soltar();
            
            // NUEVO: Reactiva todos los colisionadores para poder interactuar de nuevo
            Collider[] colliders = productoAlmacenado.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }

            productoAlmacenado.transform.SetParent(puntoDeColocacion);
            productoAlmacenado.transform.localPosition = Vector3.zero;
            productoAlmacenado.transform.localRotation = Quaternion.identity;
            productoAlmacenado.transform.localScale = Vector3.one; 
            
            Debug.Log("Has colocado el producto en el estante.");
        }
    }
}