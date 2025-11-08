#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using Unity.VisualScripting;

public static class ProductIdGenerator
{
    private const string OutputPath = "Assets/Scripts/EditGenerator/Generated/ProductID.cs";

    [MenuItem("Tools/Generate Product IDs")]
    public static void Generate()
    {
        var guids = AssetDatabase.FindAssets("t:ProductData");
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("// This file is auto-generated. Do not edit manually.");
        sb.AppendLine("public static class ProductID");
        sb.AppendLine("{");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var item = AssetDatabase.LoadAssetAtPath<ProductData>(path);
            if (item == null) continue;

            string fieldName = Sanitize(item.Id).ToUpper();
            sb.AppendLine($"    public const string {fieldName} = \"{item.Id}\";");
        }

        sb.AppendLine("}");

        Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));
        File.WriteAllText(OutputPath, sb.ToString(), Encoding.UTF8);

        AssetDatabase.Refresh();
        Debug.Log("✅ Generated ItemID.cs successfully!");
    }

    private static string Sanitize(string id)
    {
        // Replace invalid characters for C# identifiers
        return id.Replace("-", "_").Replace(" ", "_");
    }
}
#endif

