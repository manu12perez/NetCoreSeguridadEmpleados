using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryHospital repo;

        public EmpleadosController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        [AuthorizeEmpleado]
        public async Task<IActionResult> Details (int id)
        {
            Empleado empleado = await this.repo.FindEmpleadoAsync(id);
            return View(empleado);
        }

        [AuthorizeEmpleado]
        public IActionResult PerfilEmpleado()
        {
            return View();
        }

        [AuthorizeEmpleado(Policy = "SOLOJEFES")]
        public async Task<IActionResult> Compis()
        {
            //RECUPERAMOS EL DATO DEL CLAIM DE Departamento
            string dato =
                HttpContext.User.FindFirst("Departamento").Value;
            int idDepartamento = int.Parse(dato);

            List<Empleado> empleados =
                await this.repo.GetEmpleadosDepartamentoAsync(idDepartamento);

            return View(empleados);
        }

        [AuthorizeEmpleado(Policy = "SOLOJEFES")]
        [HttpPost]
        public async Task<IActionResult> Compis(int incremento)
        {
            //DEBEMOS RECUPERAR EL ID DEL DEPARTAMENTO DEL
            //USUARIO QUE SE HA VALIDADO
            string dato = HttpContext.User.FindFirst("Departamento").Value;
            int idDepartamento = int.Parse(dato);

            await this.repo.UpdateSalarioEmpleadosAsync(idDepartamento, incremento);

            List<Empleado> empleados = await this.repo.GetEmpleadosDepartamentoAsync(idDepartamento);

            return View(empleados);
        }

        [AuthorizeEmpleado(Policy = "AdminOnly")]
        public IActionResult AdminEmpleados()
        {
            return View();
        }

        [AuthorizeEmpleado(Policy = "SoloRicos")]
        public IActionResult ZonaNoble()
        {
            return View();
        }
    }
}
