using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapDragControllerSimlier : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    
    private Transform _draggedObject;
    private PuzzlePartSimlier _draggedObjectPuzzlePart;
    private bool _isDragging = false;
    private Vector3 _tapRelativePos;
    private void FixedUpdate()
    {
        if (Input.touchCount != 1)
        {
            _isDragging = false;
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            Vector3 point = _mainCamera.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag(("TapDraggable")))
            {
                _draggedObject = hit.transform;
                _draggedObjectPuzzlePart = _draggedObject.GetComponent<PuzzlePartSimlier>();
                if (_draggedObjectPuzzlePart.CanBeMoved())
                {
                    _isDragging = true;
                    _tapRelativePos = point - _draggedObject.transform.position;
                }
            }
        }
        
        if (_isDragging && touch.phase == TouchPhase.Moved)
        {
            Vector3 touchPosition = _mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0.0f));
            _draggedObjectPuzzlePart.OnMove(touchPosition - _tapRelativePos);
        }

        if (_isDragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            _draggedObject.GetComponent<PuzzlePartSimlier>().OnDrop();
            _isDragging = false;
            _draggedObject = null;
            _tapRelativePos = Vector3.zero;
        }
    }
}