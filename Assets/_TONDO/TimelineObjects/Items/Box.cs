using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Item {
    /// <summary>
    /// Predmet, ktery je polozeny na krabici
    /// </summary>
    public MovableObject ItemOnTop;

    /// <summary>
    /// Nastavi vsechny potrebne informace o krabici
    /// </summary>
    /// <param name="itemType">V jake timeline krabice je</param>
    /// <param name="presMat">Material pro pritomnost</param>
    /// <param name="pastMat">material pro minulost</param>
    /// <param name="bothMat">material pro stav, kdy je zvednuty</param>
    public void CreateBox(TimelineObject itemType, Material mat)
    {
        ItemType = itemType;
        material = mat;
        isPickable = true;
        isPushable = true;
        ThrowableDistance = 2;
        movementVelocity = 10;
    }

    /// <summary>
    /// Prochazi bednu a itemy co jsou naskladane na ni. Pokud narazi na generator, prepocita mu
    /// tily, ktere ovlivnuje
    /// </summary>
    public void CheckGeneratorOnTop()
    {
        Debug.Log("Provhazim bednu");
        Box b = this;

        while (b != null && b.ItemOnTop != null)
        {
            Box bt = b.ItemOnTop as Box;

            if (bt != null)
                b = bt;
            else
            {
                Generator g = b.ItemOnTop as Generator;

                if (g != null)                    
                    g.UpdateEffectedTiles();
                break;
            }            
        }
    }
}
