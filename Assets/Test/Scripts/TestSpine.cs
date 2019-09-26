using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Spine;
using Spine.Unity;

public class TestSpine : MonoBehaviour
{
    private void Awake()
    {
        App.inst.Init();
        App.inst.InitDefaultManagers();
        App.logManager.enabled = true;
        App.logManager.isPrint = true;

        //SpineManager.inst.CreateSpineDataFromResources("hero", "hero/hero-pro");
        SpineManager.inst.CreateSpineDataFromPath("hero-pro", "Assets/Test/Resources/ab/hero-pro.ab");

        SpineRenderer sr = SpineManager.inst.CreateSpineRenderer("hero-pro", true);
        sr.transform.SetParent(transform, false);
        sr.skeletonAnimation.state.SetAnimation(0, "idle", true);
        sr.skeletonAnimation.skeletonDataAsset.atlasAssets[0].PrimaryMaterial.shader = ShaderManager.inst.GetShader("Spine/Skeleton Fill");

        SpineRenderer sr2 = SpineManager.inst.CreateSpineRenderer("hero-pro", true);
        sr2.transform.SetParent(transform, false);
        sr2.transform.localPosition = new Vector3(2, 0, 0);
        sr2.skeletonAnimation.state.SetAnimation(0, "idle", true);
        sr2.skeletonAnimation.skeletonDataAsset.atlasAssets[0].PrimaryMaterial.shader = ShaderManager.inst.GetShader("Spine/Skeleton Fill");

        SpineManager.inst.ReleaseSpineRenderer(sr2);

        SpineRenderer sr3 = SpineManager.inst.CreateSpineRenderer("hero-pro");
        sr3.transform.SetParent(transform, false);
        sr3.transform.localPosition = new Vector3(4, 0, 0);
        sr3.skeletonAnimation.state.SetAnimation(0, "idle", true);
        sr3.skeletonAnimation.skeletonDataAsset.atlasAssets[0].PrimaryMaterial.shader = ShaderManager.inst.GetShader("Spine/Skeleton Fill");

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetFloat("_FillPhase", 0.5f);
        sr.meshRenderer.SetPropertyBlock(mpb);
        sr2.meshRenderer.SetPropertyBlock(mpb);
        mpb.SetFloat("_FillPhase", 0.8f);
        sr3.meshRenderer.SetPropertyBlock(mpb);

        SpineManager.inst.ReleaseSpineRenderer(sr3);

        SpineRenderer sr4 = SpineManager.inst.CreateSpineRenderer("hero-pro");
        sr4.transform.SetParent(transform, false);
        sr4.transform.localPosition = new Vector3(6, 0, 0);
        sr4.skeletonAnimation.state.SetAnimation(0, "attack", true);
        sr4.skeletonAnimation.skeletonDataAsset.atlasAssets[0].PrimaryMaterial.shader = ShaderManager.inst.GetShader("Spine/Skeleton Fill");
    }
}
