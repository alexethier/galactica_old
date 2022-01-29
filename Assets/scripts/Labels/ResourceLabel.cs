using UnityEngine;
using UnityEngine.UI; // The namespace for the UI stuff.
using System.Collections;
using TMPro;

public class ResourceLabel : MonoBehaviour
{

    private TextMeshProUGUI resourceLabel;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.getGameObject("Resources Label Text").text = string.Format("Money: {0}", Universe.Instance().MainPlayer().Resources().Money());
    }

    private TextMeshProUGUI getGameObject(string findtext) {
        if(resourceLabel == null) {
            resourceLabel = GameObject.Find(findtext).GetComponent<TextMeshProUGUI>();
        }

        return resourceLabel;
    }
}
