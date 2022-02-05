using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlanetSprite : MonoBehaviour {

    //public GameObject ParentPanel; //Parent Panel you want the new Images to be children of
    //public Sprite sprite;

    private GameObject me;
    private string objectName;
    private Vector3 position;

    public static PlanetSprite Create(string objectName) {
        GameObject ParentPanel = GameObject.Find("MapPanel");
        PlanetSprite planetSprite = ParentPanel.AddComponent(typeof(PlanetSprite)) as PlanetSprite;
        planetSprite.SetName(objectName);
        return planetSprite;
    }

    public void SetName(string objectName) {
        this.objectName = objectName;

        /*
        if(me != null) {
            me.name = objectName;
        }
        */
    }

    public void SetPosition(double x, double y) {
        this.position = new Vector3((float)x, (float)y, 0);
        /*
        if(me != null) {
            me.transform.position = this.position;
        }
        */
    }

    private static string BASE_PLANET_DIRPATH = "Assets/Resources/sprites/planets";
    private static int BASE_PLANET_COUNT = 1;

    // Use this for initialization
    void Start () {

        me = new GameObject();
        if(this.objectName != null) {
            me.name = objectName;
        }

        /*
        Debug.Log("Setting position?");
        if(this.position != null) {
            RectTransform rectTransform = me.GetComponent<RectTransform>();
            rectTransform.transform.position = this.position;
            //me.transform.Translate(this.position);
            Debug.Log("Setting position!!");
            Debug.Log(rectTransform.transform.position);
        }
        */

        GameObject ParentPanel = GameObject.Find("MapPanel");
        string countStr = "" + BASE_PLANET_COUNT;
        if(BASE_PLANET_COUNT < 10) {
            countStr = "0" + BASE_PLANET_COUNT;
        }
        string planetFilepath = BASE_PLANET_DIRPATH + "/planet_" + countStr + ".png";
        Sprite sprite = IMG2Sprite.LoadNewSprite(planetFilepath);
        BASE_PLANET_COUNT++;

        //GameObject newObj = new GameObject(); //Create the GameObject
        Image newImage = me.AddComponent<Image>(); //Add the Image Component script
        newImage.sprite = sprite; //Set the Sprite of the Image Component on the new GameObject

        RectTransform rectTransform = me.GetComponent<RectTransform>();
        rectTransform.SetParent(ParentPanel.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        
        rectTransform.Translate(new Vector3(100,100,0)); // TODO FIX TRANSFORM TRANSLATE
        //rectTransform.transform.position = new Vector3(0,0,0); // TODO FIX TRANSFORM TRANSLATE

        me.SetActive(true); //Activate the GameObject
    }

    /*
    void Update()
    {
        Debug.Log("Setting position?");
        if(this.position != null) {
            RectTransform rectTransform = me.GetComponent<RectTransform>();
            rectTransform.transform.position = this.position;
            //me.transform.Translate(this.position);
            Debug.Log("Setting position!!");
            Debug.Log(rectTransform.transform.position);
        }
    }
    */
}