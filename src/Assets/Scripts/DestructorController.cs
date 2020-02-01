using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityUtilities;

public class DestructorController : MonoBehaviour
{
    public LayerMask StaticEnvironmentLayer;
    public DestructorAbility CurrentAbility = DestructorAbility.None;

    private EventSystem _eventSystem;
    private DestructorCanvasController _canvasController;

    void Awake()
    {
        _eventSystem = GetComponentInChildren<EventSystem>();
        _canvasController = GetComponentInChildren<DestructorCanvasController>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }

    public void SetAbility(DestructorAbility ability)
    {
        CurrentAbility = ability;
    }

    private void OnClick()
    {
        // Check if UI was pressed - block mouse input
        if (_eventSystem.IsPointerOverGameObject()) return;
        if (CurrentAbility == DestructorAbility.None) return;

        switch (CurrentAbility)
        {
            case DestructorAbility.TRex:
                SpawnTrex();
                break;
        }

        CurrentAbility = DestructorAbility.None;
    }

    private bool GetEnvironmentClick(out RaycastHit hit)
    {
        var camera = GameManager.Instance.CameraController.Camera;

        // Spawn ray and hit the environment
        var inputPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        var worldClickPosition = camera.ScreenToWorldPoint(inputPosition);
        var direction = worldClickPosition - camera.transform.position;

        return Physics.Raycast(camera.transform.position,
            direction.normalized,
            out hit,
            100f,
            StaticEnvironmentLayer,
            QueryTriggerInteraction.Ignore);
    }

    private void SpawnTrex()
    {
        this.Log("SpawnTrex");

        if (GetEnvironmentClick(out RaycastHit hit))
        {
            var trexWrapper = PoolManager.Instance.TRexPool.GetPooledObject();
            trexWrapper.gameObject.transform.position = hit.point;

            _canvasController.UseAbility(DestructorAbility.TRex);
        }
    }
}