/* Created by José Pereira
 * 10/08/2019
 * 
 */
using System;
using System.Collections.Generic;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
using UnityEngine;
#endif


namespace JMPlib
{

    public class MathExtension
    {

        /// <summary>
        /// aka InverseLerp
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float NormalizeValue(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        public static float FindClosestValueBelow(float value, IEnumerable<float> collection)
        {
            float res = float.MinValue;

            foreach (var element in collection)
            {
                if (value > element && element > res)
                {
                    res = element;
                }
            }

            return res;
        }

        public static int FindClosestValueBelow(int value, IEnumerable<int> collection)
        {
            int res = int.MinValue;

            foreach (var element in collection)
            {
                if (value > element && element > res)
                {
                    res = element;
                }
            }

            return res;
        }

        public static float FindClosestValueAbove(float value, IEnumerable<float> collection)
        {
            float res = float.MaxValue;

            foreach (var element in collection)
            {
                if (value < element && element < res)
                {
                    res = element;
                }
            }

            return res;
        }

        public static int FindClosestValueAbove(int value, IEnumerable<int> collection)
        {
            int res = int.MaxValue;

            foreach (var element in collection)
            {
                if (value < element && element < res)
                {
                    res = element;
                }
            }

            return res;
        }

        public static void GetDirectionFromAngle(float angle, out float dirX, out float dirY)
        {
            dirX = (float)Math.Cos(angle);
            dirY = (float)Math.Sin(angle);
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS

        public static Vector2 GetDirectionFromAngle(float angle)
        {
            //float angleInRad = angle * (Mathf.PI / 360f);
            return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        }
#endif

        public static float GetAngleInRadFromDirection(float dirX, float dirY)
        {
            return (float)Math.Atan2(dirX, dirY);
        }

        public static int Calculate1DIndexFrom2D(int width, int column, int row)
        {
            return width * column + row;
        }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
        public static Vector2Int Calculate2DIndexFrom1D(int width, int index)
        {
            return new Vector2Int(index % width, index / width);
        }
#endif

    }

}


