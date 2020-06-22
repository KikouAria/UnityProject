using UnityEngine;
using System.Collections;
using LitJson;

public class Coding<T>
{
    public static string encode(T model)
    {
        return JsonMapper.ToJson(model);
    }

    public static T decode(string message)
    {
        return JsonMapper.ToObject<T>(message);
    }
}