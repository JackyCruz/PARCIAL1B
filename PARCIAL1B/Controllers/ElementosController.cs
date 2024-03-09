using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PARCIAL1B.Models;
using Microsoft.EntityFrameworkCore;

namespace PARCIAL1B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElementosController : ControllerBase
    {
        private readonly ElementosContext _ElementosContexto;

        public ElementosController(ElementosContext ElementosContexto)
        {
            _ElementosContexto = ElementosContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<Elementos> listadoEquipo = (from e in _ElementosContexto.Elementos
                                           select e).ToList();

            if (listadoEquipo.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }





        /// <summary>
        /// EndPoint que retorna el lisado de todos los equipos existentes 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            Elementos? equipo = (from e in _ElementosContexto.Elementos
                                 where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }




        /// <summary>
        /// EndPoint que retorna los registros de todos los equipos existentes 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult FindByDescription(string filtro)
        {
            Elementos? equipo = (from e in _ElementosContexto.Elementos
                                 where e.descripcion.Contains(filtro)
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }




        /// <summary>
        /// EndPoint que crea los registros de todos los equipos existentes 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]

        public IActionResult GuadarEquipo([FromBody] Elementos equipo)
        {

            try
            {

                _ElementosContexto.Elementos.Add(equipo);
                _ElementosContexto.SaveChanges();
                return Ok(equipo);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }








        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] Elementos equipoModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            Elementos? ElementosActual = (from e in _ElementosContexto.Elementos
                                     where e.id_equipos == id
                                     select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (equipoActual == null)
            { return NotFound(); }

            //Si se encuentra el registro, se alteran los campos modificables 
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos

            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;
            _equiposContexto.SaveChanges();

            return Ok(equipoModificar);
        }










        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            equipos? equipo = (from e in _equiposContexto.equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();


                _equiposContexto.equipos.Attach(equipo);
                _equiposContexto.equipos.Remove(equipo);
                _equiposContexto.SaveChanges();

            }
            return Ok(equipo);
        }
    }
}
