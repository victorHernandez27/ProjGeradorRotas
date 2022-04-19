namespace ApiEquipe.Util
{
    public interface IEquipeDatabase
    {
        string EquipeCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
