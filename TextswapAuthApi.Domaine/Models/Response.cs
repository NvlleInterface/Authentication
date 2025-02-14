
namespace TextswapAuthApi.Domaine.Models;

public static class Response
{
    public static Response<T> Fail<T>(IEnumerable<string> message, int statutCode, string titre, T data = default) =>
    new Response<T>(data, message, true, statutCode, titre);

    public static Response<T> Fail<T>(string message, int statutCode, string titre, T data = default) =>
        new Response<T>(data, message, true, statutCode, titre);

    public static Response<T> Ok<T>(T data, IEnumerable<string> message, int statutCode, string titre) =>
        new Response<T>(data, message, false, statutCode, titre);

    public static Response<T> Ok<T>(T data, string message, int statutCode, string titre) =>
        new Response<T>(data, message, false, statutCode, titre);
}

public class Response<T>
{
    public T Data { get; set; }

    public IEnumerable<string> Message { get; set; }

    public bool Error { get; set; }

    public int StatusCode { get; set; }

    public string Titre { get; set; }

    public Response(T data, string msg, bool error, int statusCode, string titre) : this(data, new List<string>() { msg }, error, statusCode, titre)
    {
    }

    public Response(T data, IEnumerable<string> msg, bool error, int statusCode, string titre)
    {
        Data = data;
        Message = msg;
        Error = error;
        StatusCode = statusCode;
        Titre = titre;
    }

}
