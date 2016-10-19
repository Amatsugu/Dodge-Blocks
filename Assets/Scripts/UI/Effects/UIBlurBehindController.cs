using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections.Generic;

namespace LuminousVector
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
	public class UIBlurBehindController : PostEffectsBase
	{
		[Range(0, 2)]
		public int downsample = 1;

		public enum BlurType
		{
			StandardGauss = 0,
			SgxGauss = 1,
		}

		[Range(0.0f, 10.0f)]
		public float blurSize = 3.0f;

		[Range(1, 4)]
		public int blurIterations = 2;

		public BlurType blurType = BlurType.StandardGauss;

		public Shader blurShader = null;
		private Material blurMaterial = null;


		private static UIBlurBehindController BLUR_CONTROLLER;

		public static UIBlurBehindController instance
		{
			get
			{
				if (!BLUR_CONTROLLER)
				{
					BLUR_CONTROLLER = FindObjectOfType<UIBlurBehindController>();
					if (!BLUR_CONTROLLER)
					{
						BLUR_CONTROLLER = Camera.main.gameObject.AddComponent<UIBlurBehindController>();
					}
					else
					{
						BLUR_CONTROLLER.Init();
					}
				}
				return BLUR_CONTROLLER;
			}
		}

		private Rect screenRect;
		private List<Rect> blurRegions;

		void Init()
		{
			blurRegions = new List<Rect>();
			screenRect = new Rect(0, 0, Screen.width, Screen.height);
		}


		//Add/Update Region
		public static int AddBlurRegion(Rect region, int id = -1)
		{
			if (instance.blurRegions == null)
				instance.Init();
			//Debug.Log(region + " " + id);
			if(id == -1)
			{
				instance.blurRegions.Add(region);
				return instance.blurRegions.Count - 1;
			}else
			{
				instance.blurRegions[id] = region;
				return id;
			}
		}


		//Remove Region
		public static void RemoveBlurRegion(int id)
		{
			if (instance.blurRegions == null)
				instance.Init();
			if (instance.blurRegions.Count > id)
				instance.blurRegions.RemoveAt(id);
		}

		public override bool CheckResources()
		{
			CheckSupport(false);

			blurMaterial = CheckShaderAndCreateMaterial(blurShader, blurMaterial);

			if (!isSupported)
				ReportAutoDisable();
			return isSupported;
		}

		public void OnDisable()
		{
			if (blurMaterial)
				DestroyImmediate(blurMaterial);
		}

		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (CheckResources() == false || BLUR_CONTROLLER == null)
			{
				Graphics.Blit(source, destination);
				return;
			}

			float widthMod = 1.0f / (1.0f * (1 << downsample));

			blurMaterial.SetVector("_Parameter", new Vector4(blurSize * widthMod, -blurSize * widthMod, 0.0f, 0.0f));
			source.filterMode = FilterMode.Bilinear;

			int rtW = source.width >> downsample;
			int rtH = source.height >> downsample;

			// downsample
			RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);

			rt.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, rt, blurMaterial, 0);

			var passOffs = blurType == BlurType.StandardGauss ? 0 : 2;

			for (int i = 0; i < blurIterations; i++)
			{
				float iterationOffs = (i * 1.0f);
				blurMaterial.SetVector("_Parameter", new Vector4(blurSize * widthMod + iterationOffs, -blurSize * widthMod - iterationOffs, 0.0f, 0.0f));

				// vertical blur
				RenderTexture rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
				rt2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(rt, rt2, blurMaterial, 1 + passOffs);
				RenderTexture.ReleaseTemporary(rt);
				rt = rt2;

				// horizontal blur
				rt2 = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);
				rt2.filterMode = FilterMode.Bilinear;
				Graphics.Blit(rt, rt2, blurMaterial, 2 + passOffs);
				RenderTexture.ReleaseTemporary(rt);
				rt = rt2;
			}

			ComputeBuffer buffer = new ComputeBuffer(blurRegions.Count, 4);
			buffer.SetData(blurRegions.ToArray());
			//blurMaterial.SetBuffer("_BlurRegions", buffer);

			Graphics.Blit(source, destination);

			RenderTexture.ReleaseTemporary(rt);
		}
	}
}
