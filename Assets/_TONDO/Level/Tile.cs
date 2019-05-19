using UnityEngine;

/// <summary>
/// Trida reprezentujici jednotlivy tile na hraci plose. Kazdy tile ma svou pozici, identifikator,
/// ktery rika, jestli je tile zrovna obsazen nejakym predmetem a pripadne referenci na item, ktery se
/// na tilu zrovna nachazi.
/// Ma v sobe podporu zmeny okupace tilu, zjisteni, zda je tile dostupny
/// </summary>
[System.Serializable]
public class Tile {
    #region PROMENNE
    /// <summary>
    /// Urcuje, zda je tile dostupny - uziti prevazne pro Tily, co jsou zdi, jamy, nedostupna mista na ktere se hrac ani 
    /// item nebude moci jednoznacne dostat
    /// </summary>
    public bool IsAccessable { get; set; }
    /// <summary>
    /// Pozice tilu ve scene
    /// </summary>
    public Vector2Int Position { get; set; }
    /// <summary>
    /// Vyska, ve ktere je Tile umisten
    /// </summary>
    public float Height { get; set; }
    /// <summary>
    /// Je na tilu umisten nejaky predmet?
    /// </summary>
    public bool IsOccupied { get; set; }
    /// <summary>
    /// Item, ktery je zrovna umisten na tomto tilu
    /// </summary>
    public PTSObject ObjectOnTile { get; set; }
    /// <summary>
    /// Reference na tile v druhe casove linii
    /// </summary>
    public Tile OtherTimelineRef { get; set; }

    public Laser LaserOnTile { get; set; }

    public TimelineObject Timeline { get; set; }
    /*
    /// <summary>
    /// Generator, ktery je zrovna umisten na tomto tilu
    /// </summary>
    public Generator GeneratorOnTile { get; set; }
    */
    /// <summary>
    /// Aktivator, ktery k danemu tilu patri
    /// </summary>
    public Activator ActivatorOnTile { get; set; }
    /*
    public Door DoorOnTile { get; set; }
    public StaticGeometry StaticObject { get; set; }
    */
    public bool isPresentTile;
    
    #region PRO TESTOVANI
    /// <summary>
    /// Cervena referencni vizualni kostka pro praci se scenou
    /// </summary>
    public GameObject referenceCube;
    #endregion
    #endregion
    #region KONSTRUKTORY
    public Tile(Vector3 pos)
    {
        Vector3Int p = Vector3Int.FloorToInt(pos);

        Position = new Vector2Int(p.x, p.z);
        Height = pos.y;
        IsOccupied = false;
        IsAccessable = true;
    }
    public Tile(Vector3 pos, bool occupied)
    {
        Vector3Int p = Vector3Int.FloorToInt(pos);

        Position = new Vector2Int(p.x, p.z);
        Height = pos.y;
        IsOccupied = occupied;
        IsAccessable = true;
    }
    public Tile(Vector3 pos, PTSObject o)
    {
        Vector3Int p = Vector3Int.FloorToInt(pos);
        Position = new Vector2Int(p.x, p.z);
        Height = pos.y;
        ObjectOnTile = o;
        IsOccupied = true;
        IsAccessable = true;
    }
    #endregion
    
    /// <summary>
    /// Zmeni obsazeni tilu na false, protoze jsme odebrali item
    /// </summary>
    public void ChangeOccupation()
    {
        IsOccupied = false;
        ObjectOnTile = null;
    }
    /// <summary>
    /// Zmeni obsazeni tilu na true, protoze jsme na nej zrovna umistili nejaky item
    /// </summary>
    /// <param name="o">Item, ktery byl na tile umisten</param>
    public void ChangeOccupation(Item o)
    {
        IsOccupied = true;
        ObjectOnTile = o;
        o.transform.parent.position = new Vector3(Position.x, Height, Position.y) + new Vector3(0, (o.gameObject.transform.localScale.x / 2), 0);
        o.transform.parent.rotation = new Quaternion(0, 0, 0, 0);
    }
    /// <summary>
    /// Zmeni obsazeni tilu na true, protoze na nej byl pridat generator
    /// </summary>
    /// <param name="o">Generator, ktery byl na tile umisten</param>
    public void ChangeOccupation(PTSObject o)
    {
        IsOccupied = true;
        ObjectOnTile = o;
        /*
        ObjectOnTile.transform.position = new Vector3(Position.x, Height, Position.y);
        ObjectOnTile.transform.rotation = new Quaternion(0, -180, 0, 0);
        */
    }

