using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CutsceneRenderFeature : ScriptableRendererFeature
{
    public Shader cutsceneShader;

    CutsceneRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        if (cutsceneShader == null)
        {
            Debug.LogError("转场shader不能为空");
        }
        m_ScriptablePass = new CutsceneRenderPass(RenderPassEvent.BeforeRenderingPostProcessing, CoreUtils.CreateEngineMaterial(cutsceneShader));
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        m_ScriptablePass.Setup(renderer);
        renderer.EnqueuePass(m_ScriptablePass);
    }
}
class CutsceneRenderPass : ScriptableRenderPass
{
    // This method is called before executing the render pass.
    // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
    // When empty this render pass will render to the active camera render target.
    // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
    // The render pipeline will ensure target setup and clearing happens in a performant manner.

    Material cutsceneMaterial;
    CutsceneVolume m_CutsceneVolume;
    ScriptableRenderer m_Renderer;

    static readonly string m_ProfilerTag = "Render cut scene effects";
    static readonly int TempTarget = Shader.PropertyToID("Cut scene temp");

    //shader参数
    static readonly int _FLIPOVER_Width = Shader.PropertyToID("_FLIPOVER_Width");
    static readonly int _FLIPOVER_Progress = Shader.PropertyToID("_FLIPOVER_Progress");

    static readonly int _CLOCKWIPE_Width = Shader.PropertyToID("_CLOCKWIPE_Width");
    static readonly int _CLOCKWIPE_Blend = Shader.PropertyToID("_CLOCKWIPE_Blend");

    static readonly int _GRAYSCALE_Value = Shader.PropertyToID("_GRAYSCALE_Value");

    static readonly int _DENSEFOG1_MainTex = Shader.PropertyToID("_DENSEFOG1_MainTex");
    static readonly int _DENSEFOG1_FlowMapTex = Shader.PropertyToID("_DENSEFOG1_FlowMapTex");
    static readonly int _DENSEFOG1_Offset = Shader.PropertyToID("_DENSEFOG1_Offset");
    static readonly int _DENSEFOG1_Speed = Shader.PropertyToID("_DENSEFOG1_Speed");
    static readonly int _DENSEFOG1_Intensity = Shader.PropertyToID("_DENSEFOG1_Intensity");

    static readonly int _DENSEFOG2_Mask = Shader.PropertyToID("_DENSEFOG2_Mask");
    static readonly int _DENSEFOG2_Noise = Shader.PropertyToID("_DENSEFOG2_Noise");
    static readonly int _DENSEFOG2_Noise1Params = Shader.PropertyToID("_DENSEFOG2_Noise1Params");
    static readonly int _DENSEFOG2_Noise2Params = Shader.PropertyToID("_DENSEFOG2_Noise2Params");
    static readonly int _DENSEFOG2_Color1 = Shader.PropertyToID("_DENSEFOG2_Color1");
    static readonly int _DENSEFOG2_Color2 = Shader.PropertyToID("_DENSEFOG2_Color2");



    public CutsceneRenderPass(RenderPassEvent evt, Material blitMaterial)
    {
        renderPassEvent = evt;
        cutsceneMaterial = blitMaterial;
    }


    // Here you can implement the rendering logic.
    // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
    // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
    // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var stack = VolumeManager.instance.stack;
        m_CutsceneVolume = stack.GetComponent<CutsceneVolume>();

        if (m_CutsceneVolume == null || !m_CutsceneVolume.IsActive())
        {
            return;
        }
        if (!cutsceneMaterial.IsKeywordEnabled(m_CutsceneVolume.mode.ToString()))
        {
            foreach (var keyword in cutsceneMaterial.shaderKeywords)
            {
                cutsceneMaterial.DisableKeyword(keyword);
            }
            cutsceneMaterial.EnableKeyword(m_CutsceneVolume.mode.value.ToString());
        }

