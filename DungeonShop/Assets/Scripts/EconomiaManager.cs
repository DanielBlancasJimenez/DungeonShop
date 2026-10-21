using UnityEngine;

public class EconomiaManager : MonoBehaviour
{
    [Header("Dinero del Jugador")]
    // Todo el dinero se guarda internamente como la moneda más pequeña
    public long cobreTotal = 0; 

    // Referencia al HUD para actualizar el texto
    public HUDManager hudManager;

    private void Start()
    {
        // Para probar, empezamos con algo de dinero (ej. 1 Oro, 5 Plata, 30 Cobre = 10530)
        AñadirDinero(105300306040); 
    }

    public void AñadirDinero(long cantidadCobre)
    {
        cobreTotal += cantidadCobre;
        ActualizarHUD();
    }

    public bool GastarDinero(long cantidadCobre)
    {
        if (cobreTotal >= cantidadCobre)
        {
            cobreTotal -= cantidadCobre;
            ActualizarHUD();
            return true; // Compra exitosa
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
            return false; // Compra fallida
        }
    }

   private void ActualizarHUD()
    {
        if (hudManager != null)
        {
            CalcularYMostrarMonedas(cobreTotal);
        }
    }

    // La magia matemática ocurre aquí
    // private string ConvertirCobreAString(long total)
    // {
    //     long oricalco = total / 100000000;
    //     long resto = total % 100000000;
        
    //     long platino = resto / 1000000;
    //     resto %= 1000000;
        
    //     long oro = resto / 10000;
    //     resto %= 10000;
        
    //     long plata = resto / 100;
    //     long cobre = resto % 100;

    //     string resultado = "";

    //     // Solo mostramos las monedas grandes si tenemos al menos 1
    //     if (oricalco > 0) resultado += $"{oricalco} Ori ";
    //     if (platino > 0) resultado += $"{platino} Plat ";
    //     if (oro > 0) resultado += $"{oro} Oro ";
    //     if (plata > 0) resultado += $"{plata} Pla ";
        
    //     // Siempre mostramos el cobre
    //     resultado += $"{cobre} Cob"; 

    //     return resultado.Trim();
    // }

    private void CalcularYMostrarMonedas(long total)
    {
        long oricalco = total / 100000000;
        long resto = total % 100000000;
        
        long platino = resto / 1000000;
        resto %= 1000000;
        
        long oro = resto / 10000;
        resto %= 10000;
        
        long plata = resto / 100;
        long cobre = resto % 100;

        // Le mandamos los 5 números al HUD
        hudManager.ActualizarMonedas(oricalco, platino, oro, plata, cobre);
    }
}