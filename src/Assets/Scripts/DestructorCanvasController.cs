using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructorCanvasController : MonoBehaviour
{
    public Color ActiveAbilityColor;
    public Image TRexButton;
    private DestructorController _destructorController;

    void Awake()
    {
        _destructorController = GetComponentInParent<DestructorController>();
    }

    public void OnClickTrex()
    {
        SetActiveAbility(DestructorAbility.TRex, TRexButton);
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
    }

    public void UseAbility(DestructorAbility ability)
    {
        ResetButtonColors();
    }
}
