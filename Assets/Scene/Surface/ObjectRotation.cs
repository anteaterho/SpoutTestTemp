using UnityEngine;
using System.Collections;

public class ObjectRotation : MonoBehaviour {

    protected GameObject tempObj;

    public GameObject head;

    void Start()
    {
        tempObj = new GameObject();
    }

    public virtual void Rotation()
    {
        Vector3 tempHead = new Vector3(head.transform.position.x, head.transform.position.y - 0.5f, head.transform.position.z);
        //this.gameObject.transform.LookAt(head.transform.position);
        //tempObj.transform.LookAt(head.transform.position);
    }
}
