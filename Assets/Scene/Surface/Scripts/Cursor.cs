using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour {

    Vector3 p;

	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        Debug.Log(ray);
        if (Physics.Raycast(ray, out hit))
        {
            
                p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.transform.position = new Vector3(p.x, p.y, this.transform.position.z);

        }
    }
}
