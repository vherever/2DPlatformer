using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour {

	public Transform coinEffect;

	public int pointsToAdd;
	void OnTriggerEnter2D (Collider2D other) {
		if (other.GetComponent<PlayerController> () == null) 
			return;
		Transform effect = Instantiate (coinEffect, transform.position, transform.rotation) as Transform;
		ScoreManager.AddPoints (pointsToAdd);


		Destroy (gameObject);
		Destroy (effect.gameObject, 0.5f);
	}
}
