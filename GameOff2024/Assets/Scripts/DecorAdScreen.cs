using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorAdScreen : MonoBehaviour
{
    private Renderer rend;
    private float cycleLength = 3;
    private float cooldown = 0;
    private float destinationOffset;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.materials[1].mainTextureOffset += new Vector2(Random.Range(0, 4) * 0.25f, 0);
        cooldown += Random.Range(0f, cycleLength * 0.875f);
        destinationOffset = rend.materials[1].mainTextureOffset.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown += Time.fixedDeltaTime;
        rend.materials[1].mainTextureOffset = new Vector2(Mathf.Lerp(destinationOffset - 0.25f, destinationOffset, Mathf.Clamp(2 * cooldown, 0, 1)), 0);
        if(cooldown > cycleLength)
        {
            destinationOffset += 0.25f;
            cooldown = 0;
        }
    }
}
