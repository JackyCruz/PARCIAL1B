using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using PARCIAL1B.Models;

namespace PARCIAL1B.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiltrosController : ControllerBase
    {
        private readonly ElementosContext _pContex;

        public FiltrosController(ElementosContext pContexto)
        {
            _pContex = pContexto;
        }

        //filtro por el nombre del plato
        [HttpGet]
        [Route("FiltrarNombrePlato/{nombrePlato}")]

        public IActionResult FiltrarPorPlato(string nombrePlato)
        {
            try
            {
                Elementos elem = (from e in _pContex.Elementos
                                  join epp in _pContex.ElementosPorPlato
                                            on e.ElementoID equals epp.ElementoID
                                  join p in _pContex.Platos
                                            on epp.PlatoID equals p.PlatosID
                                  select e).FirstOrDefault();
                if (elem == null)
                {
                    return NotFound();
                }
                return Ok(elem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al procesar la solicitud");
            }
        }

        //filtro por el nombre del elemento
        [HttpGet]
        [Route("FiltrarNombreElemento/{nombreElemento}")]

        public IActionResult FiltrarPorElemento(string nombreElemento)
        {
            try
            {
                Platos plato = (from p in _pContex.Platos
                                join epp in _pContex.ElementosPorPlato
                                          on p.PlatosID equals epp.PlatoID
                                join el in _pContex.Elementos
                                          on epp.ElementoID equals el.ElementoID
                                select p).FirstOrDefault();
                if (plato == null)
                {
                    return NotFound();
                }
                return Ok(plato);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al procesar la solicitud");
            }
        }

        //Listado de platos y elemento
        [HttpGet]
        [Route("ListadoPlatosElementos/{idCombo}")]
        public IActionResult ListadoPlatosElementos(int idCombo)
        {
            try
            {
                var platosConElementos = (from pcp in _pContex.PlatosPorCombo
                                          join p in _pContex.Platos on pcp.PlatoID equals p.PlatosID
                                          join epp in _pContex.ElementosPorPlato on p.PlatosID equals epp.PlatoID
                                          join el in _pContex.Elementos on epp.ElementoID equals el.ElementoID
                                          where pcp.ComboID == idCombo
                                          select new
                                          {
                                              PlatoID = p.PlatosID,
                                              p.EmpresaID,
                                              p.GrupoID,
                                              p.NombrePlato,
                                              Elemento = el.Elemento,
                                              p.DescripcionPlato,
                                              p.Precio
                                          }).ToList();

                if (platosConElementos.Count() == 0)
                {
                    return NotFound("No se encontraron platos para el combo especificado.");
                }

                return Ok(platosConElementos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al procesar la solicitud");
            }
        }
        //Fin del id del combo

        //Listado de platos que no tienen asignado elemento
        [HttpGet]
        [Route("ListadoPlatosSinElementos")]
        public IActionResult ListadoPlatosSinElementos()
        {
            try
            {
                var platosSinElementos = (from p in _pContex.Platos
                                          join epp in _pContex.ElementosPorPlato
                                               on p.PlatosID equals epp.PlatoID into platosConElementos
                                          where platosConElementos.Count() == 0
                                          select new
                                          {
                                              p.PlatosID,
                                              p.EmpresaID,
                                              p.GrupoID,
                                              p.NombrePlato,
                                              p.DescripcionPlato,
                                              p.Precio
                                          }).ToList();

                if (platosSinElementos.Count() == 0)
                {
                    return NotFound("No se encontraron platos sin elementos asignados.");
                }

                return Ok(platosSinElementos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al procesar la solicitud");
            }
        }


    }
}