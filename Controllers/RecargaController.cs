using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
using BancoArboleda.Data;
using BancoArboleda.Models;

namespace BancoArboleda.Controllers;

[ApiController]
[Route("api/recarga")]
[Authorize]
public class RecargaController : ControllerBase
{
    private static readonly string[] OperadoresValidos = { "Movistar", "Claro", "Entel", "Bitel" };
    private readonly DataStore _store;

    public RecargaController(DataStore store) => _store = store;

    [HttpPost("procesar")]
    public IActionResult Procesar([FromBody] RecargaRequest req)
    {
        // ── Validaciones de entrada ───────────────────────────────────────────
        if (string.IsNullOrWhiteSpace(req.NumeroCelular))
            return BadRequest(Fail("El número de celular es obligatorio."));

        if (!Regex.IsMatch(req.NumeroCelular.Trim(), @"^9\d{8}$"))
            return BadRequest(Fail("El número de celular debe tener 9 dígitos y comenzar con 9."));

        if (string.IsNullOrWhiteSpace(req.Operador) || !OperadoresValidos.Contains(req.Operador))
            return BadRequest(Fail("Debe seleccionar un operador válido (Movistar, Claro, Entel, Bitel)."));

        if (req.Monto <= 0 || req.Monto > 500)
            return BadRequest(Fail("El monto debe ser mayor a 0 y no exceder S/500."));

        if (string.IsNullOrWhiteSpace(req.ClaveOnline))
            return BadRequest(Fail("La clave online es obligatoria."));

        // ── Obtener cliente del token ─────────────────────────────────────────
        var clienteIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(clienteIdStr, out int clienteId))
            return Unauthorized(Fail("Sesión inválida."));

        var cliente = _store.Clientes.FirstOrDefault(c => c.Id == clienteId);
        if (cliente == null)
            return Unauthorized(Fail("Sesión inválida."));

        // ── Verificar clave online ────────────────────────────────────────────
        if (!BCrypt.Net.BCrypt.Verify(req.ClaveOnline, cliente.ClaveOnlineHash))
            return Unauthorized(Fail("Clave online incorrecta. Intente nuevamente."));

        // ── Prevenir doble envío ──────────────────────────────────────────────
        if (_store.ExisteRecargaReciente(clienteId, req.NumeroCelular.Trim(), req.Operador, req.Monto))
            return Conflict(Fail("La recarga ya fue procesada. Por favor espere."));

        // ── Registrar recarga ─────────────────────────────────────────────────
        var recarga = new Recarga
        {
            ClienteId = clienteId,
            NumeroCelular = req.NumeroCelular.Trim(),
            Operador = req.Operador,
            Monto = req.Monto,
            Referencia = DataStore.GenerarReferencia(),
            FechaHora = DateTime.Now
        };

        _store.SaveRecarga(recarga);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Recarga realizada con éxito.",
            Data = new
            {
                recarga.Id,
                recarga.Referencia,
                recarga.NumeroCelular,
                recarga.Operador,
                recarga.Monto,
                recarga.Estado,
                FechaHora = recarga.FechaHora.ToString("dd/MM/yyyy HH:mm:ss")
            }
        });
    }

    private static ApiResponse<object> Fail(string msg) =>
        new() { Success = false, Message = msg };
}
