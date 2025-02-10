using System.Data;
using ExamenMVCComics.Models;
using Microsoft.Data.SqlClient;

namespace ExamenMVCComics.Repositories
{
    public class RepositoryComic
    {
        private DataTable tablaComics;
        SqlConnection cn;
        SqlCommand com;
        
        public RepositoryComic()
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=COMICS;Persist Security Info=True;User ID=SA;Trust Server Certificate=True";
            string sql = "select * from COMICS";
            SqlDataAdapter adComic = new SqlDataAdapter(sql, connectionString);
            this.tablaComics = new DataTable();
            adComic.Fill(this.tablaComics);

            
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);       
            }
            return comics;
        }

        public async Task CreateComicAsync(int IdComic, string Nombre, string Imagen, string Descripcion)
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=COMICS;Persist Security Info=True;User ID=SA;Trust Server Certificate=True";

            int maxId = this.tablaComics.AsEnumerable().Max(x => x.Field<int>("IDCOMIC"));
            int nextId = maxId + 1;
            string sql = "insert into COMICS values(@IdComic, @Nombre, @Imagen, @Descripcion)";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = cn;
            this.com.Parameters.AddWithValue("@IdComic", nextId);
            this.com.Parameters.AddWithValue("@Nombre", Nombre);
            this.com.Parameters.AddWithValue("@Imagen", Imagen);
            this.com.Parameters.AddWithValue("@Descripcion", Descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public List<string> GetIdComic()
        {
            var consulta = (from datos in this.tablaComics.AsEnumerable() select datos.Field<string>("NOMBRE"));
            return consulta.ToList();
        }

        public Comic DetalleComic(string Nombre)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() where datos.Field<string>("NOMBRE") == Nombre select datos;
            var row = consulta.First();
            Comic comic = new Comic
            {
                IdComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION"),
            };
            return comic;
        }
    }
}
