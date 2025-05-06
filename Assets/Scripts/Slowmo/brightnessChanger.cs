using UnityEngine;

public class brightnessChanger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        float b = timeManager.Instance.brightness;
        GetComponent<SpriteRenderer>().color = new Color(b, b, b, 1f);
    }
}
