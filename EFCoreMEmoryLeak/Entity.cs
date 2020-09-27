namespace EFCoreMEmoryLeak
{
    public sealed class Entity
    {
        private int[] _data = new int[100_000_000];

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
