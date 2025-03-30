using UnityEngine;

public static class CONSTANTS
{
    public static readonly Vector3 CAMERA_ROTATION = new Vector3(30, 45, 0);

    public static class Layers
    {
        public static readonly LayerMask Terrain = LayerMask.GetMask(nameof(Terrain));
        public static readonly LayerMask Factory = LayerMask.GetMask(nameof(Factory));
    }
}