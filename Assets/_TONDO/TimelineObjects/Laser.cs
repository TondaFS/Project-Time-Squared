using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserDirection { upDown, leftRight }

public class Laser : PTSObject {
    /// <summary>
    /// nato4en9 laseru v levelu
    /// </summary>
    public LaserDirection orientation;

    //reference
    public Laser previousLaser;
    public Laser nextLaser;
    public LaserGenerator source;
    public Tile myTile;
    Collider c;

    public Activator receiver;
    public Material pastMaterial;

    protected override void Start()
    {
        render = GetComponent<MeshRenderer>();
        c = GetComponent<Collider>();
    }

    public void CreateLaser(Tile t)
    {
        if (render == null)
            render = GetComponent<MeshRenderer>();

        if (c == null)
            c = GetComponent<Collider>();

        render.enabled = true;

        LaserReceiver lr = t.ActivatorOnTile as LaserReceiver;
        if (lr != null)
        {
            receiver = lr;
            receiver.OnActivate();
            return;
        }

        if (t.ObjectOnTile != null)
        {
            LaserGenerator lg = t.ObjectOnTile as LaserGenerator;

            if (lr != null)
            {
                receiver = lr;
                receiver.OnActivate();
            }
            else if (lg != null && lg.isMirror)
            {
                receiver = lg;
                receiver.OnActivate();
            }
            else
            {
                render.enabled = false;
            }            
        }

        Vector3 vec = transform.position + source.DirVector;
        Debug.Log("TRANS: " + transform.position + " " + source.DirVector);
        Debug.Log(vec.x);
        Tile[] tiles = Level.GetTile(vec);

        if (tiles == null)
            return; 
        
        Tile tile;
        Debug.Log("Tile " + tiles[0].Position);

        GameObject laserObj;

        if (orientation.Equals(LaserDirection.upDown))
            laserObj = Instantiate(source.laserPrefabUpDown);
        else
            laserObj = Instantiate(source.laserPrefab);
        
        Laser l = laserObj.GetComponent<Laser>();

        if (l.render == null)
            l.render = l.GetComponent<MeshRenderer>();

        if (ItemType.Equals(TimelineObject.Present))
        {
            tile = tiles[0];
            l.ItemType = TimelineObject.Present;
            l.render.material = l.material;
        }
        else
        {
            tile = tiles[1];
            l.ItemType = TimelineObject.Past;
            l.render.material = l.pastMaterial;
        }

        if (!tile.IsAccessable)
        {
            Debug.Log("There is no such tile!");
            Destroy(laserObj);
            return;
        }       

        laserObj.transform.parent = source.transform;
        l.source = source;
        l.previousLaser = this;
        nextLaser = l;
        tile.LaserOnTile = l;
        l.myTile = tile;

        Debug.Log("TILE SET: " + myTile.Position);
        laserObj.transform.rotation = this.transform.rotation;
        laserObj.transform.position = new Vector3(tile.Position.x, tile.Height, tile.Position.y);

        Debug.Log("Visibility check");
        l.SetVisibility();
        l.CreateLaser(tile);

    }

    public void DestroyNextLasers()
    {
        Debug.Log("DISABLING LASERS");
        render.enabled = false;

        if (nextLaser != null)
            nextLaser.DisableLasers();

        //nextLaser = null;

        Debug.Log("Next Lasers Disabled...");
    }

    public void DisableLasers()
    {
        if (nextLaser != null)
            nextLaser.DisableLasers();

        if (receiver != null)
            receiver.OnActivate();

        //myTile.LaserOnTile = null;

        render.enabled = false;
        c.enabled = false;
    }

    public void EnableLasers()
    {
        render.enabled = true;
        c.enabled = true;

        if (receiver != null)
        {
            receiver.OnActivate();
            return;
        }

        if (nextLaser != null)
            nextLaser.EnableLasers();

        SetVisibility();        
    }

    public void SetVisibility()
    {
        Debug.Log("Looking for tile: " + myTile.Position);
        foreach (Generator g in Level.Instance.Generators)
        {
            foreach (Tile t in g.AffectedTiles)
            {
                
                if (t.Equals(myTile))
                {
                    Debug.Log("TILE FOUNDED");
                    if (g.IsOn)
                    {
                        if (ItemType.Equals(TimelineObject.Present))
                        {
                            SetInvisibleLayer();
                            Debug.Log("PRESENT: INVISIBLE LAYER COS ISON");
                        }
                        else
                        {
                            SetVisibleLayer();
                            Debug.Log("PAST: VISIBLE LAYER COS ISON");
                        }


                        return;
                    }
                    else
                    {
                        if (ItemType.Equals(TimelineObject.Present))
                        {
                            SetVisibleLayer();
                            Debug.Log("PRESENT: VISIBLE LAYER COS NOT ON");
                        }
                        else
                        {
                            SetInvisibleLayer();
                            Debug.Log("PAST: INVISIBLE LAYER COS NOT ON");
                        }
                        return;
                    }
                }
            }
        }

        if (ItemType.Equals(TimelineObject.Present))
        {
            SetVisibleLayer();
            Debug.Log("PRESENT: VISIBLE LAYER COS NO GEN");
        }
        else
        {
            SetInvisibleLayer();
            Debug.Log("PAST: INVISIBLE LAYER COS NO GEN");
        }
    }

    public void DestroyLaser()
    {
        if (nextLaser != null)
            nextLaser.DestroyLaser();

        if (receiver != null)
            receiver.OnActivate();

        Destroy(this.gameObject);
    }



}
