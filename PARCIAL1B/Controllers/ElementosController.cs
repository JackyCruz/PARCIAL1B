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
            List<Elementos> listadoElementos = (from e in _ElementosContexto.Elementos
                                           select e).ToList();

            if (listadoElementos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoElementos);
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
            Elementos? Elementos = (from e in _ElementosContexto.Elementos
                                 where e.ElementoID == id
                               select e).FirstOrDefault();

            if (Elementos == null)
            {
                return NotFound();
            }
            return Ok(Elementos);
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
            Elementos? Elementos = (from e in _ElementosContexto.Elementos
                                 where e.Elemento.Contains(filtro)
                               select e).FirstOrDefault();

            if (Elementos == null)
            {
                return NotFound();
            }
            return Ok(Elementos);
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
        public IActionResult ActualizarEquipo(int id, [FromBody] Elementos ElementosModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            Elementos? ElementosActual = (from e in _ElementosContexto.Elementos
                                     where e.ElementoID == id
                                     select e).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (ElementosActual == null)
            { return NotFound(); }

            //Si se encuentra el registro, se alteran los campos modificables 
            ElementosActual.ElementoID = ElementosModificar.ElementoID;
            ElementosActual.EmpresaID = ElementosModificar.EmpresaID;
            ElementosActual.Elemento = ElementosModificar.Elemento;
            ElementosActual.CantidadMinima = ElementosModificar.CantidadMinima;
            ElementosActual.UnidadMedida = ElementosModificar.UnidadMedida;
            ElementosActual.Estado = ElementosModificar.Estado;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos

            _ElementosContexto.Entry(ElementosActual).State = EntityState.Modified;
            _ElementosContexto.SaveChanges();

            return Ok(ElementosModificar);
        }






        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            Elementos? Elementos = (from e in _ElementosContexto.Elementos
                                 where e.ElementoID == id
                               select e).FirstOrDefault();

            if (Elementos == null)
            {
                return NotFound();


                _ElementosContexto.Elementos.Attach(Elementos);
                _ElementosContexto.Elementos.Remove(Elementos);
                _ElementosContexto.SaveChanges();

            }
            return Ok(Elementos);
        }
    }
}
