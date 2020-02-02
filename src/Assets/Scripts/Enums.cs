using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Loading = 0,
    Menu = 1,
    Tutorial = 2,
    Match = 3,
    VictoryScreen = 4,
}

public enum PlayerState
{
    Idle = 0,
    Moving = 1,
    Repairing = 2,
    Stunned = 3
}

public enum DestructorAbility
{
    None = 0,
    TRex = 1,
    Asteroid = 2,

}

public enum PickupType
{
    Boots = 0,
    Wrench = 1,
    Shield = 2,
}