    public void ChangeOccupation(Activator a)
    {
        if (a.OccupyTile)
        {
            IsOccupied = true;
            ObjectOnTile = a;
        }
        
        ActivatorOnTile = a;
    }
    /// <summary>
    /// Zmeni dostupnost tilu
    /// </summary>
    /// <param name="acces">parametr urcujici novou dostupnost</param>
    public void ChangeAccessability(bool acces)
    {
        IsAccessable = acces;
    }

    /// <summary>
    /// Zkontroluje, zda je cilovy tile pristupny od daneho startovniho tilu.
    /// Pokud neni dostupny, ci je obsazeny, vraci false. V opacnem pripade spocita
    /// vzdalenost mezi obema Tily a jejich uhel - nasledne kontroluje zda vypocitana
    /// vzdalenost odpovida dane vzdalenosti a kontroluje i vysku jednotlivych tilu.
    /// V pripade, ze je Tile nize nebo na stejne urovni: je dostupny
    /// V pripade, ze je Tile vyse: neni dostupny
    /// </summary>
    /// <param name="startingTile">Tile, ze ktereho vychazime</param>
    /// <param name="destinationTile">Tile, ke kteremu zjustujeme dostupnost</param>
    /// <param name="distance">(Nepovinne) Pozadovana vzdalenost. Pokud neni urceno, je pouzite hodnota: 1</param>
    /// <param name="heightCeck">(Nepovinne) Povoleny vyskovy rozdil. Pokud neni urceno, je pouzita hodnota: 0.5</param>
    /// <returns></returns>
    public static bool IsTileAccessible(Tile startingTile, Tile destinationTile, int distance = 1, float heightCeck = .5f)
    {
        if (!destinationTile.IsAccessable)
            return false;

        if (destinationTile.IsOccupied)
            return false; 

        Distance _distance = GetTilesDistance(startingTile, destinationTile);

        //rozdil mezi vyskami vzhledem k cilovemu tilu od startovniho
        float heightDistance = destinationTile.Height - startingTile.Height;

        //Tile neni na stejne x-ove nebo z-ove ose -> musime zkontrolovat vetsi vzdalenost: pythagorova veta
        if (_distance.angle % 90 != 0)
        {            
            float maxDistance = Mathf.Sqrt(2 * Mathf.Pow(distance, 2));

            //u druhe podminky testuji, zda se nachazime v povolenem vyskovem rozdilu
            if (_distance.distance <= maxDistance && (heightDistance <= heightCeck))
                return true;

            return false;
        }

        //Tile je na stejne x-ove nebo z-ove ose -> vyuzivame danou vzdalenost a druhou podmiku stejnou jak u pripadu vyse
        if (_distance.distance <= distance && (heightDistance <= heightCeck))
            return true;

        return false;
    }
    
    /// <summary>
    /// Spocita mezi dvema tily jejich vzdalenost a uhel, ktery mezi sebou sviraji
    /// </summary>
    /// <param name="start">Tile, ze ktereho vychazime</param>
    /// <param name="end">Tile ke kteremu zjistujeme vzdalenost a uhel</param>
    /// <returns></returns>
    public static Distance GetTilesDistance(Tile start, Tile end)
    {
        Distance distance;
        distance.distance = Vector2.Distance(start.Position, end.Position);
        distance.angle = Vector2.Angle(start.Position, end.Position);

        return distance;
    }

    //Prozatim pro testovaci ucely... pozdeji prepsat tak, aby vyplo zvyrazneni daneho pole
    public void SetTileToNormal()
    {
        if (referenceCube != null)
            referenceCube.layer = PTSLayers.InvisibleToCamera;
    }

    public void SetTileToHighlited()
    {
        if (referenceCube != null)
            referenceCube.layer = PTSLayers.NoInteractionWithPlayer;
    }
}

/// <summary>
/// Struktura pro reprezentaci vzdalenosti mezi dvema tily. Uchovava u sebe i uhel mezi jednotlivymi
/// tily, ktery se nasledne vyuziva pri zjisteni dostupnosti tilu.
/// </summary>
public struct Distance
{
    public float distance;
    public float angle;
}
