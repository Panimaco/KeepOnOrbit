using UnityEngine;

public class MultipleOrbitsDrawer : MonoBehaviour
{
    //Guardamos referencia de todas las orbitas descritas
    public ElipticalOrbit[] orbits;
    //Prefab de las lineas que dibujaran las orbitas
    public GameObject lineRendererPrefab;
    //Segmentos de la orbita
    public int segments = 100;

    void Start()
    {
        //Por cada orbita en la lista de orbitas
        foreach (var orbit in orbits)
        {
            //Instancia un prefab que tenga el script OrbitLineDrawer
            GameObject newLine = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity);

            //Configura su orbitData y segmentos
            OrbitLineDrawer drawer = newLine.GetComponent<OrbitLineDrawer>();
            drawer.orbitData = orbit;
            drawer.segments = segments;
        }
    }
}
