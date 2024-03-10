using System.Collections.Generic;
using UnityEngine;

public class UnitGroupMovement : MonoBehaviour
{
    private static UnitGroupMovement _instance;
    public static UnitGroupMovement Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void MoveSelectedUnits(float targetX)
    {
        List<GameObject> selectedUnits = UnitSelections.Instance.unitsSelected;

        if (selectedUnits.Count == 0)
            return;

        float minX = Mathf.Infinity;
        float maxX = Mathf.NegativeInfinity;

        foreach (GameObject unit in selectedUnits)
        {
            float unitX = unit.transform.position.x;
            minX = Mathf.Min(minX, unitX);
            maxX = Mathf.Max(maxX, unitX);
        }

        float groupWidth = maxX - minX;
        float groupCenter = (minX + maxX) / 2f;
        float offset = targetX - groupCenter;

        foreach (GameObject unit in selectedUnits)
        {
            float unitX = unit.transform.position.x;
            float relativeX = unitX - groupCenter;
            float newX = targetX + relativeX;

            Movement movement = unit.GetComponent<Movement>();
            if (movement != null)
            {
                Vector2 newPosition = new Vector2(newX, unit.transform.position.y);
                movement.MoveToPoint(newPosition, false);
            }
        }
    }
}