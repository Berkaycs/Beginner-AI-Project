using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondsUpdate : MonoBehaviour
{
    private float _timeStartOffset = 0;
    private bool _gotStartTime = false;

    void Update()
    {
        if (!_gotStartTime)
        {
            _timeStartOffset = Time.realtimeSinceStartup;
            _gotStartTime = true;
        }

        // moves per m/s for every computer it workss at same rate because it uses actual time
        transform.position = new Vector3(transform.position.x, transform.position.y, Time.realtimeSinceStartup - _timeStartOffset);
    }
}
