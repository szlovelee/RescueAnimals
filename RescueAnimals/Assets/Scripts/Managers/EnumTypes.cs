namespace EnumTypes
{
    public enum AnimalType
    {
        None = 0,
        Beagle,
        Dragon,
        Panda,
        Retreiver,
        SheepDog,
        StaffyDog
    }

    public enum BallType
    {
        None = 0, d, c, g
    }

    public enum StageType{
        None = 1 << 0,
        Mountain = 1 << 1,
        Sea = 1 << 2,
        Jungle = 1 << 3,
        Hell = 1 << 4,
        Heaven = 1 << 5
    }

    public enum BlockType
    {
        Leaf,
        Wood,
        Cement,
        Iron,
        Diamond
    }

    public enum BlockPattern
    {
        Normal,
        Heart,
    }
}