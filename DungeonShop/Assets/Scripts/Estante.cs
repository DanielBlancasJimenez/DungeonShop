using UnityEngine;

public class Estante : MonoBehaviour
{
    // Variable para saber si el estante está ocupado
    public bool tieneProducto = false; 
    
    // Referencia opcional a un modelo visual (un cubo más pequeño, por ejemplo)
    public GameObject productoVisual; 

    public void InteractuarConEstante()
    {
        if (tieneProducto)
        {
            Debug.Log("Has recogido el producto de la estantería.");
            tieneProducto = false;
            
            // Ocultamos el modelo del producto
            if (productoVisual != null) productoVisual.SetActive(false); 
        }
        else
        {
            Debug.Log("Has colocado un producto en la estantería.");
            tieneProducto = true;
            
            // Mostramos el modelo del producto
            if (productoVisual != null) productoVisual.SetActive(true);
        }
    }
}