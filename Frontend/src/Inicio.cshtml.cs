using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Frontend.src
{
    public class Inicio : PageModel
    {
        private readonly ILogger<Inicio> _logger;

        public Inicio(ILogger<Inicio> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}