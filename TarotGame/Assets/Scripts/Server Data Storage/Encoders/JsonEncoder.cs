using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonEncoder : IEncoder
{
    public T Decode<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);
    }

    public string Encode(object data)
    {
        return JsonUtility.ToJson(data);
    }
}
