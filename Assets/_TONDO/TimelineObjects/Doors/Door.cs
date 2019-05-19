using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : StateObject {
    //Rozsirit o 2 typy: DoorKey a DoorActivator - kazdy bude odpovidat typu, ktery jej dokaze otevrit
    /// <summary>
    /// Urcuje, zda jsou devere otevreny
    /// </summary>
    public bool IsOpen;
    /// <summary>
    /// Typ, kterym smerem se dvere otevrou
    /// </summary>
    public DoorOpening OpeningMode = DoorOpening.Top;
    /// <summary>
    /// Rychlost otevirani
    /// </summary>
    public float doorOpeningVelocity = 5f;

    public bool needAllButtons;
    public List<Activator> buttons;
    
    /// <summary>
    /// Pozice zavrenych dveri
    /// </summary>
    Vector3 closedPosition;
    /// <summary>
    /// Pozice otevrenych dveri
    /// </summary>
    Vector3 openedPosition;
    /// <summary>
    /// Ref. na coroutinu, behem ktere se dvere oteviraji/zaviraji
    /// </summary>
    Coroutine Closing;

    //STATICKE HODNOTY POSUNUTI OTEVRENI DVERI
    public static Vector3 TopPosition = new Vector3(0, 2, 0);
    public static Vector3 DownPostion = new Vector3(0, -2, 0);
    public static Vector3 XLeftPosition = new Vector3(-1, 0, 0);
    public static Vector3 XRightPosition = new Vector3(1, 0, 0);
    public static Vector3 ZLeftPosition = new Vector3(0, 0, -1);
    public static Vector3 ZRightPosition = new Vector3(0, 0, 1);

    public void CreateDoor(TimelineObject time, bool isOp, DoorOpening opening, Material mat, float vel, bool needAll)
    {
        ItemType = time;
        IsOpen = isOp;
        OpeningMode = opening;
        material = mat;
        doorOpeningVelocity = vel;
        needAllButtons = needAll;
    }

    protected override void Start()
    {
        base.Start();

        closedPosition = transform.localPosition;

        switch (OpeningMode)
        {
            case DoorOpening.Top:
                openedPosition = closedPosition + TopPosition;
                break;
            case DoorOpening.Down:
                openedPosition = closedPosition + DownPostion;
                break;
            case DoorOpening.XLeft:
                openedPosition = closedPosition + XLeftPosition;
                break;
            case DoorOpening.XRight:
                openedPosition = closedPosition + XRightPosition;
                break;
            case DoorOpening.ZLeft:
                openedPosition = closedPosition + ZLeftPosition;
                break;
            case DoorOpening.ZRight:
                openedPosition = closedPosition + ZRightPosition;
                break;
        }

        Level.Instance.AddDoorToList(this);
        Tile[] t = Level.GetTile(transform.position);

        CheckOnStart(t);            
    }


    /// <summary>
    /// Za je vytvorena tile reprezenace levelu, priradi dvere k odpovidajicimu tilu a nastavi svuj stav
    /// </summary>
    /// <returns></returns>
    protected override void CheckOnStart(Tile[] t)
    {
        base.CheckOnStart(t);

        if (IsOpen)
        {
            //Debug.Log("Door is open");
            transform.localPosition = openedPosition;
        }
        else
        {
            //Debug.Log("Door is Closed");
            Level.Instance.ChangeTileAccessability(transform.parent.position - new Vector3(-1, 0, 1), ItemType, false);
        }

    }
    
    public override void Activate()
    {
        if (needAllButtons)
        {
            if (!IsOpen)
            {
                Debug.Log("Buttons: " + buttons.Count);
                int i = 0;
                foreach (Activator a in buttons)
                {
                    if (!a.IsActivated)
                    {
                        Debug.Log("CANT OPEN DOOR... not all buttons activated: " + i);
                        
                        return;
                    }
                    i++;
                }
            }
        }

        IsOpen = !IsOpen;

        if (IsOpen)
            Level.Instance.ChangeTileAccessability(transform.parent.position - new Vector3(1, 0, 1), ItemType, true);
        else
            Level.Instance.ChangeTileAccessability(transform.parent.position - new Vector3(1, 0, 1), ItemType, false);
        
        if (Closing != null)
            StopCoroutine(Closing);

        Closing = StartCoroutine(MoveDoor());
    }
    IEnumerator MoveDoor()
    {
        //Debug.Log("Courotine door movement");
        bool moving = true;
        //Debug.Log(openedPosition);
        //Debug.Log(closedPosition);

        while (moving)
        {
            if (IsOpen)
            {
                //Debug.Log("OPEN");
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, openedPosition, 
                    doorOpeningVelocity * Time.deltaTime);

                if (transform.localPosition == openedPosition)
                    moving = false;
                else
                {
                    yield return null;
                }
            }
            else
            {
                //Debug.Log("Close");
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, closedPosition, 
                    doorOpeningVelocity * Time.deltaTime);

                if (transform.localPosition == closedPosition)
                    moving = false;
                else
                {
                    yield return null;
                }
            }
        }
    }
    public void SetDoorPos(DoorOpening o)
    {
        switch (OpeningMode)
        {
            case DoorOpening.Top:
                transform.localPosition = TopPosition;
                break;
            case DoorOpening.Down:
                transform.localPosition = DownPostion;
                break;
            case DoorOpening.XLeft:
                transform.localPosition = XLeftPosition;
                break;
            case DoorOpening.XRight:
                transform.localPosition = XRightPosition;
                break;
            case DoorOpening.ZLeft:
                transform.localPosition = ZLeftPosition;
                break;
            case DoorOpening.ZRight:
                transform.localPosition = ZRightPosition;
                break;
        }
    }
}

public enum DoorOpening {
    XLeft, XRight, Top, Down, ZLeft, ZRight
}

