# 🛒 Tienda ASP.NET MVC

Proyecto desarrollado en **ASP.NET MVC** siguiendo una **arquitectura en capas**, orientado a la construcción de una aplicación web tipo tienda, con separación clara de responsabilidades y buenas prácticas de desarrollo.

---

## 📌 Descripción

Esta aplicación implementa un sistema de tienda web utilizando el patrón **MVC (Model–View–Controller)** y una estructura en capas que facilita el mantenimiento, la escalabilidad y la reutilización del código.

El proyecto forma parte de un proceso de aprendizaje y práctica en desarrollo web con **.NET**, enfocado en aplicaciones reales.

---

## 🏗️ Arquitectura del proyecto

El proyecto está organizado en las siguientes capas:

- **CapaEntidad**
  - Contiene las entidades del dominio (modelos de negocio).
  - Define la estructura de los datos.

- **CapaDatos**
  - Acceso a datos.
  - Comunicación con la base de datos.
  - Implementa repositorios y consultas.

- **CapaNegocio**
  - Lógica de negocio.
  - Validaciones y reglas del sistema.

- **CapaPresentacionAdmin**
  - Interfaz web para la administración.
  - Controladores y vistas del panel administrativo.

- **CapaPresentacionTienda**
  - Interfaz web orientada al usuario final.
  - Controladores y vistas públicas de la tienda.

---

## 🧰 Tecnologías utilizadas

- **ASP.NET MVC**
- **C#**
- **HTML / CSS**
- **JavaScript**
- **Bootstrap**
- **SQL Server** 
- **Visual Studio**
- **Git & GitHub**

---

## ⚙️ Requisitos previos

- Visual Studio 2022 o superior
- .NET SDK compatible
- SQL Server (local o remoto)
- Git

---

## ▶️ Cómo ejecutar el proyecto

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/diego939/tienda_asp.net.git
