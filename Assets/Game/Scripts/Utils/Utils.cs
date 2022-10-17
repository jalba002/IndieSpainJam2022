using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Utils
{
    public static int GetClosestIndexFromList(Transform origin, List<Collider> hits)
    {
        int index = 0;
        float closestDistance = 0f;
        for (int j = 0; j < hits.Count; j++)
        {
            if (j == 0)
            {
                closestDistance = Vector3.Distance(origin.position, hits[0].transform.position);
                continue;
            }

            float newDistance = Vector3.Distance(hits[j].transform.position,
                origin.position);

            if (newDistance < closestDistance)
            {
                closestDistance = newDistance;
                index = j;
            }
        }

        return index;
    }

    public static int GetClosestIndexFromList(Vector3 origin, List<Collider> hits)
    {
        int index = 0;
        float closestDistance = 0f;
        for (int j = 0; j < hits.Count; j++)
        {
            if (j == 0)
            {
                closestDistance = Vector3.Distance(origin, hits[0].transform.position);
                continue;
            }

            float newDistance = Vector3.Distance(hits[j].transform.position,
                origin);

            if (newDistance < closestDistance)
            {
                closestDistance = newDistance;
                index = j;
            }
        }

        return index;
    }
    
    public static bool IsSet<T>(T value, T flags) where T : struct
    {
        // You can add enum type checking to be perfectly sure that T is enum, this have some cost however
        // if (!typeof(T).IsEnum)
        //     throw new ArgumentException();
        long longFlags = Convert.ToInt64(flags);
        return (Convert.ToInt64(value) & longFlags) == longFlags;
    }

    public static string[] SplitByCamelCasing(string str)
    {
        Regex rg = new Regex(@"(?<!^)(?=[A-Z])");
        return rg.Split(str);
    }
}