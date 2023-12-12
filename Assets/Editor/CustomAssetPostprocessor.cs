using UnityEngine;
using UnityEditor;

public class CustomAssetPostprocessor : AssetPostprocessor
{
    private static string OnGeneratedCSProject(string path, string content)
    {
        // Define the nullable tag to add to the project file
        string nullableTag = "<Nullable>enable</Nullable>";

        // Check if the nullable tag already exists to avoid duplicates
        if (!content.Contains(nullableTag))
        {
            // Find the place to insert the nullable tag
            int propertyGroupIndex = content.IndexOf("<PropertyGroup>");
            if (propertyGroupIndex != -1)
            {
                // Insert the nullable tag into the project file content
                content = content.Insert(propertyGroupIndex + "<PropertyGroup>".Length, "\n    " + nullableTag);
            }
        }

        return content;
    }
}