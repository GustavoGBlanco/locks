
using System;
using System.Threading;

class LockExamplesApp
{
    static void Main()
    {
        Console.WriteLine("🧪 Ejecutando ejemplos de `lock`");

        Console.WriteLine("----------Ejemplo 1: Escritura de logs----------");
        new Thread(() => Logger.EscribirLog("Desde Hilo 1")).Start();
        new Thread(() => Logger.EscribirLog("Desde Hilo 2")).Start();
        Thread.Sleep(300);
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 2: Validación de stock----------");
        for (int i = 1; i <= 7; i++)
        {
            string cliente = $"Cliente{i}";
            new Thread(() => Console.WriteLine(Inventario.IntentarCompra(cliente))).Start();
        }
        Thread.Sleep(500);
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 3: Generación de ID----------");
        new Thread(() => Console.WriteLine($"Nuevo ID: {IdGenerator.GenerarId()}")).Start();
        Thread.Sleep(300);
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 4: Registro de errores----------");
        ErrorHandler.EjecutarConCaptura(() => throw new Exception("Simulación de error"));
        Console.WriteLine("Error registrado.");
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 5: Escritura segura en consola----------");
        new Thread(() => Consola.EscribirSeguro("Mensaje A")).Start();
        new Thread(() => Consola.EscribirSeguro("Mensaje B")).Start();
        Thread.Sleep(300);
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 6: Retiro de billetera----------");
        new Thread(() => Console.WriteLine(Billetera.Retirar(100))).Start();
        Thread.Sleep(300);
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 7: Cache de valores----------");
        Console.WriteLine(Cache.ObtenerOAgregar("cliente:123"));
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 8: Cola segura----------");
        ColaSegura.Encolar("Tarea 1");
        Console.WriteLine("Desencolado: " + ColaSegura.Desencolar());
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 9: Lista de usuarios conectados----------");
        new Thread(() => Sesiones.Conectar("usuario1")).Start();
        new Thread(() => Sesiones.Conectar("usuario1")).Start();
        Thread.Sleep(300);
        Console.WriteLine("Usuarios conectados registrados.");
        Console.WriteLine();

        Console.WriteLine("----------Ejemplo 10: Acceso exclusivo a recurso----------");
        new Thread(() => RecursoCompartido.Acceder("Proceso A")).Start();
        new Thread(() => RecursoCompartido.Acceder("Proceso B")).Start();
        Thread.Sleep(1000);
        Console.WriteLine();

        Console.WriteLine("🔁 Casos especiales de lock");

        Console.WriteLine("----------Caso 1: Locks anidados bien implementados----------");
        new Thread(() => LocksAnidados.TransferenciaSegura("A", "B")).Start();
        Thread.Sleep(300);
        Console.WriteLine();

        Console.WriteLine("----------Caso 2: Deadlock real por orden invertido----------");
        new Thread(DeadlockEjemplo.Tarea1).Start();
        new Thread(DeadlockEjemplo.Tarea2).Start();
        Thread.Sleep(1000);
        Console.WriteLine();

        Console.WriteLine("----------Caso 3: Prevención de deadlock con Monitor.TryEnter----------");
        new Thread(DeadlockSafe.TareaEvitaDeadlock).Start();
        Thread.Sleep(500);
        Console.WriteLine();

        Console.WriteLine("✅ Fin de los ejemplos.");
    }
}
