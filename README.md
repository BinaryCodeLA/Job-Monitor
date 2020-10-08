# Jobs Monitor
## Librería en .NET Core 3.1 
## Conexión a SQL SERVER con Dapper

 Monitorear la ejecución de jobs SQL mediante el JobHistory y demás tablas relacionadas en la BD msdb 
 Revisión los schedules y ejecución de los jobs y validar si se ejecutó o no en las fechas preestablecidas,
 generar informe en Tabla de notificación. 

 
Se crearon 3 Proyectos en la Solución:

-    	JobsClient: Proyecto cliente de consola para ejecutar la DLL

-       JobsMonitor: Proyecto tipo DLL

-       XUnitTestJobsMonitor: Proyecto para Test Unitarios de la DLL

-       Para la conexión a la base de datos se utilizó Dapper

-       Las configuraciones de conexión a las distintas instancias y a la base de datos destino se deja en archivo de configuración en formato Json.

-       Para los Test Unitarios se utilizó XUnits.
