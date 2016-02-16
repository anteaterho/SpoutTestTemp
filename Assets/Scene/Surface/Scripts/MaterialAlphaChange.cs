using UnityEngine;
using System.Collections;

public class MaterialAlphaChange : MonoBehaviour
{

    public GameObject surface;

    private Material mat;

    
    [Range(0, 1)]
    public float slider1;
    [Range(0, 1)]
    public float slider2;
    [Range(0, 1)]
    public float slider3;


    public Texture[] diffuseTextures;

    // Use this for initialization
    void Awake()
    {

        mat = surface.GetComponent<Renderer>().material;

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Pahse1(1.0f));
        StartCoroutine(Pahse2(1.0f));
        StartCoroutine(Pahse3(10.0f));

        mat.SetFloat("_Alpha1", slider1);
        mat.SetFloat("_Alpha2", slider2);
        mat.SetFloat("_Alpha3", slider3);

        if(slider3 == 0)
        {
            mat.SetTexture("_MainTex3", diffuseTextures[(int)Random.RandomRange(0,3)]);
        }
    }

    IEnumerator Pahse1(float time)
    {
        yield return new WaitForSeconds(time);
        SliderPhase1();
    }

    IEnumerator Pahse2(float time)
    {
        yield return new WaitForSeconds(time);
        SliderPhase2();
    }

    IEnumerator Pahse3(float time)
    {
        yield return new WaitForSeconds(time);
        SliderPhase3();
    }

    void SliderPhase1()
    {
        slider1 = Mathf.Clamp(Mathf.Sin(Time.time * 0.1f) + 1.0f / 2.0f, 0.0f, 1.0f);
    }

    void SliderPhase2()
    {
        slider2 = Mathf.Clamp(Mathf.Cos(Time.time * 0.1f) + 1.0f / 2.0f, 0.0f, 1.0f);
    }

    void SliderPhase3()
    {
        slider3 = Mathf.Clamp(Mathf.Sin(Time.time * 0.5f) + 1.0f / 2.0f, 0.0f, 1.0f);
    }


}
