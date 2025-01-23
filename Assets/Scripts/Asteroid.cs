using UnityEngine;

public class Asteroid : MonoBehaviour
{
    //Referencia al sol como el centro
    public Transform Sun;
    //Referencia al script del planeta
    public PlanetController Planet;

    private void OnTriggerEnter(Collider collision)
    {
        //Si el objeto con el que colisiona es el planeta
        if (collision.CompareTag("PlayerPlanet"))
        {
            Debug.Log("Asteroid hit planet");
            //Para dictaminar si el asteroide viene del lado del sol o del otro
            //Hacemos la comparación de si está más cerca del sol que el planeta

            //Distancia del sol al planeta
            Vector3 sunToPlanet = Planet.transform.position - Sun.position;

            //Distancia del sol al asteroide
            Vector3 sunToAsteroid = transform.position - Sun.position;

            //Si la distancia del sol al asteroide es menor
            //que la distancia del sol al planeta
            if (sunToAsteroid.magnitude < sunToPlanet.magnitude)
            {
                //El asteroide viene del lado del sol
                //Se mueve a la derecha (órbita exterior)
                Planet.OrbitOutward();
            }
            //Si es al revés
            else
            {
                //El asteroide viene del lado contrario al sol
                //Se mueve a la izquierda (órbita interior)
                Planet.OrbitInward();
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Asteroid hit something else");
        }
    }
}