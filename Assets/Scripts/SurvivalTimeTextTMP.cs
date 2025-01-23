using TMPro;
using UnityEngine;

public class SurvivalTimeTextTMP : MonoBehaviour
{
    //Zona de Variables Globales
    [SerializeField]
    private TextMeshProUGUI _tmpText;
    private float _survivalTime = 0f;

    void Update()
    {
        _survivalTime += Time.deltaTime;
        int timeInt = Mathf.FloorToInt(_survivalTime);
        _tmpText.text = "Tiempo sin morir: " + timeInt + "s";
    }

    public void ResetSurvivalTime()
    {
        _survivalTime = 0f;
    }
}
