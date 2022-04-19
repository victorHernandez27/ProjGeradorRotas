namespace ApiEquipe.Util
{
    public class EquipeDatabase : IEquipeDatabase
    {
        public string EquipeCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
