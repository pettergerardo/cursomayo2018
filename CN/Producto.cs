using CD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CN
{
    public class Producto // se agrego public para permitir accesso al Windows form  HomeDepot despues de agregar la referncia
    {
        #region Propiedades
        public int idProducto { get; }
        public string unidadMed { get; }
        public string descripcion { get; }
        public float precioUnit { get; }
        public bool esActivo { get; }
        public DateTime fechaCreacion { get; }
        #endregion

        #region Contructores
        public Producto(int _idProducto, string _unidadMed, string _descripcion,
                        float _precioUnit, bool _esActivo)
        {
            idProducto = _idProducto;
            unidadMed = _unidadMed;
            descripcion = _descripcion;
            precioUnit = _precioUnit;
            esActivo = _esActivo;
        }
        public Producto(DataRow fila)
        {
            idProducto = fila.Field<int>("idProducto");
            unidadMed = fila.Field<string>("unidadMed");
            descripcion = fila.Field<string>("descripcion");
            precioUnit = fila.Field<float>("precioUnit");
            esActivo = fila.Field<bool>("esActivo");
            fechaCreacion = fila.Field<DateTime>("fechaCreacion");
        }


        #endregion

        #region Metodos y Funciones
        public void guardar()
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            
            parametros.Add(new SqlParameter("@unidadMed", idProducto));
            parametros.Add(new SqlParameter("@descripcion", descripcion));
            parametros.Add(new SqlParameter("@precioUnit", precioUnit));
            parametros.Add(new SqlParameter("@esActivo", idProducto));

            

            try
            {
                if (idProducto > 0)
                {
                    //Update
                    parametros.Add(new SqlParameter("@idProducto", idProducto));
                    if (DataBaseHelper.ExecuteNonQuery("dbo.SPUProducto", parametros.ToArray()) == 0)
                    {
                        throw new Exception("No se actualizo el registro");
                    }   
                }
                else
                {
                    //Insert
                    if (DataBaseHelper.ExecuteNonQuery("dbo.SPIProducto", parametros.ToArray()) == 0)
                    {
                        throw new Exception("No se creo el registro");
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw new Exception(ex.Message);
#else
                throw new Exception("Ha ocurrido un error con la base de datos");
#endif
            }
        }
        public static void desactivar(int idProducto,bool esActivo = false)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            parametros.Add(new SqlParameter("@idProducto", idProducto));
            parametros.Add(new SqlParameter("esActivo", esActivo));

            try
            {
                if (DataBaseHelper.ExecuteNonQuery("dbo.SPDProducto", parametros.ToArray()) == 0)
                {
                    throw new Exception("No se desactivo el registro");
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw new Exception(ex.Message);
#else
                throw new Exception("Ha ocurrido un error con la base de datos");
#endif
            }            
        }
        public static Producto buscarPorID(int idProducto)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("@idProducto", idProducto));

            DataTable dt = new DataTable();

            try
            {
                DataBaseHelper.Fill(dt, "dbo.SPSProductos", parametros.ToArray());

                Producto resultado = null;

                foreach (DataRow fila in dt.Rows)
                {
                    resultado = new Producto(fila);
                    break;
                }

                if (resultado == null)
                {
                    throw new Exception("No se han encontrado coincidencias.");
                }

                return resultado;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw new Exception(ex.Message);
#else
                throw new Exception("Ha ocurrido un error con la base de datos");
#endif
            }
        }
        public static List<Producto> traerTodos(bool filtrarSoloActivos = false)
        {
            List<SqlParameter> parametros = new List<SqlParameter>();

            if (filtrarSoloActivos)
            {
                parametros.Add(new SqlParameter("@esActivo", true));
            }
            

            DataTable dt = new DataTable();

            try
            {
                DataBaseHelper.Fill(dt, "dbo.SPSProductos", parametros.ToArray());

                List<Producto> listado = new List<Producto>();

                foreach (DataRow fila in dt.Rows)
                {
                    listado.Add(new Producto(fila));

                }

                return listado;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw new Exception(ex.Message);
#else
                throw new Exception("Ha ocurrido un error con la base de datos");
#endif
            }
        }
#endregion
    }
}
