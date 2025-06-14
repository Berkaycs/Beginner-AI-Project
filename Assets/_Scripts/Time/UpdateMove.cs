using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMove : MonoBehaviour
{
    void Update()
    {
        // If you use Time.deltaTime in every update method the all characters will have consistent speed
        transform.Translate(0, 0, 0.01f);
    }
}
