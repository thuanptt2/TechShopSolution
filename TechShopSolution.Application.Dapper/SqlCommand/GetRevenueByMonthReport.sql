ALTER PROC GetRevenueByMonth
	@fromDate VARCHAR(10),
	@toDate VARCHAR(10)
AS
BEGIN
			SELECT 
				CONCAT(month(b.create_at), '/', year(b.create_at)) as MONTH,
				SUM(b.total + b.transport_fee - b.discount) as Revenue
				FROM Bill b
				WHERE CAST(b.create_at AS Date) <= CAST(@toDate AS Date)
				AND CAST(b.create_at AS Date) >= CAST(@fromDate AS Date)
				AND b.status = 1
				AND b.isPay = 1
				GROUP BY CONCAT(month(b.create_at), '/', year(b.create_at))
END

EXEC dbo.GetRevenueByMonth @fromDate = '5/1/2021',
						   @toDate = '7/31/2021'

select * from Bill


