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

    [SerializeField]
    int commonBase = 5;
    [SerializeField]
    int commonRange = 10;
    [SerializeField]
    int uncommonBase = 5;
    [SerializeField]
    int uncommonRange = 10;
    [SerializeField]
    int rareBase = 5;
    [SerializeField]
    int rareRange = 10;

    public override void EstablishPawnComponent(Pawn owner, UniverseSimulation universeSimulation)
    {
        base.EstablishPawnComponent(owner, universeSimulation);
        renderTarget.sprite = names[index % names.Count];
        index++;

        stats.TryAdd(ComponentStat.RareResource, 0);
        stats[ComponentStat.RareResource] = Random.Range(0, rareRange)+rareBase;
        stats.TryAdd(ComponentStat.UncommonResource, 0);
        stats[ComponentStat.UncommonResource] = Random.Range(0, uncommonRange)+uncommonBase;
        stats.TryAdd(ComponentStat.CommonResource, 0);
        stats[ComponentStat.CommonResource] = Random.Range(0, commonRange)+commonBase;

    }



}
