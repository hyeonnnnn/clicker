public class WeaponState
{
    public float[] RocketLaunchTimes { get; private set; }
    public int MeteorCount { get; private set; }

    public WeaponState(float[] rocketTimes, int meteorCount)
    {
        RocketLaunchTimes = rocketTimes;
        MeteorCount = meteorCount;
    }

    public void SetRocketLaunchTimes(float[] rocketLaunchTimes)
    {
        RocketLaunchTimes = rocketLaunchTimes;
    }

    public void SetMeteorCount(int meteorCount)
    {
        MeteorCount = meteorCount;
    }
}
