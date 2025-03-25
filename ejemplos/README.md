# Ejemplos de `lock` en C# (detalle por ejemplo)

Este documento explica en profundidad cada uno de los 10 ejemplos de uso de `lock` en C#, incluyendo por qué `lock` es la mejor opción para ese caso.

---

## 🧪 Ejemplo 1: Contador compartido

```csharp
lock (_lock)
{
    _contador++;
}
```

🔍 `lock` garantiza que solo un hilo puede modificar `_contador` a la vez, evitando condiciones de carrera.

✅ **¿Por qué `lock`?**  
Porque es el enfoque más limpio y directo para proteger una variable simple. `Monitor` sería redundante y más verboso.

---

## 🧪 Ejemplo 2: Lista compartida

```csharp
lock (_lockLista)
{
    _mensajes.Add(mensaje);
}
```

🔍 Protege el acceso concurrente a una lista compartida.

✅ **¿Por qué `lock`?**  
Ofrece mayor flexibilidad que `ConcurrentBag` o `ConcurrentQueue` cuando se necesita agregar lógica extra como validaciones.

---

## 🧪 Ejemplo 3: Imprimir seguro en consola

```csharp
lock (_consoleLock)
{
    Console.WriteLine(mensaje);
}
```

🔍 Previene que varios hilos impriman simultáneamente y mezclen su salida.

✅ **¿Por qué `lock`?**  
Evita mensajes intercalados. Alternativas como `Monitor` agregarían complejidad sin valor agregado.

---

## 🧪 Ejemplo 4: Depósitos y retiros

```csharp
lock (_lockSaldo)
{
    if (_saldo >= monto)
        _saldo -= monto;
}
```

🔍 Protege lógica de negocio crítica que involucra lectura y escritura.

✅ **¿Por qué `lock`?**  
Permite agrupar operaciones atómicas de forma segura. Más natural que `ReaderWriterLockSlim` para este caso.

---

## 🧪 Ejemplo 5: Cola FIFO personalizada

```csharp
lock (_colaLock)
{
    _cola.Enqueue(dato);
}
```

🔍 Controla el acceso a una cola con validaciones opcionales.

✅ **¿Por qué `lock`?**  
Más flexible que `ConcurrentQueue` si necesitás reglas específicas de negocio.

---

## 🧪 Ejemplo 6: Locks anidados

```csharp
lock (_lockA)
{
    lock (_lockB)
    {
        // lógica
    }
}
```

🔍 Protege el uso conjunto de múltiples recursos.

✅ **¿Por qué `lock`?**  
Es reentrante y sencillo si se sigue un orden fijo. `Mutex` o `Monitor` no agregan beneficios en este caso.

---

## 🧪 Ejemplo 7: Contador por usuario

```csharp
lock (_lockUsuarios)
{
    if (!_contadores.ContainsKey(usuario))
        _contadores[usuario] = 0;

    _contadores[usuario]++;
}
```

🔍 Controla una colección con lógica condicional.

✅ **¿Por qué `lock`?**  
`ConcurrentDictionary` no permite lógica de inicialización personalizada como en este ejemplo.

---

## 🧪 Ejemplo 8: Escritura en archivo

```csharp
lock (_fileLock)
{
    File.AppendAllText("log.txt", texto + Environment.NewLine);
}
```

🔍 Protege acceso a disco entre hilos del mismo proceso.

✅ **¿Por qué `lock`?**  
`Mutex` es innecesario a menos que haya múltiples procesos. `lock` es más rápido y claro.

---

## 🧪 Ejemplo 9: Registro de errores

```csharp
lock (_errorLock)
{
    _errores.Add(ex.Message);
}
```

🔍 Asegura que múltiples hilos no corrompan la colección de errores.

✅ **¿Por qué `lock`?**  
Fácil de implementar. `Monitor` o eventos no aportan ventajas aquí.

---

## 🧪 Ejemplo 10: Control de stock limitado

```csharp
lock (_stockLock)
{
    if (_stock > 0)
    {
        _stock--;
        Console.WriteLine($"{usuario} compró. Stock restante: {_stock}");
    }
}
```

🔍 Sincroniza el decremento de una variable crítica.

✅ **¿Por qué `lock`?**  
Es el patrón clásico de sincronización simple. `Semaphore` sería excesivo si no se necesita control de acceso múltiple.

---