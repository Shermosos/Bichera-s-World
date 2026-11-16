using UnityEngine;
using UnityEngine.UI;

public class AjustesVolumen : MonoBehaviour
{
    public Slider sliderVolumen;

    void Start()
    {
        float volumen = PlayerPrefs.GetFloat("volumen", 1f);
        sliderVolumen.value = volumen;
        AudioListener.volume = volumen;
    }

    public void CambiarVolumen(float valor)
    {
        AudioListener.volume = valor;
        PlayerPrefs.SetFloat("volumen", valor);
    }
}