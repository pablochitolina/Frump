using UnityEngine;
using System.Collections;

public class Obstaculos : MonoBehaviour {

	private float tempoMinimo = 0.2f;
	private float intervalo = 0.4f;
	public GameObject personagem;
	private PersonagemScript scriptPersonagem;
	private float proximoObstaculo = 0f;
	private GameObject obstaculoInstance;
	public GameObject estrutura1;
	public GameObject estrutura2;
	public GameObject estrutura3;
	public GameObject estrutura4;
	public GameObject estrutura5;
	public GameObject estrutura6;
	public GameObject estrutura7;
	public GameObject estrutura8;
	public GameObject estrutura9;
	private Rigidbody2D bd;

	float sumTempo = 0;

	Vector2 posDir ;
	Vector2 posEsq;

	// Use this for initialization
	void Start () {	
		estrutura1.SetActive (false);
		estrutura2.SetActive (false);
		estrutura3.SetActive (false);
		estrutura4.SetActive (false);
		estrutura5.SetActive (false);
		estrutura6.SetActive (false);
		estrutura7.SetActive (false);
		estrutura8.SetActive (false);
		estrutura9.SetActive (false);
		scriptPersonagem = personagem.GetComponent<PersonagemScript>();


	}
	
	// Update is called once per frame
	void Update () {

		proximoObstaculo -= Time.deltaTime;

		if (proximoObstaculo <= 0) {
			if (scriptPersonagem.numeroPulos > 2 && !scriptPersonagem.bateuChao) {

				int obstac = Random.Range(0,9);
				switch(obstac){
				case 0:
					obstaculoInstance = Instantiate (estrutura1);

					break;
				case 1:
					obstaculoInstance = Instantiate (estrutura2);

					break;
				case 2:
					obstaculoInstance = Instantiate (estrutura3);

					break;
				case 3:
					obstaculoInstance = Instantiate (estrutura4);

					break;
				case 4:
					obstaculoInstance = Instantiate (estrutura5);

					break;
				case 5:
					obstaculoInstance = Instantiate (estrutura6);

					break;
				case 6:
					obstaculoInstance = Instantiate (estrutura7);

					break;
				case 7:
					obstaculoInstance = Instantiate (estrutura8);

					break;
				case 8:
					obstaculoInstance = Instantiate (estrutura9);

					break;
				default:
					obstaculoInstance = Instantiate (estrutura9);

					break;

				}

				obstaculoInstance.SetActive (true);
				bd = obstaculoInstance.GetComponent<Rigidbody2D>();
				if(Random.Range(0,2) == 0){
					bd.AddForceAtPosition(Vector2.right * Random.Range(1,10),new Vector2(0f,1f));
				}else{
					bd.AddForceAtPosition(Vector2.right * Random.Range(1,10) * (-1),new Vector2(0f,1f));
				}

				//obstaculoInstance.transform.Rotate(new Vector3(0f,0f,obstaculoInstance.transform.rotation.z + 1f));
			
				int pos = Random.Range ((Screen.width - (Screen.width/7)), (Screen.width/7));
				Vector2 position = Camera.main.ScreenToWorldPoint(new Vector2(pos, Screen.height + Screen.height/20));
				obstaculoInstance.transform.position = position;
				//Debug.Log ("position " + position);

				Destroy(obstaculoInstance, 4f);

				sumTempo += Time.deltaTime/50;
				if((tempoMinimo - sumTempo) > 0){
					proximoObstaculo = Random.Range (0f, intervalo) + (tempoMinimo - sumTempo);
				}else{
					proximoObstaculo = Random.Range (0f, intervalo);
				}

				//Debug.Log("time: " + sumTempo);

			}
		}

	}
	
}
