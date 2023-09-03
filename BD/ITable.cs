namespace BuildMaterials.BD
{
    public interface ITable
    {
        public int ID { get; set; }
        public bool IsValid { get; }
    }
}