﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EditorTools.SerializedReferenceInitializer.Attributes;
using UnityEditor;
using UnityEngine;

namespace EditorTools.SerializedReferenceInitializer.Editor
{
    internal static class TypeUtils
    {
        private sealed class StringComparer : IComparer<string>
        {
            public int Compare(string x, string y) => EditorUtility.NaturalCompare(x, y);
        }

        public static IEnumerable<Type> Sorted(IEnumerable<Type> types)
        {
            return types
                // Get the path-info for each underlying type.
                .Select(type =>
                {
                    var autoGenerated = type.GetCustomAttribute<AutoGeneratedWrapperAttribute>();
                    Type underlyingType = autoGenerated?.WrappedType ?? type;
                    var pathInfo = CustomPathInfo.GetFromType(underlyingType) ??
                                   new CustomPathInfo(GetTypeDisplayInfoForType(underlyingType).path, 0);
                    return (type, pathInfo);
                })
                .OrderBy(tuple => tuple.Item2.path, new StringComparer())
                .ThenBy(tuple => tuple.Item2.order)
                .Select(tuple => tuple.Item1);
        }

        public static TypeDisplayInfo GetTypeDisplayInfo(Type type)
        {
            CustomPathInfo customPathInfo = CustomPathInfo.GetFromType(type);
            if (customPathInfo != null)
            {
                return GetTypeDisplayInfoForCustomPathInfo(type, customPathInfo);
            }

            return GetTypeDisplayInfoForType(type);
        }

        private static TypeDisplayInfo GetTypeDisplayInfoForType(Type type)
        {
            string fullName = type.FullName!;
            int index = fullName.LastIndexOf(".", StringComparison.Ordinal);
            if (index < 0)
                return new TypeDisplayInfo(string.Empty, type.Name);

            return new TypeDisplayInfo(fullName.Substring(0, index), fullName.Substring(index + 1));
        }

        private static TypeDisplayInfo GetTypeDisplayInfoForCustomPathInfo(Type type, CustomPathInfo customPathInfo)
        {
            string componentMenu = customPathInfo.path;
            int separator = componentMenu.LastIndexOf("/", StringComparison.Ordinal);
            // If the path ends with "/", it means that the type's name should be used for the menu.
            if (separator == componentMenu.Length - 1)
            {
                componentMenu += type.Name;
            }

            var typeDisplayInfo = new TypeDisplayInfo(
                componentMenu.Substring(0, separator),
                componentMenu.Substring(separator + 1)
            );
            return typeDisplayInfo;
        }

        private sealed class CustomPathInfo
        {
            public readonly string path;
            public readonly int order;

            public static CustomPathInfo GetFromType(Type type)
            {
                AddComponentMenu addComponentMenuAttribute = type.GetCustomAttribute<AddComponentMenu>();
                if (addComponentMenuAttribute != null)
                    return new CustomPathInfo(addComponentMenuAttribute.componentMenu,
                        addComponentMenuAttribute.componentOrder);

                CustomMenuPathAttribute customMenuPathAttribute = type.GetCustomAttribute<CustomMenuPathAttribute>();
                if (customMenuPathAttribute != null)
                    return new CustomPathInfo(customMenuPathAttribute.path, customMenuPathAttribute.order);

                return null;
            }

            internal CustomPathInfo(string path, int order)
            {
                this.path = path;
                this.order = order;
            }
        }
    }

    internal readonly struct TypeDisplayInfo
    {
        public readonly string path;
        public readonly string displayName;

        public TypeDisplayInfo(string path, string displayName)
        {
            this.path = path;
            this.displayName = ObjectNames.NicifyVariableName(displayName);
        }
    }
}