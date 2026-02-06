using UnityEngine;

public class Spawn
{
    public float[] RocketTimes { get; private set; }


    public float[] MeteorDirections { get; private set; }
    public Vector2[] VectorMeteorDirections => ToVector2Array(MeteorDirections);



    public Spawn(float[] rocketTimes, float[] meteorDirections)
    {
        // todo: 유효성 검사

        RocketTimes = rocketTimes;
        MeteorDirections = meteorDirections;
    }

    public void SetRocketTimes(float[] rocketTimes)
    {
        // 유효성 감사
        RocketTimes = rocketTimes;
    }
        
    public void SetMeteorDirections(Vector2[] meteorDirections)
    {
        MeteorDirections = ToFloatArray(meteorDirections);
    }
    

    private float[] ToFloatArray(Vector2[] vectors)
    {
        if (vectors == null) return null;

        float[] result = new float[vectors.Length * 2];
        for (int i = 0; i < vectors.Length; i++)
        {
            result[i * 2] = vectors[i].x;
            result[i * 2 + 1] = vectors[i].y;
        }
        return result;
    }

    private Vector2[] ToVector2Array(float[] floats)
    {
        if (floats == null || floats.Length % 2 != 0) return null;

        Vector2[] result = new Vector2[floats.Length / 2];
        for (int i = 0; i < result.Length; i++)
        {
            result[i] = new Vector2(floats[i * 2], floats[i * 2 + 1]);
        }
        return result;
   
    }
}
