using System;
using UnityEngine;


public class PuzzlePartSimlier : MonoBehaviour
{
    [SerializeField] private MainControllerSimlier _mainController;
    [SerializeField] private Vector3 _bestPosition;
    [SerializeField] private bool _onBestPosition = false;
    private SpriteRenderer _spriteRenderer;
    private Material _baseMaterial;


    public void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _baseMaterial = _spriteRenderer.material;
    }

    public Vector3 GetRelatedBestPosition()
    {
        return _bestPosition - transform.position;
    }

    public void OnDrop()
    {
        if (GetRelatedBestPosition().sqrMagnitude < _mainController.GetSqrtDistanceCheck())
        {
            OnMove(_bestPosition);
            _onBestPosition = true;
            _mainController.AddPartOnBest();
            _spriteRenderer.material = _mainController.GetSuccessMaterial();
            Invoke(nameof(SetBaseMaterial), 0.2f);
        }
    }

    public void OnMove(Vector3 touchPosition)
    {
        touchPosition.z = transform.position.z;
        transform.position = touchPosition;
    }

    public bool CanBeMoved()
    {
        return !_onBestPosition;
    }

    public void SetBaseMaterial()
    {
        _spriteRenderer.material = _baseMaterial;
    }
}
