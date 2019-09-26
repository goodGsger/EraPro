using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Framework
{
    public class EditorTest
    {
        /// <summary>
        /// 发布测试Spine AssetBundle
        /// </summary>
        [MenuItem("EditorTest/Create Test Spine AssetBundle", false, 101)]
        public static void CreateTestSpineAssetBundle()
        {
            BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;
            List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = "hero-pro.ab";
            build.assetNames = new string[] { "Assets/Test/Resources/hero/hero-pro.png", "Assets/Test/Resources/hero/hero-pro.json", "Assets/Test/Resources/hero/hero-pro.atlas.txt" };
            builds.Add(build);

            string outputPath = "Assets/Test/Resources/ab";
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;

            BuildPipeline.BuildAssetBundles(outputPath, builds.ToArray(), buildOptions, buildTarget);
        }

        /// <summary>
        /// 发布测试Live2D AssetBundle
        /// </summary>
        [MenuItem("EditorTest/Create Test Live2D AssetBundle", false, 102)]
        public static void CreateTestLive2DAssetBundle()
        {
            BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;
            List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
            AssetBundleBuild build = new AssetBundleBuild();
            build.assetBundleName = "hiyori.ab";
            build.assetNames = new string[] { "Assets/Test/Resources/hiyori/hiyori.prefab", "Assets/Test/Resources/hiyori/hiyori.fadeMotionList.asset", "Assets/Test/Resources/hiyori/Animation/hiyori.controller" };
            builds.Add(build);

            string outputPath = "Assets/Test/Resources/ab";
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;

            BuildPipeline.BuildAssetBundles(outputPath, builds.ToArray(), buildOptions, buildTarget);
        }
    }
}
