using System.Collections.Generic;
using Core;
using UnityEngine;

public class ConveyorMono : CharacterMono
{
    private static Dictionary<CharacterType, List<GameObject>> CharacterPool = new Dictionary<CharacterType, List<GameObject>>();

    private Conveyor conveyor => (Conveyor)this.Character;
}