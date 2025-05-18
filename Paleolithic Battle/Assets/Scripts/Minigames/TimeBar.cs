using UnityEngine;
using UnityEngine.UI;

public class TimeBar : MonoBehaviour
{
    public Slider timeSlider; // Reference to the Slider component
    public Image fillImage; // Reference to the Image component for the fill area
    public Color startColor = Color.yellow; // Color at the start of the time bar
    
    public void SetTime(float time)
    {
        // Set the value of the slider based on the time parameter
        timeSlider.value = time;
    }

    public void SetMaxTime(float maxTime)
    {
        // Set the maximum value of the slider
        timeSlider.maxValue = maxTime;
    }


}
