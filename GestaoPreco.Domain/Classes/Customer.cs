using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Customer : Entity
    {
        [Required(ErrorMessage = "O nome � obrigat�rio.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no m�ximo 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail � obrigat�rio.")]
        [EmailAddress(ErrorMessage = "O e-mail informado n�o � v�lido.")]
        [StringLength(100, ErrorMessage = "O e-mail deve ter no m�ximo 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone � obrigat�rio.")]
        [Phone(ErrorMessage = "O telefone informado n�o � v�lido.")]
        [StringLength(20, ErrorMessage = "O telefone deve ter no m�ximo 20 caracteres.")]
        public string Phone { get; set; } = string.Empty;
    }
}