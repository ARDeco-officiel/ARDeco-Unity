using UnityEngine;
using UnityEngine.UI;

public class Soundsscript : MonoBehaviour
{
    public Slider volumeSlider; // Référence au Slider de volume
    public AudioSource audioSource; // Référence à l'objet AudioSource

    private void Start()
    {
        // Initialisez le Slider avec la valeur actuelle du volume
        volumeSlider.value = audioSource.volume;
    }

    // Méthode appelée lorsque le Slider de volume change
    public void OnVolumeChanged()
    {
        // Mettez à jour le volume de l'AudioSource en fonction de la valeur du Slider
        audioSource.volume = volumeSlider.value;
    }
}