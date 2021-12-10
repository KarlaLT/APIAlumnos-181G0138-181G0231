using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using SitioWebProfesora.Models;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Net;

namespace SitioWebProfesora.Controllers
{
    public class AlumnosController : Controller
    {
        public IHttpClientFactory Factory { get; }
        public AlumnosController(IHttpClientFactory factory)
        {
            Factory = factory;
        }
        [ActionName("Index")]
        public async Task<IActionResult> Index()
        {
            List<Alumno> alumnos = new List<Alumno>();
            HttpClient client = Factory.CreateClient("alumnos");
            var result = await client.GetAsync("api/profesora");

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                alumnos = JsonConvert.DeserializeObject<List<Alumno>>(json);
            }
            return View("Index", alumnos);
        }
        [ActionName("AddNew")]
        public IActionResult Add()
        {
            return View("NuevoAlumno");
        }
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(Alumno alumno)
        {
            HttpClient client = Factory.CreateClient("alumnos");
            if (alumno != null)
            {
                if (string.IsNullOrWhiteSpace(alumno.Nombre))
                    ModelState.AddModelError("", "Ingrese el nombre del alumno.");
                if (string.IsNullOrWhiteSpace(alumno.Apellido))
                    ModelState.AddModelError("", "Ingrese los apellidos del alumno.");
                if (string.IsNullOrWhiteSpace(alumno.Usuario))
                    ModelState.AddModelError("", "Ingrese los apellidos del alumno.");
                if (string.IsNullOrWhiteSpace(alumno.Password))
                    ModelState.AddModelError("", "Ingrese los apellidos del alumno.");

                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(alumno);
                    var result = await client.PostAsync("api/profesora",
                           new StringContent(json, Encoding.UTF8, "application/json"));

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errores = await result.Content.ReadAsStringAsync();
                        errores = errores.Replace("\"\"", "Mensajes");
                        Errores lsterror = JsonConvert.DeserializeObject<Errores>(errores);
                        lsterror.Mensajes.ForEach(x => ModelState.AddModelError("", x));

                        return View("NuevoAlumno", alumno);
                    }
                    else
                        return View("NuevoAlumno", alumno);
                }
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            HttpClient client = Factory.CreateClient("alumnos");
            var result = await client.GetAsync("api/profesora/" + id);

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
              Alumno  alumno = JsonConvert.DeserializeObject<Alumno>(json);
                return View("EditarAlumno", alumno);
            }
            else if (result.StatusCode == HttpStatusCode.NotFound)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Ha ocurrido un error: Código: " + result.StatusCode);

                if (result.Content.Headers.ContentLength > 0)
                {
                    ModelState.AddModelError("", await result.Content.ReadAsStringAsync());
                }

                return RedirectToAction("Index");

            }
        }

        [ActionName("Edit2")]
        [HttpPost]
        public async Task<IActionResult> Edit(Alumno alumno, int id)
        {
            alumno.IdAlumno = id;
            HttpClient client = Factory.CreateClient("alumnos");

            if (alumno != null && ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(alumno);
                var result = await client.PutAsync("api/profesora", new StringContent(json, Encoding.UTF8, "application/json"));

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                else if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errores = await result.Content.ReadAsStringAsync();
                    errores = errores.Replace("\"\"", "Mensajes");
                    Errores lsterror = JsonConvert.DeserializeObject<Errores>(errores);
                    lsterror.Mensajes.ForEach(x => ModelState.AddModelError("", x));

                    return View("EditarAlumno", alumno);
                }
                else
                {
                    ModelState.AddModelError("", "Ha ocurrido un error: Código: " + result.StatusCode);

                    if (result.Content.Headers.ContentLength > 0)
                    {
                        ModelState.AddModelError("", await result.Content.ReadAsStringAsync());
                    }

                    return View("EditarAlumno", alumno);
                }
            }

            return View("EditarAlumno", alumno);
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = Factory.CreateClient("alumnos");
            var result = await client.GetAsync("api/profesora/" + id);

            if (result.IsSuccessStatusCode)
            {
                var json = await result.Content.ReadAsStringAsync();
                Alumno alum = JsonConvert.DeserializeObject<Alumno>(json);
                return View("EliminarAlumno", alum);
            }
            else
            {
                ModelState.AddModelError("", "Ha ocurrido un error: Código: " + result.StatusCode);

                if (result.Content.Headers.ContentLength > 0)
                {
                    ModelState.AddModelError("", await result.Content.ReadAsStringAsync());
                }

                return View("Index");
            }
        }
        [ActionName("Delete2")]
        [HttpPost]
        public async Task<IActionResult> Delete2(int id)
        {
            HttpClient client = Factory.CreateClient("alumnos");
            var result = await client.DeleteAsync("api/profesora/" + id);

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            else if (result.StatusCode == HttpStatusCode.BadRequest)
            {
                var errores = await result.Content.ReadAsStringAsync();
                errores = errores.Replace("\"\"", "Mensajes");
                Errores lsterror = JsonConvert.DeserializeObject<Errores>(errores);
                lsterror.Mensajes.ForEach(x => ModelState.AddModelError("", x));

                return View("EliminarAlumno");
            }
            else
            {
                ModelState.AddModelError("", "Ha ocurrido un error: Código: " + result.StatusCode);

                if (result.Content.Headers.ContentLength > 0)
                {
                    ModelState.AddModelError("", await result.Content.ReadAsStringAsync());
                }

                return View("EliminarAlumno");
            }
        }
    }
}
