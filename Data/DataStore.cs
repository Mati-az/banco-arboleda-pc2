using System.Text.Json;
using BancoArboleda.Models;

namespace BancoArboleda.Data;

/// <summary>
/// Almacenamiento en memoria con persistencia en archivo JSON.
/// No requiere base de datos externa. Datos listos al iniciar.
/// </summary>
public class DataStore
{
    private static readonly string _dataPath = Path.Combine(AppContext.BaseDirectory, "Data", "recargas.json");
    private static readonly object _lock = new();

    public List<Cliente> Clientes { get; private set; } = new();
    public List<Recarga> Recargas { get; private set; } = new();
    private int _nextRecargaId = 1;

    public DataStore()
    {
        SeedClientes();
        LoadRecargas();
    }

    // ── Clientes de prueba (hardcoded, no requiere BD) ────────────────────────
    private void SeedClientes()
    {
        Clientes = new List<Cliente>
        {
            new Cliente
            {
                Id = 1,
                Usuario = "cliente01",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("clave123"),
                ClaveOnlineHash = BCrypt.Net.BCrypt.HashPassword("online456"),
                NombreCompleto = "Juan Pérez López"
            }
        };
    }

    // ── Recargas: persiste en archivo JSON ────────────────────────────────────
    private void LoadRecargas()
    {
        try
        {
            var dir = Path.GetDirectoryName(_dataPath)!;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (File.Exists(_dataPath))
            {
                var json = File.ReadAllText(_dataPath);
                Recargas = JsonSerializer.Deserialize<List<Recarga>>(json) ?? new();
                _nextRecargaId = Recargas.Count > 0 ? Recargas.Max(r => r.Id) + 1 : 1;
            }
        }
        catch { Recargas = new(); }
    }

    public void SaveRecarga(Recarga recarga)
    {
        lock (_lock)
        {
            recarga.Id = _nextRecargaId++;
            Recargas.Add(recarga);
            try
            {
                var dir = Path.GetDirectoryName(_dataPath)!;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(_dataPath, JsonSerializer.Serialize(Recargas, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch { /* continúa aunque no pueda guardar */ }
        }
    }

    public bool ExisteRecargaReciente(int clienteId, string numero, string operador, decimal monto)
    {
        lock (_lock)
        {
            return Recargas.Any(r =>
                r.ClienteId == clienteId &&
                r.NumeroCelular == numero &&
                r.Operador == operador &&
                r.Monto == monto &&
                (DateTime.Now - r.FechaHora).TotalSeconds < 10);
        }
    }

    public static string GenerarReferencia()
    {
        return $"REC-{DateTime.Now:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
    }
}
