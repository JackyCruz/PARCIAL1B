using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL1B.Models;
using Microsoft.Extensions.Hosting;

namespace PARCIAL1B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatosController : ControllerBase
    {
        private readonly ElementosContext _PlatosContexto;

        public PlatosController(ElementosContext PlatosContexto)
        {
            _PlatosContexto = PlatosContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<Platos> listadoPlatos = (from platos in _PlatosContexto.Platos
                                          select platos).ToList();

            if (listadoPlatos.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoPlatos);
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
            Platos? Platos = (from platos in _PlatosContexto.Platos
                              where platos.PlatosID == id
                              select platos).FirstOrDefault();

            if (Platos == null)
            {
                return NotFound();
            }
            return Ok(Platos);
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
            Platos? Platos = (from platos in _PlatosContexto.Platos
                              where platos.DescripcionPlato.Contains(filtro)
                              select platos).FirstOrDefault();

            if (Platos == null)
            {
                return NotFound();
            }
            return Ok(Platos);
        }




        /// <summary>
        /// EndPoint que crea los registros de todos los equipos existentes 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]

        public IActionResult GuadarEquipo([FromBody] Platos Platos)
        {

            try
            {

                _PlatosContexto.Platos.Add(Platos);
                _PlatosContexto.SaveChanges();
                return Ok(Platos);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }








        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] Platos PlatosModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            Platos? PlatosActual = (from platos in _PlatosContexto.Platos where platos.PlatosID == id select platos).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (PlatosActual == null)
            { return NotFound(); }

            //Si se encuentra el registro, se alteran los campos modificables 
            PlatosActual.PlatosID = PlatosModificar.PlatosID;
            PlatosActual.EmpresaID = PlatosModificar.EmpresaID;
            PlatosActual.GrupoID = PlatosModificar.GrupoID;
            PlatosActual.NombrePlato = PlatosModificar.NombrePlato;
            PlatosActual.DescripcionPlato = PlatosModificar.DescripcionPlato;
            PlatosActual.Precio = PlatosModificar.Precio;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos

            _PlatosContexto.Entry(PlatosActual).State = EntityState.Modified;
            _PlatosContexto.SaveChanges();

            return Ok(PlatosModificar);
        }


        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            Platos? Platos = (from platos in _PlatosContexto.Platos
                              where platos.PlatosID == id
                              select platos).FirstOrDefault();

            if (Platos == null)
            {
                return NotFound();


                _PlatosContexto.Platos.Attach(Platos);
                _PlatosContexto.Platos.Remove(Platos);
                _PlatosContexto.SaveChanges();

            }
            return Ok(Platos);
        }
    }
}
