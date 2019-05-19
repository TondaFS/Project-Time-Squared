using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateObject : PTSObject {
    [Space(10)]
    /// <summary>
    /// Reference na druhý objekt ve scéně (v jiné čas linii)
    /// </summary>
    public StateObject otherTimelineRef;
    
        
    /// <summary>
    /// Necha item "zmizet" - nastavi nekolizni vrstvu (asi neni treba diky vypnuti objektu) a obejkt vypne
    /// </summary>
    public void MakeObjectInvisible()
    {
        SetInvisibleLayer();        
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// Necha objekt znova se objevit - nastavi kolizni vrstvu a objekt zapne
    /// </summary>
    public void MakeObjectVisible()
    {
        SetVisibleLayer();
        gameObject.SetActive(true);
    }
    
    /// <summary>
    /// Kontroluje navic IsMain a na zaklade toho tez muze nastavit
    /// neviditelnou vrstvu
    /// </summary>
    /// <param name="t"></param>
    protected override void CheckOnStart(Tile[] t)
    {
        base.CheckOnStart(t);

        if (material != null)
            render.material = material;
    }

    /*
    /// <summary>
    /// Při vloženi skriptu jako komponentu do gameobjectu tato metoda zaručí,
    /// že se vytvoří ještě i kopie v druhé časové timelině. Oba nové objekty si na sebe
    /// uloží reference, každý nastaven do své timelien a jsou pak slučeny pod 
    /// společný gameobject, který by měl být referencován a pozicován ve scéně.
    /// </summary>
    public virtual void Reset()
    {
        Debug.Log("Calling reset");
        if (otherTimelineRef == null)
        {

            GameObject o = Instantiate(this.gameObject, this.transform.position, this.transform.rotation);
            IsMain = true;
            ItemType = TimelineObject.Present;

            StateObject ob = o.GetComponent<StateObject>();
            ob.IsMain = false;
            ob.otherTimelineRef = this;            
            ob.ItemType = TimelineObject.Past;

            otherTimelineRef = ob;

            GameObject parent = new GameObject();
            parent.transform.position = this.transform.position;
            parent.transform.rotation = this.transform.rotation;

            this.transform.parent = parent.transform;
            ob.transform.parent = parent.transform;
            gameObject.layer = 11;
            ob.gameObject.layer = 12;

            this.name = "PRESENT PART";
            ob.name = "PAST PART";
            parent.name = "NEW TIMELINE OBJECT PARENT";

            o.SetActive(true);
        }
    }
    */
}

