namespace BancoArboleda.Models;

public class Cliente
{
    public int Id { get; set; }
    public string Usuario { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string ClaveOnlineHash { get; set; } = "";
    public string NombreCompleto { get; set; } = "";
}

public class Recarga
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string NumeroCelular { get; set; } = "";
    public string Operador { get; set; } = "";
    public decimal Monto { get; set; }
    public string Estado { get; set; } = "COMPLETADO";
    public string Referencia { get; set; } = "";
    public DateTime FechaHora { get; set; } = DateTime.Now;
}

public class LoginRequest
{
    public string Usuario { get; set; } = "";
    public string Password { get; set; } = "";
}

public class RecargaRequest
{
    public string NumeroCelular { get; set; } = "";
    public string Operador { get; set; } = "";
    public decimal Monto { get; set; }
    public string ClaveOnline { get; set; } = "";
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public T? Data { get; set; }
}
