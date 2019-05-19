using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RobotController : MonoBehaviour {
    /// <summary>
    /// Tile, ktery byl v poslednim framu zvyrazneni jako znameni dostupnosti
    /// </summary>
    Tile LastSeenTile;
    /// <summary>
    /// Predmet, ktery postava prave drzi v ruce a nese
    /// </summary>
    public MovableObject CarriingItem;

    Tile MyLastTile;
    KarelAnimatorController karelAnimator;

    [SerializeField] bool disabledController;
    public float pushVelocity = 10f;
        
    private void Start()
    {
        LastSeenTile = null;
        karelAnimator = GetComponentInChildren<KarelAnimatorController>();
    }   

    void GetMyTile()
    {
        Tile[] t = Level.GetTile(transform.position);

        //pripady kdy neseme prenositelny generator a je zapnuty/vypnuty
        if (CarriingItem != null)
        {
            Generator g = CarriingItem as Generator;

            if (g != null)
            {
                //Debug.Log("Kontrolujeme generator co drzime...");
                if (g.IsOn)
                {
                    MyLastTile = t[1];
                    return;
                }
                else
                {
                    MyLastTile = t[0];
                    return;
                }
            }
        }

        //kontrolujeme generatory, co jsou nekde polozene 
        List<Generator> gens = Level.Instance.GetGeneratorsAffectingTile(t[0]);

        if (gens.Count == 0)
        {
            MyLastTile = t[0];
            return;
        }
        else
        {
            foreach (Generator g in gens)
            {
                if (g.IsOn)
                {
                    MyLastTile = t[1];
                    return;
                }
            }            
        }

        MyLastTile = t[0];
    }

    void Update() {
        GetMyTile();
        HighlightTile();

        //Debug.Log("LAST SEEN TILE " + LastSeenTile.Position + " TIMELINE: " + LastSeenTile.Timeline);

        if (!disabledController)
        {
            if (Input.GetButtonDown("PickUp/Drop"))
            {
                if (!InputDrop())
                    InputPickup();
            }

            if (Input.GetButtonDown("Activate/Push"))
            {
                if (!InputPickableGenerator())
                {
                    if (CarriingItem != null)
                    {
                        Debug.Log("Carrying item. Cant activate generator");
                        return;
                    }

                    InputGenerator();
                    InputPush();
                    InputActivator();
                }                
            }

            
            if (Input.GetButtonDown("Reload"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            

            /*
            if (Input.GetButtonDown("Escape"))
                SceneManager.LoadScene(0);
            */

            /*
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SceneManager.LoadScene(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                SceneManager.LoadScene(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                SceneManager.LoadScene(3);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                SceneManager.LoadScene(4);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                SceneManager.LoadScene(5);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                SceneManager.LoadScene(6);
            if (Input.GetKeyDown(KeyCode.Alpha7))
                SceneManager.LoadScene(7);
            */
        }
    }

    #region PUSH
    void InputPush()
    {     
        Debug.Log("<<<PUSH>>>");
        if (LastSeenTile.ObjectOnTile == null)
            return;

        MovableObject i = LastSeenTile.ObjectOnTile as MovableObject;

        if (i == null)
            return;

        if (!i.isPushable)
            return;

        if (LastSeenTile.LaserOnTile != null)
        {
            Debug.Log("We cant push from tile where is laser");
            return;
        }

        //Debug.Log("We are going to push...");

        Tile[] t = Level.GetTile(transform.position);        
        Vector2Int vector = LastSeenTile.Position - t[0].Position;
        Tile[] dest = Level.GetTile(LastSeenTile.Position + vector);

        if (dest == null)
        {
            Debug.LogWarning("NO VALID TILE");
            return;
        }
                
        List<Generator> gens = Level.Instance.GetGeneratorsAffectingTile(dest[0]);
        Vector3 d = new Vector3(dest[0].Position.x, dest[0].Height, dest[0].Position.y);

        Tile actualTile = null;

        Debug.Log("Pushing from " + LastSeenTile.Timeline);

        //divame se na dest v pritomnosti bez vlivu generatoru
        if (gens.Count == 0)
        {
            if (LastSeenTile.Timeline.Equals(TimelineObject.Present))
                actualTile = TimeLinePushCheck(i, dest[0], true);
            else
                actualTile = TimeLinePushCheck(i, dest[1], false);
        }
        else
        {
            //Debug.Log("Some Generators effects the new tile");
            foreach (Generator g in gens)
            {
                if (g.IsOn)
                {
                    Debug.Log("Some Generator is on");
                    
                    if (LastSeenTile.Timeline.Equals(TimelineObject.Present))
                        actualTile = TimeLinePushCheck(i, dest[0], !dest[0].Timeline.Equals(LastSeenTile.Timeline));
                    else
                        actualTile = TimeLinePushCheck(i, dest[1], dest[1].Timeline.Equals(LastSeenTile.Timeline));
                        
                    //actualTile = TimeLinePushCheck(i, dest[1], dest[1].Timeline.Equals(LastSeenTile.Timeline));
                    break;
                }
                //zadny generator ovlivnujici tile neni zaply
                else
                {                    
                    actualTile = TimeLinePushCheck(i, dest[0], dest[0].Timeline.Equals(LastSeenTile.Timeline));
                }
            }
        }

        Debug.Log("Finsihing...");

        if (actualTile == null)
            return;  

        PreassurePlate pp = actualTile.ActivatorOnTile as PreassurePlate;
        if (pp != null)
            pp.OnActivate();

        PreassurePlate pp2 = LastSeenTile.ActivatorOnTile as PreassurePlate;
        if (pp2 != null)
            pp2.OnActivate();

        i.StartCoroutine(i.PushMovement(d));
        LastSeenTile.ChangeOccupation();
        StartCoroutine(PushMovement());

        Debug.Log("ACTUAL TILE: " + actualTile.Timeline + " " + actualTile.Position);

        if (actualTile.LaserOnTile != null)
        {
            actualTile.LaserOnTile.DestroyNextLasers();
        }
            

    }
    Tile TimeLinePushCheck(MovableObject i, Tile dest, bool makeVisible)
    {
        Debug.Log("Trying push to: " + dest.Timeline);
        if (dest.IsOccupied)
        {
            Debug.Log("Cant push: Destination tile is occupied");
            return null;
        }
        if (!dest.IsAccessable)
        {
            Debug.Log("Cant push: Tile si not Aceessable");
            return null;
        }

        if (dest.Height > LastSeenTile.Height + 0.75f)
        {
            Debug.Log("Cant push: The tile is way higher");
            return null;
        }

        Debug.Log(i + " added on " + dest.Timeline + " tile with position: " + dest.Position);

        dest.ObjectOnTile = i;
        dest.IsOccupied = true;
        
        if (makeVisible)
            i.SetVisibleLayer();
        else
            i.SetInvisibleLayer();

        return dest;
    }
    IEnumerator PushMovement()
    {
        disabledController = true;
        Vector3 dest = new Vector3(LastSeenTile.Position.x - 0.5f, transform.position.y, LastSeenTile.Position.y + 0.5f);
        int i = 0;
        while (disabledController)
        {
            transform.position = Vector3.MoveTowards(transform.position, dest, pushVelocity * Time.deltaTime);

            if (transform.position == dest || i == 30)
            {
                disabledController = false;
            }                
            else
            {
                i++;
                yield return null;
            }
                
        }
    }
    #endregion

    /// <summary>
    /// Zvyrazni pole, ktere je pro hrace dostupne. Pokud doslo ke zmene Tilu,
    /// stary tile se obnovi do puvodniho stavu (zmizi reference) a je nahrazen 
    /// novym tilem.
    /// </summary>
    void HighlightTile()
    {
        //zjistime tile dle pozice
        Tile[] tile = Level.GetTile(transform.position);

        if (tile == null)
            return;

        Tile[] neigbours = Level.Instance.GetNeighbours(tile[0]);

        Vector3 vect = transform.position + transform.forward * 1.15f;
        Tile closest = null;

        Tile[] dalsiTile = Level.GetTile(vect);

        for (int i = 0; i < neigbours.Length; i++)
        {
            if (dalsiTile[0].Equals(neigbours[i]))
            {
                closest = neigbours[i];
                break;
            }
        }

        if (closest == null)
            return;

        Tile[] tilePair = { closest, closest.OtherTimelineRef };

        //Testujeme, jestli uz nevidime jiny tile nez v predchozim framu
        if (LastSeenTile != null && (tilePair[0] != LastSeenTile && tilePair[1] != LastSeenTile))
            LastSeenTile.SetTileToNormal();
        
        //pripady kdy neseme prenositelny generator a je zapnuty/vypnuty
        if (CarriingItem != null)
        {
            Generator g = CarriingItem as Generator;

            if (g != null)
            {
                //Debug.Log("Kontrolujeme generator co drzime...");
                if (g.IsOn)
                {
                    //Debug.Log("je zaply");
                    if (!tilePair[1].IsAccessable)
                        return;

                    if (tilePair[1].Height > tile[1].Height + 1.1f)
                    {
                        LastSeenTile = tile[1];
                        LastSeenTile.SetTileToHighlited();
                        return;
                    }

                    LastSeenTile = tilePair[1];
                    LastSeenTile.SetTileToHighlited();
                    return;
                }
                else
                {                   
                    if (!tilePair[0].IsAccessable)
                        return;

                    if (tilePair[0].Height > tile[0].Height + 1.1f)
                    {
                        LastSeenTile = tile[0];
                        LastSeenTile.SetTileToHighlited();
                        return;
                    }

                    LastSeenTile = tilePair[0];
                    LastSeenTile.SetTileToHighlited();
                    return;
                }
            }
        }

        //kontrolujeme generatory, co jsou nekde polozene 
        List<Generator> gens = Level.Instance.GetGeneratorsAffectingTile(tilePair[0]);

        if (gens.Count == 0)
        {
            //Debug.Log("Zadny gen v okoli");
            if (!tilePair[0].IsAccessable)
            {
                LastSeenTile = Level.GetTile(transform.position)[0];
                LastSeenTile.SetTileToHighlited();
                return;
            }
                

            if (tilePair[0].Height > tile[0].Height + 1.1f)
            {
                LastSeenTile = tile[0];
                LastSeenTile.SetTileToHighlited();
                return;
            }


            tilePair[0].SetTileToHighlited();
            LastSeenTile = tilePair[0];
            return;
        }
        else
        {
            foreach (Generator g in gens)
            {
                if (g.IsOn)
                {
                    //Debug.Log("Zaply gen");
                    if (!tilePair[1].IsAccessable)
                    {
                        LastSeenTile = Level.GetTile(transform.position)[1];
                        LastSeenTile.SetTileToHighlited();
                        return;
                    }                        

                    if (tilePair[1].Height > tile[1].Height + 1.1f)
                    {
                        LastSeenTile = tile[1];
                        LastSeenTile.SetTileToHighlited();
                        return;
                    }

                    tilePair[1].SetTileToHighlited();
                    LastSeenTile = tilePair[1];
                    return;
                }
            }

            //Debug.Log("Vyply gen");
            if (!tilePair[0].IsAccessable)
            {
                Debug.Log("Not Accessable");
                LastSeenTile = Level.GetTile(transform.position)[0];
                LastSeenTile.SetTileToHighlited();
                return;
            }                

            if (tilePair[0].Height > tile[0].Height + 1.1f)
            {
                LastSeenTile = tile[0];
                LastSeenTile.SetTileToHighlited();
                return;
            }

            tilePair[0].SetTileToHighlited();
            LastSeenTile = tilePair[0];
        }               
    }

    bool InputPickableGenerator()
    {
        if (CarriingItem == null)
            return false;

        Generator g = CarriingItem as Generator;
        if (g == null)
            return false;

        if (g.isAlwaysOn)
        {
            Debug.Log("Cant turn of generator");
            return false;
        }

        g.ActivateOnPick();
        return true;
    }

    /// <summary>
    /// Pokusi se prepnout generator
    /// </summary>
    bool InputGenerator()
    {     
        Generator g = LastSeenTile.ObjectOnTile as Generator;
        if (g == null)
            return false;
           
        if (g.isAlwaysOn)
        {
            Debug.Log("Cant turn off generator");
            return false;
        }

        if (!CheckTileInOtherTimeline())
        {
            Debug.Log("<<<There is item in other timeline. Can't activate generator>>>");
            return false;
        }

        g.Activate();
        return true;
    }

    bool CheckTileInOtherTimeline()
    {
        if (LastSeenTile.OtherTimelineRef.ObjectOnTile != null)
        {
            Generator g = LastSeenTile.OtherTimelineRef.ObjectOnTile as Generator;

            if (g == null)
                return false;
        }           

        return true;
    }

    bool InputActivator()
    {
        if (LastSeenTile.ActivatorOnTile == null)
            return false;

        PreassurePlate pp = LastSeenTile.ActivatorOnTile as PreassurePlate;
        if (pp != null)
            return false;

        LaserReceiver lr = LastSeenTile.ActivatorOnTile as LaserReceiver;
        if (lr != null)
            return false;

        LaserGenerator lg = LastSeenTile.ActivatorOnTile as LaserGenerator;
        if (lg != null)
            return false;

        LastSeenTile.ActivatorOnTile.OnActivate();
        return true;
    }

    /// <summary>
    /// Resi zvednuti itemu ze zeme.
    /// </summary>
    void InputPickup()
    {
        Debug.Log("<<<PICKUP>>>");
        Debug.Log("Last seen tile pos: " + LastSeenTile.Position + " " + LastSeenTile.Timeline);

        if (!LastSeenTile.IsAccessable)
        {
            Debug.Log(">>>No ACCESS");
            return;
        }

        if (CarriingItem != null)
        {
            Debug.Log(">>>WE ARE CARRIING ITEM ALREADY");
            return;
        }

        if (!LastSeenTile.IsOccupied)
        {
            Debug.Log(">>>THERE IS NO ITEM TO PICK UP");
            return;             
        }

        MovableObject mo = LastSeenTile.ObjectOnTile as MovableObject;

        if (mo == null)
        {
            Debug.Log(">>>NOT MOVABLE OBJ, YOU CANT PICK THAT");
            return;
        }
        
        if (mo.gameObject.layer == PTSLayers.NoInteractionWithPlayer)
        {
            Debug.Log(">>>ITEM IS IN ANOTHER TIMELINE... IGNORE");
            return;        
        }

        if (!mo.isPickable)
        {
            Debug.Log(">>>Object cannot be picked up.");
            return;
        }

        if (MyLastTile.LaserOnTile != null)
        {
            Debug.Log(">>> Cant pickup object when standing on laser.");
            Debug.Log("///// My TILE: " + MyLastTile.Position);
            return;
        }


        float height = -1;

        if (!CheckItemStackingPickUp(LastSeenTile, out mo, out height))
            return;
        
        CarriingItem = mo;
        
        if (mo.Equals(LastSeenTile.ObjectOnTile))
            LastSeenTile.ChangeOccupation();

        karelAnimator.StartPickup();

        if (LastSeenTile.LaserOnTile != null)
            StartCoroutine(LaserActivationCoroutine(LastSeenTile));

        /*
        CarriingItem.transform.parent = transform;
        CarriingItem.transform.position = transform.position + new Vector3(0.5f, 1, -0.5f);
        */

        Generator g = CarriingItem as Generator;

        if (g != null)
        {
            if (g.IsOn)
                g.OnActivePickable();

            Generator genInOtherTimeline = LastSeenTile.OtherTimelineRef.ObjectOnTile as Generator;

            if (genInOtherTimeline != null)
                LastSeenTile.OtherTimelineRef.ChangeOccupation();
        }
        else
        {
            CarriingItem.ChangeMaterial(PTSShaderEnum.Stay);
        }
        Debug.Log("   --- Succesfuly picked up");
    }

    IEnumerator LaserActivationCoroutine(Tile t)
    {
        yield return new WaitForSeconds(0.25f);
        t.LaserOnTile.EnableLasers();
         
    }

    IEnumerator LaserDeactivationCoroutine(Tile t)
    {
        yield return new WaitForSeconds(0.25f);
        t.LaserOnTile.DestroyNextLasers();
    }

    public void CheckPreassurePlateOnTIle()
    {
        Debug.Log("Checking Preassure Plate");
        PreassurePlate pp = LastSeenTile.ActivatorOnTile as PreassurePlate;
        if (pp != null)
            pp.OnActivate();
    }

    /// <summary>
    /// Kontrola toho, zda chceme polozit objekt na zem a nasledne polozeni
    /// </summary>
    bool InputDrop()
    {
        Debug.Log("<<<DROP>>>");
        bool stackingItem = false;
        Box b = null;
        float height = -1;

        if (!LastSeenTile.IsAccessable)
        {
            Debug.Log("No ACCESS");
            return false;
        }

        if (CarriingItem == null)
        {
            Debug.Log(">>>WE ARE NOT CARRIING ANY ITEM");
            return false;
        }

        if (CarriingItem.JustPickedUp)
        {
            Debug.Log(">>>WE JUST PICKED THE ITEM... IGNORE");
            return false;
        }

        if (LastSeenTile.IsOccupied)
        {
            stackingItem = CheckItemStackingDrop(LastSeenTile, out b, out height);
            if (!stackingItem)
            {
                Debug.Log(">>>TILE ALREADY HAS SOME ITEM");
                return false;
            }            
        }

        Tile[] myTile = Level.GetTile(transform.position);

        if (myTile[0] == LastSeenTile || myTile[1] == LastSeenTile)
        {
            Debug.Log(">>>PLAYER IS STANDINDG ON THE TILE! CANT DROP");
            return false;
        }

        //CarriingItem.transform.parent = null;

        if (LastSeenTile.Timeline.Equals(TimelineObject.Present))
            CarriingItem.ChangeMaterial(PTSShaderEnum.Present);
        else
            CarriingItem.ChangeMaterial(PTSShaderEnum.Past);

        if (stackingItem)
        {
            //Debug.Log("Its stacking time...");
            //Debug.Log(b + " " + height);
            karelAnimator.StartDrop(new Vector3(0, 1, 0), b.gameObject.transform.rotation, b.gameObject.transform, false);
            /*
            CarriingItem.transform.parent = b.gameObject.transform;

            
            CarriingItem.transform.localPosition = new Vector3(0, 1, 0);
            CarriingItem.transform.rotation = b.gameObject.transform.rotation;
            */
            b.ItemOnTop = CarriingItem;
        }
        else
        {
            /*
            PreassurePlate pp = LastSeenTile.ActivatorOnTile as PreassurePlate;
            if (pp != null)
                pp.OnActivate();
            */  

            LastSeenTile.ChangeOccupation(CarriingItem);            
            karelAnimator.StartDrop(new Vector3(LastSeenTile.Position.x, LastSeenTile.Height, LastSeenTile.Position.y),
                new Quaternion(0, -180, 0, 0), null, true);

        }

        //Tile t = Level.Instance.CheckTimeLineDrop(LastSeenTile, CarriingItem);
        Generator g = CarriingItem as Generator;
        
        if (g != null)
        {
            g.UpdateEffectedTiles();

            if (g.IsOn)
                g.OnActiveDropable();

            if (CheckTileInOtherTimeline() && !stackingItem)
            {
                LastSeenTile.OtherTimelineRef.ObjectOnTile = g;
                LastSeenTile.OtherTimelineRef.IsOccupied = true;
            }

            g.ChangeMaterial(PTSShaderEnum.Stay);
        }

        if (LastSeenTile.LaserOnTile != null)
            StartCoroutine(LaserDeactivationCoroutine(LastSeenTile));

        CarriingItem = null;
        Debug.Log("   --- Succesfully dropped");
        SoundsManager.instance.PlayDrop();
        return true;
    }

    bool CheckItemStackingDrop(Tile t, out Box returnBox, out float returnHeight)
    {
        Box b = t.ObjectOnTile as Box;
        returnBox = null;
        returnHeight = -900;

        if (b == null)
        {
            //Debug.Log("Object on tile is not a box");
            return false;
        }

        float height = t.Height + 1;

        while (b != null && b.ItemOnTop != null)
        {
            height += 1;
            b = b.ItemOnTop as Box;
        }

        //Debug.Log("ReturnHeight: " + height + "   myHeight: " + transform.position.y);
        if (height > transform.position.y + 1.1f)
        {
            //Debug.Log("Its a lot higher");
            return false;
        }            

        if (b == null)
        {
            //Debug.Log("Its not a box");
            return false;
        }

        returnBox = b;
        returnHeight = height;
        //Debug.Log("Muzu stackovat");
        return true;  
    }
    bool CheckItemStackingPickUp(Tile t, out MovableObject returnObj, out float returnHeight)
    {
        Box b = t.ObjectOnTile as Box;
        returnHeight = t.Height;

        if (b == null)
        {
            returnObj = t.ObjectOnTile as MovableObject;

            if (returnHeight > this.transform.position.y + 1.1f)
            {
                Debug.Log(">>> Its way higher to pickup");
                return false;
            }

            if (returnHeight < this.transform.position.y - 1.1f)
            {
                Debug.Log(">>> Its way lower to pickup");
                return false;
            }
            //Debug.Log("***Picking UP No Box");
            return true;
        }

        Box parentBox = b;
        returnHeight = t.Height;
        returnObj = b;

        //Debug.Log(b + "   ItemOnTop: " + b.ItemOnTop);
        //prochazime nastackovane bedny
        while (b != null && b.ItemOnTop != null)
        {
            returnHeight += 1;
            returnObj = b.ItemOnTop;
            parentBox = b;
            b = b.ItemOnTop as Box;            
        }

        //Debug.Log("ReturnHeight: " + returnHeight + "   myHeight: " + transform.position.y);
        if (returnHeight > transform.position.y + 1.1f)
        {
            Debug.Log(">>> Its way higher to pickup");
            return false;
        }
        
        if (returnHeight < transform.position.y - 1.1f)
        {
            Debug.Log(">>> Its way lower to pickup");
            return false;
        }

        Debug.Log(parentBox);
        parentBox.ItemOnTop = null;
        return true;       
    }
}
