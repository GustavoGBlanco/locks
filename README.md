# Módulo 1: [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) y locks anidados en C#

## 🔐 ¿Qué es [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock)?
[`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) se usa para garantizar que **una sola hebra de ejecución (hilo)** acceda a una porción de código crítica a la vez. Previene **condiciones de carrera** cuando múltiples hilos intentan modificar recursos compartidos.

```csharp
lock (obj) {
   // Solo un hilo puede estar aquí a la vez
}
```

Internamente, [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) usa [`Monitor.Enter()`](https://learn.microsoft.com/es-es/dotnet/api/system.threading.monitor.enter?view=net-8.0) y [`Monitor.Exit()`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.monitor.exit?view=net-9.0).

---

## 🏠 Escenario práctico: Gestor de inventario

Creamos una clase `InventoryManager` que maneja el stock de productos. Varios hilos pueden intentar vender o reponer productos al mismo tiempo. Usamos [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) para proteger los datos compartidos.

### Archivos

#### `InventoryManager.cs`
```csharp
using System;
using System.Collections.Generic;

public class InventoryManager
{
    private readonly Dictionary<string, int> _stock = new();
    private readonly object _lock = new();

    public void AddProduct(string product, int quantity)
    {
        lock (_lock)
        {
            if (!_stock.ContainsKey(product))
                _stock[product] = 0;

            _stock[product] += quantity;
            Console.WriteLine($"[+] Agregado: {product} x{quantity}. Total: {_stock[product]}");
        }
    }

    public void SellProduct(string product, int quantity)
    {
        lock (_lock)
        {
            if (!_stock.ContainsKey(product))
            {
                Console.WriteLine($"[!] Producto {product} no existe.");
                return;
            }

            if (_stock[product] < quantity)
            {
                Console.WriteLine($"[-] No hay suficiente stock de {product}. Disponible: {_stock[product]}");
                return;
            }

            _stock[product] -= quantity;
            Console.WriteLine($"[-] Vendido: {product} x{quantity}. Quedan: {_stock[product]}");
        }
    }

    public void ShowStock()
    {
        lock (_lock)
        {
            Console.WriteLine("\n📦 Stock actual:");
            foreach (var kvp in _stock)
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
        }
    }
}
```

#### `Program.cs`
```csharp
using System;
using System.Threading;

class Program
{
    static void Main()
    {
        var inventory = new InventoryManager();

        Thread t1 = new(() => {
            for (int i = 0; i < 5; i++)
            {
                inventory.AddProduct("Laptop", 1);
                Thread.Sleep(100);
            }
        });

        Thread t2 = new(() => {
            for (int i = 0; i < 5; i++)
            {
                inventory.SellProduct("Laptop", 1);
                Thread.Sleep(150);
            }
        });

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        inventory.ShowStock();
    }
}
```

---

## 🤔 ¿Por qué usar [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) y no [`Monitor`](https://learn.microsoft.com/es-es/dotnet/api/system.threading.monitor?view=net-8.0) directamente?

### ✅ 1. [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) es una abstracción segura y limpia
El compilador transforma:
```csharp
lock (obj) {
    // código
}
```
en:
```csharp
Monitor.Enter(obj);
try {
    // código
}
finally {
    Monitor.Exit(obj);
}
```

Evita errores como olvidarse del [`Monitor.Exit()`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.monitor.exit?view=net-9.0) si ocurre una excepción.

### ✅ 2. Código más limpio y legible
[`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) reduce ruido visual y hace que el código sea más intuitivo.

### ✅ 3. Ideal para la mayoría de los casos
Usá [`Monitor`](https://learn.microsoft.com/es-es/dotnet/api/system.threading.monitor?view=net-8.0) solo si necesitás [`Wait()`](https://learn.microsoft.com/es-es/dotnet/api/system.threading.monitor.wait?view=net-8.0#system-threading-monitor-wait(system-object)) y [`Pulse()`](https://learn.microsoft.com/es-es/dotnet/api/system.threading.monitor.pulse?view=net-8.0), que [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) no soporta directamente.

### Analogía:
> [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) es como un microondas con botones predefinidos.
> [`Monitor`](https://learn.microsoft.com/es-es/dotnet/api/system.threading.monitor?view=net-8.0) es como abrir el panel y ajustar voltajes manualmente.

---

## 🧠 Locks anidados

### ¿Qué es un lock anidado?
Es cuando un hilo entra en un [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) y dentro vuelve a usar [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock), incluso sobre el mismo objeto:

```csharp
lock (obj)
{
    // ...
    lock (obj)
    {
        // ...
    }
}
```
Esto **no genera deadlock si es el mismo hilo**. [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) es reentrante.

### ⚠️ Ejemplo de mal uso (potencial deadlock):
```csharp
lock (_lock1)
{
    lock (_lock2)
    {
        // ...
    }
}

// Otro hilo hace lo inverso:
lock (_lock2)
{
    lock (_lock1)
    {
        // ...
    }
}
```
❄️ Ambos hilos se bloquean esperando al otro: deadlock.

---

## 🧼 Buenas prácticas con [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock)

| Regla | Motivo |
|-------|--------|
| 🔒 Usá objetos privados como candado | Evitá acceso externo o colisiones |
| 🌟 Mantené el bloque [`lock`](https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/statements/lock) corto | Minimiza bloqueos innecesarios |
| 🔄 Evitá locks anidados entre objetos distintos | Previene deadlocks |
| 🤔 Siempre probá concurrencia con múltiples hilos | Verificá consistencia |

---

✅ Este fue el Módulo 1. El siguiente será sobre [`Monitor`](https://learn.microsoft.com/es-es/dotnet/api/system.threading.monitor?view=net-8.0), [`Wait()`](https://learn.microsoft.com/es-es/dotnet/api/system.threading.monitor.wait?view=net-8.0#system-threading-monitor-wait(system-object)) y [`Pulse()`](https://learn.microsoft.com/es-es/dotnet/api/system.threading.monitor.pulse?view=net-8.0) con un escenario productor-consumidor realista.

