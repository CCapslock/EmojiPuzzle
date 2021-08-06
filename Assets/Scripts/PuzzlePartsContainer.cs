using System.Collections.Generic;
using UnityEngine;


public class PuzzlePartsContainer
{
    public List<PuzzlePart> parts;

    public PuzzlePartsContainer(PuzzlePart part)
    {
        parts = new List<PuzzlePart>();
        parts.Add(part);
    }

    public void OnDrop()
    {
        var partsCurrent = parts.ToArray();
        foreach (var puzzlePart in partsCurrent)
        {
            puzzlePart.DropPart();
        }
    }

    public void OnMove(Vector3 position, Vector3 puzzlePartDistance)
    {
        foreach (PuzzlePart part in parts)
        {
            part.MovePart(position-puzzlePartDistance);
        }
    }
}
