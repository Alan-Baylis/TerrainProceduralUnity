using UnityEngine;
using System.Collections;
using Assets.src.controler.noise;

public class TestGenerationTerrain : MonoBehaviour {

    public GameObject prefab;
	// Use this for initialization
	void Start () {
        Noise n = Noise.GetInstance();
        n.Init(100, Noise.Type.VALUE);
        
        for(int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 50; j++)
            {
                float f1,f2,f3,f4,f5;
                n.GetNoise(i / 400000f, j / 400000f, out f1);
                n.GetNoise(i / 200000f, j / 200000f, out f2);
                n.GetNoise(i / 10000f, j / 10000f, out f3);
                n.GetNoise(i / 5000f, j / 5000f, out f4);
                n.GetNoise(i / 2000f, j / 2000f, out f5);

                float f = (f1 + 1) * 32 + (f2+1)*16 + (f3+1)*8 + (f4 + 1) * 4 + (f5 + 1) * 2;
                GameObject go = (GameObject) GameObject.Instantiate(prefab, new Vector3(i, (int)f, j), Quaternion.identity);
                go.GetComponent<Renderer>().material.color = new Color((f) / 62, 0, 1 - (f) / 62);
                /*go = (GameObject)GameObject.Instantiate(prefab, new Vector3(i, (int)f-1, j), Quaternion.identity);
                go.GetComponent<Renderer>().material.color = new Color((f-1) / 62, 0, 1 - (f-1) / 62);
                go = (GameObject)GameObject.Instantiate(prefab, new Vector3(i, (int)f-2, j), Quaternion.identity);
                go.GetComponent<Renderer>().material.color = new Color((f - 2) / 62, 0, 1 - (f - 2) / 62);*/
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
