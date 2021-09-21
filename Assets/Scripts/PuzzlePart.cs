using UnityEngine;


public class PuzzlePart : MonoBehaviour
{
    [SerializeField] private MainController _mainController;
    [SerializeField] private Vector3 _bestPosition;
    [SerializeField] private bool _onBestPosition = false;
    [SerializeField] private PuzzlePart[] _group;
    private SpriteRenderer _spriteRenderer;
    private Material _baseMaterial;


    public void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _baseMaterial = _spriteRenderer.material;
    }

    private Vector3 GetRelatedPosition(Vector3 position)
    {
        var result = position - transform.position;
        result.z = 0;
        return result;
    }

    public Vector3 GetRelatedBestPosition()
    {
        return GetRelatedPosition(_bestPosition);
    }

    private void OnDropBest()
    {
        GetComponent<Collider2D>().enabled = false;
        OnMove(_bestPosition);
        _onBestPosition = true;
        _mainController.AddPartOnBest();
        _spriteRenderer.material = _mainController.GetSuccessMaterial();
        Invoke(nameof(SetBaseMaterial), 0.2f);
    }

    public void OnDrop()
    {
        if (GetRelatedBestPosition().sqrMagnitude < _mainController.GetSqrtDistanceCheck())
        {
            OnDropBest();
        }
        else if (_group.Length > 0)
        {
            foreach (var groupPuzzlePart in _group)
            {
                if (groupPuzzlePart._onBestPosition)
                {
                    continue;
                }
                var bestPosition = groupPuzzlePart._bestPosition;
                if (GetRelatedPosition(bestPosition).sqrMagnitude < _mainController.GetSqrtDistanceCheck())
                {
                    groupPuzzlePart._bestPosition = _bestPosition;
                    _bestPosition = bestPosition;
                    OnDropBest();
                }
            }
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
