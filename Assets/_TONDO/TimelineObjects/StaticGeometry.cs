using UnityEngine;
/// <summary>
/// Trida reprezentujici staticky objekt ve scene
/// </summary>
public class StaticGeometry : PTSObject {
    protected override void Start()
    {
        base.Start();

        Level.Instance.AddStaticGeometryToList(this);
        if (material != null)
            render.material = material;
    }

    public void CreateStaticGeometry(TimelineObject type, Material mat)
    {
        ItemType = type;
        material = mat;
    }

    protected override void CheckOnStart(Tile[] t)
    {
        if (ItemType.Equals(TimelineObject.Present))
        {
            t[0].ObjectOnTile = this;
            t[0].IsOccupied = true;
            //t[0].IsAccessable = false;
            SetVisibleLayer();
            //Debug.Log("PTSObject to tile " + t[0].Position + " " + transform.position.y);
            t[0].Height = transform.position.y + 1;
        }
        else if (ItemType.Equals(TimelineObject.Past))
        {
            t[1].ObjectOnTile = this;
            t[1].IsOccupied = true;
            //t[1].IsAccessable = false;
            SetInvisibleLayer();
            Debug.Log("PTSObject to tile " + t[1].Position + " " + transform.position.y);
            t[1].Height = transform.position.y + 1;
        }
    }
}
