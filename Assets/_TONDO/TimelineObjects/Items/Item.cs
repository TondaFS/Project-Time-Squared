using UnityEngine;
/// <summary>
/// Trida reprezentujici item, se kterym lze manipulovat ve scene. 
/// Ma urceny vztah k casovym liniim, sve vlastnosti, viditelnost/kolidovatelnost s hracem 
/// a max vzdalenost, na kterou jej lze hodit. 
/// </summary>
public class Item : MovableObject {   
    
    protected override void Start()
    {
        base.Start();        

        Level.Instance.AddItemToList(this);
    }
}