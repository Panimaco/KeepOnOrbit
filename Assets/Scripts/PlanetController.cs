using UnityEngine;
using TMPro;

[System.Serializable]
public class PlanetController : MonoBehaviour
{
    [Header("�rbitas disponibles")]

    //Las �rbitas que el planeta puede seguir
    [SerializeField]
    private ElipticalOrbit[] _orbits;

    //La �rbita actual
    private int _currentOrbit = 0;

    //�ngulo param�trico de la elipse
    private float _angleParam = 0f;

    //Factor de direcci�n (+1 o -1) depende del sentido
    private float _rotationDirection = 1f;

    //�rbitas seguras para el planeta
    private int _safeOrbitMin = 2,
                _safeOrbitMax = 4;

    [Header("Vida y Temperatura")]

    //Vida y velocidad de desgaste de la misma
    [SerializeField]
    private float _life = 100f,
                  _lifeDecreaseRate = 5f;

    [Header("Referencias")]
    [SerializeField]
    private TextMeshProUGUI _lifeText,
                            _temperatureText;

    [SerializeField]
    private AsteroidSpawner _asteroidSpawner;

    [SerializeField]
    private GameObject DeathPanel,
                       PauseScreen;

    //Zona de Vectores para que los asteroides no se generen detras del planeta
    public Vector3 CurrentPos,
                   PreviousPos,
                   VelocityDir;

    private void Start()
    {
        //Empezamos en la tierra (orbita 3)
        _currentOrbit = 2;
    }

    private void Update()
    {
        //Actualizamos la vida
        _lifeText.text = "Vida: " + _life.ToString("F0");

        //Actualizamos la temperatura
        _temperatureText.text = "Temperatura: " + (_safeOrbitMax - _currentOrbit).ToString("F0");

        //Si no hay �rbitas asignadas
        if (_orbits.Length == 0)
        {
            //Devuelve el error01
            Debug.LogError("Error01: No se han asignado �rbitas al planeta");
            return;
        }

        //Guardamos la posici�n anterior
        PreviousPos = CurrentPos;

        //Creamos una nueva eliptical orbit manejable que sea la actual
        ElipticalOrbit orbit = _orbits[_currentOrbit];

        //Suma la �bita actual y la direcci�n
        _angleParam += orbit.AngularSpeed * Time.deltaTime * _rotationDirection;

        //Calculamos la posici�n en la elipse (matematicas)
        float alpha = Mathf.Deg2Rad * orbit.RotationDeg,
              localX = orbit.SemiMajorAxis * Mathf.Cos(_angleParam),
              localY = orbit.SemiMinorAxis * Mathf.Sin(_angleParam);

        float rotatedX = localX * Mathf.Cos(alpha) - localY * Mathf.Sin(alpha),
              rotatedY = localX * Mathf.Sin(alpha) + localY * Mathf.Cos(alpha);

        Vector3 newPosition = new Vector3(
            orbit.Center.position.x + rotatedX,
            orbit.Center.position.y,
            orbit.Center.position.z + rotatedY
        );

        //Movemos el planeta a la nueva posici�n
        transform.position = newPosition;

        //Asignamos la CurrentPos
        CurrentPos = transform.position;

        //Calculamos la direcci�n de avance
        Vector3 displacement = CurrentPos - PreviousPos;

        //Si hay desplazamiento
        if (displacement.magnitude > 0.0001f)
        {
            //Asignamos la direcci�n de avance
            VelocityDir = displacement.normalized;
        }
        //Control de la �rbita segura
        SafeOrbitControl();

        //Imputs de controles
        KeyInputs();
    }
    private void KeyInputs()
    {
        //Cambio de �rbita (cambiar direcci�n) con la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleRotationDirection();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen.SetActive(!PauseScreen.activeSelf);
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                Debug.Log("Juego Pausado");
            }
            else
            {
                Time.timeScale = 1;
                Debug.Log("Juego Reanudado");
            }
        }
    }
    //Control de la orbita segura
    private void SafeOrbitControl()
    {
        //Control de la orbita segura
        if (_currentOrbit < _safeOrbitMin || _currentOrbit > _safeOrbitMax)
        {
            int distanceFromSafeZone = 0;

            if (_currentOrbit < _safeOrbitMin)
            {
                distanceFromSafeZone = _safeOrbitMin - _currentOrbit;
                //A m�s diferencia, m�s calor
            }
            else
            {
                distanceFromSafeZone = _currentOrbit - _safeOrbitMax;
                //A m�s diferencia, m�s fr�o
            }

            float penalty = _lifeDecreaseRate * distanceFromSafeZone;
            _life -= penalty * Time.deltaTime;

            //Si la vida es menor o igual a 0
            if (_life <= 0)
            {
                _life = 0;
                PlanetDeath();
            }
        }
    }
    //Cambio de direcci�n de la rotaci�n
    private void ToggleRotationDirection()
    {
        _rotationDirection *= -1f;
    }

    //Muerte del planeta
    private void PlanetDeath()
    {
        DeathPanel.SetActive(true);
    }

    //Fuerza al planeta a cambiar de �rbita al 'newIndex'
    public void SwitchOrbit(int newIndex)
    {
        if (newIndex >= 0 && newIndex < _orbits.Length)
        {
            _currentOrbit = newIndex;
        }
        else
        {
            //Si est� fuera de rango, consideramos que muere
            _life = 0;
        }

        //Si el spawner de asteroides est� asignado
        if (_asteroidSpawner != null)
        {
            _asteroidSpawner.ClearAsteroidsAndPause(1f);
        }
        else
        {
            Debug.LogError("Error03: No se ha asignado el spawner de asteroides");
        }
    }

    //Sube un nivel de �rbita
    public void OrbitOutward()
    {
        SwitchOrbit(_currentOrbit + 1);
    }

    //Baja un nivel de �rbita
    public void OrbitInward()
    {
        SwitchOrbit(_currentOrbit - 1);
    }
}