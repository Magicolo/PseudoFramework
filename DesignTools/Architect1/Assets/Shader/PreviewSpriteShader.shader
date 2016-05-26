Shader "Pseudo/Architect/PreviewSpriteShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		_ContourColour("ContourColour",Color) = (1,1,1,1)
			
		_BorderWeight("Boarder Weight", float) = 0.1
	}
	SubShader
	{
		// No culling or depth
		Cull Off 
		ZWrite Off 
		ZTest Always
		//Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON

			#include "UnityCG.cginc"

			uniform float _BorderWeight;
			uniform float _ContourColour;

			struct Vertex
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct Fragment
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float2 uv2 : TEXCOORD1;
			};

			Fragment vert (Vertex v)
			{
				Fragment o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.uv = float2(v.uv);
				return o;
			}
			
			sampler2D _MainTex;


			fixed4 frag (Fragment input) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, input.uv);
			// just invert the colors
			//col = (1 - col) * 0.8;
			return col;
				float toto = 0.3;
				if (input.uv2.x < toto)
					return _ContourColour;
				/*if ((input.uv.x < _BorderWeight / 2 || input.uv.x  > 1 - _BorderWeight / 2) || (input.uv.y < _BorderWeight / 2 || input.uv.y > 1 - _BorderWeight / 2))
					return _ContourColour;
			*/
				else 
				{
					fixed4 col = tex2D(_MainTex, input.uv);
					// just invert the colors
					//col = (1 - col) * 0.8;
					return col;
				}
					
			}
			ENDCG
		}
	}
}
