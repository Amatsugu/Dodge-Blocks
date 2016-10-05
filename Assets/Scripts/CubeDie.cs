using UnityEngine;
using System.Collections;

public class CubeDie : MonoBehaviour
{

	public GameObject deathShatter;

	public void Die()
	{
		Debug.Log("die");
		Instantiate(deathShatter, transform.position, Quaternion.identity);
	}
}
