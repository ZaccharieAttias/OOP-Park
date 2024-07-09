using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Assets.Scripts.Common
{
    public static class Extensions
    {
        public static bool IsEmpty(this string target)
        {
            return string.IsNullOrEmpty(target);
        }

        public static void SetActive(this Component target, bool active)
        {
            target.gameObject.SetActive(active);
        }

        public static T Random<T>(this List<T> source)
        {
            return source[UnityEngine.Random.Range(0, source.Count)];
        }
    }
}