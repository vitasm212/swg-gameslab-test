namespace Control.Core
{
    public struct Ball
    {
        public DeleteFlag deleteFlag;
        public int typeBall;
        public int group;
        public BallView ballView;
        public CellView cellView;
    }

    public enum DeleteFlag
    {
        None = 0,
        Preparation = 1,
        Delete = 2,
    }
}