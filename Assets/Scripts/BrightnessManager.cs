using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrightnessManager : MonoBehaviour
{
    [SerializeField] private Slider brightnessSlider;   // Reference to the brightness slider
    [SerializeField] private Image brightnessOverlay;   // Reference to the full-screen overlay image

    void Start()
    {
        // Check if the brightness setting exists, if not set default value
        if (!PlayerPrefs.HasKey("screenBrightness"))
        {
            PlayerPrefs.SetFloat("screenBrightness", 0.5f); // Default brightness is half
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeBrightness()
    {
        // Update the overlay's alpha value based on the slider's value
        Color currentColor = brightnessOverlay.color;
        currentColor.a = 1 - brightnessSlider.value; // Inverse because higher slider value should mean brighter screen
        brightnessOverlay.color = currentColor;

        Save();
    }

    private void Load()
    {
        // Load the brightness value and apply it to the slider
        brightnessSlider.value = PlayerPrefs.GetFloat("screenBrightness");
        ChangeBrightness(); // Apply the loaded brightness value to the overlay
    }

    private void Save()
    {
        // Save the current brightness setting
        PlayerPrefs.SetFloat("screenBrightness", brightnessSlider.value);
    }
}
