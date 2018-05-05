using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CN
{
    public class Empleado : Persona //herencia clase hijo : clase padre se utiliza para reusar codigo
    {
        #region Propiedades
        public string usuario { get; set;}
        public int idEmpleado { get; set;}
        #endregion

        #region Constructores
        public Empleado(string _usuario, int _idEmpleado, string _nombreCompleto, string _direccion, DateTime _fechaNac) 
            : base(_nombreCompleto, _direccion, _fechaNac)//con base se puede acceder a las funciones del padre
        {
            this.usuario = _usuario;
            this.idEmpleado = _idEmpleado;
            #endregion
        }
        #region Metodos y Funciones 
        public override string descripcion() // escribir override descripcion tab tab... se utilizara para sobreescribir la descripcion en la clase padre
        {
            string result = String.Format("usuario:{0} - idEmpleado:{1} - {2}", usuario, idEmpleado, base.descripcion()); 
            return result;
            #endregion
        }
    }
}
