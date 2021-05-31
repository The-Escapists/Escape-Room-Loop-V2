using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TheEscapists.Core
{
    public class TagsCreator
    {
#if UNITY_EDITOR
        [MenuItem("Tools/The Escapists/Generate Tag Chache")]
        public static void UpdateTagCacheUnity()
        {

            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;
            // the path we want to write to
            string path = string.Concat(Application.dataPath, Path.DirectorySeparatorChar, "The Escapists", Path.DirectorySeparatorChar, "Scripts", Path.DirectorySeparatorChar, "Core", Path.DirectorySeparatorChar, "Generated", Path.DirectorySeparatorChar, "UnityTags.cs");

            try
            {
                // opens the file if it allready exists, creates it otherwise
                using (FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("// ----- AUTO GENERATED CODE ----- //");
                        builder.AppendLine("namespace TheEscapists.Core.Generated");
                        builder.AppendLine("{");
                        builder.AppendLine("public static class UnityTags");
                        builder.AppendLine("{");
                        builder.Append("public enum eUnityTags {");
                        foreach (string tag in tags)
                        {
                            builder.Append(tag + ", ");
                        }
                        builder.Remove(builder.Length - 2, 2);
                        builder.AppendLine("};");
                        builder.AppendLine("}");
                        builder.AppendLine("}");
                        writer.Write(builder.ToString());
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);

                // if we have an error, it is certainly that the file is screwed up. Delete to be save
                if (File.Exists(path) == true)
                    File.Delete(path);
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/The Escapists/Generate Scene Chache")]
        public static void UpdateSceneCacheUnity()
        {

            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
            string[] scenes = new string[sceneCount];
            for (int i = 0; i < sceneCount; i++)
            {
                scenes[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            }
            // the path we want to write to
            string path = string.Concat(Application.dataPath, Path.DirectorySeparatorChar, "The Escapists", Path.DirectorySeparatorChar, "Scripts", Path.DirectorySeparatorChar, "Core", Path.DirectorySeparatorChar, "Generated", Path.DirectorySeparatorChar, "UnityScenes.cs");

            try
            {
                // opens the file if it allready exists, creates it otherwise
                using (FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine("// ----- AUTO GENERATED CODE ----- //");
                        builder.AppendLine("namespace TheEscapists.Core.Generated");
                        builder.AppendLine("{");
                        builder.AppendLine("public static class UnityScenes");
                        builder.AppendLine("{");
                        builder.Append("public enum eUnityScenes {");
                        foreach (string scene in scenes)
                        {
                            builder.Append(scene + ", ");
                        }
                        builder.Remove(builder.Length - 2, 2);
                        builder.AppendLine("};");
                        builder.AppendLine("}");
                        builder.AppendLine("}");
                        writer.Write(builder.ToString());
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);

                // if we have an error, it is certainly that the file is screwed up. Delete to be save
                if (File.Exists(path) == true)
                    File.Delete(path);
            }

            AssetDatabase.Refresh();
        }
#endif
    }

}