        UpdateMaterial(context, ref renderingData);

    }

    public void Setup(ScriptableRenderer m_Renderer)
    {
        this.m_Renderer = m_Renderer;
    }
    void UpdateMaterial(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
        RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
        int width = desc.width;
        int height = desc.height;
        
        switch (m_CutsceneVolume.mode.value)
        {
            case CutsceneVolume.CutsceneMode.None:
                break;
            case CutsceneVolume.CutsceneMode._FLIPOVER:
                Shader.SetGlobalFloat(_FLIPOVER_Width, m_CutsceneVolume._FLIPOVER_Width.value);
                Shader.SetGlobalFloat(_FLIPOVER_Progress, m_CutsceneVolume._FLIPOVER_Progress.value);
                cmd.GetTemporaryRT(TempTarget, width, height, 0, FilterMode.Bilinear, desc.colorFormat);
                cmd.Blit(m_Renderer.cameraColorTarget, TempTarget);
                cmd.SetGlobalTexture("_SourceTex", TempTarget);
                cmd.Blit(TempTarget, m_Renderer.cameraColorTarget, cutsceneMaterial);
                break;
            case CutsceneVolume.CutsceneMode._CLOCKWIPE:
                Shader.SetGlobalFloat(_CLOCKWIPE_Width, m_CutsceneVolume._CLOCKWIPE_Width.value * (width + height));
                Shader.SetGlobalFloat(_CLOCKWIPE_Blend, m_CutsceneVolume._CLOCKWIPE_Blend.value);
                cmd.GetTemporaryRT(TempTarget, width, height, 0, FilterMode.Bilinear, desc.colorFormat);
                cmd.Blit(m_Renderer.cameraColorTarget, TempTarget);
                cmd.SetGlobalTexture("_SourceTex", TempTarget);
                cmd.Blit(TempTarget, m_Renderer.cameraColorTarget, cutsceneMaterial);
                break;
            case CutsceneVolume.CutsceneMode._DOUBLECLOCKWIPE:
                break;
            case CutsceneVolume.CutsceneMode._WEDEWIPE:
                break;
            case CutsceneVolume.CutsceneMode._INKFADE:
                break;
            case CutsceneVolume.CutsceneMode._SLIDINGBANDS:
                break;
            case CutsceneVolume.CutsceneMode._CHECKERWIPE:
                break;
            case CutsceneVolume.CutsceneMode._DISSOLVE:
                break;
            case CutsceneVolume.CutsceneMode._DIAMONDDISSOLVE:
                break;
            case CutsceneVolume.CutsceneMode._TRIANGLEDISSOLVE:
                break;
            case CutsceneVolume.CutsceneMode._DOOR:
                break;
            case CutsceneVolume.CutsceneMode._SPIN:
                break;
            case CutsceneVolume.CutsceneMode._CENTERMERGE:
                break;
            case CutsceneVolume.CutsceneMode._CENTERSPLIT:
                break;
            case CutsceneVolume.CutsceneMode._BANDSLIDE:
                break;
            case CutsceneVolume.CutsceneMode._IRISROUND:
                break;
            case CutsceneVolume.CutsceneMode._RANDOMBLOCKS:
                break;
            case CutsceneVolume.CutsceneMode._RANDOMBWIPE:
                break;
            case CutsceneVolume.CutsceneMode._GRAYSCALE:
                Shader.SetGlobalFloat(_GRAYSCALE_Value, m_CutsceneVolume._GRAYSCALE_Value.value);
                cmd.GetTemporaryRT(TempTarget, width, height, 0, FilterMode.Bilinear, desc.colorFormat);
                cmd.Blit(m_Renderer.cameraColorTarget, TempTarget);
                cmd.SetGlobalTexture("_SourceTex", TempTarget);
                cmd.Blit(TempTarget, m_Renderer.cameraColorTarget, cutsceneMaterial);
                break;
            case CutsceneVolume.CutsceneMode._DENSEFOG1:
                Shader.SetGlobalTexture(_DENSEFOG1_MainTex, m_CutsceneVolume._DENSEFOG1_MainTex.value);
                Shader.SetGlobalTexture(_DENSEFOG1_FlowMapTex, m_CutsceneVolume._DENSEFOG1_FlowMapTex.value);
                Shader.SetGlobalFloat(_DENSEFOG1_Offset, m_CutsceneVolume._DENSEFOG1_Offset.value);
                Shader.SetGlobalFloat(_DENSEFOG1_Speed, m_CutsceneVolume._DENSEFOG1_Speed.value);
                Shader.SetGlobalFloat(_DENSEFOG1_Intensity, m_CutsceneVolume._DENSEFOG1_Intensity.value);
                cmd.GetTemporaryRT(TempTarget, width, height, 0, FilterMode.Bilinear, desc.colorFormat);
                cmd.Blit(m_Renderer.cameraColorTarget, TempTarget);
                cmd.SetGlobalTexture("_SourceTex", TempTarget);
                cmd.Blit(TempTarget, m_Renderer.cameraColorTarget, cutsceneMaterial);
                break;
            case CutsceneVolume.CutsceneMode._DENSEFOG2:
                Shader.SetGlobalTexture(_DENSEFOG2_Mask, m_CutsceneVolume._DENSEFOG2_Mask.value);
                Shader.SetGlobalTexture(_DENSEFOG2_Noise, m_CutsceneVolume._DENSEFOG2_Noise.value);
                Shader.SetGlobalVector(_DENSEFOG2_Noise1Params, m_CutsceneVolume._DENSEFOG2_Noise1Params.value);
                Shader.SetGlobalVector(_DENSEFOG2_Noise2Params, m_CutsceneVolume._DENSEFOG2_Noise2Params.value);
                Shader.SetGlobalColor(_DENSEFOG2_Color1, m_CutsceneVolume._DENSEFOG2_Color1.value);
                Shader.SetGlobalColor(_DENSEFOG2_Color2, m_CutsceneVolume._DENSEFOG2_Color2.value);
                cmd.GetTemporaryRT(TempTarget, width, height, 0, FilterMode.Bilinear, desc.colorFormat);
                cmd.Blit(m_Renderer.cameraColorTarget, TempTarget);
                cmd.SetGlobalTexture("_SourceTex", TempTarget);
                cmd.Blit(TempTarget, m_Renderer.cameraColorTarget, cutsceneMaterial);
                break;
            default:
                break;
        }
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}

