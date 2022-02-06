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
    private Planet planet;

    public static PlanetSprite Create(Planet planet) {
        string objectName = "planet-" + planet.Name();

        GameObject ParentPanel = GameObject.Find("MapPanel");
        PlanetSprite planetSprite = ParentPanel.AddComponent(typeof(PlanetSprite)) as PlanetSprite;
        planetSprite.SetPlanet(planet);
        planetSprite.SetName(objectName);
        return planetSprite;
    }

    public void SetPlanet(Planet planet) {
        this.planet = planet;
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

        GameObject parentPanel = GameObject.Find("MapPanel");
        string countStr = "" + BASE_PLANET_COUNT;
        if(BASE_PLANET_COUNT < 10) {
            countStr = "0" + BASE_PLANET_COUNT;
        }
        string planetFilepath = BASE_PLANET_DIRPATH + "/planet_" + countStr + ".png";
        Sprite sprite = IMG2Sprite.LoadNewSprite(planetFilepath);
        BASE_PLANET_COUNT++;

        Image newImage = me.AddComponent<Image>(); //Add the Image Component script
        newImage.sprite = sprite; //Set the Sprite of the Image Component on the new GameObject

        RectTransform rectTransform = me.GetComponent<RectTransform>();
        rectTransform.SetParent(parentPanel.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        
        RectTransform parentTransform = parentPanel.GetComponent<RectTransform>();
        Vector3[] parentCorners = new Vector3[4];
        parentTransform.GetLocalCorners(parentCorners);
        float parentHeight = parentCorners[1].y - parentCorners[0].y;
        float parentWidth = parentCorners[3].x - parentCorners[0].x;
        rectTransform.localPosition = new Vector3((position.x-0.5F)*parentWidth, (position.y-0.5F)*parentHeight, 0);

        rectTransform.localScale = new Vector3(0.2F, 0.2F, 0.2F);

        this.AddText(me);

        me.SetActive(true); //Activate the GameObject
    }

    private void AddText(GameObject parent) {
        GameObject planetText = new GameObject("planet-text-" + planet.Name());
        planetText.transform.SetParent(parent.transform);

        planetText.AddComponent<Text>().text = planet.Name();
        planetText.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        planetText.transform.localPosition = new Vector3(0,0,0);
    }
}