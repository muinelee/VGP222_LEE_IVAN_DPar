using UnityEngine;

public class RepeatingBackground : MonoBehaviour
{
    public float verticalSize;

    // Update is called once per frame
    private void Update()
    {
        if (transform.position.y < -verticalSize)
        {
            RepeatBackground();
        }
    }

    private void RepeatBackground()
    {
        Vector2 offSet = new Vector2(0, verticalSize * 2f);
        transform.position = (Vector2)transform.position + offSet;
    }
}
