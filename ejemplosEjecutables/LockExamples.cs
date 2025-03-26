
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public static class Logger
{
    private static readonly object _lockLog = new();

    public static void EscribirLog(string mensaje)
    {
        lock (_lockLog)
        {
            File.AppendAllText("log.txt", $"{DateTime.Now}: {mensaje}{Environment.NewLine}");
        }
    }
}

public static class Inventario
{
    private static readonly object _stockLock = new();
    private static int _stock = 5;

    public static string IntentarCompra(string cliente)
    {
        lock (_stockLock)
        {
            if (_stock <= 0) return $"{cliente} no pudo comprar. Sin stock.";
            _stock--;
            return $"{cliente} compró. Stock restante: {_stock}";
        }
    }
}

public static class IdGenerator
{
    private static readonly object _idLock = new();
    private static int _ultimoId = 0;

    public static int GenerarId()
    {
        lock (_idLock)
        {
            return ++_ultimoId;
        }
    }
}

public static class ErrorHandler
{
    private static readonly object _errorLock = new();
    private static List<string> _errores = new();

    public static void EjecutarConCaptura(Action accion)
    {
        try
        {
            accion();
        }
        catch (Exception ex)
        {
            lock (_errorLock)
            {
                _errores.Add(ex.Message);
            }
        }
    }
}

public static class Consola
{
    private static readonly object _consoleLock = new();

    public static void EscribirSeguro(string mensaje)
    {
        lock (_consoleLock)
        {
            Console.WriteLine(mensaje);
        }
    }
}

public static class Billetera
{
    private static readonly object _saldoLock = new();
    private static decimal _saldo = 1000;

    public static string Retirar(decimal monto)
    {
        lock (_saldoLock)
        {
            if (_saldo >= monto)
            {
                _saldo -= monto;
                return $"Retiro exitoso. Saldo restante: {_saldo}";
            }
            return "Fondos insuficientes.";
        }
    }
}

public static class Cache
{
    private static readonly object _cacheLock = new();
    private static Dictionary<string, string> _cache = new();

    public static string ObtenerOAgregar(string clave)
    {
        lock (_cacheLock)
        {
            if (!_cache.ContainsKey(clave))
                _cache[clave] = $"Valor generado para {clave}";
            return _cache[clave];
        }
    }
}

public static class ColaSegura
{
    private static readonly object _colaLock = new();
    private static Queue<string> _cola = new();

    public static void Encolar(string dato)
    {
        lock (_colaLock)
        {
            _cola.Enqueue(dato);
        }
    }

    public static string Desencolar()
    {
        lock (_colaLock)
        {
            return _cola.Count > 0 ? _cola.Dequeue() : "Cola vacía";
        }
    }
}

public static class Sesiones
{
    private static readonly object _usuariosLock = new();
    private static List<string> _usuarios = new();

    public static void Conectar(string usuario)
    {
        lock (_usuariosLock)
        {
            if (!_usuarios.Contains(usuario))
                _usuarios.Add(usuario);
        }
    }
}

public static class RecursoCompartido
{
    private static readonly object _recursoLock = new();

    public static void Acceder(string nombre)
    {
        lock (_recursoLock)
        {
            Console.WriteLine($"{nombre} accediendo a recurso...");
            Thread.Sleep(500);
            Console.WriteLine($"{nombre} salió del recurso.");
        }
    }
}

// Casos especiales
public static class LocksAnidados
{
    private static readonly object _lockA = new();
    private static readonly object _lockB = new();

    public static void TransferenciaSegura(string origen, string destino)
    {
        lock (_lockA)
        {
            lock (_lockB)
            {
                Console.WriteLine($"Transfiriendo de {origen} a {destino}");
            }
        }
    }
}

public static class DeadlockEjemplo
{
    private static readonly object _lockA = new();
    private static readonly object _lockB = new();

    public static void Tarea1()
    {
        lock (_lockA)
        {
            Thread.Sleep(100);
            lock (_lockB)
            {
                Console.WriteLine("Tarea1 terminó");
            }
        }
    }

    public static void Tarea2()
    {
        lock (_lockB)
        {
            Thread.Sleep(100);
            lock (_lockA)
            {
                Console.WriteLine("Tarea2 terminó");
            }
        }
    }
}

public static class DeadlockSafe
{
    private static readonly object _lockA = new();
    private static readonly object _lockB = new();

    public static void TareaEvitaDeadlock()
    {
        bool tengoA = false, tengoB = false;

        try
        {
            tengoA = Monitor.TryEnter(_lockA, 500);
            tengoB = Monitor.TryEnter(_lockB, 500);

            if (tengoA && tengoB)
            {
                Console.WriteLine("Operación segura completada.");
            }
            else
            {
                Console.WriteLine("No se pudo obtener los locks, evitando deadlock.");
            }
        }
        finally
        {
            if (tengoA) Monitor.Exit(_lockA);
            if (tengoB) Monitor.Exit(_lockB);
        }
    }
}
