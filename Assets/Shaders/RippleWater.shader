Shader "Custom/RippleWater" {
	Properties{
	   _MainTex("MainTex",2D)="white"{}
	   _BumpMap("bjmp",2D)="Bump"{}
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
	      uniform sampler2D _BumpMap;

	      v2f vert(a2v v){
	          v2f o;
	          o.pos=mul(UNITY_MATRIX_MVP,v.vertex);
	          o.uv=v.texcoord.xy;
	          return o;

	      }

	      fixed4 frag(v2f i):SV_Target{
	         fixed4 col=fixed4(1,0,0,1);
	         float2 uv2=i.uv;
	         uv2.y=1-uv2.y;
	         float4 g=tex2D(_BumpMap,uv2).g;
	         i.uv+=g.g*0.05;
	         col=tex2D(_MainTex,i.uv);
	         return col;
	      }

	      ENDCG
	   }
	}
}
