using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    private UIDocument hudDocument;
    private VisualElement panelInfo;
    private Label textoNombre;
    private Label textoPrecio;

    // Referencias a los grupos
    private VisualElement grupoOricalco, grupoPlatino, grupoOro, grupoPlata, grupoCobre;
    // Referencias a los textos
    private Label textoOricalco, textoPlatino, textoOro, textoPlata, textoCobre;

   void Awake()
    {
        hudDocument = GetComponent<UIDocument>();
        var root = hudDocument.rootVisualElement;

        panelInfo = root.Q<VisualElement>("PanelInfo");
        textoNombre = root.Q<Label>("TextoNombre");
        textoPrecio = root.Q<Label>("TextoPrecio");
        
        // Buscamos los grupos
        grupoOricalco = root.Q<VisualElement>("GrupoOricalco");
        grupoPlatino = root.Q<VisualElement>("GrupoPlatino");
        grupoOro = root.Q<VisualElement>("GrupoOro");
        grupoPlata = root.Q<VisualElement>("GrupoPlata");
        grupoCobre = root.Q<VisualElement>("GrupoCobre");

        // Buscamos los textos
        textoOricalco = root.Q<Label>("TextoOricalco");
        textoPlatino = root.Q<Label>("TextoPlatino");
        textoOro = root.Q<Label>("TextoOro");
        textoPlata = root.Q<Label>("TextoPlata");
        textoCobre = root.Q<Label>("TextoCobre");

        OcultarInfo();
    }
    public void ActualizarMonedas(long oricalco, long platino, long oro, long plata, long cobre)
    {
        // Actualizamos los textos
        if (textoOricalco != null) textoOricalco.text = oricalco.ToString();
        if (textoPlatino != null) textoPlatino.text = platino.ToString();
        if (textoOro != null) textoOro.text = oro.ToString();
        if (textoPlata != null) textoPlata.text = plata.ToString();
        if (textoCobre != null) textoCobre.text = cobre.ToString();

        // Ocultamos o mostramos el grupo entero (número + icono) dependiendo de si tenemos de esa moneda
        ActivarGrupo(grupoOricalco, oricalco > 0);
        ActivarGrupo(grupoPlatino, platino > 0);
        ActivarGrupo(grupoOro, oro > 0);
        ActivarGrupo(grupoPlata, plata > 0);
        
        // El cobre lo dejamos siempre visible (true) para que el jugador vea que tiene 0 dinero si está arruinado
        ActivarGrupo(grupoCobre, true); 
    }

    private void ActivarGrupo(VisualElement grupo, bool activo)
    {
        if (grupo != null)
        {
            grupo.style.display = activo ? DisplayStyle.Flex : DisplayStyle.None;
        }
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
    
    // public void ActualizarTextoDinero(string textoFormateado)
    // {
    //     if (textoDinero != null)
    //     {
    //         textoDinero.text = textoFormateado;
    //     }
    // }
}