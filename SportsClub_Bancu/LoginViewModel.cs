using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required(ErrorMessage = "Введите почту")]
    [EmailAddress(ErrorMessage = "Некорректный адресс электронной почты")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; }
}
