
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

public static class LockExamples
{
    // Ejemplo 1: Contador compartido
    private static readonly object _lock = new();
    private static int _contador = 0;

    public static void Incrementar()
    {
        lock (_lock)
        {
            _contador++;
        }
    }

    public static int GetContador()
    {
        lock (_lock)
        {
            return _contador;
        }
    }

    // Ejemplo 2: Lista compartida
    private static readonly object _lockLista = new();
    private static List<string> _mensajes = new();

    public static void AgregarMensaje(string mensaje)
    {
        lock (_lockLista)
        {
            _mensajes.Add(mensaje);
        }
    }

    public static List<string> GetMensajes()
    {
        lock (_lockLista)
        {
            return new List<string>(_mensajes);
        }
    }

    // Ejemplo 3: Imprimir en consola
    private static readonly object _consoleLock = new();

    public static void ImprimirSeguro(string mensaje)
    {
        lock (_consoleLock)
        {
            Console.WriteLine(mensaje);
        }
    }

    // Ejemplo 4: Depósitos y retiros
    private static readonly object _lockSaldo = new();
    private static int _saldo = 1000;

    public static void Depositar(int monto)
    {
        lock (_lockSaldo)
        {
            _saldo += monto;
        }
    }

    public static void Retirar(int monto)
    {
        lock (_lockSaldo)
        {
            if (_saldo >= monto)
                _saldo -= monto;
        }
    }

    // Ejemplo 5: Cola FIFO personalizada
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
            if (_cola.Count == 0)
                throw new InvalidOperationException("La cola está vacía.");
            return _cola.Dequeue();
        }
    }

    // Ejemplo 6: Locks anidados
    private static readonly object _lockA = new();
    private static readonly object _lockB = new();

    public static void Transferir()
    {
        lock (_lockA)
        {
            lock (_lockB)
            {
                // Transferencia entre recursos
            }
        }
    }

    // Ejemplo 7: Contador por usuario
    private static readonly object _lockUsuarios = new();
    private static Dictionary<string, int> _contadores = new();

    public static void IncrementarUsuario(string usuario)
    {
        lock (_lockUsuarios)
        {
            if (!_contadores.ContainsKey(usuario))
                _contadores[usuario] = 0;

            _contadores[usuario]++;
        }
    }

    // Ejemplo 8: Escritura en archivo
    private static readonly object _fileLock = new();

    public static void GuardarLog(string texto)
    {
        lock (_fileLock)
        {
            File.AppendAllText("log.txt", texto + Environment.NewLine);
        }
    }

    // Ejemplo 9: Registro de errores
    private static readonly object _errorLock = new();
    private static List<string> _errores = new();

    public static void Procesar(Action tarea)
    {
        try
        {
            tarea();
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

// Ejemplo 10: Clase separada para stock limitado
public static class StockDemo
{
    private static readonly object _stockLock = new();
    private static int _stock = 5;

    public static bool IntentarComprar(string usuario)
    {
        lock (_stockLock)
        {
            if (_stock > 0)
            {
                _stock--;
                Console.WriteLine($"[32m{usuario} compró. Stock restante: {_stock}[0m");
                return true;
            }
            else
            {
                Console.WriteLine($"[31m{usuario} no pudo comprar. Sin stock.[0m");
                return false;
            }
        }
    }
}
