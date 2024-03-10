using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PARCIAL1B.Models;


namespace PARCIAL1B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatosPorComboController : ControllerBase
    {
        private readonly ElementosContext _PlatosPorComboContexto;

        public PlatosPorComboController(ElementosContext PlatosPorComboContexto)
        {
            _PlatosPorComboContexto = PlatosPorComboContexto;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<PlatosPorCombo> listadoPlatosPorCombo = (from p in _PlatosPorComboContexto.PlatosPorCombo
                                                          select p).ToList();

            if (listadoPlatosPorCombo.Count() == 0)
            {
                return NotFound();
            }
            return Ok(listadoPlatosPorCombo);
        }



        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            PlatosPorCombo? PlatosPorCombo = (from e in _PlatosPorComboContexto.PlatosPorCombo
                                              where e.PlatosPorComboID == id
                                 select e).FirstOrDefault();

            if (PlatosPorCombo == null)
            {
                return NotFound();
            }
            return Ok(PlatosPorCombo);
        }


        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult FindByDescription(string filtro)
        {
            PlatosPorCombo? PlatosPorCombo = (from p in _PlatosPorComboContexto.PlatosPorCombo
                                              where p.Estado.Contains(filtro)
                                              select p).FirstOrDefault();


            if (PlatosPorCombo == null)
            {
                return NotFound();
            }
            return Ok(PlatosPorCombo);
        }




        /// <summary>
        /// EndPoint que crea los registros de todos los equipos existentes 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Add")]

        public IActionResult GuadarEquipo([FromBody] PlatosPorCombo PlatosPorCombo)
        {

            try
            {

                _PlatosPorComboContexto.PlatosPorCombo.Add(PlatosPorCombo);
                _PlatosPorComboContexto.SaveChanges();
                return Ok(PlatosPorCombo);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }


        }


        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] PlatosPorCombo PlatosPorComboModificar)
        {
            //Para actualizar un registro, se obtiene el registro original de la base de datos
            //al cual alteraremos alguna propiedad
            PlatosPorCombo? PlatosPorComboActual = (from p in _PlatosPorComboContexto.PlatosPorCombo
                                          where p.PlatosPorComboID == id
                                          select p).FirstOrDefault();

            //Verificamos que exista el registro segun su ID
            if (PlatosPorComboActual == null)
            { return NotFound(); }

            //Si se encuentra el registro, se alteran los campos modificables 
            PlatosPorComboActual.PlatosPorComboID = PlatosPorComboModificar.PlatosPorComboID;
            PlatosPorComboActual.EmpresaID = PlatosPorComboModificar.EmpresaID;
            PlatosPorComboActual.ComboID = PlatosPorComboModificar.ComboID;
            PlatosPorComboActual.PlatoID = PlatosPorComboModificar.PlatoID;
            PlatosPorComboActual.Estado = PlatosPorComboModificar.Estado;

            //Se marca el registro como modificado en el contexto
            //y se envia la modificacion a la base de datos

            _PlatosPorComboContexto.Entry(PlatosPorComboActual).State = EntityState.Modified;
            _PlatosPorComboContexto.SaveChanges();

            return Ok(PlatosPorComboModificar);
        }






        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarEquipo(int id)
        {
            PlatosPorCombo? PlatosPorCombo = (from e in _PlatosPorComboContexto.PlatosPorCombo
                                              where e.PlatosPorComboID == id
                                 select e).FirstOrDefault();

            if (PlatosPorCombo == null)
            {
                return NotFound();


                _PlatosPorComboContexto.PlatosPorCombo.Attach(PlatosPorCombo);
                _PlatosPorComboContexto.PlatosPorCombo.Remove(PlatosPorCombo);
                _PlatosPorComboContexto.SaveChanges();

            }
            return Ok(PlatosPorCombo);
        }
    }
}
