using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    //Transform del planeta del jugador como objetivo
    public Transform PlayerPlanet;

    //Fuerza de empuje de los asteroides
    [SerializeField]
    private float _initialForce = 0.1f;

    //Distancia de destrucción
    [SerializeField]
    private float _destroyDistance = 12f;

    //Rigidbody del asteroide Instanciado
    private Rigidbody _rb;

    private void Start()
    {
        //Recogemos el componente del propio asteroide y lo asignamos a "_rb"
        _rb = GetComponent<Rigidbody>();

        //Si no hay referencia al planeta del jugador
        if (PlayerPlanet == null)
        {
            Debug.LogError("Error04: No se ha asignado el planeta del jugador");
            return;
        }

        //Si hay referencia al planeta del jugador
        if (PlayerPlanet != null && _rb != null)
        {
            //Calculamos la dirección del asteroide hacia el planeta
            Vector3 dirToPlayer = (PlayerPlanet.position - transform.position).normalized;

            //Aplicamos una fuerza inicial al asteroide
            _rb.AddForce(dirToPlayer * _initialForce, ForceMode.VelocityChange);
        }
    }

    private void Update()
    {
        //Si no hay referencia al planeta del jugador
        if (PlayerPlanet == null)
        {
            Debug.LogError("Error04: No se ha asignado el planeta del jugador");
            return;
        }

        //Calculamos la distancia del asteroide al planeta
        float distanceToPlanet = Vector3.Distance(transform.position, PlayerPlanet.position);

        //Si es mayor a la distancia de destrucción
        if (distanceToPlanet > _destroyDistance)
        {
            //Se destruye el asteroide
            Destroy(gameObject);
        }
    }
}
