﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TeamZ.Code.Helpers
{
    public static class ListExtetions
    {
        public static (int Min, int Max) RangedBinarySearch<TValue>(this List<TValue> values, float minValue, float maxValue, Func<TValue, float> getter)
        {
            var minIndex = values.NearestBinarySearch(minValue, getter);
            var maxIndex = values.NearestBinarySearch(maxValue, getter);

            return (minIndex, maxIndex);
        }

        public static IEnumerable<TValue> RangedBinarySearchValues<TValue>(this List<TValue> values, float minValue, float maxValue, Func<TValue, float> getter)
        {
            var (minIndex, maxIndex) = values.RangedBinarySearch(minValue, maxValue, getter);

            if (minIndex == -1 || maxIndex == -1)
            {
                yield break;
            }

            for (int i = minIndex; i < maxIndex; i++)
            {
                yield return values[i];
            }

            yield return values[maxIndex];
        }

        public static (TValue value, bool Empty) NearestBinarySearchValue<TValue>(this List<TValue> values, float value, Func<TValue, float> getter)
        {
            var index = values.NearestBinarySearch(value, getter);

            if (index == -1)
            {
                return (default, true);
            }

            return (values[index], false);
        }

        public static int NearestBinarySearch<TValue>(this List<TValue> values, float value, Func<TValue, float> getter)
        {
            if (!values.Any())
            {
                return -1;
            }

            var left = 0;
            var right = values.Count - 1;
            while ((right - left) > 1)
            {
                var index = left + (right - left) / 2;
                var newValue = getter(values[index]);
                if (newValue > value)
                {
                    right = index;
                    continue;
                }

                if (newValue < value)
                {
                    left = index;
                    continue;
                }

                return index;
            }

            var leftValue = getter(values[left]);
            if (leftValue >= value)
            {
                return left;
            }

            var rightValue = getter(values[right]);
            if (rightValue <= value)
            {
                return right;
            }

            if (rightValue - value > value - leftValue)
            {
                return right;
            }

            return left;
        }

        public static int ExactBinarySearch<TValue>(this List<TValue> values, float value, Func<TValue, float> getter)
        {
            if (!values.Any())
            {
                return -1;
            }

            var left = 0;
            var right = values.Count - 1;
            while ((right - left) > 1)
            {
                var index = left + (right - left) / 2;
                var newValue = getter(values[index]);
                if (newValue > value)
                {
                    right = index;
                    continue;
                }

                if (newValue < value)
                {
                    left = index;
                    continue;
                }

                return index;
            }

           var leftValue = getter(values[left]);
            if (leftValue == value)
            {
                return left;
            }

            var rightValue = getter(values[right]);
            if (rightValue == value)
            {
                return right;
            }

            return -1;
        }

        public static (TValue Value, bool Empty) ExactBinarySearchValue<TValue>(this List<TValue> values, float value, Func<TValue, float> getter)
        {
            var index = values.ExactBinarySearch(value, getter);
            if (index == -1)
            {
                return (default, true);
            }

            return (values[index], false);
        }

    }
}
