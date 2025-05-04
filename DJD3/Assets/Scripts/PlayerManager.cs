using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Slider fuelSlider;
    public int maxFuel;
    public int currentFuel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentFuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        if (fuelSlider.value != currentFuel)
        {
            fuelSlider.value = currentFuel;
        }
    }

    public void SpendFuel(int fuel)
    {
        currentFuel -= fuel;
        if (currentFuel < 0)
        {
            currentFuel = 0;
        }
    }

    public void GainFuel(int fuel)
    {
        currentFuel += fuel;
        if(currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }
}
