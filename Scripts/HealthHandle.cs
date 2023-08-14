using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthHandle : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;

    
    public void SetMaxHealth(int health)
    {
        SetupGradientColor();
        
        _slider.maxValue = health;
        _slider.value = health;

        _fill.color = _gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        _slider.value = health;

        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    private void SetupGradientColor()
    {
        // Create a new Gradient
        _gradient = new Gradient();

        // Define color keys for the Gradient
        GradientColorKey[] colorKeys = new GradientColorKey[3];

        // Color key 1 (red at time = 0)
        colorKeys[0].color = Color.red;
        colorKeys[0].time = 0.3f;

        // Color key 2 (green at time = 0.5)
        colorKeys[1].color = Color.yellow;
        colorKeys[1].time = 0.75f;

        // Color key 3 (blue at time = 1)
        colorKeys[2].color = Color.green;
        colorKeys[2].time = 1f;

        // Define alpha (transparency) keys for the Gradient (optional)
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1f;  // Start alpha (time = 0)
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 1f;  // End alpha (time = 1)
        alphaKeys[1].time = 1f;

        // Set the color and alpha keys in the gradient
        _gradient.SetKeys(colorKeys, alphaKeys);

        // Call a function to apply the gradient color to the target renderer
    }
}
