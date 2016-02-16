using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectionSurface : MonoBehaviour {

    private Mesh myMesh;
    private Vector3[] verts;

    public GameObject[] vertsGizmo;

	// Use this for initialization
	void Start () {

        myMesh = gameObject.GetComponent<MeshFilter>().mesh;

        verts = myMesh.vertices;
        Debug.Log(myMesh.vertexCount);

        for (int i = 0; i < verts.Length; i++)
        {
            //GameObject test2 = Instantiate(vertsGizmo, verts[i], Quaternion.identity) as GameObject;
            //testList.Add(test2);
            //vertsGizmo[i] = GameObject.Find("Gizmo_Sphere(1)").gameObject;
            vertsGizmo[i].transform.Translate(verts[i]);
        }

        Debug.Log("Verts: " + verts.Length);
	}
	
	// Update is called once per frame
	void Update () {

        Vector3[] verts2 = new Vector3[160];

        for(int i = 1; i < verts.Length; i++)
        {
            verts2[i] = new Vector3(vertsGizmo[i].transform.position.x, vertsGizmo[i].transform.position.y, vertsGizmo[i].transform.position.z);
        }

        myMesh.vertices = verts2;
        myMesh.RecalculateBounds();
	}
}
