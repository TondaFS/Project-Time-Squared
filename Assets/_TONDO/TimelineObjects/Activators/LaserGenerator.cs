using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserGeneratorDirection
{
    Up, Down, Left, Right
}

public class LaserGenerator : Activator {
    /// <summary>
    /// Urcuje, kterym smerem bude laser "strilet" laser
    /// </summary>
    public LaserGeneratorDirection laserDirection;
    /// <summary>
    /// Reference na prefab laseru
    /// </summary>
    public GameObject laserPrefab;
    public GameObject laserPrefabUpDown;
    public Laser firstLaserReference;
    public bool awake = false;

    /// <summary>
    /// Urcuje, jestli se jedna o zrcadlo -> meni smer laseru
    /// </summary>
    public bool isMirror;
    Vector3Int dirVector;
    public Vector3Int DirVector
    {
        get
        {
            switch (laserDirection)
            {
                case LaserGeneratorDirection.Down:
                    dirVector = new Vector3Int(1, 0, 0);
                    break;
                case LaserGeneratorDirection.Up:
                    dirVector = new Vector3Int(-1, 0, 0);
                    break;
                case LaserGeneratorDirection.Left:
                    dirVector = new Vector3Int(0, 0, -1);
                    break;
                case LaserGeneratorDirection.Right:
                    dirVector = new Vector3Int(0, 0, 1);
                    break;
            }

            return dirVector;
        }
    }

    protected override void Start()
    {
        base.Start();

        StartCoroutine(WaitASecond());
    }

    public IEnumerator WaitASecond()
    {
        yield return new WaitForSeconds(1);
        if (awake)
            CreateLaserChain();
    }

    public override void Activate()
    {
        Debug.Log("ACTIVATION");
        base.Activate();
        
        OnActivate();
    }

    public override void OnActivate()
    {
        Debug.Log("Activation");
        base.OnActivate();        

        if (IsActivated)
            CreateLaserChain();
        else
            DestroyLaserChain();
    }

    public void CreateLaserChain()
    {
        Debug.Log("Creating laser chain");
        GameObject laserObj;

        //natocim
        if (laserDirection == LaserGeneratorDirection.Down || laserDirection == LaserGeneratorDirection.Up)
        {
            laserObj = Instantiate(laserPrefabUpDown);
            //laserObj.transform.Rotate(new Vector3(0, -90, -90));
        }
        else
        {
            laserObj = Instantiate(laserPrefab);            
        }

        Laser l = laserObj.GetComponent<Laser>();

        Debug.Log("Laser generator position: " + transform.position + " vector: " + DirVector);
        Tile[] t = Level.GetTile(transform.position + DirVector);
        Tile tile;

        if (l.render == null)
            l.render = l.GetComponent<MeshRenderer>();

        if (ItemType.Equals(TimelineObject.Present))
        {
            tile = t[0];
            l.ItemType = TimelineObject.Present;
            l.render.material = l.material;
        }
        else
        {
            tile = t[1];
            l.ItemType = TimelineObject.Past;
            l.render.material = l.pastMaterial;
            l.ChangeState();
        }
        Debug.Log("Tile: " + tile.Position.x + " " + tile.Position.y);
        
        //nastavim  pozici
        laserObj.transform.parent = this.transform;
        laserObj.transform.position = new Vector3(tile.Position.x, tile.Height, tile.Position.y);

        firstLaserReference = l;
        l.source = this;
        tile.LaserOnTile = l;
        l.myTile = tile;
        l.SetVisibility();
        l.CreateLaser(tile);
    }

    public void DestroyLaserChain()
    {
        if (firstLaserReference != null)
            firstLaserReference.DestroyLaser();

        firstLaserReference = null;
    }
}
