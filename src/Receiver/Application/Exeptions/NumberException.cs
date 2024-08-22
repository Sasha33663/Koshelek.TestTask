namespace Application.Exeptions;
internal class NumberException : Exception
{

    public NumberException() : base("Уже существует сообщение с таким порядковым номером")
    {
    }

}