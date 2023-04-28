using UnityEngine;
using UnityEditor;
using Cinemachine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Core
{
    public class ModifyCinemachineCameras : EditorWindow
    {
        [MenuItem("Tools/Find Cinemachine Cameras With Near Clip Plane < 0.1")]
        public static void FindCinemachineCamerasWithNearClipPlane()
        {
            // ModifyPrefabNearClipPlane();
            ModifySceneNearClipPlane();
        }

        private static void ModifyPrefabNearClipPlane()
        {
            string[] guids = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                
                // 获取场景中的所有CinemachineVirtualCamera组件
                CinemachineVirtualCamera[] cameras = prefab.GetComponentsInChildren<CinemachineVirtualCamera>(true);
                ModifyCinemachineVirtualCameraNearClipPlane(prefab,cameras);
            }
            
            AssetDatabase.SaveAssets();
        }
        
        private static void ModifySceneNearClipPlane()
        {
            string[] guids = AssetDatabase.FindAssets("t:Scene");
            foreach (string guid in guids)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(guid);
                Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                
                Camera[] cameras = FindObjectsOfType<Camera>();
                ModifyCameraNearClipPlane(scene, cameras);
                
                EditorSceneManager.SaveScene(scene);
            }
        }

        private static void ModifyCinemachineVirtualCameraNearClipPlane(GameObject prefab,CinemachineVirtualCamera[] cameras)
        {
            // 遍历所有相机并检查NearClipPlane属性值
            foreach (CinemachineVirtualCamera camera in cameras)
            {
                if (camera.m_Lens.NearClipPlane < 0.1f)
                {
                    Debug.LogError("Prefab: " + prefab.name +" - Camera: " + camera.gameObject.name + 
                                   " - Near Clip Plane: " + camera.m_Lens.NearClipPlane);
                    camera.m_Lens.NearClipPlane = 0.1f;
                }
            }
        }

        private static void ModifyCameraNearClipPlane(Scene scene,Camera[] cameras)
        {
            // 遍历所有相机并检查NearClipPlane属性值
            foreach (Camera camera in cameras)
            {
                if (camera.nearClipPlane < 0.1f)
                {
                    Debug.LogError("Scene: " + scene.name +" - Camera: " + camera.gameObject.name + 
                                   " - Near Clip Plane: " + camera.nearClipPlane);
                    camera.nearClipPlane = 0.1f;
                }
            }
        }
    }
}