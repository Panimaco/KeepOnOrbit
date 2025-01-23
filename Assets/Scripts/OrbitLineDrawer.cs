using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitLineDrawer : MonoBehaviour
{
    [Header("Parámetros de la órbita")]
    //Referencia de la struct de las orbitas
    public ElipticalOrbit orbitData;

    [Header("Parámetros visuales")]
    //Número de segmentos para aproximar la elipse
    public int segments = 100;

    //Referencia interna al Line Renderer
    private LineRenderer lineRend;

    void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    void Start()
    {
        //Al iniciar la escena dibujamos las órbitas
        DrawOrbit();
    }

    private void DrawOrbit()
    {
        //Si no hay centro, no podemos dibujar la órbita
        if (!orbitData.Center)
        {
            Debug.LogWarning("No se ha asignado el Center en orbitData");
            return;
        }

        //Ajustamos el number of positions
        lineRend.positionCount = segments + 1;

        //Asignamos los datos para la formula
        
        //El angulo en radianes
        float angleInRad = orbitData.RotationDeg * Mathf.Deg2Rad;
        
        //El semieje mayor
        float a = orbitData.SemiMajorAxis;
        
        //El semieje menor
        float b = orbitData.SemiMinorAxis;
        
        //El centro de la elipse
        Vector3 centerPos = orbitData.Center.position;

        // Generamos los puntos de la elipse
        for (int i = 0; i <= segments; i++)
        {
            //Fracción del ángulo [0, 2π]
            float t = (float)i / (float)segments * 2f * Mathf.PI;

            //Puntos (x, y) en una elipse sin rotar
            float x = a * Mathf.Cos(t);
            float z = b * Mathf.Sin(t);

            //Rotamos en el plano XZ según angleInRad
            float xRot = x * Mathf.Cos(angleInRad) - z * Mathf.Sin(angleInRad);
            float zRot = x * Mathf.Sin(angleInRad) + z * Mathf.Cos(angleInRad);

            //Trasladamos respecto al centro
            Vector3 pos = new Vector3(
                centerPos.x + xRot,
                //Dejamos la Y en 0
                centerPos.y,
                centerPos.z + zRot
            );

            lineRend.SetPosition(i, pos);
        }
    }
}