using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemInfo : PawnComponent
{
    private static int index =0;
    [SerializeField]
    List<Sprite> names;
    [SerializeField]
    Image renderTarget;
    
    


    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
        renderTarget.sprite = names[index % names.Count];
        index++;

        stats.TryAdd(ComponentStat.RareResource, 0);
        stats[ComponentStat.RareResource] = Random.Range(0, 5)+10;
        stats.TryAdd(ComponentStat.MediumResource, 0);
        stats[ComponentStat.MediumResource] = Random.Range(0, 10)+10;
        stats.TryAdd(ComponentStat.WellResource, 0);
        stats[ComponentStat.WellResource] = Random.Range(0, 15)+10;

    }



}
