namespace ApiUsuario.Util
{
    public interface IUsuarioDatabase
    {
        string UsuarioCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
