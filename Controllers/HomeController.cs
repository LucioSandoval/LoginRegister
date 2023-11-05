using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LoginRegistration.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginRegistration.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

        [HttpPost]
        [Route("/register")]
        /* [ValidateAntiForgeryToken] */
        public async Task<IActionResult> Register(User newUser)
        {
            Console.WriteLine("legueeeeeeeeeeeeeeeeeeeee");
            Console.WriteLine("ModelState.IsValid: " + ModelState.IsValid);
            if (ModelState.IsValid)
            {   
                Console.WriteLine("entre validacion");
                // Verificar si el correo electrónico ya está en uso
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Este correo electrónico ya está en uso.");
                    return View(newUser);
                } 
                
                newUser.CreatedAt = DateTime.Now;
                newUser.UpdatedAt = DateTime.Now;

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                Console.WriteLine("Usuario creado");
                return View("Login");
            }

            
            return View("Index");
        }

        // Procesar el inicio de sesión
        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el usuario existe en la base de datos
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);

                if (user == null)
                {
                    ModelState.AddModelError("Email", "Correo electrónico o contraseña incorrectos.");
                    return View("Login");
                }

                // Verificar la contraseña
                if (loginUser.Password != user.Password)
                {
                    ModelState.AddModelError("Password", "Correo electrónico o contraseña incorrectos.");
                    return View("Login");
                }

                // Iniciar sesión (puedes implementar tu propia lógica de autenticación aquí)

                // Redirigir al usuario autenticado a su página principal
                return RedirectToAction("Privacy");
            }

            return View("Login");
        }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
