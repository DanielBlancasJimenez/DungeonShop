using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Estado del Inventario")]
    public GameObject objetoEnMano = null; 
    public Transform puntoDeAgarre; 

    public bool TieneObjetoEnMano()
    {
        return objetoEnMano != null;
    }

    public void Recoger(GameObject nuevoObjeto)
    {
        objetoEnMano = nuevoObjeto;
        
        Rigidbody rb = objetoEnMano.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // NUEVO: Busca todos los colisionadores en el objeto y en sus hijos, y los apaga
        Collider[] colliders = objetoEnMano.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        objetoEnMano.transform.SetParent(puntoDeAgarre);
        objetoEnMano.transform.localPosition = Vector3.zero;
        objetoEnMano.transform.localRotation = Quaternion.identity;
        objetoEnMano.transform.localScale = Vector3.one; 
    }

    public GameObject Soltar()
    {
        // Este método ahora solo quita el objeto de la mano, pero mantiene su Collider apagado para el estante
        GameObject objetoSoltado = objetoEnMano;
        objetoEnMano = null;
        return objetoSoltado;
    }

    // ¡NUEVO MÉTODO PARA TIRAR AL SUELO!
    public void SoltarAlSuelo()
    {
        if (objetoEnMano != null)
        {
            GameObject objeto = Soltar();
            objeto.transform.SetParent(null); 

            // NUEVO: Reactiva todos los colisionadores al tirar el objeto
            Collider[] colliders = objeto.GetComponentsInChildren<Collider>();
            foreach (Collider col in colliders)
            {
                col.enabled = true;
            }

            Rigidbody rb = objeto.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;

            Debug.Log("Has soltado el objeto al suelo.");
        }
    }
}