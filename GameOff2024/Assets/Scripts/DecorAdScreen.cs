using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class DecorAdScreen : MonoBehaviour
//{
//    private Renderer rend;
//    private float cycleLength = 3;
//    private float cooldown = 0;
//    private float destinationOffset;

//    // Start is called before the first frame update
//    void Start()
//    {
//        rend = GetComponent<Renderer>();
//        rend.materials[1].mainTextureOffset += new Vector2(Random.Range(0, 4) * 0.25f, 0);
//        cooldown += Random.Range(0f, cycleLength * 0.875f);
//        destinationOffset = rend.materials[1].mainTextureOffset.x;
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        cooldown += Time.fixedDeltaTime;
//        rend.materials[1].mainTextureOffset = new Vector2(Mathf.Lerp(destinationOffset - 0.25f, destinationOffset, Mathf.Clamp(2 * cooldown, 0, 1)), 0);
//        if(cooldown > cycleLength)
//        {
//            destinationOffset += 0.25f;
//            cooldown = 0;
//        }
//    }
//}



public class DecorAdScreen : MonoBehaviour
{
    private const float TextureStepSize = 0.25f;                     // Size of each texture offset step
    private const float CycleDuration = 3f;                          // Time in seconds for one full cycle
    private const float MaxInitialDelay = CycleDuration * 0.875f;    // Max random start delay
    private const int AdMaterialIndex = 1;                           // Index of the material with the ad texture

    private Renderer screenRenderer;
    private float elapsedTime;
    private float targetOffset;

    private void Start()
    {
        InitializeScreen();
    }

    private void FixedUpdate()
    {
        UpdateAdDisplay();
    }

    private void InitializeScreen()
    {
        screenRenderer = GetComponent<Renderer>();

                                               // Random initial ad (0-3)
        float randomStartOffset = Random.Range(0, 4) * TextureStepSize; 

        targetOffset = randomStartOffset;

        // Random delay to stagger cycles
        elapsedTime = Random.Range(0f, MaxInitialDelay); 

        SetTextureOffset(randomStartOffset);
    }

    private void UpdateAdDisplay()
    {

        elapsedTime += Time.fixedDeltaTime;


        // Smoothly interpolate between previous and current target offset
        // progress is normalized (0 to 1 over half a cycle)
        float t = Mathf.Clamp01(elapsedTime * 2f / CycleDuration); 

        float currentOffset = Mathf.Lerp(targetOffset - TextureStepSize, targetOffset, t);

        SetTextureOffset(currentOffset);




        // moves to the next add
        if (elapsedTime >= CycleDuration)
        {
            targetOffset += TextureStepSize;
            elapsedTime = 0f;
        }
    }

    private void SetTextureOffset(float xOffset)
    {

        screenRenderer.materials[AdMaterialIndex].mainTextureOffset = new Vector2(xOffset, 0f);

    }
}
