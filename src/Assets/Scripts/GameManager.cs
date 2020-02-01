using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public class GameManager : GenericManager<GameManager>, ILoadedManager
{
    public AnimationCurve PartRepairPositionAnimation;
    public AnimationCurve PartRepairRotationAnimation;

    public CameraController CameraController;

    public void Initialize()
    {
        if (!InitializeSingleton(this)) return;
    }

    public void PostInitialize() { }
}
