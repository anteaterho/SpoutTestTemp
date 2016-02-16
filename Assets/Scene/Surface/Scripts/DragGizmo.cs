using UnityEngine;
using System.Collections;

public class DragGizmo : MonoBehaviour
{

    public Camera camera;
    private Vector3 p;
    private Color Sphere = Color.red;
    private Color Cube = Color.green;

    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit = new RaycastHit();

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log(ray);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == this.gameObject.name)
                {
                    if (this.gameObject.tag == "Cube")
                    {
                        this.GetComponent<Renderer>().material.color = Color.yellow;
                    }
                    else if (this.gameObject.tag == "Sphere")
                    {
                        this.GetComponent<Renderer>().material.color = Color.magenta;
                    }
                    p = camera.ScreenToWorldPoint(Input.mousePosition);
                    this.transform.position = new Vector3(p.x, p.y, this.transform.position.z);
                }
                
            }
            
        }
        
        else if (this.gameObject.tag == "Cube")
        {
            this.GetComponent<Renderer>().material.color = Cube;
        }

        else if (this.gameObject.tag == "Sphere")
        {
            this.GetComponent<Renderer>().material.color = Sphere;
        }
        
    }

    /*
    void OnCollisionEnter(Collision col)
    {
        ContactPoint contact = col.contacts[0];
        this.gameObject.transform.Translate(contact.point);
    }
    */
    
}
