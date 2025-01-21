#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine.Device;

namespace D_Dev
{
    public static class EditorTools
    {
        #region Editor

        [MenuItem("Tools/Setup/Create Folders")]
        public static void InitProjectFolders()
        {
            CreateFolders("_Project" ,new []
            {
                "Art",
                "Animations",
                "Audio",
                "Scripts",
                "Scenes",
                "ScriptableObjects"
            });
            AssetDatabase.Refresh();
        }

        public static void InstallDoTween()
        {
            
        }
        
        #endregion

        #region Helpers

        private static void InstallPackage(string packageName) => UnityEditor.PackageManager.Client.Add(packageName);
        private static void CreateFolders(string rootDir, string[] directions)
        {
            var path = Application.dataPath;
            var combinedPath = Path.Combine(path, rootDir);
            
            foreach (var dir in directions)
                Directory.CreateDirectory(Path.Combine(combinedPath, dir));
        }

        #endregion
    }
}
#endif
