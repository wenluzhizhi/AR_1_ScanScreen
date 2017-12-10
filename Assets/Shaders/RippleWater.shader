Shader "Custom/RippleWater" {
	Properties{
	   _MainTex("MainTex",2D)="white"{}
	}
	Subshader{
	   pass{
	      CGPROGRAM
	      #pragma vertex vert
	      #pragma fragment frag

	      struct a2v{
	         float4 vertex:POSITION;
	         float4 texcoord:TEXCOORD;
	      };


	      struct v2f{
	         float4 pos:SV_POSITION;
	         float2 uv:TEXCOORD0;
	      };

	      sampler2D _MainTex;

	      v2f vert(a2v v){
	          v2f o;
	          o.pos=mul(UNITY_MATRIX_MVP,v.vertex);
	          o.uv=v.texcoord.xy;
	          return o;

	      }

	      fixed4 frag(v2f i):SV_Target{
	         fixed4 col=fixed4(1,0,0,1);
	         col=tex2D(_MainTex,i.uv);
	         return col;
	      }

	      ENDCG
	   }
	}
}
