using UnityEngine;

public class ShaderHandler : MonoBehaviour
{
    //Propriedades
    public Material effectMaterial;
    //Construtor
    private void OnRenderImage(RenderTexture source, RenderTexture destination )
    {
        Graphics.Blit( source, destination, effectMaterial);
    }
}
