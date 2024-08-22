namespace Application.Exeptions;

public class TextException : Exception
{
    public TextException() : base("Текст не может быть больше 128 символов!")
    {
    }
}