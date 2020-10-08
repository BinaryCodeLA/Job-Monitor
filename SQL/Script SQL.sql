/*
Script SQL
*/

use Dataprueba --Nombre de Base de datos destino
go
create table JobsNotificationlogs(
Instancia nvarchar(100),
Nombre_Job nvarchar(100),
Fecha_ejecucion datetime,
Estado nvarchar(50)
)