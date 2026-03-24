-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE AgregarVehiculo
@Id AS uniqueidentifier, 
@IdModelo AS uniqueidentifier, 
@Placa as varchar(MAX), 
@Color AS varchar(MAX), 
@Anio AS int, 
@Precio AS decimal (18,0), 
@CorreoPropietario AS varchar(MAX), 
@TelefonoPropietario as varchar(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRANSACTION

	INSERT INTO [dbo].[Vehiculo]
           ([Id]
           ,[IdModelo]
           ,[Placa]
           ,[Color]
           ,[Anio]
           ,[Precio]
           ,[CorreoPropietario]
           ,[TelefonoPropietario])
     VALUES
          (@Id, 
			@IdModelo, 
			@Placa, 
			@Color, 
			@Anio, 
			@Precio, 
			@CorreoPropietario , 
			@TelefonoPropietario )
	SELECT @ID
	COMMIT TRANSACTION
END