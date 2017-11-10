using UnityEngine;
using System.Collections;

public class Rotatearound : MonoBehaviour 
{
	public float m_speed = 100.0f;

	private float x;
	private float z;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		x += Time.deltaTime * Random.Range(-1.5f, 1.5f);
		z += Time.deltaTime * Random.Range(-1.5f, 1.5f);

		transform.RotateAround(transform.position, new Vector3(x, 0.6f, z).normalized, Time.deltaTime * m_speed);
	}
}
