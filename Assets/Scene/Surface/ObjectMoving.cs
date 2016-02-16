using UnityEngine;
using System.Collections;

public class ObjectMoving : MonoBehaviour {
    [Range(-1,1)]
    public float slider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        slider = Mathf.Sin(Time.time * 0.3f);

        transform.position = new Vector3( slider, -2.25f, 1.66f);

	}
}
