using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructorCanvasController : MonoBehaviour
{
    public Color ActiveAbilityColor;
    public Image TRexButton;
    public Image AsteroidButton;
    public Image QuicksandButton;
    public Image ArmageddonButton;




    private DestructorController _destructorController;

    void Awake()
    {
        _destructorController = GetComponentInParent<DestructorController>();
    }

    public void OnClickTrex()
    {
        SetActiveAbility(DestructorAbility.TRex, TRexButton);
    }

    public void OnClickAsteroid()
    {
        SetActiveAbility(DestructorAbility.Asteroid, AsteroidButton);
    }

    public void OnClickQuicksand()
    {
        SetActiveAbility(DestructorAbility.Quicksand, QuicksandButton);
    }

    public void OnClickArmageddon()
    {
        SetActiveAbility(DestructorAbility.Armageddon, ArmageddonButton);
    }


    private void SetActiveAbility(DestructorAbility ability, Image buttonImage)
    {
        var activate = _destructorController.CurrentAbility != ability;
        var nextAbility = activate ? ability : DestructorAbility.None;
        var buttonColor = activate ? ActiveAbilityColor : Color.white;

        _destructorController.SetAbility(nextAbility);

        ResetButtonColors();

        // Set active ability color
        buttonImage.color = buttonColor;
    }

    private void ResetButtonColors()
    {
        // Set color of all buttons to white
        TRexButton.color = Color.white;
        AsteroidButton.color = Color.white;
        QuicksandButton.color = Color.white;
        ArmageddonButton.color = Color.white;
    }

    public void UseAbility(DestructorAbility ability)
    {
        ResetButtonColors();
    }
}
