using UnityEngine;

[System.Serializable]
public struct ElipticalOrbit
{
    //Centro de la elipse (Sol)
    public Transform Center;

    //Semieje mayor (a)
    public float SemiMajorAxis;

    //Semieje menor (b)
    public float SemiMinorAxis;

    //Rotaci�n de la elipse en grados en X/Y
    public float RotationDeg;

    //Velocidad angular de la elipse en rad/s
    public float AngularSpeed;

}
