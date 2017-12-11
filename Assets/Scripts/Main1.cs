using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main1 : MonoBehaviour {

	[SerializeField]private RawImage webRawImage;
	[SerializeField] private GameObject MainUI;
	[SerializeField] private InputField colorIpt_X;
	[SerializeField] private InputField colorIpt_Y;
	[SerializeField] private InputField colorIpt_Z;
	public Material ma;
	public Material maRipple;
	public int shaderNum=0;


	public float[] currentRippleStrenth;
	public float[] nextCurrentRippleSrenth;
	public Color32[] normalColorBuf;

	public int width=0;
	public int height=0;
	public int StrenthSize=5;
	public int Strenth=128;
	public Texture2D td;
	void Start () {

		MainUI.SetActive (false);
		StartCoroutine (startOpenCamera());

		width = Screen.width / 10;
		height = Screen.height / 10;

		currentRippleStrenth=new float[width*height];
		nextCurrentRippleSrenth=new float[width*height];

		normalColorBuf=new Color32[width*height];

		td = new Texture2D (width, height, TextureFormat.RGBA32, false);
		maRipple.SetTexture ("_BumpMap",td);
	}
	

	void Update () {
	
	}

	void FixedUpdate(){
		RippleSpread ();
		BufferToColor ();
		if (Input.GetMouseButton (0)) {
			OnClickScreen ();
		}
	}

	void OnClickScreen(){

		Vector3 v = Input.mousePosition;
		AddRippleStrenth ((int)v.x,(int)v.y,StrenthSize,Strenth);
	}


	public void AddRippleStrenth(int x,int y,int size,int strenth){
		int count = currentRippleStrenth.Length;
		x = x / 10;
		y = y / 10;
		if (y * width + x >= count) {
			return;
		}
		for (int i = x - size; i < x + size; i++) 
		{
			if (i < 0 || i >= width)
				return;
			for (int j = y - size; j < y + size; j++)
			{
				if (j < 0 || j >= height)
					return;
				nextCurrentRippleSrenth [i + y * width] = strenth;
			}
		}


	}


	void BufferToColor(){
		for (int i = 0; i < nextCurrentRippleSrenth.Length; i++) {
			normalColorBuf[i].g=(byte)(currentRippleStrenth[i]);
		}
		td.SetPixels32 (normalColorBuf);
		td.Apply ();
	}




	IEnumerator startOpenCamera(){
		yield return Application.RequestUserAuthorization (UserAuthorization.WebCam);
		if (Application.HasUserAuthorization (UserAuthorization.WebCam)) {
			WebCamTexture wbt = new WebCamTexture ();
			WebCamDevice[] devices= WebCamTexture.devices;
			string _deviceName = "";
			if (devices.Length >= 1) {
				_deviceName = devices [0].name;
			}
			wbt.name = _deviceName;
			webRawImage.texture = wbt;
			wbt.Play ();
		}

	}



	void OnRenderImage(RenderTexture src,RenderTexture dest){
		if (ma != null && shaderNum == 0) {
			
			Graphics.Blit (src, dest, ma);
			
		} else if (maRipple != null && shaderNum == 1) {
			Graphics.Blit (src, dest, maRipple);
		}
		else {
			Graphics.Blit (src,dest);
		}
	}


	public void setMF(float value){
		if (ma != null) {
			ma.SetFloat ("Range1",value);
		}
	}



	public void OpenSetingPanel(){
		MainUI.SetActive (!MainUI.activeInHierarchy);
		int cx = int.Parse (colorIpt_X.text);
		int cy = int.Parse (colorIpt_Y.text);
		int cz = int.Parse (colorIpt_Z.text);


		Vector4 c1 = new Vector4 (cx/255.0f,cy/255.0f,cz/255.0f,1);
		ma.SetVector ("edgeColor",c1);

	}

	void RippleSpread(){

		float leftPoint;
		float rightPoint;
		float bottomPoint;
		float topPoint;



		for(int i=0;i<currentRippleStrenth.Length;i++)
		{
			
			if ((i % width - 1) >=0) {
				leftPoint = nextCurrentRippleSrenth [i - 1];
			} else {
				leftPoint = nextCurrentRippleSrenth [i + 1];
			}

			if ((i % width +1) <width) {
				rightPoint = nextCurrentRippleSrenth [i + 1];
			} else {
				rightPoint = nextCurrentRippleSrenth [i - 1];
			}


			if (i - width  >=0) {
				bottomPoint = nextCurrentRippleSrenth [i - width];
			} else {
				bottomPoint = nextCurrentRippleSrenth [i + width];
			}

			if (i+width<nextCurrentRippleSrenth.Length) {
				topPoint = nextCurrentRippleSrenth [i + width];
			} else {
				topPoint = nextCurrentRippleSrenth [i - width];
			}
			currentRippleStrenth[i]=(leftPoint+rightPoint+topPoint+bottomPoint)*0.5f-currentRippleStrenth[i];

			currentRippleStrenth [i] -= currentRippleStrenth [i]/128f;
			if (currentRippleStrenth [i]<= 1) {
				currentRippleStrenth [i] = 0;
			}
		}


		float[] buf3 = nextCurrentRippleSrenth;
		nextCurrentRippleSrenth = currentRippleStrenth;
		currentRippleStrenth = buf3;
	}


	public void OnClickScanBtn(){
		Debug.Log ("scan......");
		shaderNum = 0;
	}

	public void OnClickRippleBtn(){
		Debug.Log ("ripple");
		shaderNum = 1;
	}



	void OnGUI1(){
		if (GUILayout.Button ("testBtn")) {
			RippleSpread ();
		}
	}

}
