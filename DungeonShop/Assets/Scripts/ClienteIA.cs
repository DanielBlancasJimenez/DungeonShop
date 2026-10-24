using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ClienteIA : MonoBehaviour
{
    [Header("Ruta del Cliente")]
    public Transform estanteDestino;
    public Transform cajaDestino;

    [Header("Scripts de la Tienda")]
    public CajaRegistradora scriptCajaRegistradora;
    
    [Header("Inventario IA")]
    public Transform puntoAgarreIA; 
    public GameObject productoEnMano = null; 
    
    private NavMeshAgent agente;
    private enum EstadoCliente { YendoAEstante, YendoACaja, EsperandoAtencion }
    private EstadoCliente estadoActual = EstadoCliente.YendoAEstante;
    private bool destinoInicialFijado = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!agente.isOnNavMesh) return;

        if (!destinoInicialFijado)
        {
            if (estanteDestino != null)
            {
                agente.SetDestination(estanteDestino.position);
            }
            destinoInicialFijado = true;
        }

        if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance)
        {
            if (estadoActual == EstadoCliente.YendoAEstante)
            {
                // Extraemos el objeto del estante
                Estante estante = estanteDestino.GetComponentInParent<Estante>();
                if (estante != null)
                {
                    productoEnMano = estante.ExtraerProducto();
                    
                    if (productoEnMano != null)
                    {
                        // Desactivamos colisiones para que no arrastre al personaje
                        Collider[] colliders = productoEnMano.GetComponentsInChildren<Collider>();
                        foreach (Collider col in colliders) col.enabled = false;

                        // Posicionamos el objeto en la mano del cliente
                        productoEnMano.transform.SetParent(puntoAgarreIA);
                        productoEnMano.transform.localPosition = Vector3.zero;
                        productoEnMano.transform.localRotation = Quaternion.identity;
                    }
                }

                estadoActual = EstadoCliente.YendoACaja;
                
                if (cajaDestino != null)
                {
                    agente.SetDestination(cajaDestino.position);
                }
            }
            else if (estadoActual == EstadoCliente.YendoACaja)
            {
                estadoActual = EstadoCliente.EsperandoAtencion;
                
                // NUEVO: En lugar de buscar en el padre, usamos la referencia directa
                if (scriptCajaRegistradora != null)
                {
                    scriptCajaRegistradora.clienteEsperando = this;
                    Debug.Log("IA: He llegado a la caja y estoy listo para que me cobres.");
                }
            }
        }
    }

    public void FinalizarCompra()
    {
        if (productoEnMano != null)
        {
            Destroy(productoEnMano);
        }
        Destroy(gameObject);
    }
}