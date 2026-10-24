using UnityEngine;
using System.Collections;

public class ClientManager : MonoBehaviour
{
    [Header("Configuración de Generación")]
    public GameObject clientePrefab;
    public float tiempoMinimoSpawn = 5f;
    public float tiempoMaximoSpawn = 15f;
    public int maxClientesEnTienda = 3;

    [Header("Rutas para los Clientes")]
    public Transform estanteDestino;
    public Transform puntoCaja;
    public CajaRegistradora scriptCajaRegistradora;

    void Start()
    {
        // Iniciamos el temporizador infinito al empezar el juego
        StartCoroutine(RutinaSpawnClientes());
    }

    IEnumerator RutinaSpawnClientes()
    {
        while (true)
        {
            // Esperamos un tiempo aleatorio antes de crear el siguiente cliente
            float tiempoEspera = Random.Range(tiempoMinimoSpawn, tiempoMaximoSpawn);
            yield return new WaitForSeconds(tiempoEspera);

            // Comprobamos cuántos clientes hay vivos en la escena
            ClienteIA[] clientesEnTienda = FindObjectsOfType<ClienteIA>();

            // Solo creamos uno nuevo si no hemos superado el límite
            if (clientesEnTienda.Length < maxClientesEnTienda)
            {
                GenerarCliente();
            }
        }
    }

    void GenerarCliente()
    {
        // 1. Creamos la copia del cliente exactamente en la posición de este Spawner
        GameObject nuevoClienteObj = Instantiate(clientePrefab, transform.position, Quaternion.identity);
        
        // 2. Buscamos su script de IA para darle las instrucciones
        ClienteIA scriptCliente = nuevoClienteObj.GetComponent<ClienteIA>();
        
        if (scriptCliente != null)
        {
            // 3. Le pasamos las referencias que necesita para saber a dónde ir
            scriptCliente.estanteDestino = estanteDestino;
            scriptCliente.cajaDestino = puntoCaja;
            scriptCliente.scriptCajaRegistradora = scriptCajaRegistradora;
        }
    }
}