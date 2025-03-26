# Ejemplos prácticos y profesionales de `lock` en C#

Este documento presenta 10 ejemplos realistas y técnicamente justificados del uso de `lock` en C#, todos diseñados con hilos (`Thread`) para ilustrar cómo `lock` previene condiciones de carrera en escenarios concretos de desarrollo profesional.

---

## 🧪 Ejemplo 1: Registro de logs en múltiples hilos

```csharp
private static readonly object _lockLog = new();

public static void EscribirLog(string mensaje)
{
    lock (_lockLog)
    {
        File.AppendAllText("log.txt", $"{DateTime.Now}: {mensaje}{Environment.NewLine}");
    }
}
```

🔍 Cada hilo escribe su mensaje al mismo archivo, evitando corrupción del archivo.

✅ **¿Por qué `lock`?**  
Es rápido, local al proceso y perfecto para sincronizar una sección crítica. Usar `Mutex` sería excesivo (se usa entre procesos) y `Monitor` agrega complejidad innecesaria.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

## 🧪 Ejemplo 2: Validación de stock antes de venta

```csharp
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
```

🔍 Garantiza que dos clientes no compren el mismo stock simultáneamente.

✅ **¿Por qué `lock`?**  
Acceso crítico a un recurso simple. `SemaphoreSlim` sería útil si quisiéramos limitar el acceso concurrente sin lógica condicional. Pero `lock` es ideal para lógica de negocio encapsulada.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

## 🧪 Ejemplo 3: Asignación única de identificadores

```csharp
private static readonly object _idLock = new();
private static int _ultimoId = 0;

public static int GenerarId()
{
    lock (_idLock)
    {
        return ++_ultimoId;
    }
}
```

🔍 Garante unicidad de IDs generados por múltiples hilos.

✅ **¿Por qué `lock`?**  
Protege un contador compartido. Alternativas como `Interlocked` pueden funcionar, pero `lock` permite incluir más lógica si fuera necesario.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

## 🧪 Ejemplo 4: Registro de errores capturados

```csharp
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
```

🔍 Permite a múltiples hilos registrar errores sin sobrescribir o perder mensajes.

✅ **¿Por qué `lock`?**  
Colecciones compartidas deben protegerse. `ConcurrentBag` sería válido, pero no permite personalizar la captura del error.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

## 🧪 Ejemplo 5: Consola como recurso exclusivo

```csharp
private static readonly object _consoleLock = new();

public static void EscribirSeguro(string mensaje)
{
    lock (_consoleLock)
    {
        Console.WriteLine(mensaje);
    }
}
```

🔍 Evita que los mensajes de múltiples hilos se mezclen en la consola.

✅ **¿Por qué `lock`?**  
`lock` es directo y eficiente. `Monitor` o `Mutex` solo agregarían complejidad en este caso.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

## 🧪 Ejemplo 6: Billetera digital con múltiples operaciones

```csharp
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
```

🔍 Proceso transaccional requiere leer y escribir atómicamente el saldo.

✅ **¿Por qué `lock`?**  
Permite proteger el estado completo de una transacción. `ReaderWriterLockSlim` sería útil si sólo leyéramos, pero aquí hay escritura crítica.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

## 🧪 Ejemplo 7: Cache local con verificación y creación

```csharp
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
```

🔍 Solo un hilo debe inicializar una entrada en caché.

✅ **¿Por qué `lock`?**  
`ConcurrentDictionary` existe, pero no permite fácilmente lógica condicional personalizada como en este patrón.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

## 🧪 Ejemplo 8: Cola FIFO con validación

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
        return _cola.Count > 0 ? _cola.Dequeue() : "Cola vacía";
    }
}
```

🔍 Dos métodos que acceden y modifican la cola deben estar sincronizados.

✅ **¿Por qué `lock`?**  
Permite validar, controlar errores y mantener lógica clara. `ConcurrentQueue` no da control sobre errores o lógica personalizada.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

## 🧪 Ejemplo 9: Lista de usuarios conectados

```csharp
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
```

🔍 Evita conexiones duplicadas en un servidor multihilo.

✅ **¿Por qué `lock`?**  
Permite lógica condicional que no se puede expresar con `ConcurrentBag` o `ConcurrentQueue`.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

## 🧪 Ejemplo 10: Control de acceso por recurso compartido

```csharp
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
```

🔍 Simula el uso exclusivo de un recurso costoso (como una base de datos o impresora).

✅ **¿Por qué `lock`?**  
`lock` asegura exclusión mutua con mínima sobrecarga. `Semaphore` o `Barrier` no aportan beneficios aquí, salvo que se quiera concurrencia parcial.



📊 **Comparación con otros mecanismos:**
- 🔐 `Monitor`: Equivalente, pero requiere más código (`Enter`/`Exit`). `lock` es preferible para claridad y simplicidad.
- 🧵 `Mutex`: No necesario salvo sincronización entre procesos (otro ejecutable o servicio).
- 🔄 `Barrier`: No aplica. `Barrier` sincroniza fases entre hilos, no protege secciones críticas.
- 📉 `Semaphore(Slim)`: Útil si se desea concurrencia parcial. En estos ejemplos se busca exclusividad total.
---

# Casos especiales de `lock` en C#: Anidados y Deadlocks

1. Cómo implementar correctamente `lock` anidados sin riesgo.
2. Cómo se puede producir un **deadlock real** y cómo prevenirlo.

---

## 🔁 Caso 1: Locks anidados bien implementados

```csharp
private static readonly object _lockA = new();
private static readonly object _lockB = new();

public static void TransferenciaSegura(string origen, string destino)
{
    // Siempre adquirir locks en el mismo orden
    lock (_lockA)
    {
        lock (_lockB)
        {
            Console.WriteLine($"Transfiriendo de {origen} a {destino}");
        }
    }
}
```

✅ **¿Por qué es seguro?**  
Todos los hilos que necesiten ambos recursos los adquieren en el mismo orden (`_lockA` → `_lockB`). Esto evita bloqueos circulares.

📊 **Comparación**:
- 🔐 `lock`: perfecto para operaciones críticas entre recursos acoplados.
- 🧵 `Mutex`: más costoso y generalmente para sincronización entre procesos.
- 🔄 `Monitor`: requiere más código pero el mismo principio.

---

## 🔁 Caso 2: Deadlock por orden de adquisición inverso

```csharp
private static readonly object _lockA = new();
private static readonly object _lockB = new();

public static void Tarea1()
{
    lock (_lockA)
    {
        Thread.Sleep(100); // Simula trabajo
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
```

🔍 **¿Qué ocurre?**  
- `Tarea1` bloquea `_lockA` y espera por `_lockB`.
- `Tarea2` bloquea `_lockB` y espera por `_lockA`.
Ninguno avanza: **deadlock**.

❌ **Consecuencias:**  
El sistema queda congelado. Los hilos no pueden terminar.

🛡 **¿Cómo prevenirlo?**
- Siempre adquirir locks en el **mismo orden**.
- O usar un mecanismo como `Monitor.TryEnter` con timeout para evitar bloqueos indefinidos.

---

## 🛡 Caso 3: Evitar deadlock con `Monitor.TryEnter`

```csharp
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
```

✅ **¿Por qué es seguro?**  
Al usar `TryEnter` con timeout, se evita quedar esperando indefinidamente si otro hilo ya tiene el lock.

📌 **Ventaja sobre `lock`:**  
Control explícito del tiempo de espera. Útil cuando el sistema debe continuar funcionando aún si no puede ejecutar la operación crítica.

---