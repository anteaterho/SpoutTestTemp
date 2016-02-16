using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TextureRefractionExample : MonoBehaviour
{
    public float speed = 1;

    private Material _material;
    private Material Material
    {
        get
        {
            if (_material == null)
                _material = new Material(Shader.Find("Hidden/ShaderToy/TextureRefractionExample2"));

            return _material;
        }
    }


    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Material.SetFloat("_Speed", speed);
        Graphics.Blit(source, destination, Material);
    }
}