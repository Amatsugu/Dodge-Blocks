using UnityEngine;
using ProtoBuf;
using System.Collections;

namespace LuminousVector
{
	[ProtoContract]
	public class SColor
	{
		//Public
		//RGBA
		public float r
		{
			set
			{
				_r = value;
				RGBToHSV(color, out _h, out _s, out _v);
			}
			get { return _r; }
		}
		public float g
		{
			set
			{
				_g = value;
				RGBToHSV(color, out _h, out _s, out _v);
			}
			get { return _g; }
		}
		public float b
		{
			set
			{
				_b = value;
				RGBToHSV(color, out _h, out _s, out _v);
			}
			get { return _b; }
		}
		public float a { set { _a = value; } get { return _a; } }
		//HSV
		public float h
		{
			set
			{
				_h = value;
				Color rgb = HSVToRGB(_h, _s, _v);
				_r = rgb.r;
				_g = rgb.g;
				_b = rgb.b;
			}
			get { return _h; }
		}
		public float s
		{
			set
			{
				_s = value;
				Color rgb = HSVToRGB(_h, _s, _v);
				_r = rgb.r;
				_g = rgb.g;
				_b = rgb.b;
			}
			get { return _s; }
		}
		public float v
		{
			set
			{
				_v = value;
				Color rgb = HSVToRGB(_h, _s, _v);
				_r = rgb.r;
				_g = rgb.g;
				_b = rgb.b;
			}
			get { return _v; }
		}
		//RGBA
		[ProtoMember(1)]
		private float _r;
		[ProtoMember(2)]
		private float _g;
		[ProtoMember(3)]
		private float _b;
		[ProtoMember(4)]
		private float _a = 1;
		//HSV
		[ProtoMember(5)]
		private float _h;
		[ProtoMember(6)]
		private float _s;
		[ProtoMember(7)]
		private float _v;

		public Color color
		{
			get
			{
				return new Color(r, g, b, a);
			}
			set
			{
				r = value.r;
				g = value.g;
				b = value.b;
				a = value.a;
			}
		}

		public static SColor Srandom
		{
			get
			{
				return new SColor(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
			}
		}

		public static Color random
		{
			get
			{
				return new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
			}
		}

		public SColor()
		{
		}

		public SColor(Color col)
		{
			r = col.r;
			g = col.g;
			b = col.b;
			a = col.a;
		}

		public SColor(float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public SColor(float r, float g, float b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			a = 1;
		}

		public void SetHSV(float H, float S, float V)
		{
			h = H;
			s = S;
			v = V;
		}

		public void GetHSV(out float H, out float S, out float V)
		{
			H = _h;
			S = _s;
			V = _v;
		}

		public static Color HSVToRGB(float H, float S, float V)
		{
			if (S == 0f)
				return new Color(V, V, V);
			else if (V == 0f)
				return Color.black;
			else
			{
				Color col = Color.black;
				float Hval = H * 6f;
				int sel = Mathf.FloorToInt(Hval);
				float mod = Hval - sel;
				float v1 = V * (1f - S);
				float v2 = V * (1f - S * mod);
				float v3 = V * (1f - S * (1f - mod));
				switch (sel + 1)
				{
					case 0:
						col.r = V;
						col.g = v1;
						col.b = v2;
						break;
					case 1:
						col.r = V;
						col.g = v3;
						col.b = v1;
						break;
					case 2:
						col.r = v2;
						col.g = V;
						col.b = v1;
						break;
					case 3:
						col.r = v1;
						col.g = V;
						col.b = v3;
						break;
					case 4:
						col.r = v1;
						col.g = v2;
						col.b = V;
						break;
					case 5:
						col.r = v3;
						col.g = v1;
						col.b = V;
						break;
					case 6:
						col.r = V;
						col.g = v1;
						col.b = v2;
						break;
					case 7:
						col.r = V;
						col.g = v3;
						col.b = v1;
						break;
				}
				col.r = Mathf.Clamp(col.r, 0f, 1f);
				col.g = Mathf.Clamp(col.g, 0f, 1f);
				col.b = Mathf.Clamp(col.b, 0f, 1f);
				return col;
			}
		}

		public static void RGBToHSV(Color rgbColor, out float H, out float S, out float V)
		{
			if (rgbColor.b > rgbColor.g && rgbColor.b > rgbColor.r)
			{
				RGBToHSVHelper(4f, rgbColor.b, rgbColor.r, rgbColor.g, out H, out S, out V);
			}
			else
			{
				if (rgbColor.g > rgbColor.r)
				{
					RGBToHSVHelper(2f, rgbColor.g, rgbColor.b, rgbColor.r, out H, out S, out V);
				}
				else
				{
					RGBToHSVHelper(0f, rgbColor.r, rgbColor.g, rgbColor.b, out H, out S, out V);
				}
			}
		}

		private static void RGBToHSVHelper(float offset, float dominantcolor, float colorone, float colortwo, out float H, out float S, out float V)
		{
			V = dominantcolor;
			if (V != 0f)
			{
				float num = 0f;
				if (colorone > colortwo)
				{
					num = colortwo;
				}
				else
				{
					num = colorone;
				}
				float num2 = V - num;
				if (num2 != 0f)
				{
					S = num2 / V;
					H = offset + (colorone - colortwo) / num2;
				}
				else
				{
					S = 0f;
					H = offset + (colorone - colortwo);
				}
				H /= 6f;
				if (H < 0f)
				{
					H += 1f;
				}
			}
			else
			{
				S = 0f;
				H = 0f;
			}
		}

	}
}
