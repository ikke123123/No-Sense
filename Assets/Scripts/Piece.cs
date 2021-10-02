using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Piece : MonoBehaviour
{
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private float speed = 0.2f;

    public Vector2Int Position => location.gridLocation;

    [HideInInspector] public Location location;
    [HideInInspector] public bool isSpecialPiece = false;
    [HideInInspector] public Team team;

    AudioSource sound;

    private void OnEnable()
    {
        sound = GetComponent<AudioSource>();
    }

    public IEnumerator Move(Vector3 targetPos)
    {
        while (transform.position != targetPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
            yield return new WaitForFixedUpdate();
        }
    }

    public bool MoveTo(Location target)
    {
        sound.Play();
        if (target.IsOccupied) 
            return false; 
        location.LeaveLocation();
        target.OccupyLocation(this);
        StartCoroutine(Move(target.Position));
        location = target;
        return true;
    }

    public void MakeSpecialPiece()
    {
        isSpecialPiece = true;
        GameObject specialPart = Instantiate(prefab, transform);
        specialPart.transform.localPosition = new Vector3(0,0.3f,0);
        Destroy(specialPart.GetComponent<Piece>());
    }
}
