using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    public ComputeShader cs;
    public ComputeBuffer buffer;
    public Shader outputShader;

    private Material outputMat;
    private int kernel;
    private int pixelCount;
    private void Start()
    {
        outputMat = new Material(outputShader);
        pixelCount = Screen.width * Screen.height;
        float[] data = new float[pixelCount];
        data.Initialize();
        buffer = new ComputeBuffer(pixelCount, sizeof(float), ComputeBufferType.Counter);
        buffer.SetData(data);
        kernel = cs.FindKernel("CSMain");
        cs.SetBuffer(kernel, "res", buffer);
        cs.SetInt("width", Screen.width);
        cs.SetInt("height", Screen.height);
        outputMat.SetBuffer("res", buffer);
    }

    private void OnPreRender()
    {
        cs.Dispatch(kernel, Screen.width / 8, Screen.height / 8, 1);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if(outputMat != null)
        {
            Graphics.Blit(src, dst, outputMat);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }
}
