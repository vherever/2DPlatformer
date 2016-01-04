using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject currentCheckpoint;

	private PlayerController player;

	public GameObject deathParticle;
	public GameObject respawnParticle;

	public int pointPenaltyOnDeath;

	public float respawnDelay;

	private new CameraController camera;

	private float gravityStore;

	public HealthManager healthManager;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
	
		camera = FindObjectOfType<CameraController> ();
	
		healthManager = FindObjectOfType<HealthManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RespawnPlayer() {
		StartCoroutine ("RespawnPlayerGo");
	}

	public IEnumerator RespawnPlayerGo() {
		Instantiate (deathParticle, player.transform.position, player.transform.rotation);
		player.enabled = false;

		Collider2D collider = player.gameObject.GetComponent<Collider2D>();
		collider.enabled = false; // disable collider
		player.GetComponent<Renderer>().enabled = false;
		camera.isFollowing = false;
		//gravityStore = player.myRigidbody.gravityScale;
		//player.myRigidbody.gravityScale = 0f;
		//player.myRigidbody.velocity = Vector2.zero; // stop moving the camera
		ScoreManager.AddPoints (-pointPenaltyOnDeath);
		Debug.Log ("Player Respawn");
		yield return new WaitForSeconds (respawnDelay);
		collider.enabled = true;
		//player.myRigidbody.gravityScale = gravityStore;
		player.transform.position = currentCheckpoint.transform.position;
		player.enabled = true;
		player.GetComponent<Renderer>().enabled = true;
		healthManager.FullHealth ();
		healthManager.isDead = false;
		camera.isFollowing = true;
		Instantiate (respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);

	}
}
