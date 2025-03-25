using System;
using System.Threading;

class LockExamplesApp
{
    static void Main()
    {
        Console.WriteLine("Ejemplos de lock en C# ejecutándose...");

        // ----------------------
        // Ejemplo 1: Contador compartido
        Console.WriteLine("----------Ejemplo1----------");
        for (int i = 0; i < 1000; i++)
            new Thread(LockExamples.Incrementar).Start();
        Thread.Sleep(500);
        Console.WriteLine("Contador final: " + LockExamples.GetContador());
        Console.WriteLine();

        // ----------------------
        // Ejemplo 2: Lista compartida
        Console.WriteLine("----------Ejemplo2----------");
        for (int i = 0; i < 5; i++)
            new Thread(() => LockExamples.AgregarMensaje($"Mensaje {i}")).Start();
        Thread.Sleep(500);
        Console.WriteLine("Mensajes en la lista:");
        foreach (var mensaje in LockExamples.GetMensajes())
        {
            Console.WriteLine(" - " + mensaje);
        }
        Console.WriteLine();

        // ----------------------
        // Ejemplo 3: Imprimir en consola
        Console.WriteLine("----------Ejemplo3----------");
        for (int i = 0; i < 3; i++)
            new Thread(() => LockExamples.ImprimirSeguro($"Impresión segura {i}")).Start();
        Thread.Sleep(500);
        Console.WriteLine();

        // ----------------------
        // Ejemplo 4: Depósitos y retiros
        Console.WriteLine("----------Ejemplo4----------");
        new Thread(() => LockExamples.Depositar(500)).Start();
        new Thread(() => LockExamples.Retirar(300)).Start();
        Thread.Sleep(500);
        Console.WriteLine("Operaciones de saldo realizadas (no se imprime saldo actual)");
        Console.WriteLine();

        // ----------------------
        // Ejemplo 5: Cola FIFO personalizada
        Console.WriteLine("----------Ejemplo5----------");
        LockExamples.Encolar("Dato A");
        LockExamples.Encolar("Dato B");
        try
        {
            Console.WriteLine("Dato desencolado: " + LockExamples.Desencolar());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al desencolar: " + ex.Message);
        }
        Console.WriteLine();

        // ----------------------
        // Ejemplo 6: Locks anidados
        Console.WriteLine("----------Ejemplo6----------");
        LockExamples.Transferir();
        Console.WriteLine("Transferencia entre recursos realizada.");
        Console.WriteLine();

        // ----------------------
        // Ejemplo 7: Contador por usuario
        Console.WriteLine("----------Ejemplo7----------");
        new Thread(() => LockExamples.IncrementarUsuario("Juan")).Start();
        new Thread(() => LockExamples.IncrementarUsuario("Ana")).Start();
        Thread.Sleep(500);
        Console.WriteLine("Contadores por usuario incrementados (no se imprime diccionario).");
        Console.WriteLine();

        // ----------------------
        // Ejemplo 8: Escritura en archivo
        Console.WriteLine("----------Ejemplo8----------");
        LockExamples.GuardarLog("Log generado desde Main");
        Console.WriteLine("Log guardado en archivo 'log.txt'.");
        Console.WriteLine();

        // ----------------------
        // Ejemplo 9: Registro de errores
        Console.WriteLine("----------Ejemplo9----------");
        LockExamples.Procesar(() => throw new Exception("Error simulado en Main"));
        Console.WriteLine("Error capturado y registrado.");
        Console.WriteLine();

        // ----------------------
        // Ejemplo 10: Control de stock con múltiples hilos
        Console.WriteLine("----------Ejemplo10----------");
        for (int i = 1; i <= 10; i++)
        {
            string usuario = $"Usuario{i}";
            new Thread(() => StockDemo.IntentarComprar(usuario)).Start();
        }

        Thread.Sleep(1000); // Esperar que todos terminen
        Console.WriteLine("Fin de los ejemplos.");
    }
}