using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Stat {
    [SerializeField]
    StatBarController bar;
    [SerializeField]
    float maximum;
    [SerializeField]
    float minimum;
    [SerializeField]
    float current;

    public float Current
    {
        get
        {
            return current;
        }
        set
        {
            current = value;
            bar.Value = value;
        }
    }
    public float Maximum
    {
        get
        {
            return maximum;
        }
        set
        {
            bar.MaxVal = value;
            maximum = value;
        }
    }
    public float Minimum
    {
        set
        {
            bar.MinVal = value;
            minimum = value;
        }
    }
}
