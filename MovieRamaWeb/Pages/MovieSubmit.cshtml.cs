using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieRamaWeb.Data;
using System.ComponentModel.DataAnnotations;

namespace MovieRamaWeb.Pages
{
    public class MovieSubmitModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        
        [Required]
        public string MovieTitle { get; set; }
        
        [Required]
        public string MovieDescription { get; set; }
        
        public MovieSubmitModel(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            if(!_signInManager.IsSignedIn(User))
            {
                return Redirect("/");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            //ModelState.AddModelError("key", "error");
            //if(!ModelState.IsValid)
            //{
            //    return Page();
            //}
            return Redirect("/");
        }
    }
}
