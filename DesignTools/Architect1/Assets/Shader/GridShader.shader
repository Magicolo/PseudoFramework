Shader "Pseudo/Architect/GridShader"{
	
	Properties{
		_GridThickness("Grid Thickness", Float) = 0.01
		_GridXAmount("Grid X Amount", Int) = 10
		_GridYAmount("Grid Y Amount", Int) = 10
		_GridColour("Grid Colour", Color) = (0.5, 1.0, 1.0, 0.7)
		_ContourColour("Grid Contour Colour", Color) = (1.0, 1.0, 1.0, 1.0)
		_BaseColour("Base Colour", Color) = (0.0, 0.0, 0.0, 0.0)
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" }

		Pass{
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

		// Define the vertex and fragment shader functions
		#pragma vertex vert
		#pragma fragment frag

		// Access Shaderlab properties
		uniform float _GridThickness;
		uniform float _GridXAmount;
		uniform float _GridYAmount;
		uniform float4 _GridColour;
		uniform float4 _BaseColour;
		uniform float4 _ContourColour;

		// Input into the vertex shader
		struct vertexInput {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		// Output from vertex shader into fragment shader
		struct vertexOutput {
			float4 pos : SV_POSITION;
			float4 worldPos : TEXCOORD0;
			float2 uv : TEXCOORD1;
		};

		// VERTEX SHADER
		vertexOutput vert(vertexInput input) {
			vertexOutput output;
			output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
			// Calculate the world position coordinates to pass to the fragment shader
			output.worldPos = mul(_Object2World, input.vertex);
			output.uv = input.uv;
			return output;
		}


		//if (frac(input.worldPos.x / _GridSpacing) < _GridThickness || frac(input.worldPos.y / _GridSpacing) < _GridThickness) {


		// FRAGMENT SHADER
		float4 frag(vertexOutput input) : COLOR{
	
			float spacingX = 1 / (_GridXAmount);
			float spacingY = 1 / (_GridYAmount);
			
			if( (input.uv.x < _GridThickness/2 || input.uv.x  > 1 - _GridThickness/2) || (input.uv.y < _GridThickness/2 || input.uv.y > 1 - _GridThickness/2 ))
				return _ContourColour;
	
			else if (frac( (input.uv.x + (_GridThickness ) /2 ) / spacingX ) < _GridThickness / spacingX || frac((input.uv.y + (_GridThickness) / 2) / spacingY) < _GridThickness / spacingY)
				return _GridColour;
			
			else 
				return _BaseColour;
		}
			ENDCG
		}
	}
}
