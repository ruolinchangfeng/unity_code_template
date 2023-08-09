using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Core
{
    public class UpdateMaterial : EditorWindow
    {
        private static ReferenceFinderData _refFinderData;

        [MenuItem("Tools/Fix Shader Missed")]
        public static void FindCinemachineCamerasWithNearClipPlane()
        {
            ModifyPrefabNearClipPlane();
        }

        private static void ModifyPrefabNearClipPlane()
        {
            // string shaderPath = "Assets/GameRes/Shaders/ShaderGraph/Effect/p33_add.shader";
            // Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
            //
            //
            // string materialPath = "Assets/GameRes/Effects/_Material/em_guang_shifa_04.mat";
            // // string materialPath = "Assets/GameRes/Effects/_Material/em_zhusheng_glow_01.mat";
            // Material material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            //
            // var renderQueue = material.renderQueue;
            // var shaderRenderQueue = material.shader.renderQueue;
            //
            // material.shader = null;
            // material.shader = shader;
            // if (renderQueue!=shaderRenderQueue)
            // {
            //     material.renderQueue = renderQueue;
            // }
            //
            // AssetDatabase.SaveAssets();
            

            if (_refFinderData == null)
            {
                _refFinderData = new ReferenceFinderData();
            }
            
            _refFinderData.ClearCache();
            _refFinderData.CollectDependenciesInfo();
            
            // TODO 
            string[] guids = AssetDatabase.FindAssets("t:shader",new string[]{"Assets/GameRes/Shaders/ShaderGraph"});
            
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(assetPath);
            
                // 找到shader引用的所有材质
                if (_refFinderData.IsReference(guid))
                {
                    var data = _refFinderData.assetDict[guid];
                    foreach (var reference in data.references)
                    {
                        string refPath = AssetDatabase.GUIDToAssetPath(reference);
                        Material material = AssetDatabase.LoadAssetAtPath<Material>(refPath);
            
                        var renderQueue = material.renderQueue;
                        var shaderRenderQueue = material.shader.renderQueue;
            
                        material.shader = null;
                        material.shader = shader;
            
                        if (renderQueue!=shaderRenderQueue)
                        {
                            material.renderQueue = renderQueue;
                        }
                        
                    }
                }
            }
            
            AssetDatabase.SaveAssets();

            // string shaderPath = "Assets/GameRes/Shaders/ShaderGraph/WindTree.shader";
            // Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
            // var guid = shader.GetGUID();
            //
            // // 找到shader引用的所有材质
            // if (_refFinderData.IsReference(guid))
            // {
            //     var data = _refFinderData.assetDict[guid];
            //     foreach (var reference in data.references)
            //     {
            //         string refPath = AssetDatabase.GUIDToAssetPath(reference);
            //         Material material = AssetDatabase.LoadAssetAtPath<Material>(refPath);
            //
            //         var renderQueue = material.renderQueue;
            //         var shaderRenderQueue = material.shader.renderQueue;
            //
            //         material.shader = null;
            //         material.shader = shader;
            //
            //         if (renderQueue!=shaderRenderQueue)
            //         {
            //             material.renderQueue = renderQueue;
            //         }
            //             
            //     }
            // }
            //
            // AssetDatabase.SaveAssets();
        }
        
    }
}
