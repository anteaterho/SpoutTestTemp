using UnityEngine;
using System.Collections;

public class LogoRotation : ObjectRotation {

    public GameObject tempObj;


	// Update is called once per frame
	void Update () {
        Rotation();
        if(head.transform.position == Vector3.zero)
        {
            //transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * 1.0f);
        }
        else
        {
            //transform.rotation = Quaternion.Slerp(new Quaternion(1, 1, 0, 30 * Mathf.Deg2Rad), tempObj.transform.rotation, Time.deltaTime * 0.1f);
            Rotation();
            transform.rotation = Quaternion.Slerp(transform.rotation, tempObj.transform.rotation, Time.deltaTime * 1.0f);
            transform.rotation = new Quaternion(transform.rotation.x * 0.92f, transform.rotation.y, 0, transform.rotation.w);
        }
    }

    public override void Rotation()
    {
        base.Rotation();
        tempObj.transform.LookAt(head.transform.position);
        tempObj.transform.rotation.SetEulerAngles(0.0f, transform.rotation.eulerAngles.y, 0.0f);
    }
}
