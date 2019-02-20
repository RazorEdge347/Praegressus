using UnityEngine;
using UnityEngine.Experimental.PostProcessing;

namespace UnityEditor.Experimental.PostProcessing
{
    public static class VolumeCreator
    {
        [MenuItem("GameObject/3D Object/Post-process Volume")]
        static void CreateVolume()
        {
            var gameObject = new GameObject("Post-process Volume");
            var collider = gameObject.AddComponent<BoxCollider>();
            collider.size = Vector3.one;
            collider.isTrigger = true;
            gameObject.AddComponent<PostProcessVolume>();

            Selection.objects = new [] { gameObject };
            EditorApplication.ExecuteMenuItem("GameObject/Move To View");
        }
    }
}
