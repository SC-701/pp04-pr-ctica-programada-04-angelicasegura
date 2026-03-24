-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE EditarVehiculo
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
	UPDATE [dbo].[Vehiculo]
   SET 
      [IdModelo] = @IdModelo
      ,[Placa] = @Placa
      ,[Color] = @Color
      ,[Anio] = @Anio
      ,[Precio] = @Precio
      ,[CorreoPropietario] = @CorreoPropietario
      ,[TelefonoPropietario] = @TelefonoPropietario
 WHERE Id = @Id
 SELECT @ID
 COMMIT TRANSACTION

END