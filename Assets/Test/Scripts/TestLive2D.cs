using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using Live2D.Cubism.Framework.Motion;

public class TestLive2D : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        App.inst.Init();
        App.inst.InitDefaultManagers();
        App.logManager.enabled = true;
        App.logManager.isPrint = true;
        Live2DManager.inst.defaultScale = 5f;

        Live2DManager.inst.CreateLive2DDataFromPath("hiyori", "Assets/Test/Resources/ab/hiyori.ab");
        Live2DRenderer renderer = Live2DManager.inst.CreateLive2DRenderer("hiyori");
        renderer.transform.localPosition = new Vector3();
        renderer.transform.SetParent(transform, false);

        //Live2DManager.inst.ReleaseLive2DRenderer(renderer);

        Live2DRenderer renderer2 = Live2DManager.inst.CreateLive2DRenderer("hiyori");
        renderer2.transform.localPosition = new Vector3(2, 0, 0);
        renderer2.transform.SetParent(transform, false);
        renderer2.animator.runtimeAnimatorController = renderer.live2DData.animatorController;

        //Live2DManager.inst.ReleaseLive2DRenderer(renderer2);

        Live2DRenderer renderer3 = Live2DManager.inst.CreateLive2DRenderer("hiyori");
        renderer3.transform.localPosition = new Vector3(4, 0, 0);
        renderer3.transform.SetParent(transform, false);
        renderer3.motionController.PlayAnimation(renderer3.live2DData.animatorController.animationClips[5]);
    }

    private void Update()
    {

    }
}
