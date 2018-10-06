namespace ImageRenamer
{
    public interface IMover
    {
        void Move(string from, string to);
    }

    public class Mover : IMover
    {
        public void Move(string from, string to)
        {
            System.IO.File.Move(from, to);
        }
    }

    public class MoverMock : IMover
    {
        public string from { get; set; }
        public string to { get; set; }

        public void Move(string from, string to)
        {
            this.from = from;
            this.to = to;
        }
    }
}