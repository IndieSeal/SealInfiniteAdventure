using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public static class Utilities
{
    public static T GetRandomOf<T>(this List<T> myList)
    {
        if (myList.Count == 0) return default;

        return myList[myList.GetRandomIndexOf()];
    }
    public static int GetRandomIndexOf<T>(this List<T> myList) => Random.Range(0, myList.Count);
}