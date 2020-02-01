using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TRexAnimationController : MonoBehaviour
{
    private TRexController _controller;

    void Awake()
    {
        _controller = GetComponentInParent<TRexController>();
    }

    public void StompLeft()
    {
        _controller.StompLeft();
    }

    public void StompRight()
    {
        _controller.StompRight();
    }
}
