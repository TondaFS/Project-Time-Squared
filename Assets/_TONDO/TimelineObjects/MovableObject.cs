using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Třída reprezentujici objekty, se kterými půjde hýbat a přenášet je (itemy, generátory). Obsahuje
/// funkcionalitu, umožňující přesun po objektech
/// </summary>
public class MovableObject : PTSObject {
    /// <summary>
    /// Urcuje, zda lze objekt sebrat a nekam jej prenest
    /// </summary>
    public bool isPickable;
    /// <summary>
    /// Urcuje, zda lze s objektem posouvat
    /// </summary>
    public bool isPushable;
    /// <summary>
    /// Jak daleko lze s predmetem hazet
    /// </summary>
    public int ThrowableDistance = 1;
    //public bool JustDropped { get; set; }
    public bool JustPickedUp { get; set; }
 
    /// <summary>
    /// kam se ma objekt pohnout
    /// </summary>
    Vector3 currentMovementDestinatoin;
    /// <summary>
    /// Rychlostpohybu
    /// </summary>
    public float movementVelocity = 10f;
    
    protected override void Start()
    {
        base.Start();      
    }

    public IEnumerator PushMovement(Vector3 dest)
    {
        bool isMoving = true;
        currentMovementDestinatoin = dest;

        while (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentMovementDestinatoin, movementVelocity * Time.deltaTime);

            if (transform.position == currentMovementDestinatoin)
                isMoving = false;
            else
                yield return null;
        }

        Box b = this as Box;
        if (b != null)
            b.CheckGeneratorOnTop();
    }
    
    /// <summary>
    /// Metoda pro item na zaklade jeho casoveho zarazeni, nastavi ulozeni reference do 
    /// odpovidajiciho tilu a nastavi collidery a viditelnost.
    /// </summary>
    /// <param name="t"></param>
    protected override void CheckOnStart(Tile[] t)
    {
        if (material != null)
            render.material = material;

        if (ItemType.Equals(TimelineObject.Present))
        {
            t[0].ObjectOnTile = this;
            t[0].IsOccupied = true;
            SetVisibleLayer();

            ChangeMaterial(PTSShaderEnum.Present);
        }
        else if (ItemType.Equals(TimelineObject.Past))
        {
            t[1].ObjectOnTile = this;
            t[1].IsOccupied = true;
            SetInvisibleLayer();
            ChangeMaterial(PTSShaderEnum.Past);
        }
    }        
}


