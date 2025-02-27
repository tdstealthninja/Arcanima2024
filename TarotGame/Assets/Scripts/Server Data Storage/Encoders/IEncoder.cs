using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEncoder
{
    string Encode(object data);
    T Decode<T>(string data);
}
