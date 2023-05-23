﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EditorTools.SerializedReferenceInitializer.Attributes;
using UnityEditor;
using UnityEngine;

namespace EditorTools.SerializedReferenceInitializer.Editor
{
    public sealed class UnityEventsListener : AssetPostprocessor
    {
        private static bool expectingRefresh = false;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            // Extract the extenstion of all the processed assets.
            var allAssets = importedAssets.Concat(deletedAssets).Concat(movedAssets).Concat(movedFromAssetPaths)
                .Select(Path.GetExtension);
            // Check if any C# scripts were processed.
            if (!allAssets.Any(extension => extension.Equals(".cs", StringComparison.Ordinal)))
                return;

            // Prevent unnecessary execution of the code below after we created the wrapper-classes.
            if (expectingRefresh)
            {
                expectingRefresh = false;
                return;
            }

            RemoveWrapperClassesForModifiedInterfaces();
        }

        private static void RemoveWrapperClassesForModifiedInterfaces()
        {
            var generatedClasses = TypeCache.GetTypesWithAttribute<AutoGeneratedWrapperAttribute>();
            var assetsToDelete = new List<string>(generatedClasses.Count);
            foreach (var generatedClassType in generatedClasses)
            {
                string wrapperPath =
                    PathsHandler.GetScriptFilePath(generatedClassType, $"{generatedClassType.Name}.cs");
                if (string.IsNullOrEmpty(wrapperPath))
                    continue;

                Type wrappedInterfaceType = generatedClassType.GetInterfaces()[0];
                string wrappedInterfacePath =
                    PathsHandler.GetScriptFilePath(wrappedInterfaceType, $"{wrappedInterfaceType.Name}.cs");
                // If the wrapped interface was deleted, or modified after the creation of the wrapper class.
                if (!File.Exists(wrappedInterfacePath) ||
                    File.GetLastWriteTime(wrappedInterfacePath) > File.GetLastWriteTime(wrapperPath))
                {
                    assetsToDelete.Add(PathsHandler.AbsolutePathToProjectRelativePath(wrapperPath));
                }
            }

            if (assetsToDelete.Count == 0)
                return;
            expectingRefresh = true;
            // If there was any change, delete the modified wrapper-classes so they would be recreated in the next compilation.
            try
            {
                AssetDatabase.DeleteAssets(assetsToDelete.ToArray(), new List<string>());
            }
            finally
            {
                expectingRefresh = false;
            }
        }

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            // After all the scripts were compiled, create the wrapper classes.
            AssemblyReloadEvents.afterAssemblyReload -= GenerateWrapperClasses;
            AssemblyReloadEvents.afterAssemblyReload += GenerateWrapperClasses;
        }

        private static void GenerateWrapperClasses()
        {
            CodeGenerator.GenerateWrapperClasses();
            expectingRefresh = true;
            try
            {
                AssetDatabase.Refresh();
            }
            finally
            {
                expectingRefresh = false;
            }
        }

        [MenuItem("Utilities/Re-Generate all wrapper classes")]
        private static void ReGenerateWrapperClasses()
        {
            var generatedClasses = TypeCache.GetTypesWithAttribute<AutoGeneratedWrapperAttribute>();
            List<string> assetsToDelete = new List<string>(generatedClasses.Count);
            List<string> outFailedPaths = new List<string>(generatedClasses.Count);
            foreach (Type generatedClassType in generatedClasses)
            {
                string scriptFilePath =
                    PathsHandler.GetScriptFilePath(generatedClassType, $"{generatedClassType.Name}.cs");
                string projectRelativeScriptPath = PathsHandler.AbsolutePathToProjectRelativePath(scriptFilePath);
                assetsToDelete.Add(projectRelativeScriptPath);
            }

            expectingRefresh = true;
            try
            {
                AssetDatabase.DeleteAssets(assetsToDelete.ToArray(), outFailedPaths);
                GenerateWrapperClasses();
            }
            finally
            {
                expectingRefresh = false;
            }
        }
    }
}