using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Parámetros de Spawneo")]

    //Prefab del asteroide
    [SerializeField]
    private GameObject _asteroidPrefab;

    //Intervalo de aparición
    [SerializeField]
    private float _spawnInterval = 0.1f;

    //Distancia al frente para spawnear
    [SerializeField]
    private float _maxDistance = 8f;

    [Header("Referencias")]

    //Referencia al controlador del planeta
    [SerializeField]
    private PlanetController _planetController;

    //Referemcia añ Transform del planeta
    [SerializeField]
    private Transform _planet;

    //Lista para llevar el control de los asteroides
    private List<GameObject> _activeAsteroids = new List<GameObject>();

    //Control de temporizador
    private float _spawnTimer = 0f;
    private bool _isSpawning = true;

    [SerializeField]
    private Asteroid _asteroid;
    [SerializeField]
    private GameObject _asteroidManager;
    [SerializeField]
    private PlanetController _planetGameObject;
    [SerializeField]
    private Transform _sun;

    private void Awake()
    {
        _asteroid = _asteroidManager.GetComponent<Asteroid>();
        _asteroid.Sun = _sun;
        _asteroid.Planet = _planetGameObject;
    }
    private void Update()
    {
        //Si no está spawneando, no hace nada
        if (!_isSpawning) return;

        //Ponemos el temporizador a contar
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval && _isSpawning)
        {
            //Reseteamos el temporizador
            _spawnTimer = 0f;
            //Spawneamos un asteroide
            SpawnAsteroid();
        }
    }

    //Instancia un asteroide en una posición "delantera + lateral" al planeta,
    //usando la dirección de avance para que no aparezcan detrás.
    private void SpawnAsteroid()
    {
        //Si no hay planeta o prefab de asteroide o PlanetController
        if (_planet == null || _asteroidPrefab == null || _planetController == null)
        {
            Debug.LogError("Error02: No se ha asignado el planeta o el prefab de asteroide");
            return;
        }

        //Dirección hacia adelante del planeta
        Vector3 fowardDir = _planetController.VelocityDir.normalized;

        //Calculamos la "derecha" en el plano XZ
        Vector3 rightDir = Vector3.Cross(Vector3.up, fowardDir).normalized;

        //Posición de spawneo base (delante del planeta)
        Vector3 spawnPos = _planetController.transform.position + fowardDir * _maxDistance;

        //Offset lateral
        float sideOffset = 3f;  // Ajusta a gusto
        float sideSign = (Random.value > 0.5f) ? 1f : -1f;
        spawnPos += rightDir * sideOffset * sideSign;

        //Instanciamos el asteroide, le añadimos el movimiento
        //y lo añadimos a la lista
        GameObject newAsteroid = Instantiate(_asteroidPrefab, spawnPos, Quaternion.identity);

        AsteroidMovement movement = newAsteroid.GetComponent<AsteroidMovement>();
        //Si el asteroide tiene el script de movimiento
        //Se le asigna el planeta
        if (movement != null)
        {
            movement.PlayerPlanet = _planet;
        }

        _activeAsteroids.Add(newAsteroid);
    }

    //Limpiar los asteroides de la lista y pausa el spawneo
    public void ClearAsteroidsAndPause(float delay = 1f)
    {
        //Destruir todos los asteroides instanciados que no han sido destruidos
        foreach (GameObject asteroid in _activeAsteroids)
        {
            Destroy(asteroid);
        }
        _activeAsteroids.Clear();

        //Pausar el spawneo
        _isSpawning = false;
        _spawnTimer = 0f;

        //Reanudar el spawneo después de un tiempo
        StartCoroutine(ResumeSpawningAfterDelay(delay));
    }

    //Reanudar el spawneo después de un tiempo
    private IEnumerator ResumeSpawningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isSpawning = true;
    }
}