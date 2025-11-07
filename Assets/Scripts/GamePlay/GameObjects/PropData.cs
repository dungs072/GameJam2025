using System;
using UnityEngine;

[Serializable]
public class PropData
{
    [field: SerializeField] public int PropID { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
}
