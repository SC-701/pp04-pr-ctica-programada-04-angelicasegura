
CREATE PROCEDURE ObtenerModelos
	@IdMarca uniqueidentifier
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT Id, IdMarca, Nombre
	FROM Modelos
	WHERE (IdMarca = @IdMarca)
END