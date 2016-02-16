using UnityEngine;
using System.Collections;

public class LightRotation : ObjectRotation {

    private Quaternion tempRot;

    public GameObject tempObj;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (head.transform.position == Vector3.zero)
        {
            //transform.rotation = new Quaternion(1, 1, 0, 30 * Mathf.Deg2Rad);
            transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(1, 1, 0, 30 * Mathf.Deg2Rad), Time.deltaTime * 2.0f);
        }
        else
        {
            //transform.rotation = Quaternion.Slerp(new Quaternion(1, 1, 0, 30 * Mathf.Deg2Rad), tempObj.transform.rotation, Time.deltaTime * 0.1f);
            Rotation();
            transform.rotation = Quaternion.Slerp(transform.rotation, tempObj.transform.rotation, Time.deltaTime * 3.0f);
        }
    }

    public override void Rotation()
    {
        base.Rotation();
        //gameObject.transform.LookAt(head.transform.position);
        tempObj.transform.LookAt(head.transform.position);
        //transform.rotation.SetEulerAngles(transform.rotation.eulerAngles.x + 10.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        tempObj.transform.rotation.SetEulerAngles(transform.rotation.eulerAngles.x + 10.0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
