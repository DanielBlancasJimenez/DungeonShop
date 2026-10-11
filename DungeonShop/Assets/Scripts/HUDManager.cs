using UnityEngine;
using UnityEngine.UIElements; // Necesario para UI Toolkit

public class HUDManager : MonoBehaviour
{
    private UIDocument hudDocument;
    private VisualElement panelInfo;
    private Label textoNombre;
    private Label textoPrecio;
    private Label textoDinero;

   void Awake()
    {
        hudDocument = GetComponent<UIDocument>();
        var root = hudDocument.rootVisualElement;

        panelInfo = root.Q<VisualElement>("PanelInfo");
        textoNombre = root.Q<Label>("TextoNombre");
        textoPrecio = root.Q<Label>("TextoPrecio");
        
        // Buscamos el nuevo elemento de dinero
        textoDinero = root.Q<Label>("TextoDinero");

        OcultarInfo();
    }

    public void MostrarInfoProducto(string nombre, float precio)
    {
        textoNombre.text = nombre;
        textoPrecio.text = precio.ToString("F2") + " €"; // Formatea el número a 2 decimales
        
        // Hacemos visible el panel
        panelInfo.style.display = DisplayStyle.Flex; 
    }

    public void OcultarInfo()
    {
        // Ocultamos el panel
        panelInfo.style.display = DisplayStyle.None; 
    }
    
    public void ActualizarTextoDinero(string textoFormateado)
    {
        if (textoDinero != null)
        {
            textoDinero.text = textoFormateado;
        }
    }
}