# Ejemplos de `lock` en C# (detalle por ejemplo)

Este documento explica en profundidad cada uno de los 10 ejemplos de uso de `lock` en C#, incluyendo su propósito, ejecución y por qué `lock` es la mejor opción para ese caso. Todos los ejemplos fueron organizados y adaptados para poder ejecutarse fácilmente en un entorno multihilo.

---

## 🧪 Ejemplo 1: Contador compartido

```csharp
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
```

🔍 Se incrementa un contador desde múltiples hilos. Se expone el valor final mediante el método `GetContador()`.

✅ **¿Por qué `lock`?**  
Es el enfoque más directo y limpio. `Monitor` sería más verboso sin aportar valor. `lock` es suficiente para proteger una operación tan simple.

---

## 🧪 Ejemplo 2: Lista compartida

```csharp
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
```

🔍 Agrega mensajes a una lista compartida desde múltiples hilos. Se accede con `GetMensajes()` para mostrar el resultado.

✅ **¿Por qué `lock`?**  
A diferencia de `ConcurrentBag`, este patrón permite insertar lógica adicional como validaciones, logs, o restricciones.

---

## 🧪 Ejemplo 3: Imprimir seguro en consola

```csharp
private static readonly object _consoleLock = new();

public static void ImprimirSeguro(string mensaje)
{
    lock (_consoleLock)
    {
        Console.WriteLine(mensaje);
    }
}
```

🔍 Asegura que los mensajes en consola no se mezclen cuando varios hilos imprimen simultáneamente.

✅ **¿Por qué `lock`?**  
Evita mensajes intercalados. `Monitor` o cualquier otra alternativa sería innecesariamente compleja.

---

## 🧪 Ejemplo 4: Depósitos y retiros

```csharp
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
```

🔍 Se protege el acceso a una cuenta simulada con retiros y depósitos desde múltiples hilos.

✅ **¿Por qué `lock`?**  
Permite mantener la lógica completa como una transacción segura. `ReaderWriterLockSlim` sería innecesariamente complejo.

---

## 🧪 Ejemplo 5: Cola FIFO personalizada

```csharp
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
```

🔍 Se simula una cola FIFO donde se puede controlar validaciones, errores y estructura personalizada.

✅ **¿Por qué `lock`?**  
`ConcurrentQueue` no permite lanzar excepciones o imponer reglas de negocio. `lock` da más flexibilidad.

---

## 🧪 Ejemplo 6: Locks anidados

```csharp
private static readonly object _lockA = new();
private static readonly object _lockB = new();

public static void Transferir()
{
    lock (_lockA)
    {
        lock (_lockB)
        {
            // lógica de transferencia
        }
    }
}
```

🔍 Protege dos recursos simultáneamente, útil para operaciones como transferencias entre cuentas.

✅ **¿Por qué `lock`?**  
Es reentrante, seguro y claro si se respeta el orden de adquisición. Alternativas como `Mutex` son más costosas.

---

## 🧪 Ejemplo 7: Contador por usuario

```csharp
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
```

🔍 Cuenta ocurrencias por usuario de forma concurrente.

✅ **¿Por qué `lock`?**  
`ConcurrentDictionary` no permite lógica condicional o inicialización personalizada tan claramente.

---

## 🧪 Ejemplo 8: Escritura en archivo

```csharp
private static readonly object _fileLock = new();

public static void GuardarLog(string texto)
{
    lock (_fileLock)
    {
        File.AppendAllText("log.txt", texto + Environment.NewLine);
    }
}
```

🔍 Protege el acceso concurrente a un archivo desde múltiples hilos.

✅ **¿Por qué `lock`?**  
Más simple que un `Mutex`, que solo sería necesario para sincronizar entre procesos. `lock` basta si estás en el mismo proceso.

---

## 🧪 Ejemplo 9: Registro de errores

```csharp
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
```

🔍 Captura y almacena mensajes de error desde cualquier hilo que falle.

✅ **¿Por qué `lock`?**  
Es la opción más clara para proteger acceso concurrente a listas. Otros mecanismos serían innecesarios para este caso puntual.

---

## 🧪 Ejemplo 10: Control de stock limitado

```csharp
private static readonly object _stockLock = new();
private static int _stock = 5;

public static bool IntentarComprar(string usuario)
{
    lock (_stockLock)
    {
        if (_stock > 0)
        {
            _stock--;
            Console.WriteLine($"{usuario} compró. Stock restante: {_stock}");
            return true;
        }
        else
        {
            Console.WriteLine($"{usuario} no pudo comprar. Sin stock.");
            return false;
        }
    }
}
```

🔍 Simula una venta concurrente de productos limitados.

✅ **¿Por qué `lock`?**  
Es un patrón clásico. `SemaphoreSlim` podría usarse si fuera necesario limitar acceso por diseño, pero `lock` es más adecuado cuando hay lógica crítica que depende de condiciones internas (como stock > 0).

---

## 🚀 ¿Cómo ejecutar este proyecto?

1. Asegurate de tener [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) instalado.
2. Abrí la carpeta del proyecto en Visual Studio Code.
3. En terminal, corré:

```bash
dotnet run
```

---

## ✅ Recomendación de `.gitignore`

Incluí un archivo `.gitignore` para evitar subir archivos binarios y de configuración locales:

```
bin/
obj/
.vscode/
*.user
*.suo
*.log
log.txt
```

---
