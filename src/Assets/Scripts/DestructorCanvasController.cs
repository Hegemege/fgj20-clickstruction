using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructorCanvasController : MonoBehaviour
{
    public Color ActiveAbilityColor;
    public Button TRexButton;
    public Button AsteroidButton;
    public Button QuicksandButton;
    public Button ArmageddonButton;

    public Text TRexPrice;
    public Text AsteroidPrice;
    public Text QuicksandPrice;
    public Text ArmageddonPrice;


    private DestructorController _destructorController;

    void Awake()
    {
        _destructorController = GetComponentInParent<DestructorController>();
    }

    void Update()
    {
        TRexButton.enabled = GameManager.Instance.Mana >= GameManager.Instance.TRexManaCost;
        AsteroidButton.enabled = GameManager.Instance.Mana >= GameManager.Instance.AsteroidManaCost;
        QuicksandButton.enabled = GameManager.Instance.Mana >= GameManager.Instance.QuicksandManaCost;
        ArmageddonButton.enabled = GameManager.Instance.Mana >= GameManager.Instance.ArmageddonManaCost;
    }

    public void OnClickTrex()
    {
        if (GameManager.Instance.SpendMana(GameManager.Instance.TRexManaCost))
        {
            SetActiveAbility(DestructorAbility.TRex, TRexButton);
        }
    }

    public void OnClickAsteroid()
    {
        if (GameManager.Instance.SpendMana(GameManager.Instance.AsteroidManaCost))
        {
            SetActiveAbility(DestructorAbility.Asteroid, AsteroidButton);
        }
    }

    public void OnClickQuicksand()
    {
        if (GameManager.Instance.SpendMana(GameManager.Instance.QuicksandManaCost))
        {
            SetActiveAbility(DestructorAbility.Quicksand, QuicksandButton);
        }
    }

    public void OnClickArmageddon()
    {
        if (GameManager.Instance.SpendMana(GameManager.Instance.ArmageddonManaCost))
        {
            SetActiveAbility(DestructorAbility.Armageddon, ArmageddonButton);
        }
    }


    private void SetActiveAbility(DestructorAbility ability, Button button)
    {
        var activate = _destructorController.CurrentAbility != ability;
        var nextAbility = activate ? ability : DestructorAbility.None;
        var buttonColor = activate ? ActiveAbilityColor : Color.white;

        _destructorController.SetAbility(nextAbility);

        ResetButtonColors();

        // Set active ability color
        button.image.color = buttonColor;
    }

    private void ResetButtonColors()
    {
        // Set color of all buttons to white
        TRexButton.image.color = Color.white;
        AsteroidButton.image.color = Color.white;
        QuicksandButton.image.color = Color.white;
        ArmageddonButton.image.color = Color.white;
    }

    public void UseAbility(DestructorAbility ability)
    {
        ResetButtonColors();
    }
}
