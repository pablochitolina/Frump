using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class PersonagemScript : MonoBehaviour {

	public GameObject personagem;
	private float sizePersonagem;
	private bool direita = true;
	private bool esquerda = false;
	public AudioClip pula;
	public AudioClip bateu;
	public AudioClip morre;
	private bool pulo = false;
	public int numeroPulos = 0;
	private Rigidbody2D sapo;
	private float forca = 1300;
	private float tempoSemPulo = 0;
	public Texture exit;
	public Texture again;
	public GameObject objMenuNow;
	public GameObject objMenuMax;
	public GameObject objMetros;
	public GameObject paredeDir;
	public GameObject paredeEsq;
	private float posicaoX;
	private float tamParede;
	private GameObject paredeDirInstance, paredeEsqInstance;
	private float deslocamentos = 1;
	public bool bateuChao = false;
	private bool morto = true;
	private float metros;
	private float maxMeters;
	private TextMesh textMetrosMenu;
	private TextMesh textMaxMenu;
	private TextMesh textMetros;
	public GameObject menu;
	private float maximo;
	private Animator animatorSapo;
	public GameObject grid;
	public GameObject blackGrid;
	private bool subRankingBool = false;
	public GameObject tuto;
	public GameObject subRanking;
	public GameObject connect;
	private Vector3 posInicial;

	public GameObject bg1;
	public GameObject bg2;
	public GameObject bg3;
	public GameObject bg4;
	public GameObject bg5;

	// banner ca-app-pub-8832799678197868/1398069834
	// interstitial ca-app-pub-8832799678197868/4351536234

	//string adUnitId = "ca-app-pub-8832799678197868/1398069834";
	//BannerView bannerView;
	//AdRequest request;
	
	string adUnitIdInterstitial = "ca-app-pub-8832799678197868/4351536234";
	InterstitialAd interstitialAgain;
	InterstitialAd interstitialClose;
	AdRequest requestInterstitialAgain;
	AdRequest requestInterstitialClose;


	// Use this for initialization
	void Start () {

		//subRanking = GameObject.Find ("subRanking");
		grid.SetActive (false);
		blackGrid.SetActive (false);
		//interstitial
		interstitialAgain = new InterstitialAd(adUnitIdInterstitial);
		interstitialClose = new InterstitialAd(adUnitIdInterstitial);

		requestInterstitialAgain = new AdRequest.Builder().Build();
		interstitialAgain.LoadAd(requestInterstitialAgain);
		
		requestInterstitialClose = new AdRequest.Builder().Build();
		interstitialClose.LoadAd(requestInterstitialClose);
		
		interstitialClose.AdClosed += delegate(object sender, System.EventArgs args)
		{
			interstitialClose.Destroy();
			Application.Quit ();
		};
		
		interstitialAgain.AdClosed += delegate(object sender, System.EventArgs args)
		{
			interstitialAgain.Destroy();
			Application.LoadLevel("frump");
		};

		if (PlayerPrefs.GetString ("mostraMenu") == "again") {
			PlayerPrefs.SetString("mostraMenu","");
			tuto.SetActive(false);
		}

		//banner
		/*bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		request = new AdRequest.Builder().Build();
		bannerView.LoadAd(request);
		bannerView.Hide ();*/

		sapo = personagem.GetComponent<Rigidbody2D> ();
		animatorSapo = sapo.GetComponent<Animator> ();
		//personagem.transform.Rotate (0,-180,0);
		posicaoX = 0.0f;

		menu.SetActive (false);

		textMetrosMenu = objMenuNow.GetComponent<TextMesh> ();
		textMaxMenu = objMenuMax.GetComponent<TextMesh> ();
		textMetros = objMetros.GetComponent<TextMesh> ();

		//PlayerPrefs.SetFloat ("maxMeters",4.3F);
		
		maximo = PlayerPrefs.GetFloat ("maxMeters");

		//Google Play
		//if (PlayerPrefs.GetString ("gpConnect") == "sim") {
			PlayGamesPlatform.Activate ();
			//Social.localUser.Authenticate ((bool success) => {});
		//}
		
		//Vector2 posDir = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width,Screen.height/2) );
		//paredeDir.transform.position = posDir;
		
		//Vector2 posEsq = Camera.main.ScreenToWorldPoint(new Vector2(0,Screen.height/2) );
		//paredeEsq.transform.position = posEsq;
		
		Renderer rendParede = paredeDir.GetComponent<Renderer>();
		tamParede = rendParede.bounds.size.y;

		paredeDirInstance = Instantiate (paredeDir);
		paredeDirInstance.transform.position = new Vector3 (paredeDir.transform.position.x, paredeDir.transform.position.y + tamParede, paredeDir.transform.position.z);
		paredeEsqInstance = Instantiate (paredeEsq);
		paredeEsqInstance.transform.position = new Vector3 (paredeEsq.transform.position.x, paredeEsq.transform.position.y + tamParede, paredeEsq.transform.position.z);

		morto = false;

		posInicial = Camera.main.transform.position;
	}

	// Update is called once per frame
	void Update () {
		
		if (!morto) {
			
			if (Input.GetButtonDown ("Fire1") && !pulo) {
				tuto.SetActive(false);

				AudioSource.PlayClipAtPoint (pula, personagem.transform.position, 1f);
				
				numeroPulos ++;
				pulo = true;
				
				if (direita) {
					sapo.AddForce (Vector2.right * forca);	
					direita = false;
					esquerda = true;	
					animatorSapo.Play ("pulaDir");
				} else if (esquerda) {
					sapo.AddForce (Vector2.right * -1 * forca);
					direita = true;
					esquerda = false;
					animatorSapo.Play ("pulaEsq");
				}
			}
			
			if (pulo) {
				tempoSemPulo = 0;
				if(numeroPulos == 1){
					sapo.transform.Translate (Vector2.up * Time.deltaTime * 10);
					metros += Time.deltaTime * 10;
				}else{
					sapo.transform.Translate (Vector2.up * Time.deltaTime * 4);
					metros += Time.deltaTime * 4;
				}

			} else if (!pulo && numeroPulos > 0) {
				tempoSemPulo += Time.deltaTime;
				sapo.transform.Translate (Vector2.up * Time.deltaTime * (-1) / 2 * tempoSemPulo);
				metros -= Time.deltaTime / 2 * tempoSemPulo;

			}

			if (sapo.transform.position.y > (deslocamentos * tamParede) - 3) {
				deslocamentos ++;
				
				paredeDirInstance = Instantiate (paredeDir);
				paredeDirInstance.transform.position = new Vector3 (paredeDirInstance.transform.position.x, paredeDirInstance.transform.position.y + (tamParede * deslocamentos), paredeDirInstance.transform.position.z);
				paredeEsqInstance = Instantiate (paredeEsq);
				paredeEsqInstance.transform.position = new Vector3 (paredeEsqInstance.transform.position.x, paredeEsqInstance.transform.position.y + (tamParede * deslocamentos), paredeEsqInstance.transform.position.z);
				
			}
			textMetros.text = metros.ToString ("0.0") + " M";
		} else {
			if (Input.GetButtonDown ("Fire1")) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				//Debug.Log ("Raycast: " + hit.collider.gameObject.tag);
				if(hit.collider != null){
					if(hit.collider.gameObject.tag == "exit"){
						GameObject.Find ("exit").GetComponent<Animator>().Play("exit");
						//Exit();
					}
					if(hit.collider.gameObject.tag == "again"){
						GameObject.Find ("again").GetComponent<Animator>().Play("again");
						//Again();
					}

					if(hit.collider.gameObject.tag == "ranking"){
						GameObject.Find ("ranking").GetComponent<Animator>().Play("ranking");
						if(!subRankingBool){
							subRankingBool = true;
							subRanking.SetActive(true);
							GameObject.Find ("achievements").GetComponent<Animator>().Play("achievementsApareceSome");
							GameObject.Find ("leaderboard").GetComponent<Animator>().Play("leaderboardApareceSome");
						}else{
							subRankingBool = false;
							subRanking.SetActive (false);
						}
						//Ranking();



					}

					if(hit.collider.gameObject.tag == "achiev"){
						GameObject.Find ("achievements").GetComponent<Animator>().Play("achievements");
						//Achiev();
					}

					if(hit.collider.gameObject.tag == "leader"){
						GameObject.Find ("leaderboard").GetComponent<Animator>().Play("leaderboard");
						//Leader();
					}

					if(hit.collider.gameObject.tag == "yes"){
						GameObject.Find ("yes_0").GetComponent<Animator>().Play("yesConnect");


					}

					if(hit.collider.gameObject.tag == "no"){
						GameObject.Find ("no_0").GetComponent<Animator>().Play("noConnect");

					}

				}
			}
		}
		


		if (personagem.transform.position.y < -2) {
			Camera.main.transform.position = posInicial;
			bateuChao = true;
		} else {
			Vector3 posSapo = new Vector3 (posicaoX, personagem.transform.position.y + 1.33f, -10f);
			Camera.main.transform.position = posSapo;
		}

		if (metros > maxMeters) {
			//PlayerPrefs.SetFloat ("maxMeters", metros);
			maxMeters = metros;
		}
		if (mudaMaxBool) {
			mudaMax ();
		}

		if (!bateuChao) {
			bg1.transform.position = new Vector3 (bg1.transform.position.x, Camera.main.transform.position.y - (personagem.transform.position.y + 1.33f) / 5, bg1.transform.position.z);
			bg2.transform.position = new Vector3 (bg2.transform.position.x, Camera.main.transform.position.y - (personagem.transform.position.y + 1.33f) / 10, bg2.transform.position.z);
			bg3.transform.position = new Vector3 (bg3.transform.position.x, Camera.main.transform.position.y - (personagem.transform.position.y + 1.33f) / 20, bg3.transform.position.z);
			bg4.transform.position = new Vector3 (bg4.transform.position.x, Camera.main.transform.position.y - (personagem.transform.position.y + 1.33f) / 35, bg4.transform.position.z);
			bg5.transform.position = new Vector3 (bg5.transform.position.x, Camera.main.transform.position.y - (personagem.transform.position.y + 1.33f) / 55, bg5.transform.position.z);
		} 

	}

	public void YesConnect(){
		connect.SetActive(false);
		PlayerPrefs.SetString ("gpConnect", "sim");
		PlayerPrefs.SetString ("gpConnectSuccess", "");
		Application.Quit ();
	}

	public void NoConnect(){
		connect.SetActive(false);
		PlayerPrefs.SetString ("gpConnect", "nao");
		PlayerPrefs.SetString ("gpConnectSuccess", "");
	}

	public void Leader(){
		grid.SetActive (false);
		blackGrid.SetActive (true);
		//subRankingBool = false;
		//subRanking.SetActive (false);
		Social.localUser.Authenticate((bool success) => {
			if(success){
				PlayerPrefs.SetString ("gpConnectSuccess","");
				grid.SetActive (true);
				blackGrid.SetActive (false);
				//Social.ReportScore((long) (maxMeters*10), "CgkI9_qoq7gPEAIQCQ", (bool successPost) => {});
				//Social.ShowLeaderboardUI("CgkI9_qoq7gPEAIQCQ"); 
				PlayGamesPlatform.Instance.ShowLeaderboardUI("CgkI9_qoq7gPEAIQCQ");
			}else{
				//PlayerPrefs.SetString ("gpConnectSuccess","falhou");
				//grid.SetActive (true);
				//blackGrid.SetActive (false);
				//connect.SetActive(true);
			}
		});
	}

	public void Achiev(){
		grid.SetActive (false);
		blackGrid.SetActive (true);
		//subRankingBool = false;
		//subRanking.SetActive (false);
		Social.localUser.Authenticate((bool success) => {
			if(success){
				PlayerPrefs.SetString ("gpConnectSuccess","");
				grid.SetActive (true);
				blackGrid.SetActive (false);
				Social.ShowAchievementsUI();
			}else{
				//PlayerPrefs.SetString ("gpConnectSuccess","falhou");
				//grid.SetActive (true);
				//blackGrid.SetActive (false);
				//connect.SetActive(true);
			}
		});
	}

	public void Exit(){
		PlayerPrefs.SetString("mostraMenu","");
		//Application.Quit();
		if (interstitialClose.IsLoaded()) {
			interstitialClose.Show();
		}else{
			interstitialClose.Destroy();
			Application.Quit ();
		}
	}

	public void Again(){
		PlayerPrefs.SetString("mostraMenu","again");
		//Application.LoadLevel("frump");
		if (interstitialAgain.IsLoaded() && metros > 5) {
			interstitialAgain.Show();
		}else{
			Application.LoadLevel("frump");
		}
	}

	public void Ranking(){

		//if (PlayerPrefs.GetString ("gpConnect") != "sim"){
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.DebugLogEnabled = false;
			PlayGamesPlatform.Activate ();
			Social.localUser.Authenticate((bool success) => {
				if(success){
					PlayerPrefs.SetString ("gpConnectSuccess","");
				}else{
					PlayerPrefs.SetString ("gpConnectSuccess","falhou");
					//connect.SetActive(true);
				}
			});
		//}
		//PlayerPrefs.SetString ("gpConnect","sim");

	}

	void OnCollisionEnter2D(Collision2D coll) {
		
		pulo = false;
		
		if (coll.gameObject.tag == "parede") {
			if(direita && !morto){
				animatorSapo.Play ("esquerda");
			}else if(esquerda && !morto){
				animatorSapo.Play ("direita");
			}
			//Debug.Log ("Collider " + coll.gameObject.tag);
		}
		if (coll.gameObject.tag == "obstaculo") {
			MostraMenu();

			if(!morto){
				AudioSource.PlayClipAtPoint(morre,personagem.transform.position,1f);
				//Handheld.Vibrate();
			}else{
				AudioSource.PlayClipAtPoint(bateu,personagem.transform.position,0.5f);
			}
			morto = true;
			if(!bateuChao){
				float scale = 1f;
				if(metros > 10){
					scale = metros/10;
				}
				sapo.gravityScale = scale;
				//sapo.AddForce (Vector2.up * (-1) * forca );
				animatorSapo.Play("caindo");
			}
		}
		if (coll.gameObject.tag == "ground") {
			if(numeroPulos > 0 && !morto){
				morto = true;
				AudioSource.PlayClipAtPoint(morre,personagem.transform.position,1f);
				MostraMenu();
			}
			if(morto){
				bateuChao = true;
				if(maxMeters > maximo){
					mudaMaxBool = true;
				}
				//StartCoroutine(inicia ());
				animatorSapo.Play("morto");
			}
		}
	}

	private bool mudaMaxBool = false;
	private float intervaloMuda = 0.1f;
	void mudaMax(){
		//Debug.Log ("Muda");
		intervaloMuda -= Time.deltaTime;
		if(intervaloMuda < 0) {
			maximo += 0.1f;
			if(maximo >= maxMeters && mudaMaxBool){
				textMaxMenu.text =  maxMeters.ToString("0.0") + " M";
				mudaMaxBool = false;
			}else{
				textMaxMenu.text =  maximo.ToString("0.0") + " M";
				intervaloMuda = 0.0005f;
			}
		}

		//textMaxMenu.text =  maximo.ToString("0.0") + " M";
	}

	void MostraMenu(){
		if (maxMeters > maximo) {
			PlayerPrefs.SetFloat ("maxMeters", maxMeters);
		}
		//bannerView.Show();
		//subRanking.SetActive (false);
		grid.SetActive (true);
		menu.SetActive (true);
		textMetros.text = "";
		textMetrosMenu.text = maxMeters.ToString("0.0") + " M";
		textMaxMenu.text =  maximo.ToString("0.0") + " M";

		//if (PlayerPrefs.GetString ("gpConnectSuccess") == "falhou") {
			//PlayerPrefs.SetString ("gpConnect", "nao");
			//connect.SetActive(true);

		//}else 
        if (PlayerPrefs.GetString ("gpConnect") == "sim") {
			atualizaGP();
		}

		
	}

	void atualizaGP(){

		if (metros >= 10 && metros < 50) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ReportProgress ("CgkI9_qoq7gPEAIQAA", 100.0f, (bool successAchiev) => {});
				}
			});
		} else if (metros >= 50 && metros < 100) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ReportProgress ("CgkI9_qoq7gPEAIQAQ", 100.0f, (bool successAchiev) => {});
				}
			});
		} else if (metros >= 100 && metros < 150) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ReportProgress ("CgkI9_qoq7gPEAIQAg", 100.0f, (bool successAchiev) => {});
				}
			});
		} else if (metros >= 150 && metros < 200) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ReportProgress ("CgkI9_qoq7gPEAIQAw", 100.0f, (bool successAchiev) => {});
				}
			});
		} else if (metros >= 200 && metros < 250) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ReportProgress ("CgkI9_qoq7gPEAIQBA", 100.0f, (bool successAchiev) => {});
				}
			});
		} else if (metros >= 250 && metros < 300) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ReportProgress ("CgkI9_qoq7gPEAIQBQ", 100.0f, (bool successAchiev) => {});
				}
			});
		} else if (metros >= 300 && metros < 350) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ReportProgress ("CgkI9_qoq7gPEAIQBg", 100.0f, (bool successAchiev) => {});
				}
			});
		} else if (metros >= 350 && metros < 400) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ReportProgress ("CgkI9_qoq7gPEAIQBw", 100.0f, (bool successAchiev) => {});
				}
			});
		} else if (metros >= 400) {
			Social.localUser.Authenticate ((bool success) => {
				if (success) {
					Social.ReportProgress ("CgkI9_qoq7gPEAIQCA", 100.0f, (bool successAchiev) => {});
				}
			});
		}
		//Leaderboarding
		Social.localUser.Authenticate ((bool success) => {
			if (success) {
				Social.ReportScore ((long)(maxMeters * 10), "CgkI9_qoq7gPEAIQCQ", (bool successPost) => {});
			}
		});

	}

	void OnTriggerEnter2D(Collider2D coll){
		if(morto && coll.gameObject.tag == "ground"){
			animatorSapo.Play("morto");
			bateuChao = true;
		}

	}	
}
