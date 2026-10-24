using UnityEngine;

public class CajaRegistradora : MonoBehaviour
{
    [Header("Referencias")]
    public EconomiaManager economia; 
    
    // NUEVO: Variable para saber si hay un cliente en la fila
    public ClienteIA clienteEsperando = null;

    private void Start()
    {
        if (economia == null)
        {
            economia = FindObjectOfType<EconomiaManager>();
        }
    }

    public void InteractuarConCaja(PlayerInventory inventario)
    {
        // 1. Priorizamos cobrar al cliente si hay alguien esperando
        // 1. Cobrar al cliente si hay alguien esperando Y tiene un producto
        if (clienteEsperando != null && clienteEsperando.productoEnMano != null)
        {
            Producto productoCliente = clienteEsperando.productoEnMano.GetComponent<Producto>();
            
            if (productoCliente != null)
            {
                // Cobramos el valor real del producto
                economia.AñadirDinero(productoCliente.precioCobre);
                Debug.Log($"Has cobrado {productoCliente.precioCobre} al cliente IA.");
            }
            
            clienteEsperando.FinalizarCompra();
            clienteEsperando = null;
        }
        // 2. Si no hay cliente, mantenemos tu lógica anterior de vender objetos de la mano
        else if (inventario.TieneObjetoEnMano())
        {
            GameObject objetoAVender = inventario.Soltar();
            Producto producto = objetoAVender.GetComponent<Producto>();
            
            if (producto != null)
            {
                economia.AñadirDinero(producto.precioCobre);
                Destroy(objetoAVender);
                Debug.Log($"¡Has vendido {producto.nombreProducto} por {producto.precioCobre} cobres!");
            }
            else
            {
                inventario.Recoger(objetoAVender);
                Debug.Log("Este objeto no se puede vender.");
            }
        }
        else
        {
            Debug.Log("No hay clientes esperando ni productos en la mano.");
        }
    }
}