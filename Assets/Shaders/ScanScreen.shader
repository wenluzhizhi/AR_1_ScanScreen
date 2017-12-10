Shader "Custom/ScanScreen" {
	properties{
	    _MainTex("MainTex",2D)=""{}
	    edgeColor("edge",Color)=(1,0,0,1)
	    Range1("R",Range(0,5))=1
	}
	Subshader{
	   pass{
	       CGPROGRAM
	       #pragma vertex vert
	       #pragma fragment frag

	       struct a2v{
	          float4  vertex:POSITION;
	          float4  texcoord:TEXCOORD;
	       };

	       struct v2f{
	          float4 pos:SV_POSITION;
	          half2 uv[9]:TEXCOORD0;
	       };

	       sampler2D _MainTex;
	       float4 _MainTex_ST;
	       float4 _MainTex_TexelSize;
	       fixed4 edgeColor;
	       float Range1;
	       v2f vert(a2v v){
	          v2f o;
	          o.pos=mul(UNITY_MATRIX_MVP,v.vertex);
	          o.uv[0]=v.texcoord.xy+_MainTex_TexelSize.xy*half2(-1,-1);
	          o.uv[1]=v.texcoord.xy+_MainTex_TexelSize.xy*half2(0,-1);
	          o.uv[2]=v.texcoord.xy+_MainTex_TexelSize.xy*half2(1,-1);

	          o.uv[3]=v.texcoord.xy+_MainTex_TexelSize.xy*half2(-1,0);
	          o.uv[4]=v.texcoord.xy+_MainTex_TexelSize.xy*half2(0,0);
	          o.uv[5]=v.texcoord.xy+_MainTex_TexelSize.xy*half2(1,0);

	          o.uv[6]=v.texcoord.xy+_MainTex_TexelSize.xy*half2(-1,-1);
	          o.uv[7]=v.texcoord.xy+_MainTex_TexelSize.xy*half2(0,-1);
	          o.uv[8]=v.texcoord.xy+_MainTex_TexelSize.xy*half2(1,-1);


	          return o;
	       }

	        fixed luminance(fixed4 c){
	          return 0.2125*c.r+0.7154*c.g+0.0721*c.b;
	       }

	       half Sobel(v2f x){
	          const half Gx[9]={
	               -1,-2,-1,
	                0,0,0,
	                1,2,1
	          };

	          const half Gy[9]={
	               -1,0,1,
	               -2,0,2,
	               -1,0,1
	          };

	          half edgeX=0;
	          half edgeY=0;
	          half t;
	          for(int i=0;i<9;i++)
	          {
	             t=tex2D(_MainTex,x.uv[i]);
	             edgeX+=t*Gx[i];
	             edgeY+=t*Gy[i];
	          }

	          half edge=abs(edgeX)+abs(edgeY);

	          return edge; 
	       }

	      





	       fixed4 frag(v2f i):SV_Target{
	          fixed4 col=fixed4(1,0,0,1);
	          half2 v1=i.uv[4];
	           col=tex2D(_MainTex,v1);
	           half d=Sobel(i);
	           if(d>Range1)
	           {
	             if(abs(v1.y-frac(_Time.y))<0.05)
	             {
	                col=lerp(col,edgeColor,d);
	             }
	           }
	           return col;
	       }


	       ENDCG
	   }
	}
}
