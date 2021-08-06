using System.Collections.Generic;
using UnityEngine;


public class PuzzlePart : MonoBehaviour
{
    [SerializeField] private MainController _mainController;
    [SerializeField] private List<PuzzlePart> _canLeapTo;
    [SerializeField] private Vector3 _bestPosition;

    public PuzzlePartsContainer _container;
    public Vector3 _containerDistance = Vector3.zero;


    public void Start()
    {
        _container = new PuzzlePartsContainer(this);
    }

    public Vector3 GetRelatedBestPosition()
    {
        return _bestPosition - transform.position;
    }

    public void OnDrop()
    {
        _container.OnDrop();
    }
    
    public void DropPart()
    {
        foreach (PuzzlePart other in _canLeapTo)
        {
            if ((other.GetRelatedBestPosition() - GetRelatedBestPosition()).sqrMagnitude < _mainController.GetSqrtDistanceCheck())
            {
                OnMove(_bestPosition - other.GetRelatedBestPosition());
                MergeContainers(other.gameObject.GetComponent<PuzzlePart>());
            }
        }
    }
    
    public void MergeContainers(PuzzlePart otherPuzzlePart)
    {
        if (_container.parts.Contains(otherPuzzlePart))
        {
            return;
        }
        _container.parts.AddRange(otherPuzzlePart._container.parts);
        foreach (var containerPart in otherPuzzlePart._container.parts)
        {
            containerPart._container = _container;
            containerPart._containerDistance = _containerDistance - _bestPosition + containerPart._bestPosition;
        }

        if (_container.parts.Count >= _mainController.GetTotalNumberOfParts())
        {
            _mainController.EndGame();
        }
    }

    public void OnMove(Vector3 touchPosition)
    {
        _container.OnMove(touchPosition, _containerDistance);
    }

    public void MovePart(Vector3 position)
    {
        var positionToMove = position + _containerDistance;
        positionToMove.z = transform.position.z;
        transform.position = positionToMove;
    }
    
}
