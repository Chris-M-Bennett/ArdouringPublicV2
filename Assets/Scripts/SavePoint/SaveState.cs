using UnityEngine;

public class SaveState
{ 
    public Coords playerPos { get; set; }
}

public class Coords
{
    public Coords(float x, float y)
    {
        this.X = x;
        this.Y = y;
    }
    public float X { get; set; }
    public float Y { get; set; }

}