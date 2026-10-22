using UnityEngine;

public class CajaRegistradora : MonoBehaviour
{
    [Header("Referencias")]
    // Necesitamos saber a quién darle el dinero
    public EconomiaManager economia; 

    private void Start()
    {
        // Si se nos olvida asignarlo en el Inspector, lo busca automáticamente
        if (economia == null)
        {
            economia = FindObjectOfType<EconomiaManager>();
        }
    }

    public void InteractuarConCaja(PlayerInventory inventario)
    {
        // Comprobamos si el jugador tiene algo en las manos
        if (inventario.TieneObjetoEnMano())
        {
            // 1. Le quitamos el objeto de la mano al jugador
            GameObject objetoAVender = inventario.Soltar();

            // 2. Comprobamos si ese objeto es realmente un Producto que se puede vender
            Producto producto = objetoAVender.GetComponent<Producto>();
            
            if (producto != null)
            {
                // 3. Sumamos el dinero a nuestra economía
                economia.AñadirDinero(producto.precioCobre);
                
                // 4. Destruimos el objeto del mundo 3D (se vendió)
                Destroy(objetoAVender);

                Debug.Log($"¡Has vendido {producto.nombreProducto} por {producto.precioCobre} cobres!");
            }
            else
            {
                // Si por algún motivo cogiste algo que no es un producto (una caja vacía, etc.)
                // se lo devolvemos a la mano
                inventario.Recoger(objetoAVender);
                Debug.Log("Este objeto no se puede vender.");
            }
        }
        else
        {
            Debug.Log("No tienes ningún producto en la mano para vender.");
        }
    }
}