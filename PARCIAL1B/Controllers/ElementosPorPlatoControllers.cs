using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL1B.Models;

namespace PARCIAL1B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElementosPorPlatoControllers : ControllerBase
    {
        private readonly ElementosContext _ElementosPorPlatoContexto;

        public ElementosPorPlatoControllers(ElementosContext ElementosContexto)
        {
            _ElementosPorPlatoContexto = ElementosContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<ElementosPorPlato> listadoElementosPorPlato = (from ep in _ElementosPorPlatoContexto.ElementosPorPlato
                                                                select ep).ToList();

            if (listadoElementosPorPlato.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoElementosPorPlato);
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
            ElementosPorPlato? ElementosPorPlato = (from ep in _ElementosPorPlatoContexto.ElementosPorPlato
                                                    where ep.ElementoPorPlatoID == id
                                 select ep).FirstOrDefault();

            if (ElementosPorPlato == null)
            {
                return NotFound();
            }
            return Ok(ElementosPorPlato);
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
            ElementosPorPlato? ElementosPorPlato = (from ep in _ElementosPorPlatoContexto.ElementosPorPlato
                                                    where ep.Estado.Contains(filtro)
                                 select ep).FirstOrDefault();

            if (ElementosPorPlato == null)
            {
                return NotFound();
            }
            return Ok(ElementosPorPlato);
        }




        /// <summary>
        /// EndPoint que crea los registros de todos los equipos existentes 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]

        public IActionResult GuadarEquipo([FromBody] ElementosPorPlato ElementosPorPlato)
        {

            try
            {

                _ElementosPorPlatoContexto.ElementosPorPlato.Add(ElementosPorPlato);
                _ElementosPorPlatoContexto.SaveChanges();
                return Ok(ElementosPorPlato);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }



        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] ElementosPorPlato ElementosPorPlatoModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            ElementosPorPlato? ElementosPorPlatoActual = (from ep in _ElementosPorPlatoContexto.ElementosPorPlato
                                          where ep.ElementoPorPlatoID == id
                                          select ep).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (ElementosPorPlatoActual == null)
            { return NotFound(); }

            //Si se encuentra el registro, se alteran los campos modificables 
            ElementosPorPlatoActual.ElementoPorPlatoID = ElementosPorPlatoModificar.ElementoPorPlatoID;
            ElementosPorPlatoActual.EmpresaID = ElementosPorPlatoModificar.EmpresaID;
            ElementosPorPlatoActual.PlatoID = ElementosPorPlatoModificar.PlatoID;
            ElementosPorPlatoActual.ElementoID = ElementosPorPlatoModificar.ElementoID;
            ElementosPorPlatoActual.Cantidad = ElementosPorPlatoModificar.Cantidad;
            ElementosPorPlatoActual.Estado = ElementosPorPlatoModificar.Estado;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos

            _ElementosPorPlatoContexto.Entry(ElementosPorPlatoActual).State = EntityState.Modified;
            _ElementosPorPlatoContexto.SaveChanges();

            return Ok(ElementosPorPlatoModificar);
        }






        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            ElementosPorPlato? ElementosPorPlato = (from ep in _ElementosPorPlatoContexto.ElementosPorPlato
                                 where ep.ElementoPorPlatoID == id
                                 select ep).FirstOrDefault();

            if (ElementosPorPlato == null)
            {
                return NotFound();


                _ElementosPorPlatoContexto.ElementosPorPlato.Attach(ElementosPorPlato);
                _ElementosPorPlatoContexto.ElementosPorPlato.Remove(ElementosPorPlato);
                _ElementosPorPlatoContexto.SaveChanges();

            }
            return Ok(ElementosPorPlato);
        }
    }
}
