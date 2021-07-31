ALTER PROC GetRevenueDaily 
	@fromDate VARCHAR(10),
	@toDate VARCHAR(10)
AS
BEGIN
			SELECT 
				CAST(b.create_at as DATE) as Date,
				SUM(b.total + b.transport_fee - b.discount) as Revenue
				FROM Bill b
				WHERE b.create_at <= CAST(@toDate AS Date)
				AND b.create_at >= CAST(@fromDate AS Date)
				AND b.status = 1
				AND b.isPay = 1
				group by CAST(b.create_at as DATE)
END

EXEC dbo.GetRevenueDaily @fromDate = '07/02/2021',
						 @toDate = '07/30/2021'
